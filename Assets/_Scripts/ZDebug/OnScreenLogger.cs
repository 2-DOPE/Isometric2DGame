
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class DebugScreenLog : MonoBehaviour
{
    [System.Serializable]
    public class LogEntry
    {
        public string message;
        public float timestamp;

        public LogEntry(string message)
        {
            this.message = message;
            timestamp = Time.time;
        }
    }

    [Header("Log Settings")]
    [SerializeField, Tooltip("Number of lines to display on screen")] private uint qsize = 15;

    [Header("Screen Transform")]
    [SerializeField] private Vector2 screenPosition = new Vector2(20, 20);
    [SerializeField] private Vector2 screenSize = new Vector2(500, 300);

    [Header("Font Settings")]
    [SerializeField] private int fontSize = 14;
    [SerializeField] private Color fontColor = Color.white;
    [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
    [SerializeField] private TextAnchor textAlignment = TextAnchor.UpperLeft;

    [Header("Spacing")]
    [SerializeField] private float lineSpacing = 5f;

    [Header("Auto Clear Settings")]
    [SerializeField, Min(0.01f)]
    private float clearInterval = 0.333f;

    private List<LogEntry> myLogQueue = new List<LogEntry>();
    private GUIStyle logStyle;

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
        myLogQueue.Insert(
            0,
            new LogEntry("[" + type + "] : " + logString)
        );

        if (type == LogType.Exception)
        {
            myLogQueue.Insert(0, new LogEntry(stackTrace));
        }

        while (myLogQueue.Count > qsize)
        {
            myLogQueue.RemoveAt(myLogQueue.Count - 1);
        }
    }

    void Update()
    {
        myLogQueue.RemoveAll(entry =>
            Time.time - entry.timestamp >= clearInterval
        );
    }


    void OnGUI()
    {
        // Create an independent style, not a shared GUI skin style.
        if (logStyle == null)
        {
            logStyle = new GUIStyle();
            logStyle.wordWrap = true;
            logStyle.richText = false;
            logStyle.padding = new RectOffset(0, 0, 0, 0);
            logStyle.margin = new RectOffset(0, 0, 0, 0);
        }

        // Apply font settings to every GUI state.
        logStyle.fontSize = fontSize;
        logStyle.fontStyle = fontStyle;
        logStyle.alignment = textAlignment;
        logStyle.wordWrap = true;

        logStyle.normal.textColor = fontColor;
        logStyle.hover.textColor = fontColor;
        logStyle.active.textColor = fontColor;
        logStyle.focused.textColor = fontColor;

        float currentY = 0f;

        GUI.BeginGroup(new Rect(
            screenPosition.x,
            screenPosition.y,
            screenSize.x,
            screenSize.y
        ));

        foreach (LogEntry entry in myLogQueue)
        {
            GUIContent content = new GUIContent(entry.message);

            float textHeight = logStyle.CalcHeight(
                content,
                screenSize.x
            );

            Rect textRect = new Rect(
                0,
                currentY,
                screenSize.x,
                textHeight
            );

            GUI.Label(textRect, content, logStyle);

            currentY += textHeight + lineSpacing;

            if (currentY >= screenSize.y)
                break;
        }

        GUI.EndGroup();
    }
}
