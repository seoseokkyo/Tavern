using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LogMarker : MonoBehaviour
{
    private static LogMarker instance;

    public Text logText;
    private List<string> logs = new List<string>();
    private const int maxLogs = 30;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string fullLog;

        if (type == LogType.Exception)
        {
            fullLog = $"<color=red><size=14>{logString}</size></color>\n<color=grey><size=10>{stackTrace}</size></color>";
        }
        else
        {
            fullLog = $"<size=12>{logString}</size>";
        }

        logs.Add(fullLog);
        if (logs.Count > maxLogs)
        {
            logs.RemoveAt(0);
        }

        logText.text = string.Join("\n\n", logs);
    }
}