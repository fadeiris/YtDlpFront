using Serilog;
using Serilog.Events;
using Serilog.Sinks.WinForms.Base;
using System.Configuration;
using System.Diagnostics;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YtDlpFront.Common;
using YtDlpFront.Common.Sets;
using YtDlpFront.Common.Utils;
using YtDlpFront.Extensions;
using YtDlpFront.Models;

namespace YtDlpFront;

public partial class MainForm : Form
{
    public MainForm(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();

        // 設定 IHttpClientFactory。
        SharedHttpClientFactory = httpClientFactory;

        // 建立 httpClient。
        SharedHttpClient = SharedHttpClientFactory?.CreateClient();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        // 自定義初始化，放於此處執行以避免發生例外。
        CustomInit();
        CheckAppVersion();

        // 設定 Log 清除機制。
        WindFormsSink.SimpleTextBoxSink.OnLogReceived += (string sourceContext, string str) =>
        {
            RtbLogCtrlLog.InvokeIfRequired(() =>
            {
                int limitLength = 101000;
                int currentLength = RtbLogCtrlLog.TextLength;

                if (currentLength >= limitLength)
                {
                    RtbLogCtrlLog.ClearLogs();

                    SerilogUtil.WriteLog($"顯示的日誌已達最大長度 {limitLength}，已清除先前的日誌內容。");
                }
            });
        };
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            // 先取消作業。
            BtnCancel_Click(null, null);

            // 釋放 HttpClient。
            if (SharedHttpClient != null)
            {
                SharedHttpClient.Dispose();
                SharedHttpClient = null;
            }

            // 關閉 Serilog。
            Log.CloseAndFlush();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void TBUrl_TextChanged(object sender, EventArgs e)
    {
        TBUrl.InvokeIfRequired(() =>
        {
            if (!string.IsNullOrEmpty(TBUrl.Text))
            {
                ImportUtil.ParseSecondsFromUrl(
                    TBUrl.Text,
                    SharedDataSource,
                    TBAutoEndSeconds);
            }
        });
    }

    private void TBFormatSelection_TextChanged(object sender, EventArgs e)
    {
        TBFormatSelection.InvokeIfRequired(() =>
        {
            string value = TBFormatSelection.Text;

            if (value != Properties.Settings.Default.FormatSelection)
            {
                Properties.Settings.Default.FormatSelection = value;
                Properties.Settings.Default.Save();
            }
        });
    }

    private void BtnAddNewClip_Click(object sender, EventArgs e)
    {
        try
        {
            double startSeconds = ImportUtil.GetStartSeconds(TBStartSeconds, MTBStartTime),
                   endSeconds = ImportUtil.GetEndSeconds(TBEndSeconds, MTBEndtime);

            string errorMessage = CustomFunction.IsSecondsValid(
                startSeconds,
                endSeconds);

            if (string.IsNullOrEmpty(errorMessage))
            {
                DgvClipList.InvokeIfRequired(() =>
                {
                    int lastRowIdx = DgvClipList.RowCount - 1;
                    int clipNo = 1;

                    if (DgvClipList.Rows.Count > 0)
                    {
                        DataGridViewCell dgvCell = DgvClipList.Rows[lastRowIdx].Cells[0];

                        if (dgvCell.Value != null)
                        {
                            clipNo = int.TryParse(dgvCell.Value.ToString(), out int parsedNo) ? parsedNo : 0;

                            clipNo++;
                        }
                    }

                    SharedDataSource.Add(new ClipData()
                    {
                        ClipNo = clipNo,
                        ClipName = TBClipName.Text,
                        StartSeconds = startSeconds,
                        EndSeconds = endSeconds,
                        IsAudioOnly = CBIsAudioOnly.Checked
                    });

                    if (lastRowIdx >= 0)
                    {
                        DgvClipList.FirstDisplayedScrollingRowIndex = lastRowIdx;
                    }
                });
            }
            else
            {
                errorMessage = errorMessage.TrimEnd(Environment.NewLine.ToCharArray());

                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
            }

            DgvClipList.InvokeIfRequired(() =>
            {
                if (DgvClipList.RowCount > 0)
                {
                    DgvClipList.FirstDisplayedScrollingRowIndex = DgvClipList.RowCount - 1;
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnResetNewClip_Click(object sender, EventArgs e)
    {
        try
        {
            TBClipName.InvokeIfRequired(() =>
            {
                TBClipName.Text = string.Empty;
            });

            TBStartSeconds.InvokeIfRequired(() =>
            {
                TBStartSeconds.Text = "0.0";
            });

            MTBStartTime.InvokeIfRequired(() =>
            {
                MTBStartTime.Text = "00:00:00.000";
            });

            TBEndSeconds.InvokeIfRequired(() =>
            {
                TBEndSeconds.Text = "0.0";
            });

            MTBEndtime.InvokeIfRequired(() =>
            {
                MTBEndtime.Text = "00:00:00.000";
            });

            CBIsAudioOnly.InvokeIfRequired(() =>
            {
                CBIsAudioOnly.Checked = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void TBStartSeconds_TextChanged(object sender, EventArgs e)
    {
        TBStartSeconds.InvokeIfRequired(() =>
        {
            if (!string.IsNullOrEmpty(TBStartSeconds.Text) &&
                !double.TryParse(TBStartSeconds.Text, out double _))
            {
                MessageBox.Show(
                    "請輸入有效的數值。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        });
    }

    private void TBEndSeconds_TextChanged(object sender, EventArgs e)
    {
        TBEndSeconds.InvokeIfRequired(() =>
        {
            if (!string.IsNullOrEmpty(TBEndSeconds.Text) &&
                !double.TryParse(TBEndSeconds.Text, out double _))
            {
                MessageBox.Show(
                    "請輸入有效的數值。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        });
    }

    private void CBBrowser_SelectedIndexChanged(object sender, EventArgs e)
    {
        CBBrowser.InvokeIfRequired(() =>
        {
            string? value = CBBrowser.SelectedItem.ToString();

            if (value != Properties.Settings.Default.Browser)
            {
                Properties.Settings.Default.Browser = value ?? string.Empty;
                Properties.Settings.Default.Save();
            }
        });
    }

    private void TBBrowserProfile_TextChanged(object sender, EventArgs e)
    {
        TBBrowserProfile.InvokeIfRequired(() =>
        {
            string value = TBBrowserProfile.Text;

            if (value != Properties.Settings.Default.BrowserProfile)
            {
                Properties.Settings.Default.BrowserProfile = value;
                Properties.Settings.Default.Save();
            }
        });
    }

    private void CBEnableLoadCookiesFromBrowser_CheckedChanged(object sender, EventArgs e)
    {
        CBEnableLoadCookiesFromBrowser.InvokeIfRequired(() =>
        {
            bool value = CBEnableLoadCookiesFromBrowser.Checked;

            if (value != Properties.Settings.Default.LoadCookiesFromBrowser)
            {
                Properties.Settings.Default.LoadCookiesFromBrowser = value;
                Properties.Settings.Default.Save();
            }
        });
    }

    private async void BtnFetchVideoInfo_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            SharedCancellationTokenSource = new();
            SharedCancellationToken = SharedCancellationTokenSource.Token;

            string url = string.Empty;

            TBUrl.InvokeIfRequired(() =>
            {
                url = TBUrl.Text;
            });

            if (!string.IsNullOrEmpty(url))
            {
                RtbLogCtrlLog.InvokeIfRequired(() =>
                {
                    // 清除 Log。
                    RtbLogCtrlLog.ClearLogs();
                });

                string formatSelection = string.Empty;

                TBFormatSelection.InvokeIfRequired(() =>
                {
                    formatSelection = TBFormatSelection.Text;
                });

                OptionSet optionSet = CustomFunction.GetCustomOptionSet(
                    formatSelection,
                    CBEnableLoadCookiesFromBrowser,
                    CBBrowser,
                    TBBrowserProfile,
                    enableEmbedMeta: false,
                    enableLiveFromStart: false,
                    enableEmbedSubs: false,
                    enableWaitForVideo: false,
                    enableSplitChapters: false);

                await OperationSet.FetchVideo(url, optionSet, SharedCancellationToken);
            }
            else
            {
                MessageBox.Show(
                    "請輸入網址。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);
        }
    }

    private async void BtnDownloadVideo_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            string url = string.Empty;

            TBUrl.InvokeIfRequired(() =>
            {
                url = TBUrl.Text;
            });

            string formatSelection = string.Empty;

            TBFormatSelection.InvokeIfRequired(() =>
            {
                formatSelection = TBFormatSelection.Text;
            });

            bool downloadFirstSplitLater = false;

            CBDownloadFirstSplitLater.InvokeIfRequired(() =>
            {
                downloadFirstSplitLater = CBDownloadFirstSplitLater.Checked;
            });

            bool enableEmbedMeta = false;

            CBEnableEmbedMeta.InvokeIfRequired(() =>
            {
                enableEmbedMeta = CBEnableEmbedMeta.Checked;
            });

            bool enableLiveFromStart = false;

            CBEnableLiveFromStart.InvokeIfRequired(() =>
            {
                enableLiveFromStart = CBEnableLiveFromStart.Checked;
            });

            bool enableWaitForVideo = false;

            CBEnableWaitForVideo.InvokeIfRequired(() =>
            {
                enableWaitForVideo = CBEnableWaitForVideo.Checked;
            });

            bool enableSplitChapters = false;

            CBEnableSplitChapters.InvokeIfRequired(() =>
            {
                enableSplitChapters = CBEnableSplitChapters.Checked;
            });

            bool deleteSourceFile = false;

            CBDeleteSourceFile.InvokeIfRequired(() =>
            {
                deleteSourceFile = CBDeleteSourceFile.Checked;
            });

            bool useHardwareAcceleration = false;

            CBUseHardwareAcceleration.InvokeIfRequired(() =>
            {
                useHardwareAcceleration = CBUseHardwareAcceleration.Checked;
            });

            VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel;

            CBHardwareAcceleratorType.InvokeIfRequired(() =>
            {
                if (!string.IsNullOrEmpty(CBHardwareAcceleratorType.Text))
                {
                    hardwareAcceleratorType = CBHardwareAcceleratorType.Text switch
                    {
                        "Intel" => VariableSet.HardwareAcceleratorType.Intel,
                        "NVIDIA" => VariableSet.HardwareAcceleratorType.NVIDIA,
                        "AMD" => VariableSet.HardwareAcceleratorType.AMD,
                        _ => VariableSet.HardwareAcceleratorType.Intel
                    };
                }
            });

            int deviceNo = 0;

            CBDeviceNo.InvokeIfRequired(() =>
            {
                VideoCardData videoCard = (VideoCardData)CBDeviceNo.SelectedItem;

                deviceNo = videoCard.DeviceNo;
            });

            if (!string.IsNullOrEmpty(url))
            {
                RtbLogCtrlLog.InvokeIfRequired(() =>
                {
                    // 清除 Log。
                    RtbLogCtrlLog.ClearLogs();
                });

                if (SharedDataSource.Count > 0)
                {
                    // 在需要分割影片的狀況時，取消崁入後設資訊。
                    enableEmbedMeta = false;

                    // 判斷是否先載全後分割。
                    if (downloadFirstSplitLater)
                    {
                        OptionSet optionSet = CustomFunction.GetCustomOptionSet(
                            formatSelection,
                            CBEnableLoadCookiesFromBrowser,
                            CBBrowser,
                            TBBrowserProfile,
                            enableEmbedMeta: enableEmbedMeta,
                            enableLiveFromStart: enableLiveFromStart,
                            // TODO: 2022-07-23 預設先吃 enableEmbedMeta 變數的值。
                            enableEmbedSubs: enableEmbedMeta,
                            enableWaitForVideo: enableWaitForVideo,
                            enableSplitChapters: enableSplitChapters);

                        SharedCancellationTokenSource = new();
                        SharedCancellationToken = SharedCancellationTokenSource.Token;

                        await OperationSet.DownloadVideo(
                            url,
                            DgvClipList,
                            SharedDataSource,
                            deleteSourceFile,
                            optionSet,
                            SharedCancellationToken,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo);
                    }
                    else
                    {
                        // 批次下載短片。
                        foreach (ClipData clipData in SharedDataSource)
                        {
                            DataGridViewRow? dgvRow = null;

                            int currentIndex = SharedDataSource.IndexOf(clipData);

                            DgvClipList.InvokeIfRequired(() =>
                            {
                                DataGridViewRow dgvRow = DgvClipList.Rows[currentIndex];

                                // 設定已選擇。
                                dgvRow.Selected = true;

                                // 設定指示器的位置。
                                DgvClipList.CurrentCell = dgvRow.Cells[0];
                                DgvClipList.FirstDisplayedScrollingRowIndex = dgvRow.Index;
                            });

                            string clipName = clipData.ClipName;

                            if (string.IsNullOrEmpty(clipName))
                            {
                                clipName = $"{clipData.ClipNo:#000}.";
                            }
                            else
                            {
                                clipName = $"{clipData.ClipNo:#000}.{clipName}";
                            }

                            OptionSet optionSet = CustomFunction.GetCustomOptionSet(
                                formatSelection,
                                CBEnableLoadCookiesFromBrowser,
                                CBBrowser,
                                TBBrowserProfile,
                                clipData.StartSeconds,
                                clipData.EndSeconds,
                                enableEmbedMeta: enableEmbedMeta,
                                enableLiveFromStart: enableLiveFromStart,
                                // TODO: 2022-07-23 預設先吃 enableEmbedMeta 變數的值。
                                enableEmbedSubs: enableEmbedMeta,
                                enableWaitForVideo: enableWaitForVideo,
                                // 此處永遠不啟用此參數。
                                enableSplitChapters: false);

                            SharedCancellationTokenSource = new();
                            SharedCancellationToken = SharedCancellationTokenSource.Token;

                            await OperationSet.DownloadVideo(
                                url,
                                clipName,
                                clipData.StartSeconds,
                                clipData.EndSeconds,
                                deleteSourceFile,
                                optionSet,
                                SharedCancellationToken,
                                useHardwareAcceleration,
                                hardwareAcceleratorType,
                                deviceNo,
                                clipData.IsAudioOnly);

                            DgvClipList.InvokeIfRequired(() =>
                            {
                                if (currentIndex == DgvClipList.Rows.Count - 1)
                                {
                                    MessageBox.Show("作業完成。",
                                        Text,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                }

                                if (dgvRow != null)
                                {
                                    // 取消選擇狀態。
                                    dgvRow.Selected = false;
                                }
                            });
                        }
                    }
                }
                else
                {
                    string clipName = string.Empty;

                    TBClipName.InvokeIfRequired(() =>
                    {
                        clipName = TBClipName.Text;
                    });

                    OptionSet optionSet = CustomFunction.GetCustomOptionSet(
                        formatSelection,
                        CBEnableLoadCookiesFromBrowser,
                        CBBrowser,
                        TBBrowserProfile,
                        ImportUtil.GetStartSeconds(TBStartSeconds, MTBStartTime),
                        ImportUtil.GetEndSeconds(TBEndSeconds, MTBEndtime),
                        enableEmbedMeta: enableEmbedMeta,
                        enableLiveFromStart: enableLiveFromStart,
                        // TODO: 2022-07-23 預設先吃 enableEmbedMeta 變數的值。
                        enableEmbedSubs: enableEmbedMeta,
                        enableWaitForVideo: enableWaitForVideo,
                        enableSplitChapters: enableSplitChapters);

                    SharedCancellationTokenSource = new();
                    SharedCancellationToken = SharedCancellationTokenSource.Token;

                    await OperationSet.DownloadVideo(
                        url,
                        clipName,
                        ImportUtil.GetStartSeconds(TBStartSeconds, MTBStartTime),
                        ImportUtil.GetEndSeconds(TBEndSeconds, MTBEndtime),
                        deleteSourceFile,
                        optionSet,
                        SharedCancellationToken,
                        useHardwareAcceleration,
                        hardwareAcceleratorType,
                        deviceNo,
                        isAudioOnly: false);
                }
            }
            else
            {
                MessageBox.Show(
                    "請輸入網址。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);
        }
    }

    private async void BtnSplitLocalVideo_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            if (SharedDataSource.Count > 0)
            {
                string notice = "請注意，若選取的視訊檔案內有包含預覽圖或" +
                    "其後設資料內有包含章節資訊時，分割出來的影片可能會無效、無法播放。";

                DialogResult dialogResult1 = MessageBox.Show(
                    notice,
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (dialogResult1 == DialogResult.OK)
                {
                    OpenFileDialog openFileDialog = new()
                    {
                        Title = "請選擇視訊檔案",
                        Filter = "MPEG-4 Part 14|*.mp4|Matroska|*.mkv",
                        FilterIndex = 1
                    };

                    DialogResult dialogResult2 = openFileDialog.ShowDialog();

                    if (dialogResult2 == DialogResult.OK)
                    {
                        string fileName = openFileDialog.FileName;

                        bool useHardwareAcceleration = false;

                        CBUseHardwareAcceleration.InvokeIfRequired(() =>
                        {
                            useHardwareAcceleration = CBUseHardwareAcceleration.Checked;
                        });

                        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel;

                        CBHardwareAcceleratorType.InvokeIfRequired(() =>
                        {
                            if (!string.IsNullOrEmpty(CBHardwareAcceleratorType.Text))
                            {
                                hardwareAcceleratorType = CBHardwareAcceleratorType.Text switch
                                {
                                    "Intel" => VariableSet.HardwareAcceleratorType.Intel,
                                    "NVIDIA" => VariableSet.HardwareAcceleratorType.NVIDIA,
                                    "AMD" => VariableSet.HardwareAcceleratorType.AMD,
                                    _ => VariableSet.HardwareAcceleratorType.Intel
                                };
                            }
                        });

                        int deviceNo = 0;

                        CBDeviceNo.InvokeIfRequired(() =>
                        {
                            VideoCardData videoCard = (VideoCardData)CBDeviceNo.SelectedItem;

                            deviceNo = videoCard.DeviceNo;
                        });

                        RtbLogCtrlLog.InvokeIfRequired(() =>
                        {
                            // 清除 Log。
                            RtbLogCtrlLog.ClearLogs();
                        });

                        // 批次分割短片。
                        foreach (ClipData clipData in SharedDataSource)
                        {
                            DataGridViewRow? dgvRow = null;

                            int currentIndex = SharedDataSource.IndexOf(clipData);

                            DgvClipList.InvokeIfRequired(() =>
                            {
                                DataGridViewRow dgvRow = DgvClipList.Rows[currentIndex];

                                // 設定已選擇。
                                dgvRow.Selected = true;

                                // 設定指示器的位置。
                                DgvClipList.CurrentCell = dgvRow.Cells[0];
                                DgvClipList.FirstDisplayedScrollingRowIndex = dgvRow.Index;
                            });

                            SharedCancellationTokenSource = new();
                            SharedCancellationToken = SharedCancellationTokenSource.Token;

                            await OperationSet.SplitVideo(
                                clipData,
                                fileName,
                                SharedCancellationToken,
                                useHardwareAcceleration,
                                hardwareAcceleratorType,
                                deviceNo,
                                clipData.IsAudioOnly);

                            DgvClipList.InvokeIfRequired(() =>
                            {
                                if (currentIndex == DgvClipList.Rows.Count - 1)
                                {
                                    MessageBox.Show("作業完成。",
                                        Text,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                }

                                if (dgvRow != null)
                                {
                                    // 取消選擇狀態。
                                    dgvRow.Selected = false;
                                }
                            });
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "短片清單是空的，請先新增短片。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);
        }
    }

    private async void BtnBurnIn_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            string? videoFilePath = string.Empty,
                subtitleFilePath = string.Empty,
                subtitleStyle = string.Empty,
                subtitleEncode = string.Empty;

            bool applyFontSetting = false,
                useHardwareAcceleration = false;

            OpenFileDialog openFileDialog1 = new()
            {
                Title = "請選擇視訊檔案",
                Filter = "MPEG-4 Part 14|*.mp4|Matroska|*.mkv",
                FilterIndex = 1
            };

            DialogResult dialogResult1 = openFileDialog1.ShowDialog();

            if (dialogResult1 == DialogResult.OK)
            {
                // 設定視訊檔案的路徑。
                videoFilePath = openFileDialog1.FileName;

                OpenFileDialog openFileDialog2 = new()
                {
                    Title = "請選擇字幕檔案",
                    Filter = "SupRip Text|*.srt|WebVTT|*.vtt|SubStation Alpha|*.ssa|Advanced SubStation Alpha|*.ass",
                    FilterIndex = 1
                };

                DialogResult dialogResult2 = openFileDialog2.ShowDialog();

                if (dialogResult2 == DialogResult.OK)
                {
                    // 設定字幕檔案的路徑。
                    subtitleFilePath = openFileDialog2.FileName;

                    CBApplyFontSetting.InvokeIfRequired(() =>
                    {
                        applyFontSetting = CBApplyFontSetting.Checked;
                    });

                    CBFont.InvokeIfRequired(() =>
                    {
                        subtitleStyle = $"FontName='{CBFont.SelectedItem}'";
                    });

                    TBCustomFont.InvokeIfRequired(() =>
                    {
                        if (!string.IsNullOrEmpty(TBCustomFont.Text))
                        {
                            subtitleStyle = $"FontName='{TBCustomFont.Text}'";
                        }
                    });

                    CBEncode.InvokeIfRequired(() =>
                    {
                        subtitleEncode = CBEncode.SelectedItem.ToString();
                    });

                    TBCustomEncode.InvokeIfRequired(() =>
                    {
                        if (!string.IsNullOrEmpty(TBCustomEncode.Text))
                        {
                            subtitleEncode = TBCustomEncode.Text;
                        }
                    });

                    CBUseHardwareAcceleration.InvokeIfRequired(() =>
                    {
                        useHardwareAcceleration = CBUseHardwareAcceleration.Checked;
                    });

                    VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel;

                    CBHardwareAcceleratorType.InvokeIfRequired(() =>
                    {
                        if (!string.IsNullOrEmpty(CBHardwareAcceleratorType.Text))
                        {
                            hardwareAcceleratorType = CBHardwareAcceleratorType.Text switch
                            {
                                "Intel" => VariableSet.HardwareAcceleratorType.Intel,
                                "NVIDIA" => VariableSet.HardwareAcceleratorType.NVIDIA,
                                "AMD" => VariableSet.HardwareAcceleratorType.AMD,
                                _ => VariableSet.HardwareAcceleratorType.Intel
                            };
                        }
                    });

                    VideoCardData videoCard = new();

                    CBDeviceNo.InvokeIfRequired(() =>
                    {
                        videoCard = (VideoCardData)CBDeviceNo.SelectedItem;
                    });

                    int deviceNo = videoCard.DeviceNo;

                    string videoExtName = Path.GetExtension(videoFilePath);
                    string videoFileName = Path.GetFileName(videoFilePath);
                    string videoFileNameNoExt = Path.GetFileNameWithoutExtension(videoFilePath);
                    string videoNewFileName = $"{videoFileNameNoExt}_Subtitled{videoExtName}";
                    string outputPath = Path.GetFullPath(videoFilePath).Replace(videoFileName, videoNewFileName);

                    // 判斷選擇的影片的副檔名。
                    if (videoExtName != ".mp4" && videoExtName != ".mkv")
                    {
                        BtnCancel_Click(sender, e);

                        MessageBox.Show(
                            "您選擇的視訊檔案為不支援的格式。",
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }

                    SharedCancellationTokenSource = new();
                    SharedCancellationToken = SharedCancellationTokenSource.Token;

                    await OperationSet.BurnInSubtitle(
                        videoFilePath,
                        subtitleFilePath,
                        outputPath,
                        SharedCancellationToken,
                        subtitleEncode,
                        applyFontSetting,
                        subtitleStyle,
                        useHardwareAcceleration,
                        hardwareAcceleratorType,
                        deviceNo);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);
        }
    }

    private async void BtnUpdateYtDlp_Click(object? sender, EventArgs? e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            // 因需求而手動禁用。
            BtnCancel.InvokeIfRequired(() =>
            {
                BtnCancel.Enabled = false;
            });

            RtbLogCtrlLog.InvokeIfRequired(() =>
            {
                // 清除 Log。
                RtbLogCtrlLog.ClearLogs();
            });

            YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

            string result = await ytdl.RunUpdate();

            SerilogUtil.WriteLog(result);

            // 當 yt-dlp 更新後，才需要提示要更新 FFmpeg。
            if (result.Contains("Updated yt-dlp to version"))
            {
                SerilogUtil.WriteLog("請手動點選 yt-dlp/FFmpeg-Builds 按鈕以更新 FFmpeg。");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);

            // 更新顯示版本號。
            ExternalProgram.SetYtDlpVer(LVersion, LYtDlpVer);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs? e)
    {
        try
        {
            if (SharedCancellationTokenSource != null &&
                !SharedCancellationTokenSource.IsCancellationRequested)
            {
                SharedCancellationTokenSource.Cancel();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CBDownloadFirstSplitLater_CheckedChanged(object sender, EventArgs e)
    {
        CBDownloadFirstSplitLater.InvokeIfRequired(() =>
        {
            bool value = CBDownloadFirstSplitLater.Checked;

            if (value != Properties.Settings.Default.DownloadFirstSplitLater)
            {
                Properties.Settings.Default.DownloadFirstSplitLater = value;
                Properties.Settings.Default.Save();
            }

            if (!IsInitialing && !value)
            {
                string message = "注意！若此項目未被勾選，所下載的短片視訊檔案" +
                    "容易會有時間點不在關鍵幀（I-frame）上，導致影片會有非預期內容的問題。";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void CBDeleteSourceFile_CheckedChanged(object sender, EventArgs e)
    {
        CBDeleteSourceFile.InvokeIfRequired(() =>
        {
            bool value = CBDeleteSourceFile.Checked;

            if (value != Properties.Settings.Default.DeleteSourceFile)
            {
                Properties.Settings.Default.DeleteSourceFile = value;
                Properties.Settings.Default.Save();
            }
        });
    }

    private void CBEnableEmbedMeta_CheckedChanged(object sender, EventArgs e)
    {
        CBEnableEmbedMeta.InvokeIfRequired(() =>
        {
            bool value = CBEnableEmbedMeta.Checked;

            if (value != Properties.Settings.Default.EnableEmbedMetaData)
            {
                Properties.Settings.Default.EnableEmbedMetaData = value;
                Properties.Settings.Default.Save();
            }

            if (!IsInitialing && value)
            {
                string message = "注意！此設定只會套用於「直接」下載，其他類型的下載皆不會套用此設定值。";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void CBEnableLiveFromStart_CheckedChanged(object sender, EventArgs e)
    {
        CBEnableLiveFromStart.InvokeIfRequired(() =>
        {
            bool value = CBEnableLiveFromStart.Checked;

            if (value != Properties.Settings.Default.EnableLiveFromStart)
            {
                Properties.Settings.Default.EnableLiveFromStart = value;
                Properties.Settings.Default.Save();
            }

            if (!IsInitialing && value)
            {
                string message = "注意！此設定只適用於下載「正在直播中」的影片。";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void CBEnableWaitForVideo_CheckedChanged(object sender, EventArgs e)
    {
        CBEnableWaitForVideo.InvokeIfRequired(() =>
        {
            bool value = CBEnableWaitForVideo.Checked;

            if (value != Properties.Settings.Default.EnableWaitForVideo)
            {
                Properties.Settings.Default.EnableWaitForVideo = value;
                Properties.Settings.Default.Save();
            }

            if (!IsInitialing && value)
            {
                string message = "注意！不建議對已播放的影片使用此設定，可能會發生非預期的行為。";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void CBEnableSplitChapters_CheckedChanged(object sender, EventArgs e)
    {
        CBEnableSplitChapters.InvokeIfRequired(() =>
        {
            bool value = CBEnableSplitChapters.Checked;

            if (value != Properties.Settings.Default.EnableSplitChapters)
            {
                Properties.Settings.Default.EnableSplitChapters = value;
                Properties.Settings.Default.Save();
            }

            if (!IsInitialing && value)
            {
                string message = "注意！此設定不會影響需要分割影片、修正影片秒數的下載影片功能。";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void CBUseHardwareAcceleratation_CheckedChanged(object sender, EventArgs e)
    {
        CBUseHardwareAcceleration.InvokeIfRequired(() =>
        {
            bool value = CBUseHardwareAcceleration.Checked;

            if (value != Properties.Settings.Default.UseHardwareAcceleration)
            {
                Properties.Settings.Default.UseHardwareAcceleration = value;
                Properties.Settings.Default.Save();
            }

            if (!IsInitialing && value)
            {
                string message = "注意！不保證可以在所有的裝置上正常使用，" +
                    "請先確認您的裝置符合硬體加速的需求後，再使用此功能。";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void CBHardwareAcceleratorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CBHardwareAcceleratorType.InvokeIfRequired(() =>
        {
            CBDeviceNo.InvokeIfRequired(() =>
            {
                string? value = CBHardwareAcceleratorType.SelectedItem.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    foreach (VideoCardData videoCard in CBDeviceNo.Items)
                    {
                        if (!string.IsNullOrEmpty(videoCard.DeviceName) &&
                            videoCard.DeviceName.Contains(value))
                        {
                            CBDeviceNo.SelectedItem = videoCard;

                            break;
                        }
                    }
                }

                if (!IsInitialing && value != Properties.Settings.Default.HardwareAcceleratorType)
                {
                    Properties.Settings.Default.HardwareAcceleratorType = value;
                    Properties.Settings.Default.Save();
                }
            });
        });
    }

    private void CBDeviceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CBDeviceNo.InvokeIfRequired(() =>
        {
            int value = CBDeviceNo.SelectedIndex;

            CBHardwareAcceleratorType.InvokeIfRequired(() =>
            {
                if (CBDeviceNo.SelectedItem is VideoCardData videoCard)
                {
                    foreach (string hardwareAcceleratorType in CBHardwareAcceleratorType.Items)
                    {
                        if (!string.IsNullOrEmpty(videoCard.DeviceName) &&
                            videoCard.DeviceName.Contains(hardwareAcceleratorType))
                        {
                            CBHardwareAcceleratorType.Text = hardwareAcceleratorType;

                            break;
                        }
                    }
                }
            });

            if (!IsInitialing && value != Properties.Settings.Default.DeviceIndex)
            {
                Properties.Settings.Default.DeviceIndex = value;
                Properties.Settings.Default.Save();
            }
        });
    }


    private void BtnOpenBinFolder_Click(object sender, EventArgs e)
    {
        try
        {
            // 來源：https://stackoverflow.com/a/1132559
            Process.Start(new ProcessStartInfo()
            {
                FileName = Path.Combine(VariableSet.ExecPath + Path.DirectorySeparatorChar),
                UseShellExecute = true,
                Verb = "Open"
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnOpenDownloadFolder_Click(object sender, EventArgs e)
    {
        try
        {
            // 來源：https://stackoverflow.com/a/1132559
            Process.Start(new ProcessStartInfo()
            {
                FileName = Path.Combine(VariableSet.VideoDownloadFolderPath + Path.DirectorySeparatorChar),
                UseShellExecute = true,
                Verb = "Open"
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnOpenLogsFolder_Click(object sender, EventArgs e)
    {
        try
        {
            if (Directory.Exists(VariableSet.BaseLogPath))
            {
                // 來源：https://stackoverflow.com/a/1132559
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Path.Combine(VariableSet.BaseLogPath),
                    UseShellExecute = true,
                    Verb = "Open"
                });
            }
            else
            {
                MessageBox.Show(
                    "Logs 資料夾尚未存在，請先等應用程式儲存紀錄後再使用此功能。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnOpenConfigFolder_Click(object sender, EventArgs e)
    {
        try
        {
            // 來源：https://stackoverflow.com/a/7069366
            string configFilePath = ConfigurationManager
                .OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
                .FilePath,
                fileName = Path.GetFileName(configFilePath),
                folderPath = Path.GetFullPath(configFilePath)
                .Replace(fileName, string.Empty);

            if (Directory.Exists(folderPath))
            {
                // 來源：https://stackoverflow.com/a/1132559
                Process.Start(new ProcessStartInfo()
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "Open"
                });
            }
            else
            {
                MessageBox.Show(
                    "設定檔資料夾尚未存在，請先調整選項後再使用此功能。",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnExportLog_Click(object sender, EventArgs e)
    {
        try
        {
            RtbLogCtrlLog.InvokeIfRequired(() =>
            {
                if (RtbLogCtrlLog.TextLength > 0)
                {
                    RtbLogCtrlLog.SaveLogToFile();
                }
                else
                {
                    MessageBox.Show(
                        "日誌是空的，無法匯出日誌檔案。",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnClearLog_Click(object sender, EventArgs e)
    {
        try
        {
            RtbLogCtrlLog.InvokeIfRequired(() =>
            {
                RtbLogCtrlLog.ClearLogs();
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void BtnDlYtDlp_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            // 因需求而手動禁用。
            BtnCancel.InvokeIfRequired(() =>
            {
                BtnCancel.Enabled = false;
            });

            SharedCancellationTokenSource = new();
            SharedCancellationToken = SharedCancellationTokenSource.Token;

            if (SharedHttpClient != null)
            {
                await ExternalDownloader.DownloadYtDlp(SharedHttpClient, SharedCancellationToken);
            }
            else
            {
                SerilogUtil.WriteLog("httpClient 是 null。", LogEventLevel.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);

            // 更新顯示版本號。
            ExternalProgram.SetYtDlpVer(LVersion, LYtDlpVer);
        }
    }

    private async void BtnDlFFmpeg_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            // 因需求而手動禁用。
            BtnCancel.InvokeIfRequired(() =>
            {
                BtnCancel.Enabled = false;
            });

            SharedCancellationTokenSource = new();
            SharedCancellationToken = SharedCancellationTokenSource.Token;

            if (SharedHttpClient != null)
            {
                await ExternalDownloader.DownloadFFmpeg(SharedHttpClient, SharedCancellationToken);
            }
            else
            {
                SerilogUtil.WriteLog("httpClient 是 null。", LogEventLevel.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);
        }
    }

    private async void BtnDlAria2_Click(object sender, EventArgs e)
    {
        try
        {
            // 設定控制項。
            SetControl(false);

            // 因需求而手動禁用。
            BtnCancel.InvokeIfRequired(() =>
            {
                BtnCancel.Enabled = false;
            });

            SharedCancellationTokenSource = new();
            SharedCancellationToken = SharedCancellationTokenSource.Token;

            if (SharedHttpClient != null)
            {
                await ExternalDownloader.DownloadAria2(SharedHttpClient, SharedCancellationToken);
            }
            else
            {
                SerilogUtil.WriteLog("httpClient 是 null。", LogEventLevel.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 設定控制項。
            SetControl(true);
        }
    }

    private void LLYtDlp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/yt-dlp/yt-dlp");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LLYtDlpFFmpeg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/yt-dlp/FFmpeg-Builds");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LLAria2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/aria2/aria2");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LLYouTube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://www.youtube.com/");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LLTwitch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://www.twitch.tv/");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LLbilibili_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://www.bilibili.com/");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LLTwitCasting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://twitcasting.tv/");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnImportTimestamp_Click(object sender, EventArgs e)
    {
        try
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "請選擇時間標記文字檔案、播放清單檔",
                Filter = "時間標記文字檔案（*.txt）|*.txt|時間標記播放清單檔案、秒數播放清單檔案（*.json）|*.json|JSON 檔案（含備註）|*.jsonc",
                FilterIndex = 1
            };

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(filePath))
                {
                    DoImportFile(filePath);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnClearClipList_Click(object sender, EventArgs e)
    {
        try
        {
            DgvClipList.InvokeIfRequired(() =>
            {
                DgvClipList.Rows.Clear();
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void TBAutoEndSeconds_TextChanged(object sender, EventArgs e)
    {
        TBAutoEndSeconds.InvokeIfRequired(() =>
        {
            if (!string.IsNullOrEmpty(TBAutoEndSeconds.Text))
            {
                if (!double.TryParse(TBAutoEndSeconds.Text, out double parseResult))
                {
                    MessageBox.Show(
                        "請輸入有效的數值。",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    if (parseResult == 0.0)
                    {
                        MessageBox.Show(
                            "數值必須大於 0.0。",
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        });
    }

    private void DgvClipList_DragEnter(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void DgvClipList_DragDrop(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] allowedExts =
                {
                    ".txt",
                    ".json",
                    ".jsonc"
                };

                List<string>? filePathList = ((string[]?)e.Data.GetData(DataFormats.FileDrop))
                    ?.Where(n => allowedExts.Contains(Path.GetExtension(n)))
                    .ToList();

                if (filePathList != null)
                {
                    if (filePathList.Count == 0)
                    {
                        MessageBox.Show(
                            "請選擇時間標記文字檔案或播放清單檔案。",
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }

                    foreach (string filePath in filePathList)
                    {
                        DoImportFile(filePath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void DgvClipList_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        if (e.Exception != null)
        {
            string exceptionMsg;

            if (!string.IsNullOrEmpty(e.Exception.Message))
            {
                exceptionMsg = e.Exception.Message;
            }
            else
            {
                exceptionMsg = e.Exception.ToString();
            }

            if (!string.IsNullOrEmpty(exceptionMsg))
            {
                DgvClipList.CancelEdit();

                MessageBox.Show(
                    exceptionMsg,
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private void DgvClipList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        // 避免新列觸發驗證事件。
        if (DgvClipList.Rows[e.RowIndex].IsNewRow) { return; }

        DataGridViewColumn dgvColumn = DgvClipList.Columns[e.ColumnIndex];

        if (e.FormattedValue != null)
        {
            if (dgvColumn.Name == nameof(ClipData.ClipNo))
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out int clipNo) || clipNo < 0)
                {
                    e.Cancel = true;

                    MessageBox.Show("請輸入有效的短片編號數值。",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else if (dgvColumn.Name == nameof(ClipData.StartSeconds))
            {
                if (!double.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;

                    MessageBox.Show($"請輸入有效的開始秒數數值。{Environment.NewLine}格式：0.0",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else if (dgvColumn.Name == nameof(ClipData.EndSeconds))
            {
                if (!double.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;

                    MessageBox.Show($"請輸入有效的結束秒數數值。{Environment.NewLine}格式：0.0",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                // 不進行任何處理。
            }
        }
    }

    private void BtnOpenSubtitleCreator_Click(object sender, EventArgs e)
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets\\SubtitleCreator.html");

            CustomFunction.OpenUrl(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnOpenYTSecConverter_Click(object sender, EventArgs e)
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets\\YTSecConverter.html");

            CustomFunction.OpenUrl(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}