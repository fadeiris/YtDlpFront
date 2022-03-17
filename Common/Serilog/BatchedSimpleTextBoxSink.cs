using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.WinForms.Base;
using YtDlpFront.Common.Sets;

namespace YtDlpFront.Common.Serilog;

/// <summary>
/// BatchedSimpleTextBoxSink
/// </summary>
public class BatchedSimpleTextBoxSink : IBatchedLogEventSink
{
    public BatchedSimpleTextBoxSink()
    {
        WindFormsSink.MakeSimpleTextBoxSink(
            new MessageTemplateTextFormatter(VariableSet.SerilogOutputTemplate1));
    }

    public Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        foreach (LogEvent logEvent in batch)
        {
            WindFormsSink.SimpleTextBoxSink.Emit(logEvent);
        }

        return Task.FromResult(0);
    }

    public Task OnEmptyBatchAsync()
    {
        return Task.FromResult(0);
    }
}