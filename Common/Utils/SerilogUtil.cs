using Serilog;
using Serilog.Events;

namespace YtDlpFront.Common.Utils;

/// <summary>
/// Serilog 工具
/// </summary>
public static class SerilogUtil
{
    /// <summary>
    /// 共用的 ILogger
    /// </summary>
    public static readonly ILogger SharedLogger = Log.ForContext<MainForm>();

    /// <summary>
    /// 寫紀錄
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="logLevel">LogEventLevel，預設值為 LogEventLevel.Information</param>
    public static void WriteLog(string message, LogEventLevel logLevel = LogEventLevel.Information)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        switch (logLevel)
        {
            case LogEventLevel.Verbose:
                SharedLogger.Verbose(message);

                break;

            case LogEventLevel.Debug:
                SharedLogger.Debug(message);

                break;

            case LogEventLevel.Information:
                SharedLogger.Information(message);

                break;

            case LogEventLevel.Warning:
                SharedLogger.Warning(message);

                break;

            case LogEventLevel.Error:
                SharedLogger.Error(message);

                break;

            case LogEventLevel.Fatal:
                SharedLogger.Fatal(message);

                break;

            default:
                break;
        }
    }
}