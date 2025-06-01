using UnityEngine;


public class DebugTool
{
    const bool ENABLED_INFO = true;
    const bool ENABLED_ERROR = true;
    const bool ENABLED_DEBUG = true;
    const bool DebugLogShortText = true;
    public static void Info(string message, Object @object)
    {
        if(ENABLED_INFO)
            UnityEngine.Debug.Log(GetDebugString("Info", message), @object);
    }
    public static void Info(string message)
    {
        if (ENABLED_INFO)
            UnityEngine.Debug.Log(GetDebugString("Info", message));
    }
    public static void Error(string message, Object @object)
    {
        if (ENABLED_ERROR)
            UnityEngine.Debug.LogError(GetDebugString("Error", message), @object);
    }
    public static void Error(string message)
    {
        if (ENABLED_ERROR)
            UnityEngine.Debug.LogError(GetDebugString("Error", message));
    }

    public static void Debug(string message, Object @object)
    {
        if (ENABLED_DEBUG)
            UnityEngine.Debug.LogError(GetDebugString("Debug", message), @object);
    }
    public static void Debug(string message)
    {
        if (ENABLED_DEBUG)
            UnityEngine.Debug.LogError(GetDebugString("Debug", message));
    }
    private static string GetDebugString(string tag, object log)
    {
        string logString = log != null ? log.ToString() : "null";
        if (DebugLogShortText) return string.Format("{0}> {1} ", tag, logString); ;
        return string.Format("{0}> {1} #### frame = {2}, time = {3}", tag, logString, Time.frameCount, Time.realtimeSinceStartup);
    }

}
