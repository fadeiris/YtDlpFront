using System.Text.Json;

namespace YtDlpFront.Common.Sets;

/// <summary>
/// 變數組
/// </summary>
public static class VariableSet
{
    /// <summary>
    /// 應用程式的名稱
    /// </summary>
    public static string AppName { get; set; } = "簡易 yt-dlp 前端";

    /// <summary>
    /// 列舉：外部應用程式
    /// </summary>
    public enum ExteralProgram
    {
        FFmpeg,
        aria2
    };

    /// <summary>
    /// 列舉：硬體加速類型
    /// </summary>
    public enum HardwareAcceleratorType
    {
        Intel,
        NVIDIA,
        AMD
    };

    /// <summary>
    /// 預設附加秒數
    /// </summary>
    public static readonly double AppendSeconds = Properties.Settings.Default.AppendSeconds;

    /// <summary>
    /// 執行檔路徑
    /// </summary>
    public static readonly string ExecPath = Path.Combine(AppContext.BaseDirectory, "Bin");

    /// <summary>
    /// yt-dlp.exe 路徑
    /// </summary>
    public static readonly string YtDlpExePath = Path.Combine(ExecPath, "yt-dlp.exe");

    /// <summary>
    /// ffmpeg.exe 路徑
    /// </summary>
    public static readonly string FFmpegExecPath = Path.Combine(ExecPath, "ffmpeg.exe");

    /// <summary>
    /// ffprobe.exe 路徑
    /// </summary>
    public static readonly string FFprobeExecPath = Path.Combine(ExecPath, "ffprobe.exe");

    /// <summary>
    /// aria2c.exe 路徑
    /// </summary>
    public static readonly string Aria2ExecPath = Path.Combine(ExecPath, "aria2c.exe");

    /// <summary>
    /// 影片下載資料夾路徑
    /// </summary>
    public static readonly string VideoDownloadFolderPath = Path.Combine(AppContext.BaseDirectory, "Download");

    /// <summary>
    /// yt-dlp 下載網址
    /// </summary>
    public static readonly string YtDlpBinaryUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

    /// <summary>
    /// yt-dlp FFmpeg 下載網址
    /// </summary>
    public static readonly string FFmpegZipUrl = "https://github.com/yt-dlp/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip";

    /// <summary>
    /// Aria2 版本
    /// <para>※未來需要手動更新此變數的值。</para>
    /// </summary>
    public static readonly string Aria2Version = Properties.Settings.Default.Aria2Version;

    /// <summary>
    /// aria2 下載網址
    /// <para>參考：https://docs.github.com/en/repositories/releasing-projects-on-github/linking-to-releases </para>
    /// <para>※未來需要手動更新此網址。</para>
    /// </summary>
    public static readonly string Aria2ZipUrl = $"https://github.com/aria2/aria2/releases/latest/download/aria2-{Aria2Version}-win-64bit-build1.zip";

    /// <summary>
    /// FFmpeg 的下載檔案路徑
    /// </summary>
    public static readonly string FFmpegZipSavedPath = Path.Combine(ExecPath, "ffmpeg-master-latest-win64-gpl.zip");

    /// <summary>
    /// aria2 的下載檔案路徑
    /// <para>※未來需要手動更新此變數的值。</para>
    /// </summary>
    public static readonly string Aria2ZipSavedPath = Path.Combine(ExecPath, $"aria2-{Aria2Version}-win-64bit-build1.zip");

    /// <summary>
    /// Log 檔案的路徑
    /// </summary>
    public static readonly string BaseLogPath = Path.Combine(AppContext.BaseDirectory, "Logs");

    /// <summary>
    /// Log 檔案路徑
    /// </summary>
    public static readonly string LogFile = Path.Combine(BaseLogPath, "log.txt");

    /// <summary>
    /// Log 檔案歸檔的路徑
    /// </summary>
    public static readonly string LogArchivePath = Path.Combine(BaseLogPath, "Archives");

    /// <summary>
    /// Serilog 的輸出模板 1
    /// </summary>
    public static readonly string SerilogOutputTemplate1 = "[{Timestamp:yyyy/MM/dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// Serilog 的輸出模板 2
    /// </summary>
    public static readonly string SerilogOutputTemplate2 = "[{Timestamp:yyyy/MM/dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 共享的 JsonSerializerOptions
    /// </summary>
    public static readonly JsonSerializerOptions SharedJsonSerializerOptions = new()
    {
        // 忽略掉註解。
        ReadCommentHandling = JsonCommentHandling.Skip,
        WriteIndented = true
    };
}