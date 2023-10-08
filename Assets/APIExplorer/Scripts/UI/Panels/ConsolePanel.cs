using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class ConsolePanel : MonoBehaviour
    {
        private static ConsolePanel _instance;
        [SerializeField] private bool _forwardToRealConsole;
        private TMP_InputField _textField;

        public static ConsolePanel Instance { get => _instance; }

        private void Awake()
        {
            if (_instance)
            {
                Debug.LogError("Do not instantiate multiple InGameConsole component");
                Destroy(this);
            }
            else
            {
                _instance = this;
                _textField = GetComponentInChildren<TMP_InputField>();
            }
        }

        public void Log(object message)
        {
            _textField.text += message.ToString() + "\n";
            if (_forwardToRealConsole)
            {
                Debug.Log(message.ToString());
            }
        }
        public void LogError(object message)
        {
            _textField.text += "<color=red>" + message + "</color>\n";
            if (_forwardToRealConsole)
            {
                Debug.LogError(message);
            }
        }
        public void LogException(System.Exception e, object customMessage = null)
        {
            _textField.text += "<color=red>" + (customMessage == null ? e.Message : customMessage) + "</color>\n";
            if (_forwardToRealConsole)
            {
                Debug.LogException(e);
            }
        }
    }
}
