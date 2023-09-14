using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _wait;
        private CanvasGroup _group;

        protected virtual void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }
        public void SetInteractable(bool interactable)
        {
            _group.interactable = interactable;
        }
        protected void StartWait()
        {
            SetInteractable(false);
            _wait.SetActive(true);
        }
        protected void StopWait()
        {
            SetInteractable(true);
            _wait.SetActive(false);
        }
        protected void Log(string message)
        {
            InGameConsole.Instance.Log(message);
        }
        protected void LogError(string message)
        {
            InGameConsole.Instance.LogError(message);
        }
        protected void LogException(System.Exception e)
        {
            InGameConsole.Instance.LogException(e);
        }
    }
}