using System.ComponentModel;
using System.Text.Json.Serialization;

namespace YtDlpFront.Models;

/// <summary>
/// 類別：時間標記歌曲資料
/// </summary>
public class TimestampSongData
{
    /// <summary>
    /// 影片 ID
    /// </summary>
    [JsonPropertyName("videoID")]
    [Description("影片 ID")]
    public string? VideoID { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [JsonPropertyName("name")]
    [Description("名稱")]
    public string? Name { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    [JsonPropertyName("startTime")]
    [Description("開始時間")]
    public TimeSpan? StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    [JsonPropertyName("endTime")]
    [Description("結束時間")]
    public TimeSpan? EndTime { get; set; }

    /// <summary>
    /// 字幕檔案來源
    /// </summary>
    [JsonPropertyName("subSrc")]
    [Description("字幕檔案來源")]
    public string? SubSrc { get; set; }
}