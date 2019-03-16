using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class ZarinpalLog
{
    private static bool _configCheckd;
    private static ZarinpalConfig _config;
    private static ZarinpalConfig Config
    {
        get
        {
            if (_config == null && !_configCheckd)
            {
                _config = Resources.Load<ZarinpalConfig>("ZarinpalSetting");
                _configCheckd = true;
            }
            return _config;
        }
    }
    [Conditional("DEBUG")]
    public static void Log(string log)
    {
        if (Config == null || Config.LogEnabled)
            Debug.Log(log);
    }

    [Conditional("DEBUG")]
    public static void Log(string log,params string[] args)
    {
        if (Config == null || Config.LogEnabled)
            Debug.Log(string.Format(log, args));
    }

    [Conditional("DEBUG")]
    public static void LogError(string log, params string[] args)
    {
        if (Config == null || Config.LogEnabled)
            Debug.LogError(string.Format(log, args));
    }

    [Conditional("DEBUG")]
    public static void LogError(string log)
    {
        if (Config == null || Config.LogEnabled)
            Debug.LogError(log);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(string log, params string[] args)
    {
        if (Config == null || Config.LogEnabled)
            Debug.LogWarning(string.Format(log, args));
    }
}
