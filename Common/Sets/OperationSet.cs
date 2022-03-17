using Serilog.Events;
using System.ComponentModel;
using Xabe.FFmpeg;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;
using YtDlpFront.Common.Utils;
using YtDlpFront.Extensions;
using YtDlpFront.Models;

namespace YtDlpFront.Common.Sets;

/// <summary>
/// 操作組
/// </summary>
public static class OperationSet
{
    /// <summary>
    /// 獲取影片資訊
    /// </summary>
    /// <param name="videoUrl">字串，影片的網址</param>
    /// <param name="optionSet">OptionSet</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task FetchVideo(
        string videoUrl,
        OptionSet optionSet,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

        RunResult<VideoData> runResult = await ytdl.RunVideoDataFetch(videoUrl,
            ct: ct,
            overrideOptions: optionSet);

        string resultText = runResult.Success ? "成功" : "失敗";

        SerilogUtil.WriteLog($"作業結果：{resultText}");

        if (runResult.Success)
        {
            VideoData? videoData = runResult.Data;

            if (videoData != null)
            {
                string message = $"頻道：{videoData.Channel}{Environment.NewLine}" +
                    $"上傳者：{videoData.Uploader}{Environment.NewLine}" +
                    $"上傳時間：{videoData.UploadDate}{Environment.NewLine}" +
                    $"影片 ID 值：{videoData.ID}{Environment.NewLine}" +
                    $"影片標題：{videoData.Title}{Environment.NewLine}" +
                    $"觀看次數：{videoData.ViewCount}{Environment.NewLine}" +
                    $"描述：{Environment.NewLine}" +
                    // 替換行字元至 Environment.NewLine。
                    $"{CustomFunction.RegexAscii.Replace(videoData.Description, Environment.NewLine)}" +
                    $"{Environment.NewLine}" +
                    $"格式：{Environment.NewLine}";

                string formats = string.Empty,
                       seperatorLine = string.Empty;

                if (videoData.Formats.Length > 0)
                {
                    string titleLine = "| 格式".PadRight(37) + " |" +
                        " 副檔名".PadRight(4) + " |" +
                        " 影格率".PadRight(9) + " |" +
                        " 視訊編碼器".PadRight(26) + " |" +
                        " 音訊編碼器".PadRight(26) + " |" +
                        " 檔案大小".PadRight(8) + " |" +
                        " 通訊協定".PadRight(19) + " |";

                    // 預設應為 0。
                    int magicNum = 26;
                    int requiredLength = titleLine.Length + magicNum;

                    for (int i = 0; i < requiredLength; i++)
                    {
                        seperatorLine += "-";
                    }

                    formats += seperatorLine +
                        Environment.NewLine +
                        titleLine +
                        Environment.NewLine +
                        seperatorLine +
                        Environment.NewLine;
                }

                foreach (FormatData formatData in videoData.Formats)
                {
                    string fileSizeStr = CustomFunction.GetBytesReadable(formatData.FileSize ?? 0);
                    string[] tempFileSizeArray = fileSizeStr.Split(" ");

                    formats += $"| {formatData.Format,-37} |" +
                        $" {formatData.Extension,-6} |" +
                        $" {formatData.FrameRate,-11} |" +
                        $" {formatData.VideoCodec,-30} |" +
                        $" {formatData.AudioCodec,-30} |" +
                        $" {tempFileSizeArray[0],-9}{tempFileSizeArray[1],-2} |" +
                        $" {formatData.Protocol,-22} |" +
                        Environment.NewLine;
                }

                formats += seperatorLine;

                message += formats;

                SerilogUtil.WriteLog($"{Environment.NewLine}{message}");
            }
            else
            {
                SerilogUtil.WriteLog("無法獲取影片資訊。");
            }
        }
        else
        {
            string errorMessage = string.Empty;

            int currentIndex = 0;

            foreach (string errResult in runResult.ErrorOutput)
            {
                errorMessage += errResult;

                if (currentIndex < runResult.ErrorOutput.Length - 1)
                {
                    errorMessage += Environment.NewLine;
                }

                currentIndex++;
            }

            SerilogUtil.WriteLog(errorMessage);
        }
    }

    /// <summary>
    /// 下載影片
    /// </summary>
    /// <param name="videoUrl">字串，影片的網址</param>
    /// <param name="clipName">短片的名稱</param>
    /// <param name="startSeconds">數值，開始的秒數</param>
    /// <param name="endSeconds">數值，結束的秒數</param>
    /// <param name="deleteSourceFile">布林值，刪除原始檔案</param>
    /// <param name="optionSet">OptionSet</param>
    /// <param name="ct">CancellationToken</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <param name="isAudioOnly">布林值，是否僅音訊，預設為 false</param>
    /// <returns>Task</returns>
    public static async Task DownloadVideo(
        string videoUrl,
        string clipName,
        double startSeconds,
        double endSeconds,
        bool deleteSourceFile,
        OptionSet optionSet,
        CancellationToken ct,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0,
        bool isAudioOnly = false)
    {
        try
        {
            // 檢查下載資料夾是否已存在。
            if (!Directory.Exists(VariableSet.VideoDownloadFolderPath))
            {
                // 建立下載資料夾。
                Directory.CreateDirectory(VariableSet.VideoDownloadFolderPath);
            }

            ct.ThrowIfCancellationRequested();

            YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

            // 計算間隔秒數。
            // 當 durationSeconds 的值大於等於 0.0 時，不需要修正影片。
            // 當 durationSeconds 的值小於 0.0 時，要修正影片。
            double durationSeconds = startSeconds - endSeconds;

            // 當不需要修正影片時，改用下方的 yt-dlp Output Template。
            if (durationSeconds >= 0.0)
            {
                // 當 durationSeconds 大於等於 0 時，就等於要下載全片。
                string outputTemplate = "%(uploader)s_%(title)s-%(id)s.%(ext)s";

                optionSet.Output = Path.Combine(VariableSet.VideoDownloadFolderPath, outputTemplate);
            }
            else
            {
                // 當需要修正影片時，需要移除下列的設定值。（不管是否已設定，皆移除）
                optionSet.DeleteCustomOption("--split-chapters");
                optionSet.DeleteCustomOption("-o");
            }

            RunResult<string> runResult = await ytdl.RunVideoDownload(videoUrl,
                format: optionSet.Format,
                mergeFormat: DownloadMergeFormat.Unspecified,
                recodeFormat: VideoRecodeFormat.None,
                progress: GetDownloadProgress(),
                output: GetStringProgress(),
                ct: ct,
                overrideOptions: optionSet);

            string resultText = runResult.Success ? "成功" : "失敗";

            SerilogUtil.WriteLog($"作業結果：{resultText}");

            if (runResult.Success)
            {
                if (!string.IsNullOrEmpty(runResult.Data))
                {
                    string outputFilePath = Path.GetFullPath(runResult.Data);
                    string originFileName = Path.GetFileNameWithoutExtension(outputFilePath);
                    string originExtName = Path.GetExtension(outputFilePath);

                    SerilogUtil.WriteLog($"檔案路徑：{outputFilePath}");

                    // 當 durationSeconds 的值小於 0.0 時，要修正影片。
                    if (durationSeconds < 0.0)
                    {
                        IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(outputFilePath);

                        // 處理轉檔後的檔案名稱。
                        if (!string.IsNullOrEmpty(clipName))
                        {
                            // 當 clipName 沒有其他內容時，長度會是 4。
                            if (clipName.Length > 4)
                            {
                                // 針對 clipName 替換特殊字元。
                                clipName = $"{CustomFunction.ReplaceInvalidChars(clipName)}";
                            }
                            else
                            {
                                clipName = $"{originFileName}" +
                                    $"_{CustomFunction.ReplaceInvalidChars(clipName)}Clip";
                            }
                        }
                        else
                        {
                            clipName = $"{originFileName}_Clip";
                        }

                        string newExtName = originExtName;

                        // 針對僅音訊處理副檔名。
                        if (isAudioOnly)
                        {
                            newExtName = originExtName switch
                            {
                                ".mp4" => ".m4a",
                                ".mkv" => ".mka",
                                _ => originExtName,
                            };
                        }

                        string newFileName = $"{clipName}_Fixed{newExtName}";
                        string newOutputPath = Path.Combine(VariableSet.VideoDownloadFolderPath, newFileName);

                        IConversion conversion = ExternalProgram.GetConversion(
                            mediaInfo,
                            startSeconds,
                            endSeconds,
                            newOutputPath,
                            fixDuration: true,
                            useCodecCopy: false,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo,
                            isAudioOnly);

                        IConversionResult conversionResult = await conversion.Start(ct);

                        ExternalProgram.WriteConversionResult(conversionResult);

                        string changedFileName = $"{clipName}{originExtName}";
                        string changedOutputFilePath = Path.Combine(VariableSet.VideoDownloadFolderPath, changedFileName);

                        // 更新來源檔案的檔案名稱。
                        if (File.Exists(outputFilePath))
                        {
                            File.Move(outputFilePath, changedOutputFilePath, true);

                            string message = $"已重新命名檔案：{Environment.NewLine}" +
                                $"[原]：{outputFilePath}{Environment.NewLine}" +
                                $"[新]：{changedOutputFilePath}";

                            SerilogUtil.WriteLog(message);
                        }

                        if (deleteSourceFile)
                        {
                            if (File.Exists(changedOutputFilePath))
                            {
                                File.Delete(changedOutputFilePath);

                                SerilogUtil.WriteLog($"已刪除原始檔案：{changedOutputFilePath}");
                            }
                        }
                    }
                    else
                    {
                        SerilogUtil.WriteLog($"下載完成，檔案：{outputFilePath}");
                    }
                }
                else
                {
                    SerilogUtil.WriteLog("錯誤訊息：無法取得輸出的檔案路徑。", LogEventLevel.Error);
                }
            }
            else
            {
                string errorMessage = string.Empty;

                int currentIndex = 0;

                foreach (string errResult in runResult.ErrorOutput)
                {
                    errorMessage += errResult;

                    if (currentIndex < runResult.ErrorOutput.Length - 1)
                    {
                        errorMessage += Environment.NewLine;
                    }

                    currentIndex++;
                }

                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
            }
        }
        catch (Exception ex)
        {
            SerilogUtil.WriteLog(ex.Message, LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 下載影片 (先下載全片後分割)
    /// </summary>
    /// <param name="videoUrl">字串，影片的網址</param>
    /// <param name="control">DataGridView，短片清單</param>
    /// <param name="dataSource">BindingList&lt;ClipData&gt;</param>
    /// <param name="deleteSourceFile">布林值，刪除原始檔案</param>
    /// <param name="optionSet">OptionSet</param>
    /// <param name="ct">CancellationToken</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <returns>Task</returns>
    public static async Task DownloadVideo(
        string videoUrl,
        DataGridView control,
        BindingList<ClipData> dataSource,
        bool deleteSourceFile,
        OptionSet optionSet,
        CancellationToken ct,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        try
        {
            // 檢查下載資料夾是否已存在。
            if (!Directory.Exists(VariableSet.VideoDownloadFolderPath))
            {
                // 建立下載資料夾。
                Directory.CreateDirectory(VariableSet.VideoDownloadFolderPath);
            }

            ct.ThrowIfCancellationRequested();

            YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

            // 避免 FFmpeg 轉換出問題，故取消相關的設定。
            // 請參考 ExternalProgram.GetDefaultOptionSet() 方法。
            optionSet.AddMetadata = false;
            optionSet.EmbedThumbnail = false;

            // 當不需要分割短片時，改用下方的 yt-dlp Output Template。
            if (dataSource.Count <= 0)
            {
                string outputTemplate = "%(uploader)s_%(title)s-%(id)s.%(ext)s";

                optionSet.Output = Path.Combine(VariableSet.VideoDownloadFolderPath, outputTemplate);
            }
            else
            {
                // 當需要分割檔案時，需要移除下列的設定值。（不管是否已設定，皆移除）
                optionSet.DeleteCustomOption("--split-chapters");
                optionSet.DeleteCustomOption("-o");
            }

            RunResult<string> runResult = await ytdl.RunVideoDownload(videoUrl,
                format: optionSet.Format,
                mergeFormat: DownloadMergeFormat.Unspecified,
                recodeFormat: VideoRecodeFormat.None,
                progress: GetDownloadProgress(),
                output: GetStringProgress(),
                ct: ct,
                overrideOptions: optionSet);

            string resultText = runResult.Success ? "成功" : "失敗";

            SerilogUtil.WriteLog($"作業結果：{resultText}");

            if (runResult.Success)
            {
                if (!string.IsNullOrEmpty(runResult.Data))
                {
                    string outputFilePath = Path.GetFullPath(runResult.Data);

                    SerilogUtil.WriteLog($"檔案路徑：{outputFilePath}");

                    if (dataSource.Count > 0)
                    {
                        foreach (ClipData clipData in dataSource)
                        {
                            DataGridViewRow? dgvRow = null;

                            bool isAudioOnly = clipData.IsAudioOnly;

                            int currentIndex = dataSource.IndexOf(clipData);

                            control.InvokeIfRequired(() =>
                            {
                                DataGridViewRow dgvRow = control.Rows[currentIndex];

                                // 設定已選擇。
                                dgvRow.Selected = true;

                                // 設定指示器的位置。
                                control.CurrentCell = dgvRow.Cells[0];
                                control.FirstDisplayedScrollingRowIndex = dgvRow.Index;
                            });

                            // 可能會有一取消，就全部取消的問題。
                            await SplitVideo(
                                clipData,
                                outputFilePath,
                                ct,
                                useHardwareAcceleration,
                                hardwareAcceleratorType,
                                deviceNo,
                                isAudioOnly);

                            control.InvokeIfRequired(() =>
                            {
                                if (currentIndex == control.Rows.Count - 1)
                                {
                                    if (deleteSourceFile)
                                    {
                                        if (File.Exists(outputFilePath))
                                        {
                                            File.Delete(outputFilePath);

                                            SerilogUtil.WriteLog($"已刪除原始檔案：{outputFilePath}");
                                        }
                                    }

                                    MessageBox.Show("作業完成。",
                                        VariableSet.AppName,
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
                    else
                    {
                        SerilogUtil.WriteLog("錯誤訊息：短片清單無資料。", LogEventLevel.Error);
                    }
                }
                else
                {
                    SerilogUtil.WriteLog("錯誤訊息：無法取得輸出的檔案路徑。", LogEventLevel.Error);
                }
            }
            else
            {
                string errorMessage = string.Empty;

                int currentIndex = 0;

                foreach (string errResult in runResult.ErrorOutput)
                {
                    errorMessage += errResult;

                    if (currentIndex < runResult.ErrorOutput.Length - 1)
                    {
                        errorMessage += Environment.NewLine;
                    }

                    currentIndex++;
                }

                SerilogUtil.WriteLog(errorMessage, LogEventLevel.Error);
            }
        }
        catch (Exception ex)
        {
            SerilogUtil.WriteLog(ex.Message, LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 分割影片 (不使用 -c:v copy -c:a copy)
    /// </summary>
    /// <param name="clipData">ClipData</param>
    /// <param name="sourceFilePath">字串，來源檔案路徑</param>
    /// <param name="ct">CancellationToken</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <param name="isAudioOnly">布林值，是否僅音訊，預設為 false</param>
    /// <returns>Task</returns>
    public static async Task SplitVideo(
        ClipData clipData,
        string sourceFilePath,
        CancellationToken ct,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0,
        bool isAudioOnly = false)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            string originFileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            string originExtName = Path.GetExtension(sourceFilePath);
            string clipName = clipData.ClipName;

            if (string.IsNullOrEmpty(clipName))
            {
                clipName = $"{clipData.ClipNo:#000}.";
            }
            else
            {
                clipName = $"{clipData.ClipNo:#000}.{clipName}";
            }

            double startSeconds = clipData.StartSeconds,
                   endSeconds = clipData.EndSeconds,
                   // 計算間隔秒數。
                   durationSeconds = startSeconds - endSeconds;

            // 當 durationSeconds 的值小於 0.0 時，要修正影片。
            if (durationSeconds < 0.0)
            {
                IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(sourceFilePath);

                // 當 clipName 沒有其他內容時，長度會是 4。
                if (clipName.Length > 4)
                {
                    // 針對 clipName 替換特殊字元。
                    clipName = $"{CustomFunction.ReplaceInvalidChars(clipName)}";
                }
                else
                {
                    clipName = $"{originFileName}" +
                        $"_{CustomFunction.ReplaceInvalidChars(clipName)}Clip";
                }

                // 針對僅音訊處理副檔名。
                if (isAudioOnly)
                {
                    switch (originExtName)
                    {
                        case ".mp4":
                            originExtName = ".m4a";

                            break;
                        case ".mkv":
                            originExtName = ".mka";

                            break;
                    }
                }

                // 新的根目錄路徑。
                string newRootPath = Path.Combine(VariableSet.VideoDownloadFolderPath, originFileName);
                // 用臨時檔案名稱。
                string newFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}" +
                    $"{originExtName}";
                string newOutputPath = Path.Combine(newRootPath, newFileName);

                // 當 newRootPath 不存在時，則建立 newRootPath。
                if (!Directory.Exists(newRootPath))
                {
                    Directory.CreateDirectory(newRootPath);
                }

                IConversion conversion = ExternalProgram.GetConversion(
                    mediaInfo,
                    startSeconds,
                    endSeconds,
                    newOutputPath,
                    fixDuration: false,
                    useCodecCopy: false,
                    useHardwareAcceleration,
                    hardwareAcceleratorType,
                    deviceNo,
                    isAudioOnly);

                // 移除全部的 Meta 資料，以利傻瓜式操作。
                conversion.AddParameter("-map_metadata -1");

                IConversionResult conversionResult = await conversion.Start(ct);

                ExternalProgram.WriteConversionResult(conversionResult);

                string changedFileName = $"{clipName}{originExtName}";
                string changedOutputFilePath = Path.Combine(newRootPath, changedFileName);

                // 更新來源檔案的檔案名稱。
                if (File.Exists(newOutputPath))
                {
                    File.Move(newOutputPath, changedOutputFilePath, true);

                    string message = $"已重新命名檔案：{Environment.NewLine}" +
                        $"[原]：{newOutputPath}{Environment.NewLine}" +
                        $"[新]：{changedOutputFilePath}";

                    SerilogUtil.WriteLog(message);
                }
            }
            else
            {
                SerilogUtil.WriteLog($"無效的時間間隔，忽略此短片不進行分割，檔案：{clipName}");
            }
        }
        catch (Exception ex)
        {
            SerilogUtil.WriteLog(ex.Message, LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 燒錄字幕檔
    /// </summary>
    /// <param name="videoFilePath">字串，視訊檔案的路徑</param>
    /// <param name="subtitleFilePath">字串，字幕檔案的路徑</param>
    /// <param name="outputPath">字串，輸出檔案的路徑</param>
    /// <param name="ct">CancellationToken</param>
    /// <param name="subtitleEncode">字串，字幕的文字編碼，預設值為 "UTF-8"</param>
    /// <param name="applyFontSetting">布林值，套用字型設定，預設值為 false</param>
    /// <param name="subtitleStyle">字串，字幕的覆寫風格，預設值為 null</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <returns>Task</returns>
    public static async Task BurnInSubtitle(
        string videoFilePath,
        string subtitleFilePath,
        string outputPath,
        CancellationToken ct,
        string subtitleEncode = "UTF-8",
        bool applyFontSetting = false,
        string? subtitleStyle = null,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            string videoExtName = Path.GetExtension(videoFilePath);

            // 取得影片的資訊。
            IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(videoFilePath);

            switch (videoExtName)
            {
                case ".mp4":
                    IConversion conversionMP4 = ExternalProgram.GetBurnInSubtitleConversion(
                            mediaInfo,
                            subtitleFilePath,
                            outputPath,
                            subtitleEncode,
                            applyFontSetting,
                            subtitleStyle,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo);

                    IConversionResult conversionResultMP4 = await conversionMP4.Start(ct);

                    ExternalProgram.WriteConversionResult(conversionResultMP4);

                    break;
                case ".mkv":
                    // 取得字幕檔的資訊。
                    IMediaInfo subtitleInfo = await FFmpeg.GetMediaInfo(subtitleFilePath);

                    IConversion conversionMKV = ExternalProgram.GetAddSubtitleStreamConversion(
                            mediaInfo,
                            subtitleInfo,
                            outputPath,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo);

                    IConversionResult conversionResultMKV = await conversionMKV.Start(ct);

                    ExternalProgram.WriteConversionResult(conversionResultMKV);

                    break;
                default:
                    SerilogUtil.WriteLog("您選擇的視訊檔案為不支援的格式。", LogEventLevel.Warning);

                    break;
            }
        }
        catch (Exception ex)
        {
            SerilogUtil.WriteLog(ex.Message, LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 取得 Progress&lt;DownloadProgress&gt;
    /// </summary>
    /// <returns>Progress&lt;DownloadProgress&gt;</returns>
    private static Progress<DownloadProgress> GetDownloadProgress()
    {
        return new Progress<DownloadProgress>(downloadProgress =>
        {
            string message = $"({downloadProgress.State})";

            message += $" 影片索引值：{downloadProgress.VideoIndex}";
            message += $" 下載進度：{downloadProgress.Progress * 100}%";

            if (!string.IsNullOrEmpty(downloadProgress.DownloadSpeed))
            {
                message += $" 下載速度：{downloadProgress.DownloadSpeed}";
            }

            if (!string.IsNullOrEmpty(downloadProgress.ETA))
            {
                message += $" 剩餘時間：{downloadProgress.ETA}";
            }

            if (!string.IsNullOrEmpty(downloadProgress.TotalDownloadSize))
            {
                message += $" 檔案大小：{downloadProgress.TotalDownloadSize}";
            }

            if (!string.IsNullOrEmpty(downloadProgress.Data))
            {
                message += $" 資料：{downloadProgress.Data}";
            }

            switch (downloadProgress.State)
            {
                case DownloadState.None:
                    SerilogUtil.WriteLog(message, LogEventLevel.Verbose);
                    break;
                case DownloadState.PreProcessing:
                    SerilogUtil.WriteLog(message, LogEventLevel.Information);
                    break;
                case DownloadState.Downloading:
                    // 垃圾資訊。
                    SerilogUtil.WriteLog(message, LogEventLevel.Verbose);
                    break;
                case DownloadState.PostProcessing:
                    SerilogUtil.WriteLog(message, LogEventLevel.Information);
                    break;
                case DownloadState.Error:
                    // 在部份情況下是垃圾資訊。
                    SerilogUtil.WriteLog(message, LogEventLevel.Verbose);
                    break;
                case DownloadState.Success:
                    SerilogUtil.WriteLog(message, LogEventLevel.Information);
                    break;
                default:
                    break;
            }
        });
    }

    /// <summary>
    /// 取得 Progress&lt;string&gt;
    /// </summary>
    /// <returns>Progress&lt;string&gt;</returns>
    private static Progress<string> GetStringProgress()
    {
        string tempFrag = string.Empty,
            tempValue = string.Empty;

        return new Progress<string>(value =>
        {
            value = value.TrimEnd();

            // 手動減速機制，針對 "(frag " 開的字串進行減速。
            int starIdx = value.LastIndexOf("(frag ");

            if (starIdx > -1)
            {
                string fragPart = value[starIdx..]
                    .Replace("(frag ", string.Empty)
                    .Replace(")", string.Empty);

                if (tempFrag != fragPart)
                {
                    SerilogUtil.WriteLog(value, LogEventLevel.Information);

                    tempFrag = fragPart;
                }
            }
            else
            {
                // 手動減速機制，讓前後一樣的字串不輸出。
                if (value != tempValue)
                {
                    SerilogUtil.WriteLog(value, LogEventLevel.Information);
                }
            }

            tempValue = value;
        });
    }
}