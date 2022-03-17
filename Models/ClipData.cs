namespace YtDlpFront.Models;

/// <summary>
/// 類別：短片資料
/// </summary>
public class ClipData
{
    /// <summary>
    /// 短片編號
    /// </summary>
    public int ClipNo { get; set; }

    /// <summary>
    /// 短片名稱
    /// </summary>
    public string ClipName { get; set; } = string.Empty;

    /// <summary>
    /// 開始秒數
    /// </summary>
    public double StartSeconds { get; set; } = 0.0;

    /// <summary>
    /// 結束秒數
    /// </summary>
    public double EndSeconds { get; set; } = 0.0;

    /// <summary>
    /// 僅音訊
    /// </summary>
    public bool IsAudioOnly { get; set; } = false;
}