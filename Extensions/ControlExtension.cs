﻿namespace YtDlpFront.Extensions;

/// <summary>
/// Control Extension
/// <para>來源：https://dotblogs.com.tw/shinli/2015/04/16/151076 </para>
/// </summary>
public static class ControlExtension
{
    /// <summary>
    /// 非同步委派更新 UI
    /// </summary>
    /// <param name="control">Control</param>
    /// <param name="action">MethodInvoker</param>
    public static void InvokeIfRequired(this Control control, MethodInvoker action)
    {
        // 在非當前執行緒內，使用委派。
        if (control.InvokeRequired)
        {
            control.Invoke(action);
        }
        else
        {
            action();
        }
    }
}