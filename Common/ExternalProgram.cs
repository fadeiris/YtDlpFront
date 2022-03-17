using Serilog.Events;
using System.Diagnostics;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YtDlpFront.Common.Sets;
using YtDlpFront.Common.Utils;
using YtDlpFront.Extensions;

namespace YtDlpFront.Common;

/// <summary>
/// 外部程式
/// </summary>
public static class ExternalProgram
{
    /// <summary>
    /// 檢查 yt-dlp 的相依性檔案
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="control1">Label，LVersion</param>
    /// <param name="control2">Label，LYtDlpVer</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task CheckYtDlpDeps(
        HttpClient httpClient,
        Label control1,
        Label control2,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        // 檢查下載資料夾是否已存在。
        if (!Directory.Exists(VariableSet.VideoDownloadFolderPath))
        {
            // 建立下載資料夾。
            Directory.CreateDirectory(VariableSet.VideoDownloadFolderPath);

            SerilogUtil.WriteLog($"已建立下載資料夾，路徑：{VariableSet.VideoDownloadFolderPath}");
        }

        bool IsYtDlpExist = false;

        // 檢查 yt-dlp.exe。
        if (!File.Exists(VariableSet.YtDlpExePath))
        {
            if (httpClient != null)
            {
                await ExternalDownloader.DownloadYtDlp(httpClient, ct);
            }
            else
            {
                SerilogUtil.WriteLog("httpClient 是 null。", LogEventLevel.Warning);
            }
        }
        else
        {
            IsYtDlpExist = true;

            SerilogUtil.WriteLog("已找到 yt-dlp.exe。");
        }

        // 檢查 FFmpeg。
        if (!File.Exists(VariableSet.FFmpegExecPath) || !File.Exists(VariableSet.FFprobeExecPath))
        {
            if (httpClient != null)
            {
                await ExternalDownloader.DownloadFFmpeg(httpClient, ct);

                // 設定 FFmpeg 的執行檔路徑。
                FFmpeg.SetExecutablesPath(VariableSet.ExecPath);
            }
            else
            {
                SerilogUtil.WriteLog("httpClient 是 null。", LogEventLevel.Warning);
            }
        }
        else
        {
            // 設定 FFmpeg 的執行檔路徑。
            FFmpeg.SetExecutablesPath(VariableSet.ExecPath);

            SerilogUtil.WriteLog("已找到 ffmpeg.exe、ffprobe.exe。");
        }

        // 檢查 aria2。
        if (File.Exists(VariableSet.Aria2ExecPath))
        {
            SerilogUtil.WriteLog("已找到 aria2c.exe。");
        }

        // 設定顯示 yt-dlp 的版本號。
        SetYtDlpVer(control1, control2);

        // 若 yt-dlp 已存在時，則執行更新，
        // 並提示使用者需要手動更新 FFmpeg。
        if (IsYtDlpExist)
        {
            DateTime dtYtDlpCheckTime = Properties.Settings.Default.YtDlpCheckTime;

            // 先確認在設定檔案的更新檢查時間。
            if (DateTime.Now.Date > dtYtDlpCheckTime.Date)
            {
                // 當檢查時間比較早時，則重設 IsYtDlpChecked 的值為 false。
                Properties.Settings.Default.IsYtDlpChecked = false;
                Properties.Settings.Default.Save();
            }

            if (!Properties.Settings.Default.IsYtDlpChecked)
            {
                YoutubeDL ytdl = GetYoutubeDL();

                string result = await ytdl.RunUpdate();

                SerilogUtil.WriteLog(result);

                // 當 yt-dlp 更新後，才需要提示要更新 FFmpeg。
                if (result.Contains("Updated yt-dlp to version"))
                {
                    SerilogUtil.WriteLog("請手動點選 yt-dlp/FFmpeg-Builds 按鈕以更新 FFmpeg。");
                }

                // 設定顯示 yt-dlp 的版本號。
                SetYtDlpVer(control1, control2);

                Properties.Settings.Default.YtDlpCheckTime = DateTime.Now;
                Properties.Settings.Default.IsYtDlpChecked = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                string alreadyUpdatedMessage = $"已於 {dtYtDlpCheckTime:yyyy/MM/dd HH:mm:ss} " +
                    $"檢查 yt-dlp 是否有新版本，故今日不會再自動檢查；如有需要，請手動執行更新。";

                SerilogUtil.WriteLog(alreadyUpdatedMessage);
            }
        }
    }

    /// <summary>
    /// 取得 YoutubeDL
    /// </summary>
    /// <returns>YoutubeDL</returns>
    public static YoutubeDL GetYoutubeDL()
    {
        return new()
        {
            YoutubeDLPath = VariableSet.YtDlpExePath,
            FFmpegPath = VariableSet.FFmpegExecPath,
            IgnoreDownloadErrors = true,
            OverwriteFiles = true,
            OutputFolder = VariableSet.VideoDownloadFolderPath,
            RestrictFilenames = false
        };
    }

    /// <summary>
    /// 設定顯示 yt-dlp 的版本號
    /// </summary>
    /// <param name="control1">Label，LVersion</param>
    /// <param name="control2">Label，LYtDlpVer</param>
    public static void SetYtDlpVer(Label control1, Label control2)
    {
        control2.InvokeIfRequired(() =>
        {
            YoutubeDL ytdl = GetYoutubeDL();

            control2.Text = $"yt-dlp 版本：{ytdl.Version}";

            Point sourcePoint = control1.Location;

            int newX = sourcePoint.X + control1.Width + 4;

            control2.Location = new Point(newX, sourcePoint.Y);
        });
    }

    /// <summary>
    /// 取得預設的 OptionSet
    /// </summary>
    /// <param name="formatSelection">字串，格式</param>
    /// <param name="enableCookiesFromBrowser">布林值，載入網頁瀏覽器的 Cookies</param>
    /// <param name="browserName">字串，網頁瀏覽器的名稱</param>
    /// <param name="startSeconds">數值，開始秒數</param>
    /// <param name="endSeconds">數值，結束秒數</param>
    /// <param name="enableEmbedMeta">布林值，是否嵌入後設資訊，預設值為 false</param>
    /// <param name="enableLiveFromStart">布林值，是否使用 --live-from-start 參數，預設值為 false</param>
    /// <param name="enableEmbedSubs">布林值，是否嵌入字幕，預設值為 false</param>
    /// <param name="enableWaitForVideo">布林值，是否使用 --wait-for-video 參數，預設值為 false</param>
    /// <param name="enableSplitChapters">布林值，是否使用 --split-chapters 參數，預設值為 false</param>
    /// <returns>OptionSet</returns>
    public static OptionSet GetDefaultOptionSet(
        string formatSelection,
        bool enableCookiesFromBrowser,
        string browserName,
        double startSeconds,
        double endSeconds,
        bool enableEmbedMeta = false,
        bool enableLiveFromStart = false,
        bool enableEmbedSubs = false,
        bool enableWaitForVideo = false,
        bool enableSplitChapters = false)
    {
        // 因 yt-dlp.exe 的文字編碼機制問題，故採用下列格式的名稱。
        string outputTemplate = $"%(id)s_{DateTime.Now:yyyyMMddHHmmss}.%(ext)s";

        // 當 formatSelection 為空白或 null 時，手動設定預設值。
        if (string.IsNullOrEmpty(formatSelection))
        {
            // 來源：https://github.com/yt-dlp/yt-dlp#formatSelection-selection
            // 
            // -f bv*+ba/b
            //
            // 參考：https://github.com/yt-dlp/yt-dlp#format-selection-examples

            // 此處選一個比較通用的格式設定。（不會是最高畫質）
            formatSelection = "b/bv*+ba";
        }

        OptionSet optionSet = new()
        {
            Format = formatSelection,
            SkipUnavailableFragments = true,
            Output = Path.Combine(VariableSet.VideoDownloadFolderPath, outputTemplate),
            NoCacheDir = true
        };

        // 設定 --concurrent-fragments 參數，預設值是 1。
        optionSet.AddCustomOption("--concurrent-fragments", Properties.Settings.Default.ConcurrentFragments);
        // 設定 --sub-langs 參數。
        optionSet.AddCustomOption("--sub-langs", Properties.Settings.Default.SubLangs);

        // NOTICE: 2022-06-12 不能輕易設定此參數。
        //optionSet.AddCustomOption("--encoding", "UTF-8");

        // 判斷是否要載入 Cookies。
        if (enableCookiesFromBrowser)
        {
            optionSet.AddCustomOption("--cookies-from-browser", browserName);
        }

        // 判斷是否要使用 --live-from-start 參數。
        if (enableLiveFromStart)
        {
            optionSet.AddCustomOption("--live-from-start", true);
        }

        // 計算間隔秒數。
        double durationSeconds = startSeconds - endSeconds;

        // 當 durationSeconds 為負數時，執行下載影片的片段。
        if (durationSeconds < 0)
        {
            // 參考來源：https://www.reddit.com/r/youtubedl/wiki/howdoidownloadpartsofavideo/
            optionSet.ExternalDownloader = "ffmpeg";
            optionSet.ExternalDownloaderArgs = $"ffmpeg_i:-ss {startSeconds} -to {endSeconds}";

            // 為避免 Xabe.FFmpeg 轉檔時發生錯誤，故停用下列設定。
            optionSet.AddMetadata = false;
            optionSet.EmbedThumbnail = false;
            optionSet.EmbedSubs = false;
        }
        else
        {
            // 預設使用 "native"
            optionSet.ExternalDownloader = "native";
            // 2022-10-08 看起來 ExternalDownloaderArgs 的預設值是針對 ffmpeg 設定的，
            // 設為 null 的話則會使用預設值。
            optionSet.ExternalDownloaderArgs = string.Empty;

            // 判斷有沒有使用 --live-from-start 參數。
            if (!enableLiveFromStart)
            {
                // 當 aria2c.exe 存在時，使用 aria2  作為外部下載器。
                if (File.Exists(VariableSet.Aria2ExecPath))
                {
                    optionSet.ExternalDownloader = VariableSet.Aria2ExecPath;
                    optionSet.ExternalDownloaderArgs = $"aria2c:--no-netrc=false";
                }
            }

            optionSet.AddMetadata = enableEmbedMeta;
            optionSet.EmbedThumbnail = enableEmbedMeta;

            // 2022-07-23 加入判斷應用程式設定，用以控制是否要嵌入字幕檔案在影片內。
            if (Properties.Settings.Default.EnableEmbedSubs)
            {
                optionSet.EmbedSubs = enableEmbedSubs;
            }

            // 判斷是否要使用 --split-chapters 參數。
            if (enableSplitChapters)
            {
                string pathForChapter = Path.Combine(
                    VariableSet.VideoDownloadFolderPath,
                    @"%(uploader)s_%(title)s-%(id)s\%(section_number)03d.%(section_title)s.%(ext)s");

                // 範例用影片：https://www.youtube.com/watch?v=beNSZ5LTm2M
                optionSet.AddCustomOption("--split-chapters", true);
                optionSet.AddCustomOption("-o", $"chapter:{pathForChapter}");
            }
        }

        // 判斷是否要使用 --wait-for-video 參數。
        if (enableWaitForVideo)
        {
            // 預設等待 3 秒。
            optionSet.AddCustomOption("--wait-for-video", Properties.Settings.Default.WaitSeconds);
        }

        return optionSet;
    }

    /// <summary>
    /// 取得 IConversion
    /// </summary>
    /// <param name="mediaInfo">IMediaInfo</param>
    /// <param name="startSeconds">數值，開始秒數</param>
    /// <param name="endSeconds">數值，結束秒數</param>
    /// <param name="output">字串，檔案輸出的路徑</param>
    /// <param name="fixDuration">布林值，是否執行使用步驟，預設值為 false</param>
    /// <param name="useCodecCopy">布林值，是否使用 -c:v copy -c:a copy，預設值為 true</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <param name="isAudioOnly">布林值，是否僅音訊，預設為 false</param>
    /// <returns>IConversion</returns>
    public static IConversion GetConversion(
        IMediaInfo mediaInfo,
        double startSeconds,
        double endSeconds,
        string output,
        bool fixDuration = false,
        bool useCodecCopy = true,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0,
        bool isAudioOnly = false)
    {
        IConversion conversion = FFmpeg.Conversions.New()
            // 來源：https://trac.ffmpeg.org/wiki/Encode/YouTube
            // 因會產生錯誤，故取消。
            //.AddParameter("-pix_fmt yuv420p")
            .AddParameter("-movflags +faststart")
            .SetOutput(output)
            .SetOverwriteOutput(true);

        if (useHardwareAcceleration)
        {
            SetHardwareAccelerator(
                conversion,
                hardwareAcceleratorType,
                deviceNo);
        }

        if (fixDuration)
        {
            double durationSeconds = startSeconds - endSeconds;

            conversion.AddParameter($"-sseof {durationSeconds}", ParameterPosition.PreInput);

            if (!isAudioOnly)
            {
                conversion.AddStream(mediaInfo.VideoStreams);
            }

            conversion.AddStream(mediaInfo.AudioStreams);
        }
        else
        {
            List<IStream> streams = new();

            if (!isAudioOnly)
            {
                foreach (VideoStream stream in mediaInfo.VideoStreams.Cast<VideoStream>())
                {
                    if (useCodecCopy)
                    {
                        streams.Add(stream.SetCodec(VideoCodec.copy));
                    }
                    else
                    {
                        streams.Add(stream);
                    }
                }
            }

            foreach (AudioStream stream in mediaInfo.AudioStreams.Cast<AudioStream>())
            {
                if (useCodecCopy)
                {
                    streams.Add(stream.SetCodec(AudioCodec.copy));
                }
                else
                {
                    streams.Add(stream);
                }
            }

            conversion.AddStream(streams)
                .SetSeek(TimeSpan.FromSeconds(startSeconds))
                .SetInputTime(TimeSpan.FromSeconds(endSeconds));
        }

        conversion.OnDataReceived += (object sender, DataReceivedEventArgs e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                SerilogUtil.WriteLog($"[Xabe.FFmpeg][資料接收]：{e.Data}");
            }
        };

        conversion.OnProgress += (object sender, ConversionProgressEventArgs args) =>
        {
            int percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);

            string message = $"[Xabe.FFmpeg][進度]：處理識別：{args.ProcessId} " +
                $"[{args.Duration}/{args.TotalLength}] {percent}%";

            SerilogUtil.WriteLog(message);
        };

        return conversion;
    }

    /// <summary>
    /// 取得燒錄字幕的 IConversion
    /// </summary>
    /// <param name="mediaInfo">IMediaInfo，影片的資訊</param>
    /// <param name="subtitlePath">字串，字幕檔案的路徑</param>
    /// <param name="outputPath">字串，輸出檔案的路徑</param>
    /// <param name="subtitleEncode">字串，字幕的文字編碼，預設值為 "UTF-8"</param>
    /// <param name="applyFontSetting">布林值，套用字型設定，預設值為 false</param>
    /// <param name="subtitleStyle">字串，字幕的覆寫風格，預設值為 null</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <returns>IConversion</returns>
    public static IConversion GetBurnInSubtitleConversion(
        IMediaInfo mediaInfo,
        string subtitlePath,
        string outputPath,
        string subtitleEncode = "UTF-8",
        bool applyFontSetting = false,
        string? subtitleStyle = null,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        bool isSubtitleAdded = false;

        List<IStream> outputStreams = new();

        // 循例每一個 IStream。
        foreach (IStream stream in mediaInfo.Streams)
        {
            if (stream.StreamType == StreamType.Video)
            {
                // 將 IStream 轉換成 IVideoStream。
                IVideoStream? videoStream = stream as IVideoStream;

                if (!isSubtitleAdded)
                {
                    // 判斷是否有套用字型設定。
                    if (!applyFontSetting)
                    {
                        subtitleStyle = null;
                    }

                    // 針對該 IVideoStream 燒錄字幕。
                    videoStream?.AddSubtitles(
                        subtitlePath,
                        encode: subtitleEncode,
                        style: subtitleStyle);

                    // 變更開關作用變數的值。
                    isSubtitleAdded = true;
                }

                if (videoStream != null)
                {
                    // 將 IVideoStream 加入 outputStreams。
                    outputStreams.Add(videoStream);
                }
            }
            else
            {
                // 將 IStream 加入 outputStreams。
                outputStreams.Add(stream);
            }
        }

        IConversion conversion = FFmpeg.Conversions.New()
            // 來源：https://trac.ffmpeg.org/wiki/Encode/YouTube
            // 因會產生錯誤，故取消。
            //.AddParameter("-pix_fmt yuv420p")
            .AddParameter("-movflags +faststart")
            .AddStream(outputStreams)
            .SetOutput(outputPath)
            .SetOverwriteOutput(true);

        if (useHardwareAcceleration)
        {
            SetHardwareAccelerator(
                conversion,
                hardwareAcceleratorType,
                deviceNo);
        }

        conversion.OnDataReceived += (object sender, DataReceivedEventArgs e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                SerilogUtil.WriteLog($"[Xabe.FFmpeg][資料接收]：{e.Data}");
            }
        };

        conversion.OnProgress += (object sender, ConversionProgressEventArgs args) =>
        {
            int percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);

            string message = $"[Xabe.FFmpeg][進度]：處理識別：{args.ProcessId} " +
                $"[{args.Duration}/{args.TotalLength}] {percent}%";

            SerilogUtil.WriteLog(message);
        };

        return conversion;
    }

    /// <summary>
    /// 取得加入 ISubtitleStream 的 IConversion
    /// </summary>
    /// <param name="mediaInfo">IMediaInfo，影片的資訊</param>
    /// <param name="subtitleInfo">IMediaInfo，字幕檔的資訊</param>
    /// <param name="outputPath">字串，輸出檔案的路徑</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <returns>IConversion</returns>
    public static IConversion GetAddSubtitleStreamConversion(
        IMediaInfo mediaInfo,
        IMediaInfo subtitleInfo,
        string outputPath,
        bool useHardwareAcceleration = false,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        // 取得 ISubtitleStream。
        IEnumerable<ISubtitleStream> subtitleStream = subtitleInfo.SubtitleStreams;

        IConversion conversion = FFmpeg.Conversions.New()
            // 來源：https://trac.ffmpeg.org/wiki/Encode/YouTube
            // 因會產生錯誤，故取消。
            //.AddParameter("-pix_fmt yuv420p")
            .AddParameter("-movflags +faststart")
            .AddStream(mediaInfo.Streams)
            .AddStream(subtitleStream)
            .SetOutput(outputPath)
            .SetOverwriteOutput(true);

        if (useHardwareAcceleration)
        {
            SetHardwareAccelerator(
                conversion,
                hardwareAcceleratorType,
                deviceNo);
        }

        conversion.OnDataReceived += (object sender, DataReceivedEventArgs e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                SerilogUtil.WriteLog($"[Xabe.FFmpeg][資料接收]：{e.Data}");
            }
        };

        conversion.OnProgress += (object sender, ConversionProgressEventArgs args) =>
        {
            int percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);

            string message = $"[Xabe.FFmpeg][進度]：處理識別：{args.ProcessId} " +
                $"[{args.Duration}/{args.TotalLength}] {percent}%";

            SerilogUtil.WriteLog(message);
        };

        return conversion;
    }

    /// <summary>
    /// 對 IConversion 設定硬體加速相關參數
    /// </summary>
    /// <param name="conversion">IConversion</param>
    /// <param name="hardwareAcceleratorType">VariableSet.HardwareAcceleratorType，硬體的類型，預設是 VariableSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    public static void SetHardwareAccelerator(
        IConversion conversion,
        VariableSet.HardwareAcceleratorType hardwareAcceleratorType = VariableSet.HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        switch (hardwareAcceleratorType)
        {
            case VariableSet.HardwareAcceleratorType.Intel:
                conversion.UseHardwareAcceleration(
                    nameof(HardwareAccelerator.d3d11va),
                    nameof(VideoCodec.h264),
                    "h264_qsv",
                    deviceNo);

                break;
            case VariableSet.HardwareAcceleratorType.NVIDIA:
                conversion.UseHardwareAcceleration(
                    HardwareAccelerator.d3d11va,
                    VideoCodec.h264,
                    VideoCodec.h264_nvenc,
                    deviceNo);

                break;
            case VariableSet.HardwareAcceleratorType.AMD:
                conversion.UseHardwareAcceleration(
                    nameof(HardwareAccelerator.d3d11va),
                    nameof(VideoCodec.h264),
                    "h264_amf",
                deviceNo);

                break;
            default:
                conversion.UseHardwareAcceleration(
                    nameof(HardwareAccelerator.d3d11va),
                    nameof(VideoCodec.h264),
                    "h264_qsv",
                    deviceNo);

                break;
        };
    }

    /// <summary>
    /// 輸出 IConversionResult
    /// </summary>
    /// <param name="conversionResult">IConversionResult</param>
    public static void WriteConversionResult(IConversionResult conversionResult)
    {
        string message = $"[Xabe.FFmpeg] 作業完成。{Environment.NewLine}" +
            $"開始時間：{conversionResult.StartTime}{Environment.NewLine}" +
            $"結束時間：{conversionResult.EndTime}{Environment.NewLine}" +
            $"耗時：{conversionResult.Duration}{Environment.NewLine}" +
            $"使用的參數：{conversionResult.Arguments}";

        SerilogUtil.WriteLog(message);
    }
}