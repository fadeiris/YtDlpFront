using YtDlpFront.Models;
using System.ComponentModel;

namespace YtDlpFront;

// 阻擋設計工具。
partial class DesignerBlocker { }

public partial class MainForm
{
    private readonly ToolTip SharedToolTip = new();
    private readonly BindingList<ClipData> SharedDataSource = new();

    private static IHttpClientFactory? SharedHttpClientFactory;
    private static HttpClient? SharedHttpClient;

    private CancellationTokenSource? SharedCancellationTokenSource;
    private CancellationToken SharedCancellationToken;

    private bool IsInitialing = false;
}