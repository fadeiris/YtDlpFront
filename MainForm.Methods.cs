using YtDlpFront.Common.Utils;
using YtDlpFront.Common;
using YtDlpFront.Extensions;
using YtDlpFront.Models;
using System.Reflection;
using System.Drawing.Text;
using System.Text.Json;
using Serilog.Events;
using YtDlpFront.Common.Sets;

namespace YtDlpFront;

// 阻擋設計工具。
partial class DesignerBlocker { }

public partial class MainForm
{
    /// <summary>
    /// 自定義初始化
    /// </summary>
    private async void CustomInit()
    {
        try
        {
            IsInitialing = true;

            // 設定應用程式的名稱。
            Text = VariableSet.AppName;

            // 設應用程式的圖示。
            Icon = Properties.Resources.app_icon;

            InitFontComboBox();
            InitEncodeComboBox();

            // 設定控制項。
            SetControl(false);

            // 因需求而手動禁用。
            BtnCancel.InvokeIfRequired(() =>
            {
                BtnCancel.Enabled = false;
            });

            // 設定控制項的 Tooltip 資訊。
            SharedToolTip.SetToolTip(TBClipName, "當網址為播放清單的網址時，請將此處留空");
            SharedToolTip.SetToolTip(TBStartSeconds, "開始時間的秒數值");
            SharedToolTip.SetToolTip(MTBStartTime, "開始時間的時間標記值");
            SharedToolTip.SetToolTip(TBEndSeconds, "結束時間的秒數值");
            SharedToolTip.SetToolTip(MTBEndtime, "結束時間的時間標記值");
            SharedToolTip.SetToolTip(CBBrowser, "網頁瀏覽器");
            SharedToolTip.SetToolTip(TBBrowserProfile, "網頁瀏覽器的使用者設定資料夾名稱");
            SharedToolTip.SetToolTip(BtnFetchVideoInfo, "獲取影片的資訊");
            SharedToolTip.SetToolTip(BtnDownloadVideo, "下載影片");
            SharedToolTip.SetToolTip(BtnUpdateYtDlp, "更新 yt-dlp（執行 yt-dlp.exe -U）");
            SharedToolTip.SetToolTip(BtnSplitLocalVideo, "分割本機的視訊檔案");
            SharedToolTip.SetToolTip(BtnBurnIn, "將指定的字幕檔案燒錄至指定的本機視訊檔案");
            SharedToolTip.SetToolTip(BtnCancel, "取消作業");
            SharedToolTip.SetToolTip(CBDownloadFirstSplitLater, "先下載完整的視訊檔案後再分割視訊檔案");
            SharedToolTip.SetToolTip(CBDeleteSourceFile, "在「下載」作業結束後刪除原始的視訊檔案");

            string appendTextForEnableEmbedSubs = string.Empty;

            if (Properties.Settings.Default.EnableEmbedSubs)
            {
                appendTextForEnableEmbedSubs = "以及字幕";
            }

            SharedToolTip.SetToolTip(CBEnableEmbedMeta, $"在下載的視訊檔案內嵌入後設資料、預覽圖{appendTextForEnableEmbedSubs}");
            SharedToolTip.SetToolTip(
                CBEnableLiveFromStart,
                "勾選此選項，以讓 yt-dlp 使用 --live-from-start 參數；從頭開始下載串流影片，目前僅支援 YouTube 網站（實驗性質）");
            SharedToolTip.SetToolTip(
                CBEnableWaitForVideo,
                $"勾選此選項，以讓 yt-dlp 使用 --wait-for-video 參數；預設是等待 {Properties.Settings.Default.WaitSeconds} 秒後自動重試一次");
            SharedToolTip.SetToolTip(CBEnableSplitChapters, $"勾選此選項，以讓 yt-dlp 使用 --split-chapters 參數");
            SharedToolTip.SetToolTip(GBSubtitleOption, "此處設定只會影響非 *.mkv 格式的檔案");
            SharedToolTip.SetToolTip(CBApplyFontSetting, "勾選以啟用字型設定，否則會讓 FFmpeg 根據字幕檔的字型設定來選擇要使用的字型");
            SharedToolTip.SetToolTip(TBCustomFont, "在此欄位輸入值，會覆蓋在下拉清單中所選擇的項目");
            SharedToolTip.SetToolTip(TBCustomEncode, "在此欄位輸入值，會覆蓋在下拉清單中所選擇的項目");
            SharedToolTip.SetToolTip(CBUseHardwareAcceleration, "使用硬體加速");
            SharedToolTip.SetToolTip(CBHardwareAcceleratorType, "硬體加速類型");
            SharedToolTip.SetToolTip(CBDeviceNo, "GPU 裝置");
            SharedToolTip.SetToolTip(BtnDlYtDlp, "下載 yt-dlp 的執行檔");
            SharedToolTip.SetToolTip(BtnDlFFmpeg, "下載 FFmpeg 的執行檔");
            SharedToolTip.SetToolTip(BtnDlAria2, "下載 aria2 的執行檔");
            SharedToolTip.SetToolTip(BtnOpenBinFolder, "開啟 Bin 資料夾");
            SharedToolTip.SetToolTip(BtnOpenDownloadFolder, "開啟下載資料夾");
            SharedToolTip.SetToolTip(BtnOpenConfigFolder, "開啟設定檔資料夾");
            SharedToolTip.SetToolTip(BtnOpenLogsFolder, "開啟 Logs 資料夾");
            SharedToolTip.SetToolTip(LLYtDlp, "開啟 yt-dlp 的 GitHub 頁面");
            SharedToolTip.SetToolTip(LLYtDlpFFmpeg, "開啟 yt-dlp/FFmpeg-Builds 的 GitHub 頁面");
            SharedToolTip.SetToolTip(LLAria2, "開啟 aria2 的 GitHub 頁面");
            SharedToolTip.SetToolTip(LLYouTube, "開啟 YouTube 的頁面");
            SharedToolTip.SetToolTip(LLTwitch, "開啟 Twitch 的頁面");
            SharedToolTip.SetToolTip(LLbilibili, "開啟 bilibili 的頁面");
            SharedToolTip.SetToolTip(LLTwitCasting, "開啟 TwitCasting 的頁面");
            SharedToolTip.SetToolTip(TBAutoEndSeconds, "自動設定結束秒數的秒數值");

            // 設定預設資料。
            TBUrl.InvokeIfRequired(() =>
            {
                TBUrl.PlaceholderText = "（影片的網址）";

                // 預設選中此欄位。
                TBUrl.Focus();
            });

            TBClipName.InvokeIfRequired(() =>
            {
                TBClipName.PlaceholderText = "（可以留空）";
            });

            TBFormatSelection.InvokeIfRequired(() =>
            {
                TBFormatSelection.PlaceholderText = "（預設：b/bv*+ba）";

                string value = Properties.Settings.Default.FormatSelection;

                if (!string.IsNullOrEmpty(value))
                {
                    TBFormatSelection.Text = value;
                }
                else
                {
                    TBFormatSelection.Text = "b/bv*+ba";
                }
            });

            TBStartSeconds.InvokeIfRequired(() =>
            {
                TBStartSeconds.PlaceholderText = "（秒數值）";
            });

            TBEndSeconds.InvokeIfRequired(() =>
            {
                TBEndSeconds.PlaceholderText = "（秒數值）";
            });

            CBBrowser.InvokeIfRequired(() =>
            {
                CBBrowser.Text = Properties.Settings.Default.Browser;
            });

            TBBrowserProfile.InvokeIfRequired(() =>
            {
                TBBrowserProfile.PlaceholderText = "（可以留空）";
                TBBrowserProfile.Text = Properties.Settings.Default.BrowserProfile;
            });

            CBDownloadFirstSplitLater.InvokeIfRequired(() =>
            {
                CBDownloadFirstSplitLater.Checked = Properties.Settings.Default.DownloadFirstSplitLater;
            });

            CBDeleteSourceFile.InvokeIfRequired(() =>
            {
                CBDeleteSourceFile.Checked = Properties.Settings.Default.DeleteSourceFile;
            });

            CBEnableEmbedMeta.InvokeIfRequired(() =>
            {
                CBEnableEmbedMeta.Checked = Properties.Settings.Default.EnableEmbedMetaData;
            });

            CBEnableLiveFromStart.InvokeIfRequired(() =>
            {
                CBEnableLiveFromStart.Checked = Properties.Settings.Default.EnableLiveFromStart;
            });

            CBEnableWaitForVideo.InvokeIfRequired(() =>
            {
                CBEnableWaitForVideo.Checked = Properties.Settings.Default.EnableWaitForVideo;
            });

            CBEnableSplitChapters.InvokeIfRequired(() =>
            {
                CBEnableSplitChapters.Checked = Properties.Settings.Default.EnableSplitChapters;
            });

            CBUseHardwareAcceleration.InvokeIfRequired(() =>
            {
                CBUseHardwareAcceleration.Checked = Properties.Settings.Default.UseHardwareAcceleration;
            });

            CBDeviceNo.InvokeIfRequired(() =>
            {
                CBDeviceNo.DataSource = VideoCardUtil.GetDeviceList();
                CBDeviceNo.DisplayMember = "DeviceName";
                CBDeviceNo.ValueMember = "DeviceNo";

                CBDeviceNo.SelectedIndex = Properties.Settings.Default.DeviceIndex;

                CBHardwareAcceleratorType.InvokeIfRequired(() =>
                {
                    if (CBDeviceNo.SelectedItem is VideoCardData videoCard)
                    {
                        if (string.IsNullOrEmpty(Properties.Settings.Default.HardwareAcceleratorType))
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
                        else
                        {
                            CBHardwareAcceleratorType.Text = Properties.Settings.Default.HardwareAcceleratorType;
                        }
                    }
                    else
                    {
                        CBHardwareAcceleratorType.Text = "Intel";
                    }
                });
            });

            TBAutoEndSeconds.InvokeIfRequired(() =>
            {
                TBAutoEndSeconds.PlaceholderText = $"自動結束秒數（預設：{VariableSet.AppendSeconds} 秒）";
            });

            DgvClipList.InvokeIfRequired(() =>
            {
                DgvClipList.DataSource = SharedDataSource;
            });

            // 設定顯示應用程式的版本號。
            LVersion.InvokeIfRequired(() =>
            {
                Version? version = Assembly.GetEntryAssembly()?.GetName().Version;

                if (version != null)
                {
                    LVersion.Text = $"版本：{version}";
                }
                else
                {
                    LVersion.Text = $"版本：無";
                }
            });

            // 初始化 SharedCancellationTokenSource、SharedCancellationToken。
            SharedCancellationTokenSource = new();
            SharedCancellationToken = SharedCancellationTokenSource.Token;

            // 檢查 yt-dlp 的相依性檔案及資料夾是否已存在。
            await ExternalProgram.CheckYtDlpDeps(
                SharedHttpClient!,
                LVersion,
                LYtDlpVer,
                SharedCancellationToken);
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

            IsInitialing = false;
        }
    }

    /// <summary>
    /// 初始化字型的 ComboBox
    /// </summary>
    private void InitFontComboBox()
    {
        CBFont.InvokeIfRequired(() =>
        {
            using InstalledFontCollection installedFontCollection = new();

            List<string> dataSet = new();

            for (int i = 0; i < installedFontCollection.Families.Length; i++)
            {
                dataSet.Add(installedFontCollection.Families[i].Name);
            }

            CBFont.DataSource = dataSet;
            CBFont.SelectedItem = "微軟正黑體";
        });
    }

    /// <summary>
    /// 初始化文字編碼的 ComboBox
    /// </summary>
    private void InitEncodeComboBox()
    {
        CBEncode.InvokeIfRequired(() =>
        {
            // 資料來源：https://trac.ffmpeg.org/attachment/ticket/2431/sub_charenc_parameters.txt
            CBEncode.SelectedItem = "UTF-8";
        });
    }

    /// <summary>
    /// 設定控制項
    /// </summary>
    /// <param name="enable">布林值，啟用，預設值為 true</param>
    private void SetControl(bool enable = true)
    {
        try
        {
            TBUrl.InvokeIfRequired(() =>
            {
                TBUrl.Enabled = enable;
            });

            TBClipName.InvokeIfRequired(() =>
            {
                TBClipName.Enabled = enable;
            });

            TBFormatSelection.InvokeIfRequired(() =>
            {
                TBFormatSelection.Enabled = enable;
            });

            TBStartSeconds.InvokeIfRequired(() =>
            {
                TBStartSeconds.Enabled = enable;
            });

            MTBStartTime.InvokeIfRequired(() =>
            {
                MTBStartTime.Enabled = enable;
            });

            TBEndSeconds.InvokeIfRequired(() =>
            {
                TBEndSeconds.Enabled = enable;
            });

            MTBEndtime.InvokeIfRequired(() =>
            {
                MTBEndtime.Enabled = enable;
            });

            BtnAddNewClip.InvokeIfRequired(() =>
            {
                BtnAddNewClip.Enabled = enable;
            });

            BtnResetNewClip.InvokeIfRequired(() =>
            {
                BtnResetNewClip.Enabled = enable;
            });

            CBIsAudioOnly.InvokeIfRequired(() =>
            {
                CBIsAudioOnly.Enabled = enable;
            });

            CBBrowser.InvokeIfRequired(() =>
            {
                CBBrowser.Enabled = enable;
            });

            TBBrowserProfile.InvokeIfRequired(() =>
            {
                TBBrowserProfile.Enabled = enable;
            });

            CBEnableLoadCookiesFromBrowser.InvokeIfRequired(() =>
            {
                CBEnableLoadCookiesFromBrowser.Enabled = enable;
            });

            BtnFetchVideoInfo.InvokeIfRequired(() =>
            {
                BtnFetchVideoInfo.Enabled = enable;
            });

            BtnDownloadVideo.InvokeIfRequired(() =>
            {
                BtnDownloadVideo.Enabled = enable;
            });

            BtnUpdateYtDlp.InvokeIfRequired(() =>
            {
                BtnUpdateYtDlp.Enabled = enable;
            });

            BtnSplitLocalVideo.InvokeIfRequired(() =>
            {
                BtnSplitLocalVideo.Enabled = enable;
            });

            BtnBurnIn.InvokeIfRequired(() =>
            {
                BtnBurnIn.Enabled = enable;
            });

            BtnCancel.InvokeIfRequired(() =>
            {
                BtnCancel.Enabled = !enable;
            });

            CBDownloadFirstSplitLater.InvokeIfRequired(() =>
            {
                CBDownloadFirstSplitLater.Enabled = enable;
            });

            CBDeleteSourceFile.InvokeIfRequired(() =>
            {
                CBDeleteSourceFile.Enabled = enable;
            });

            CBEnableEmbedMeta.InvokeIfRequired(() =>
            {
                CBEnableEmbedMeta.Enabled = enable;
            });

            CBEnableLiveFromStart.InvokeIfRequired(() =>
            {
                CBEnableLiveFromStart.Enabled = enable;
            });

            CBEnableWaitForVideo.InvokeIfRequired(() =>
            {
                CBEnableWaitForVideo.Enabled = enable;
            });

            CBEnableSplitChapters.InvokeIfRequired(() =>
            {
                CBEnableSplitChapters.Enabled = enable;
            });

            CBApplyFontSetting.InvokeIfRequired(() =>
            {
                CBApplyFontSetting.Enabled = enable;
            });

            CBFont.InvokeIfRequired(() =>
            {
                CBFont.Enabled = enable;
            });

            TBCustomFont.InvokeIfRequired(() =>
            {
                TBCustomFont.Enabled = enable;
            });

            CBEncode.InvokeIfRequired(() =>
            {
                CBEncode.Enabled = enable;
            });

            TBCustomEncode.InvokeIfRequired(() =>
            {
                TBCustomEncode.Enabled = enable;
            });

            CBUseHardwareAcceleration.InvokeIfRequired(() =>
            {
                CBUseHardwareAcceleration.Enabled = enable;
            });

            CBHardwareAcceleratorType.InvokeIfRequired(() =>
            {
                CBHardwareAcceleratorType.Enabled = enable;
            });

            CBDeviceNo.InvokeIfRequired(() =>
            {
                CBDeviceNo.Enabled = enable;
            });

            BtnDlYtDlp.InvokeIfRequired(() =>
            {
                BtnDlYtDlp.Enabled = enable;
            });

            BtnDlFFmpeg.InvokeIfRequired(() =>
            {
                BtnDlFFmpeg.Enabled = enable;
            });

            BtnDlAria2.InvokeIfRequired(() =>
            {
                BtnDlAria2.Enabled = enable;
            });

            BtnImportTimestamp.InvokeIfRequired(() =>
            {
                BtnImportTimestamp.Enabled = enable;
            });

            BtnClearClipList.InvokeIfRequired(() =>
            {
                BtnClearClipList.Enabled = enable;
            });

            TBAutoEndSeconds.InvokeIfRequired(() =>
            {
                TBAutoEndSeconds.Enabled = enable;
            });

            DgvClipList.InvokeIfRequired(() =>
            {
                DgvClipList.ReadOnly = !enable;
                DgvClipList.AllowUserToAddRows = enable;
                DgvClipList.AllowUserToDeleteRows = enable;
                DgvClipList.AllowDrop = enable;
            });

            BtnExportLog.InvokeIfRequired(() =>
            {
                BtnExportLog.Enabled = enable;
            });

            BtnClearLog.InvokeIfRequired(() =>
            {
                BtnClearLog.Enabled = enable;
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

    /// <summary>
    /// 執行匯入檔案
    /// </summary>
    /// <param name="filePath">字串，檔案的路徑</param>
    private void DoImportFile(string filePath)
    {
        string extName = Path.GetExtension(filePath), jsonContent;

        switch (extName)
        {
            case ".txt":
                ImportUtil.ImportFile(
                    DgvClipList,
                    SharedDataSource,
                    filePath,
                    TBUrl,
                    TBAutoEndSeconds,
                    Text);

                break;
            case ".json":
                jsonContent = File.ReadAllText(filePath);

                List<TimestampSongData>? dataSet1 = JsonSerializer
                    .Deserialize<List<TimestampSongData>>(jsonContent, VariableSet.SharedJsonSerializerOptions);

                List<SecondsSongData>? dataSet2 = JsonSerializer
                    .Deserialize<List<SecondsSongData>>(jsonContent, VariableSet.SharedJsonSerializerOptions);

                TimeSpan? timeSpan = dataSet1?.FirstOrDefault()?.StartTime;

                double? seconds = dataSet2?.FirstOrDefault()?.StartSeconds;

                // 當 timeSpan 不為 null 時，則表示讀取到的是時間標記的 *.json。
                if (timeSpan != null)
                {
                    ImportUtil.ImportFile(
                        DgvClipList,
                        SharedDataSource,
                        dataSet1,
                        TBUrl,
                        TBAutoEndSeconds,
                        Text);
                }
                else
                {
                    // 當 seconds 為 null 或 0 時，則表示讀取到的是秒數的 *.json。
                    if (seconds == null || seconds == 0)
                    {
                        ImportUtil.ImportFile(
                            DgvClipList,
                            SharedDataSource,
                            dataSet2,
                            TBUrl,
                            TBAutoEndSeconds,
                            Text);
                    }
                }

                break;
            case ".jsonc":
                jsonContent = File.ReadAllText(filePath);

                // 支援來自：https://github.com/jim60105/Playlists 的 *.jsonc 檔案。
                List<List<object>>? dataSet3 = JsonSerializer
                    .Deserialize<List<List<object>>>(jsonContent, VariableSet.SharedJsonSerializerOptions);

                ImportUtil.ImportFile(
                    DgvClipList,
                    SharedDataSource,
                    dataSet3,
                    TBUrl,
                    TBAutoEndSeconds,
                    Text);

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 檢查應用程式的版本
    /// </summary>
    private async void CheckAppVersion()
    {
        if (SharedHttpClient != null)
        {
            UpdateNotifier.CheckResult checkResult = await UpdateNotifier.CheckVersion(SharedHttpClient);

            if (!string.IsNullOrEmpty(checkResult.MessageText))
            {
                SerilogUtil.WriteLog(checkResult.MessageText);
            }

            if (checkResult.HasNewVersion &&
                !string.IsNullOrEmpty(checkResult.DownloadUrl))
            {
                DialogResult dialogResult = MessageBox.Show($"您是否要下載新版本 v{checkResult.VersionText}？",
                    Text,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.OK)
                {
                    CustomFunction.OpenUrl(checkResult.DownloadUrl);
                }
            }

            if (checkResult.NetVersionIsOdler &&
                !string.IsNullOrEmpty(checkResult.DownloadUrl))
            {
                DialogResult dialogResult = MessageBox.Show($"您是否要下載舊版本 v{checkResult.VersionText}？",
                    Text,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.OK)
                {
                    CustomFunction.OpenUrl(checkResult.DownloadUrl);
                }
            }
        }
        else
        {
            SerilogUtil.WriteLog("httpClient 是 null。", LogEventLevel.Warning);
        }
    }
}