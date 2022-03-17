using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File.Archive;
using System.IO.Compression;
using YtDlpFront.Common.Serilog;
using YtDlpFront.Common.Sets;
using YtDlpFront.Extensions;

namespace YtDlpFront;

internal static class Program
{
    private static IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        ConfigureServices();
        ConfigureSerilog();

        Application.Run((MainForm)ServiceProvider?.GetService(typeof(MainForm))!);
    }

    /// <summary>
    /// 設定服務
    /// <para>來源：https://docs.microsoft.com/zh-tw/archive/msdn-magazine/2019/may/net-core-3-0-create-a-centralized-pull-request-hub-with-winforms-in-net-core-3-0 </para>
    /// </summary>
    private static void ConfigureServices()
    {
        ServiceCollection services = new();

        services.AddHttpClient()
            .AddSingleton<MainForm>();

        ServiceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// 設定 Serilog
    /// <para>來源：https://github.com/umairsyed613/Serilog.Sinks.WinForms/blob/main/src/Sample/TestApplicationNet6/Program.cs </para>
    /// </summary>
    private static void ConfigureSerilog()
    {
        LoggerConfiguration loggerConfiguration = new();

        int fileLoglLevel = Properties.Settings.Default.FileLogLevel;

        switch (fileLoglLevel)
        {
            case 0:
                loggerConfiguration.MinimumLevel.Verbose();

                break;
            case 1:
                loggerConfiguration.MinimumLevel.Debug();

                break;
            case 2:
                loggerConfiguration.MinimumLevel.Information();

                break;
            case 3:
                loggerConfiguration.MinimumLevel.Warning();

                break;
            case 4:
                loggerConfiguration.MinimumLevel.Error();

                break;
            case 5:
                loggerConfiguration.MinimumLevel.Fatal();

                break;
            default:
                loggerConfiguration.MinimumLevel.Debug();

                break;
        }

        loggerConfiguration.Enrich.FromLogContext()
            // 輸出到 GUI。
            .WriteTo.Async(wt => wt.BatchedSink(
                new BatchedSimpleTextBoxSink(),
                LogEventLevel.Information))
            // 輸出到檔案。
            .WriteTo.Async(wt => wt.File(VariableSet.LogFile,
                outputTemplate: VariableSet.SerilogOutputTemplate2,
                buffered: true,
                rollingInterval: RollingInterval.Day,
                hooks: new ArchiveHooks(
                    CompressionLevel.SmallestSize,
                    VariableSet.LogArchivePath)));

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}