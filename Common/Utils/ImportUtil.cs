using Serilog.Events;
using System.ComponentModel;
using System.Text.RegularExpressions;
using YtDlpFront.Common.Sets;
using YtDlpFront.Extensions;
using YtDlpFront.Models;

namespace YtDlpFront.Common.Utils;

/// <summary>
/// 匯入工具
/// </summary>
public static partial class ImportUtil
{
    /// <summary>
    /// 取得秒數
    /// </summary>
    /// <param name="value">字串</param>
    /// <returns>數值</returns>
    public static double GetSeconds(string value)
    {
        double seconds = 0.0;

        if (TimeSpan.TryParse(value, out TimeSpan timeSpan))
        {
            seconds = timeSpan.TotalSeconds;
        }

        return seconds;
    }

    /// <summary>
    /// 取得開始秒數
    /// </summary>
    /// <param name="control1">TextBox，TBStartSeconds</param>
    /// <param name="control2">MaskedTextBox，MTBStartTime</param>
    /// <returns>數值</returns>
    public static double GetStartSeconds(TextBox control1, MaskedTextBox control2)
    {
        bool isOkay = false;

        double outputSeconds = 0.0;

        control1.InvokeIfRequired(() =>
        {
            if (double.TryParse(control1.Text, out double seconds))
            {
                if (seconds > 0.0)
                {
                    outputSeconds = seconds;

                    isOkay = true;
                }
            }
        });

        if (!isOkay || outputSeconds == 0.0)
        {
            control2.InvokeIfRequired(() =>
            {
                double seconds = GetSeconds(control2.Text);

                if (seconds > 0.0)
                {
                    outputSeconds = seconds;
                }
            });
        }

        return outputSeconds;
    }

    /// <summary>
    /// 取得結束秒數
    /// </summary>
    /// <param name="control1">TextBox，TBEndSeconds</param>
    /// <param name="control2">MaskedTextBox，MTBEndtime</param>
    /// <returns>數值</returns>
    public static double GetEndSeconds(TextBox control1, MaskedTextBox control2)
    {
        bool isOkay = false;

        double outputSeconds = 0.0;

        control1.InvokeIfRequired(() =>
        {
            if (double.TryParse(control1.Text, out double seconds))
            {
                if (seconds > 0.0)
                {
                    outputSeconds = seconds;

                    isOkay = true;
                }
            }
        });

        if (!isOkay || outputSeconds == 0.0)
        {
            control2.InvokeIfRequired(() =>
            {
                double seconds = GetSeconds(control2.Text);

                if (seconds > 0.0)
                {
                    outputSeconds = seconds;
                }
            });
        }

        return outputSeconds;
    }

    /// <summary>
    /// 取得自動結束秒數
    /// </summary>
    /// <param name="control">TextBox，TBAutoEndSeconds</param>
    /// <param name="startSeconds">數值，開始秒數</param>
    /// <returns>數值</returns>
    public static double GetAutoEndSeconds(TextBox control, double startSeconds)
    {
        double outputSeconds = 0.0;

        control.InvokeIfRequired(() =>
        {
            // 判斷是否有填寫欄位。
            if (!string.IsNullOrEmpty(control.Text))
            {
                // 進行數值轉換。
                if (double.TryParse(control.Text, out double parseResult))
                {
                    outputSeconds = startSeconds + parseResult;
                }
                else
                {
                    outputSeconds = startSeconds + VariableSet.AppendSeconds;
                }
            }
            else
            {
                outputSeconds = startSeconds + VariableSet.AppendSeconds;
            }
        });

        return outputSeconds;
    }

    /// <summary>
    /// 從網址解析秒數
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <param name="dataSource">BindingList&lt;ClipData&gt;</param>
    /// <param name="control">TextBox，TBAutoEndSeconds</param>
    public static void ParseSecondsFromUrl(
        string url,
        BindingList<ClipData> dataSource,
        TextBox control)
    {
        string pattern = @"(\?t=\d+s)|(&t=\d+s)|(\?startSeconds=\d+)|(&startSeconds=\d+)|(&endSeconds=\d+)";

        // 只拉出成功的
        IEnumerable<Match> matches = Regex.Matches(url, pattern).Where(n => n.Success == true);

        if (matches.Any())
        {
            ClipData? clipData = new();

            int clipNo = dataSource.Count;

            clipData.ClipNo = ++clipNo;

            foreach (Match match in matches)
            {
                if (match.Value.Contains("?t=") || match.Value.Contains("&t="))
                {
                    string secsStr = match.Value
                        .Replace("?t=", string.Empty)
                        .Replace("&t=", string.Empty)
                        .Replace("s", string.Empty);

                    if (double.TryParse(secsStr, out double secs))
                    {
                        if (clipData != null)
                        {
                            clipData.StartSeconds = secs;
                        }
                    }
                }
                else if (match.Value.Contains("?startSeconds=") || match.Value.Contains("&startSeconds="))
                {
                    string secsStr = match.Value
                        .Replace("?startSeconds=", string.Empty)
                        .Replace("&startSeconds=", string.Empty)
                        .Replace("s", string.Empty);

                    if (double.TryParse(secsStr, out double secs))
                    {
                        if (clipData != null)
                        {
                            clipData.StartSeconds = secs;
                        }
                    }
                }
                else if (match.Value.Contains("&endSeconds="))
                {
                    if (clipData?.StartSeconds != 0.0)
                    {
                        string secsStr = match.Value
                            .Replace("&endSeconds=", string.Empty)
                            .Replace("s", string.Empty);

                        if (double.TryParse(secsStr, out double secs))
                        {
                            if (clipData != null)
                            {
                                clipData.EndSeconds = secs;
                            }
                        }
                    }
                    else
                    {
                        // 當進行到此處時，若 clipData?.StartSeconds 為 0.0，
                        // 則將 clipData 設為 null，表示網址內沒有包含有效的時間值。
                        clipData = null;
                    }
                }
            }

            if (clipData != null)
            {
                if (clipData.EndSeconds == 0.0)
                {
                    clipData.EndSeconds =
                        GetAutoEndSeconds(
                            control,
                            clipData.EndSeconds);
                }

                // 排除起訖秒數一樣的資料重複加入。
                if (!dataSource.Any(n => n.StartSeconds == clipData.StartSeconds &&
                    n.EndSeconds == clipData.EndSeconds))
                {
                    dataSource.Add(clipData);
                }
                else
                {
                    clipData = null;
                }
            }
        }
    }

    /// <summary>
    /// 匯入時間標記文字檔案
    /// </summary>
    /// <param name="control1">DataGridView，DgvClipList</param>
    /// <param name="dataSource">BindingList&lt;ClipData&gt;</param>
    /// <param name="fileName">字串，檔案的路徑</param>
    /// <param name="control2">TextBox，TBUrl</param>
    /// <param name="control3">TextBox，TBAutoEndSeconds</param>
    /// <param name="appName">字串，應用程式的名稱</param>
    public static void ImportFile<T>(
        DataGridView control1,
        BindingList<ClipData> dataSource,
        T source,
        TextBox control2,
        TextBox control3,
        string appName)
    {
        int clipNo = 1;

        switch (source)
        {
            case string fileName:
                List<ClipData> dataSet1 = ParseImportData(
                    fileName,
                    control2,
                    control3,
                    clipNo);

                if (dataSet1.Count > 0)
                {
                    control1.InvokeIfRequired(() =>
                    {
                        control1.SuspendLayout();

                        // 先刪除掉舊資料。
                        dataSource.Clear();

                        dataSource.RaiseListChangedEvents = false;

                        foreach (ClipData clipData in dataSet1)
                        {
                            string errorMessage = CustomFunction.IsSecondsValid(
                                clipData.StartSeconds,
                                clipData.EndSeconds);

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                if (!dataSource.Any(n => n.ClipName == clipData.ClipName))
                                {
                                    dataSource.Add(clipData);
                                }
                                else
                                {
                                    SerilogUtil.WriteLog(
                                        $"短片「{clipData.ClipName}」已存在，故忽略不加入。",
                                        LogEventLevel.Warning);
                                }
                            }
                            else
                            {
                                errorMessage = $"時間標記：「{clipData.ClipName}」匯入失敗，" +
                                    $"錯誤訊息：{Environment.NewLine}{errorMessage}";
                                errorMessage = errorMessage.TrimEnd(Environment.NewLine.ToCharArray());

                                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
                            }
                        }

                        dataSource.RaiseListChangedEvents = true;
                        dataSource.ResetBindings();

                        control1.AutoResizeRowHeadersWidth(
                             DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                        control1.ResumeLayout();

                        if (control1.RowCount > 0)
                        {
                            control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                        }
                    });
                }
                else
                {
                    MessageBox.Show("無法解析出有效的時間標記資訊，請確認該檔案的內容格式是否正確。",
                        appName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                break;
            case List<TimestampSongData> dataSet2:
                if (dataSet2.Count > 0)
                {
                    control1.InvokeIfRequired(() =>
                    {
                        control1.SuspendLayout();

                        // 先刪除掉舊資料。
                        dataSource.Clear();

                        dataSource.RaiseListChangedEvents = false;

                        foreach (TimestampSongData data in dataSet2)
                        {
                            double startSeconds = data.StartTime?.TotalSeconds ?? 0.0d,
                                endSeconds = data.EndTime?.TotalSeconds ?? 0.0d;

                            if (endSeconds == 0.0)
                            {
                                endSeconds = GetAutoEndSeconds(control3, startSeconds);
                            }

                            string clipName = data.Name ?? "未知名稱";

                            string errorMessage = CustomFunction.IsSecondsValid(startSeconds, endSeconds);

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                if (!dataSource.Any(n => n.ClipName == clipName))
                                {
                                    control2.InvokeIfRequired(() =>
                                    {
                                        control2.Text = data.VideoID;
                                    });

                                    dataSource.Add(new ClipData()
                                    {
                                        ClipNo = clipNo,
                                        ClipName = clipName,
                                        StartSeconds = startSeconds,
                                        EndSeconds = endSeconds
                                    });

                                    clipNo++;
                                }
                                else
                                {
                                    SerilogUtil.WriteLog(
                                        $"短片「{clipName}」已存在，故忽略不加入。",
                                        LogEventLevel.Warning);
                                }
                            }
                            else
                            {
                                errorMessage = $"時間標記：「{data.Name}」匯入失敗，" +
                                    $"錯誤訊息：{Environment.NewLine}{errorMessage}";
                                errorMessage = errorMessage.TrimEnd(Environment.NewLine.ToCharArray());

                                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
                            }
                        }

                        dataSource.RaiseListChangedEvents = true;
                        dataSource.ResetBindings();

                        control1.AutoResizeRowHeadersWidth(
                             DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                        control1.ResumeLayout();

                        if (control1.RowCount > 0)
                        {
                            control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                        }
                    });
                }
                else
                {
                    MessageBox.Show("無法解析出有效的播放清單資訊，請確認該檔案的內容格式是否正確。",
                        appName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                break;
            case List<SecondsSongData> dataSet2:
                if (dataSet2.Count > 0)
                {
                    control1.InvokeIfRequired(() =>
                    {
                        control1.SuspendLayout();

                        // 先刪除掉舊資料。
                        dataSource.Clear();

                        dataSource.RaiseListChangedEvents = false;

                        foreach (SecondsSongData data in dataSet2)
                        {
                            double startSeconds = data.StartSeconds ?? 0.0d,
                                endSeconds = data.EndSeconds ?? 0.0d;

                            if (endSeconds == 0.0)
                            {
                                endSeconds = GetAutoEndSeconds(control3, startSeconds);
                            }

                            string clipName = data.Name ?? "未知名稱";

                            string errorMessage = CustomFunction.IsSecondsValid(startSeconds, endSeconds);

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                if (!dataSource.Any(n => n.ClipName == clipName))
                                {
                                    control2.InvokeIfRequired(() =>
                                    {
                                        control2.Text = data.VideoID;
                                    });

                                    dataSource.Add(new ClipData()
                                    {
                                        ClipNo = clipNo,
                                        ClipName = clipName,
                                        StartSeconds = startSeconds,
                                        EndSeconds = endSeconds
                                    });

                                    clipNo++;
                                }
                                else
                                {
                                    SerilogUtil.WriteLog(
                                        $"短片「{clipName}」已存在，故忽略不加入。",
                                        LogEventLevel.Warning);
                                }
                            }
                            else
                            {
                                errorMessage = $"時間標記：「{data.Name}」匯入失敗，" +
                                    $"錯誤訊息：{Environment.NewLine}{errorMessage}";
                                errorMessage = errorMessage.TrimEnd(Environment.NewLine.ToCharArray());

                                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
                            }
                        }

                        dataSource.RaiseListChangedEvents = true;
                        dataSource.ResetBindings();

                        control1.AutoResizeRowHeadersWidth(
                             DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                        control1.ResumeLayout();

                        if (control1.RowCount > 0)
                        {
                            control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                        }
                    });
                }
                else
                {
                    MessageBox.Show("無法解析出有效的播放清單資訊，請確認該檔案的內容格式是否正確。",
                        appName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                break;
            case List<List<object>> dataSet2:
                if (dataSet2.Count > 0)
                {
                    control1.InvokeIfRequired(() =>
                    {
                        control1.SuspendLayout();

                        // 先刪除掉舊資料。
                        dataSource.Clear();

                        dataSource.RaiseListChangedEvents = false;

                        foreach (List<object> data in dataSet2)
                        {
                            double startSeconds = double.TryParse(data[1].ToString(), out double result1) ? result1 : 0.0d,
                                endSeconds = double.TryParse(data[2].ToString(), out double result2) ? result2 : 0.0d;

                            if (endSeconds == 0.0)
                            {
                                endSeconds = GetAutoEndSeconds(control3, startSeconds);
                            }

                            string clipName = data[3].ToString() ?? "未知名稱";

                            string errorMessage = CustomFunction.IsSecondsValid(startSeconds, endSeconds);

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                if (!dataSource.Any(n => n.ClipName == clipName))
                                {
                                    control2.InvokeIfRequired(() =>
                                    {
                                        control2.Text = data[0].ToString();
                                    });

                                    dataSource.Add(new ClipData()
                                    {
                                        ClipNo = clipNo,
                                        ClipName = clipName,
                                        StartSeconds = startSeconds,
                                        EndSeconds = endSeconds
                                    });

                                    clipNo++;
                                }
                                else
                                {
                                    SerilogUtil.WriteLog(
                                        $"短片「{clipName}」已存在，故忽略不加入。",
                                        LogEventLevel.Warning);
                                }
                            }
                            else
                            {
                                errorMessage = $"時間標記：「{clipName}」匯入失敗，" +
                                    $"錯誤訊息：{Environment.NewLine}{errorMessage}";
                                errorMessage = errorMessage.TrimEnd(Environment.NewLine.ToCharArray());

                                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
                            }
                        }

                        dataSource.RaiseListChangedEvents = true;
                        dataSource.ResetBindings();

                        control1.AutoResizeRowHeadersWidth(
                             DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                        control1.ResumeLayout();

                        if (control1.RowCount > 0)
                        {
                            control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                        }
                    });
                }
                else
                {
                    MessageBox.Show("無法解析出有效的播放清單資訊，請確認該檔案的內容格式是否正確。",
                        appName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                break;
        }
    }

    /// <summary>
    /// 解析匯入資料
    /// </summary>
    /// <param name="filePath">字串，檔案的路徑</param>
    /// <param name="control1">TextBox，TBUrl</param>
    /// <param name="control2">TextBox，TBAutoEndSeconds</param>
    /// <param name="clipNo">數值，短片的編號，預設值為 1</param>
    /// <returns>List&lt;ClipData&gt;</returns>
    private static List<ClipData> ParseImportData(
        string filePath,
        TextBox control1,
        TextBox control2,
        int clipNo = 1)
    {
        string[] lines = File.ReadAllLines(filePath);

        bool canProcess = false;

        List<ClipData> dataSet = new();

        ClipData? tempClipData = null;

        int timestampCount = 0, currentIndex = 0;

        foreach (string currentLine in lines)
        {
            // 分隔用行。
            if (currentLine == "-------")
            {
                canProcess = false;

                continue;
            }

            if (currentLine.Contains("網址："))
            {
                canProcess = false;

                control1.InvokeIfRequired(() =>
                {
                    string url = currentLine.Replace("網址：", string.Empty);

                    control1.Text = url;
                });

                continue;
            }

            if (currentLine == "時間標記：")
            {
                canProcess = true;

                continue;
            }

            if (canProcess && !string.IsNullOrEmpty(currentLine))
            {
                // 判斷是否為備註列。
                if (currentLine.Contains('#'))
                {
                    string clipName = string.Empty;

                    // 判斷是否為開始的點。
                    if (currentLine.Contains("（開始）"))
                    {
                        // 當遇到連續的 "（開始）" 時。
                        if (tempClipData != null)
                        {
                            // 設定結束秒數。
                            tempClipData.EndSeconds =
                                GetAutoEndSeconds(
                                    control2,
                                    tempClipData.StartSeconds);

                            // 重置 timestampCount 為 0，以供下一個流程使用。
                            timestampCount = 0;

                            // 將 tempClipData 加入 dataSet;
                            dataSet.Add(tempClipData);

                            // 在加入 dataSet 後清空 tempClipData。
                            tempClipData = null;
                        }

                        tempClipData = new ClipData();

                        // 去除不必要的內容。
                        clipName = currentLine.Replace("#", string.Empty)
                            .Replace("（開始）", string.Empty)
                            .TrimStart();
                    }

                    if (!string.IsNullOrEmpty(clipName))
                    {
                        if (tempClipData != null)
                        {
                            tempClipData.ClipNo = clipNo;
                            tempClipData.ClipName = clipName;
                        }

                        clipNo++;
                    }
                }
                else
                {
                    string[] timestampSet = currentLine.Split(
                        new char[] { '｜' },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (timestampSet.Length > 0 &&
                        !string.IsNullOrEmpty(timestampSet[2]))
                    {
                        if (tempClipData != null)
                        {
                            // 當 timestampCount 為 0 時，設定 clipData.StartSeconds。
                            if (timestampCount == 0)
                            {
                                if (double.TryParse(timestampSet[2], out double seconds))
                                {
                                    tempClipData.StartSeconds = seconds;
                                }
                                else
                                {
                                    tempClipData.StartSeconds = GetSeconds(timestampSet[0]);
                                }

                                timestampCount++;
                            }
                            else if (timestampCount == 1)
                            {
                                // 當 timestampCount 為 1 時，設定 clipData.EndSeconds。
                                if (double.TryParse(timestampSet[2], out double seconds))
                                {
                                    tempClipData.EndSeconds = seconds;
                                }
                                else
                                {
                                    tempClipData.EndSeconds = GetSeconds(timestampSet[0]);
                                }

                                // 重置 timestampCount 為 0，以供下一個流程使用。
                                timestampCount = 0;

                                // 將 tempClipData 加入 dataSet;
                                dataSet.Add(tempClipData);

                                // 在加入 dataSet 後清空 tempClipData。
                                tempClipData = null;
                            }
                        }
                    }
                }

                // 當剛好是最後一個 "（開始）" 組合，且後面沒有其他有效資料行時。
                if (currentIndex >= lines.Length - 2)
                {
                    // 判斷 tempClipData 是否無為 null。
                    if (tempClipData != null)
                    {
                        // 判斷結束秒數是否為 0.0。
                        if (tempClipData.EndSeconds == 0.0)
                        {
                            // 設定結束秒數。
                            tempClipData.EndSeconds =
                                GetAutoEndSeconds(
                                    control2,
                                    tempClipData.StartSeconds);

                            // 重置 timestampCount 為 0，以供下一個流程使用。
                            timestampCount = 0;

                            // 將 tempClipData 加入 dataSet;
                            dataSet.Add(tempClipData);

                            // 在加入 dataSet 後清空 tempClipData。
                            tempClipData = null;
                        }
                    }
                }
            }

            currentIndex++;
        }

        return dataSet;
    }
}