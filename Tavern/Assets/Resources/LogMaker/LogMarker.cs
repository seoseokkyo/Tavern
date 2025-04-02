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
               
        if(instance != null)
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
        logs.Add(logString);
        if (logs.Count > maxLogs)
        {
            logs.RemoveAt(0);
        }

        logText.text = string.Join("\n", logs);
    }
}