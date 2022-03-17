using System.Diagnostics;
using System.Text.RegularExpressions;
using YoutubeDLSharp.Options;
using YtDlpFront.Extensions;

namespace YtDlpFront.Common;

/// <summary>
/// 自定義函式
/// </summary>
public static partial class CustomFunction
{
    /// <summary>
    /// Regex 判斷 ASCII 換行字元
    /// <para>來源：https://social.msdn.microsoft.com/Forums/en-US/34ddba13-d352-4a55-b144-1cf75c2f954d/form-view-c-how-to-replace-carriage-return-with-ltbrgt?forum=aspwebformsdata </para>
    /// </summary>
    public static readonly Regex RegexAscii = new(@"(\r\n|\r|\n)+");

    /// <summary>
    /// 取得時間標記
    /// <para>來源：https://stackoverflow.com/a/463668 </para>
    /// </summary>
    /// <param name="seconds">數值，秒數</param>
    /// <returns>字串</returns>
    public static string GetTimestamp(double seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        // Here backslash is must to tell that colon is
        // not the part of formatSelection, it just a character that we want in output.
        return timeSpan.ToString(@"hh\:mm\:ss\.fff");
    }

    /// <summary>
    /// 替換無效的字元
    /// <para>來源：https://stackoverflow.com/a/23182807 </para>
    /// </summary>
    /// <param name="value">字串，輸入值<param>
    /// <returns>字串</returns>
    public static string ReplaceInvalidChars(string value)
    {
        return string.Join("_", value.Split(Path.GetInvalidFileNameChars()));
    }

    /// <summary>
    /// 檢查秒數是否符合規則
    /// </summary>
    /// <param name="startSeconds">數值，開始秒數</param>
    /// <param name="endSeconds">數值，結束秒數</param>
    /// <returns>字串</returns>
    public static string IsSecondsValid(double startSeconds, double endSeconds)
    {
        string errorMessage = string.Empty;

        if (startSeconds >= 0.0 && endSeconds >= 0.0)
        {
            if (startSeconds == endSeconds)
            {
                // 當開始與結束秒數為不為 0.0 的等值時。
                if (startSeconds != 0.0)
                {
                    errorMessage += $"短片的開始時間不可等於結束時間。{Environment.NewLine}";
                }
            }

            if (startSeconds > endSeconds)
            {
                errorMessage += $"短片的開始時間不可晚於結束時間。{Environment.NewLine}";
            }
        }

        return errorMessage;
    }

    /// <summary>
    /// Returns the human-readable file size for an arbitrary, 64-bit file size
    /// <para>The default formatSelection is "0.### XB", e.g. "4.2 KB" or "1.434 GB"</para>
    /// <para>來源：https://stackoverflow.com/a/11124118 </para>
    /// </summary>
    /// <param name="value">數值，輸入值</param>
    /// <returns>字串</returns>
    public static string GetBytesReadable(long value)
    {
        // Get absolute value.
        long absolute_i = value < 0 ? -value : value;

        // Determine the suffix and readable value.
        string suffix;

        double readable;

        if (absolute_i >= 0x1000000000000000)
        {
            // Exabyte.
            suffix = "EB";

            readable = value >> 50;
        }
        else if (absolute_i >= 0x4000000000000)
        {
            // Petabyte.
            suffix = "PB";

            readable = value >> 40;
        }
        else if (absolute_i >= 0x10000000000)
        {
            // Terabyte.
            suffix = "TB";

            readable = value >> 30;
        }
        else if (absolute_i >= 0x40000000)
        {
            // Gigabyte.
            suffix = "GB";

            readable = value >> 20;
        }
        else if (absolute_i >= 0x100000)
        {
            // Megabyte.
            suffix = "MB";

            readable = value >> 10;
        }
        else if (absolute_i >= 0x400)
        {
            // Kilobyte.
            suffix = "KB";

            readable = value;
        }
        else
        {
            // Byte.
            return value.ToString("0 B");
        }

        // Divide by 1024 to get fractional value.
        readable /= 1024;

        // Return formatted number with suffix.
        return readable.ToString("0.### ") + suffix;
    }

    /// <summary>
    /// 取得自定義的 OptionSet
    /// </summary>
    /// <param name="formatSelection">字串，格式</param>
    /// <param name="control1">CheckBox，CBEnableLoadCookiesFromBrowser</param>
    /// <param name="control2">CheckBox，CBBrowser</param>
    /// <param name="control3">TextBox，TBBrowserProfile</param>
    /// <param name="startSeconds">數值，開始秒數，預設值為 0.0</param>
    /// <param name="endSeconds">數值，結束秒數，預設值為 0.0</param>
    /// <param name="enableEmbedMeta">布林值，是否崁入後設資訊，預設值為 false</param>
    /// <param name="enableLiveFromStart">布林值，是否使用 --live-from-start 參數，預設值為 false</param>
    /// <param name="enableEmbedSubs">布林值，是否嵌入字幕，預設值為 false</param>
    /// <param name="enableWaitForVideo">布林值，是否使用 --wait-for-video 參數，預設值為 false</param>
    /// <param name="enableSplitChapters">布林值，是否使用 --split-chapters 參數，預設值為 false</param>
    /// <returns>OptionSet</returns>
    public static OptionSet GetCustomOptionSet(
        string formatSelection,
        CheckBox control1,
        ComboBox control2,
        TextBox control3,
        double startSeconds = 0.0,
        double endSeconds = 0.0,
        bool enableEmbedMeta = false,
        bool enableLiveFromStart = false,
        bool enableEmbedSubs = false,
        bool enableWaitForVideo = false,
        bool enableSplitChapters = false)
    {
        bool enableCookiesFromBrowser = false;

        control1.InvokeIfRequired(() =>
        {
            enableCookiesFromBrowser = control1.Checked;
        });

        string browserName = string.Empty;

        if (enableCookiesFromBrowser)
        {
            control2.InvokeIfRequired(() =>
            {
                browserName = control2.Text;
            });

            if (!string.IsNullOrEmpty(control3.Text))
            {
                control3.InvokeIfRequired(() =>
                {
                    browserName += $":{control3.Text}";
                });
            }
        }

        return ExternalProgram
            .GetDefaultOptionSet(
                formatSelection,
                enableCookiesFromBrowser,
                browserName,
                startSeconds,
                endSeconds,
                enableEmbedMeta,
                enableLiveFromStart,
                enableEmbedSubs,
                enableWaitForVideo,
                enableSplitChapters);
    }

    /// <summary>
    /// 開啟網址
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>Process</returns>
    public static Process? OpenUrl(string url)
    {
        return Process.Start(new ProcessStartInfo()
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}