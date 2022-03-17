using System.Management;
using YtDlpFront.Models;

namespace YtDlpFront.Common.Utils;

/// <summary>
/// 顯示卡工具
/// </summary>
public class VideoCardUtil
{
    /// <summary>
    /// 取得裝置的列表
    /// <para>來源：https://stackoverflow.com/questions/37521359/how-to-find-all-graphic-cards-c-sharp </para>
    /// </summary>
    /// <returns>List&lt;VideoCard&gt;</returns>
    public static List<VideoCardData> GetDeviceList()
    {
        List<VideoCardData> deviceList = new();

        ManagementObjectSearcher managementObjectSearcher = new("SELECT * FROM Win32_VideoController");

        foreach (ManagementObject managementObject in managementObjectSearcher.Get())
        {
            VideoCardData videoCard = new();

            foreach (PropertyData propertyData in managementObject.Properties)
            {
                if (propertyData.Name == "DeviceID")
                {
                    videoCard.DeviceNo = GetDeviceNo(propertyData.Value.ToString());
                }

                if (propertyData.Name == "Name")
                {
                    videoCard.DeviceName = $"[{videoCard.DeviceNo}] {propertyData.Value}";
                }
            }

            deviceList.Add(videoCard);
        }

        return deviceList;
    }

    /// <summary>
    /// 取得裝置的編號
    /// </summary>
    /// <param name="value">字串，值</param>
    /// <returns>數值，裝置的編號</returns>
    public static int GetDeviceNo(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }

        string rawDeviceNo = value.Replace("VideoController", string.Empty);

        if (int.TryParse(rawDeviceNo, out int deviceNo))
        {
            deviceNo--;
        }
        else
        {
            deviceNo = 0;
        }

        return deviceNo;
    }
}