using Serilog.Events;
using System.IO.Compression;
using YtDlpFront.Common.Sets;
using YtDlpFront.Common.Utils;
using YtDlpFront.Extensions;

namespace YtDlpFront.Common;

/// <summary>
/// 外部程式下載器
/// </summary>
public static class ExternalDownloader
{
    /// <summary>
    /// 下載 yt-dlp
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadYtDlp(HttpClient httpClient, CancellationToken ct)
    {
        if (!Directory.Exists(VariableSet.ExecPath))
        {
            Directory.CreateDirectory(VariableSet.ExecPath);

            SerilogUtil.WriteLog($"已建立 Bin 資料夾，路徑：{VariableSet.ExecPath}");
        }

        using FileStream fileStream = new(
            VariableSet.YtDlpExePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);

        Progress<float> progressFloat = new();

        progressFloat.ProgressChanged += (object? sender, float progress) =>
        {
            if (progress >= 100f)
            {
                SerilogUtil.WriteLog("已下載 yt-dlp.exe。");
            }
        };

        Progress<string> progressString = new();

        progressString.ProgressChanged += (object? sender, string progress) =>
        {
            SerilogUtil.WriteLog($"yt-dlp.exe 下載進度：{progress}");
        };

        await httpClient.DownloadDataAsync(
            VariableSet.YtDlpBinaryUrl,
            fileStream,
            progressFloat,
            progressString,
            cancellationToken: ct);
    }

    /// <summary>
    /// 下載 FFmpeg
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadFFmpeg(HttpClient httpClient, CancellationToken ct)
    {
        if (!Directory.Exists(VariableSet.ExecPath))
        {
            Directory.CreateDirectory(VariableSet.ExecPath);

            SerilogUtil.WriteLog($"已建立 Bin 資料夾，路徑：{VariableSet.ExecPath}");
        }

        using FileStream fileStream = new(
            VariableSet.FFmpegZipSavedPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);

        Progress<float> progressFloat = new();

        progressFloat.ProgressChanged += async (object? sender, float progress) =>
        {
            if (progress >= 100f)
            {
                fileStream.Close();
                fileStream.Dispose();

                await UnZip(VariableSet.ExteralProgram.FFmpeg);
            }
        };

        Progress<string> progressString = new();

        progressString.ProgressChanged += (object? sender, string progress) =>
        {
            SerilogUtil.WriteLog($"FFmpeg 下載進度：{progress}");
        };

        await httpClient.DownloadDataAsync(
            VariableSet.FFmpegZipUrl,
            fileStream,
            progressFloat,
            progressString,
            cancellationToken: ct);
    }

    /// <summary>
    /// 下載 aria2
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadAria2(HttpClient httpClient, CancellationToken ct)
    {
        if (!Directory.Exists(VariableSet.ExecPath))
        {
            Directory.CreateDirectory(VariableSet.ExecPath);

            SerilogUtil.WriteLog($"已建立 Bin 資料夾，路徑：{VariableSet.ExecPath}");
        }

        using FileStream fileStream = new(
            VariableSet.Aria2ZipSavedPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);

        Progress<float> progressFloat = new();

        progressFloat.ProgressChanged += async (object? sender, float progress) =>
        {
            if (progress >= 100f)
            {
                fileStream.Close();
                fileStream.Dispose();

                await UnZip(VariableSet.ExteralProgram.aria2);
            }
        };

        Progress<string> progressString = new();

        progressString.ProgressChanged += (object? sender, string progress) =>
        {
            SerilogUtil.WriteLog($"aria2 下載進度：{progress}");
        };

        await httpClient.DownloadDataAsync(
            VariableSet.Aria2ZipUrl,
            fileStream,
            progressFloat,
            progressString,
            cancellationToken: ct);
    }

    /// <summary>
    /// 解壓縮壓縮檔
    /// </summary>
    /// <param name="exteralProgram">VariableSet.ExteralProgram</param>
    /// <returns>Task</returns>
    public static async Task UnZip(VariableSet.ExteralProgram exteralProgram)
    {
        // 解壓縮下載的壓縮檔。
        await Task.Run(() =>
        {
            // 來源：https://docs.microsoft.com/zh-tw/dotnet/standard/io/how-to-compress-and-extract-files

            // Normalizes the path.
            string execPath = Path.GetFullPath(VariableSet.ExecPath);

            if (!Directory.Exists(execPath))
            {
                Directory.CreateDirectory(execPath);

                SerilogUtil.WriteLog($"已建立 Bin 資料夾，路徑：{execPath}");
            }

            // Ensures that the last character on the extraction path
            // is the directory separator char.
            // Without this, a malicious zip file could try to traverse outside of the expected
            // extraction path.
            if (!execPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                execPath += Path.DirectorySeparatorChar;
            }

            ZipArchive? zipArchive = exteralProgram switch
            {
                VariableSet.ExteralProgram.FFmpeg =>
                    ZipFile.OpenRead(VariableSet.FFmpegZipSavedPath),
                VariableSet.ExteralProgram.aria2 =>
                    ZipFile.OpenRead(VariableSet.Aria2ZipSavedPath),
                _ => null
            };

            if (zipArchive != null)
            {
                int itemCount = 0, targetCount = zipArchive.Entries.Count;

                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    string fileName = Path.GetFileName(zipArchiveEntry.FullName);

                    // 只解壓縮出需要的壓縮檔。
                    switch (exteralProgram)
                    {
                        case VariableSet.ExteralProgram.FFmpeg:
                            if (fileName.EndsWith("ffmpeg.exe", StringComparison.OrdinalIgnoreCase) ||
                                fileName.EndsWith("ffprobe.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                // Gets the full path to ensure that relative segments are removed.
                                string destinationPath = Path.GetFullPath(Path.Combine(execPath, fileName));

                                // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                                // are case-insensitive.
                                if (destinationPath.StartsWith(execPath, StringComparison.Ordinal))
                                {
                                    zipArchiveEntry.ExtractToFile(destinationPath, true);

                                    SerilogUtil.WriteLog($"已解壓縮 {fileName}。");
                                }
                            }

                            break;
                        case VariableSet.ExteralProgram.aria2:
                            if (fileName.EndsWith("aria2c.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                // Gets the full path to ensure that relative segments are removed.
                                string destinationPath = Path.GetFullPath(Path.Combine(execPath, fileName));

                                // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                                // are case-insensitive.
                                if (destinationPath.StartsWith(execPath, StringComparison.Ordinal))
                                {
                                    zipArchiveEntry.ExtractToFile(destinationPath, true);

                                    SerilogUtil.WriteLog($"已解壓縮 {fileName}。");
                                }
                            }

                            break;
                        default:
                            break;
                    };

                    itemCount++;
                }

                // 先釋放 zipArchive。
                zipArchive.Dispose();
                zipArchive = null;

                // 當循例到最後一個項目時。
                if (itemCount == targetCount)
                {
                    // 刪除下載的壓縮檔。
                    switch (exteralProgram)
                    {
                        case VariableSet.ExteralProgram.FFmpeg:
                            DeleteFile(VariableSet.FFmpegZipSavedPath);

                            break;
                        case VariableSet.ExteralProgram.aria2:
                            DeleteFile(VariableSet.Aria2ZipSavedPath);

                            break;
                        default:
                            break;
                    };
                }
            }
            else
            {
                SerilogUtil.WriteLog("解壓縮失敗，zipArchive 是 null。", LogEventLevel.Warning);
            }
        });
    }

    /// <summary>
    /// 刪除檔案
    /// </summary>
    /// <param name="filePath">字串，檔案名稱</param>
    /// <param name="maximumAttempt">數值，最大重試次數，預設值是 3</param>
    public static void DeleteFile(string filePath, int maximumAttempt = 3)
    {
        for (int i = 0; i < maximumAttempt; i++)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    SerilogUtil.WriteLog(ex.Message, LogEventLevel.Error);
                }

                // 待睡眠 1 秒後再次執行刪除一次。
                Thread.Sleep(1000);
            }
            else
            {
                break;
            }
        }

        // 在迴圈結束後，判斷檔案是否還是存在。
        if (File.Exists(filePath))
        {
            SerilogUtil.WriteLog($"已重試 {maximumAttempt} 次刪除檔案皆失敗，請手動刪除檔案：{filePath}");
        }
        else
        {
            SerilogUtil.WriteLog($"已刪除：{filePath}");
        }
    }
}