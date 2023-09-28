using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private Button _expandBar;
        [SerializeField] private Button _collapseBar;
        [SerializeField] private GameObject _content;
        VerticalLayoutGroup _mainLayout;
        private CanvasGroup _group;
        private bool _startDebug;

        protected virtual void Awake()
        {
            _mainLayout = GetComponent<VerticalLayoutGroup>();
            _group = GetComponent<CanvasGroup>();
            _expandBar.onClick.AddListener(Expand);
            _collapseBar.onClick.AddListener(Collapse);
        }
        private void Expand()
        {
            _content.SetActive(true);
            _expandBar.gameObject.SetActive(false);
            _collapseBar.gameObject.SetActive(true);
            //_mainLayout.CalculateLayoutInputVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }

        private void Collapse()
        {
            _content.SetActive(false);
            _expandBar.gameObject.SetActive(true);
            _collapseBar.gameObject.SetActive(false);
            //_mainLayout.CalculateLayoutInputVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }

        public void SetInteractable(bool interactable)
        {
            _group.interactable = interactable;
        }
        protected void StartWait()
        {
            SetInteractable(false);
        }
        protected void StopWait()
        {
            SetInteractable(true);
        }
        protected void Log(string message)
        {
            ConsolePanel.Instance.Log(message);
        }
        protected void LogError(string message)
        {
            ConsolePanel.Instance.LogError(message);
        }
        protected void LogException(System.Exception e)
        {
            ConsolePanel.Instance.LogException(e);
        }
    }
}