using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;
using Serilog;
using Serilog.Events;

namespace YtDlpFront.Extensions;

/// <summary>
/// LoggerConfiguration Extension
/// <para>來源：https://github.com/serilog/serilog-sinks-periodicbatching </para>
/// </summary>
public static class LoggerConfigurationExtension
{
    /// <summary>
    /// BatchedSink
    /// </summary>
    /// <param name="loggerSinkConfiguration">LoggerSinkConfiguration</param>
    /// <param name="batchedLogEventSink">IBatchedLogEventSink</param>
    /// <param name="logEventLevel">LogEventLevel</param>
    /// <returns>LoggerConfiguration</returns>
    public static LoggerConfiguration BatchedSink(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        IBatchedLogEventSink batchedLogEventSink,
        LogEventLevel logEventLevel = LogEventLevel.Verbose)
    {
        return loggerSinkConfiguration.Sink(new PeriodicBatchingSink(
            batchedLogEventSink,
            new PeriodicBatchingSinkOptions()
            {
                BatchSizeLimit = 1000,
                Period = TimeSpan.FromSeconds(2),
                EagerlyEmitFirstEvent = true,
                QueueLimit = 100000
            }),
            restrictedToMinimumLevel: logEventLevel);
    }
}