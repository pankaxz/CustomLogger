using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PNKZ{
    public class CustomLogger : MonoBehaviour
    {
        [Header("Toggle Logs")]
        public bool showLogs = true;
        public bool saveLogs = false; // NOT IMPLEMENTED

        public bool LogMessages = true;
        public bool LogWarnings = true;
        public bool LogErrors = true;

        [SerializeField]
        public static bool bIsPersistent = true; 

        [Header("Log Format")]
        public Color MessageColor = new(0f, 1f, 0f);
        public Color WarningColor = new(1f, 0.5f , 0f);
        public Color ErrorColor = new(1, 0f, 0f);

        GUIStyle logContainer;
        GUIStyle logText;

        public int padding = 1;
        public int margin = 20;
        public int fontSize = 10; 

        [Range(0.3f, 1.0f)]
        public float height = 0.5f;

        [Range(0.3f, 1.0f)]
        public float width = 0.5f;

        [Range(0f, 01f)]
        public float BackgroundOpacity = 0.5f;
        public Color BackgroundColor = Color.black;

        static Queue<LogQueue> logQueue = new Queue<LogQueue>();

        public static CustomLogger CustomLoggerInstance { get; private set; }

        protected virtual void OnApplicationQuit()
        {
            CleanUp();
        }
        protected virtual void OnDestroy()
        {
            CleanUp();
        }

        protected void CleanUp()
        {
            Debug.Log($"Destroying {CustomLoggerInstance} ");
            CustomLoggerInstance = null;
            Destroy(gameObject);
            Debug.Log($"Destroyed");
        }

        private void Awake()
        {
            if (!showLogs) {
                CleanUp();
                return;
            }
            
            if (CustomLoggerInstance != null)
            {
                Debug.LogError($"{CustomLoggerInstance} is not null, Destroying extra Instance");
                Destroy(gameObject);
                return;
            }

            if (CustomLoggerInstance == null) Debug.Log($"{this} is null, creating a new Instance");
            else Debug.Log($"{this} is not null, overwriting the old one");

            CustomLoggerInstance = this;

            if (CustomLoggerInstance == null)
            {
                Debug.LogError("Something is wrong WTF");
                try
                {
                    CustomLoggerInstance = (CustomLogger)Instantiate(Resources.Load("ScreenLoggerPrefab", typeof(CustomLogger)));
                }
                catch
                {
                    Debug.Log("Failed to load default Screen Logger prefab...");
                    CustomLoggerInstance = new GameObject("CustomLogger", typeof(CustomLogger)).GetComponent<CustomLogger>();
                }
            }

            if (CustomLoggerInstance == null) Debug.Log("IDK bro");

            if (bIsPersistent == true)
                DontDestroyOnLoad(this);
            
        }

        private void OnEnable()
        {
        //Start On Loading
            SceneManager.sceneLoaded += OnSceneLoaded;
            Application.logMessageReceivedThreaded += HandleLog;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializeStyles();
        }

        void InitializeStyles()
        {
            Texture2D background = new Texture2D(1, 1);
            BackgroundColor.a = BackgroundOpacity;
            background.SetPixel(0, 0, BackgroundColor);
            background.Apply();

            logContainer = new GUIStyle();
            logText = new GUIStyle();

            logContainer.normal.background = background;

            logContainer.wordWrap = false;
            logContainer.padding = new RectOffset(padding, padding, padding, padding);
            logText.fontSize = fontSize;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; 
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        void HandleLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Assert && !LogErrors) return;
            if (type == LogType.Error && !LogErrors) return;
            if (type == LogType.Exception && !LogErrors) return;

            if (type == LogType.Log && !LogMessages) return;

            if (type == LogType.Warning && !LogWarnings) return;

            string[] lines = message.Split(new char[] { '\n' });

            foreach (string l in lines)
                logQueue.Enqueue(new LogQueue(l, type));
        }

        void Update()
        {
            float InnerHeight = (Screen.height - 2 * margin) * height - 2 * padding;
            int TotalRows = (int)(InnerHeight / logText.lineHeight);

            while (logQueue.Count > TotalRows)
                logQueue.Dequeue();
        }

        private void OnGUI()
        {
            if (!showLogs && Application.isPlaying) return;
            
            InitializeStyles();
            float w = (Screen.width - margin) * width;
            float h = (Screen.height - margin) * height;
            float x = 1, y = 1;

            GUILayout.BeginArea(new Rect(x, y, w, h), logContainer);

            foreach (LogQueue log in logQueue)
            {
                switch (log.LogType)
                {
                    case LogType.Warning:
                        logText.normal.textColor = WarningColor;
                        break;

                    case LogType.Log:
                        logText.normal.textColor = MessageColor;
                        break;

                    case LogType.Assert:
                    case LogType.Exception:
                    case LogType.Error:
                        logText.normal.textColor = ErrorColor;
                        break;

                    default:
                        logText.normal.textColor = MessageColor;
                        break;
                }

                GUILayout.Label(log.LogText, logText);
            }

            GUILayout.EndArea();
        }
    }
}
