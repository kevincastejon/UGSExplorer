using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class ServicesPanel : UIPanel
    {
        [SerializeField] private Button _initializeBtn;
        [SerializeField] private TextMeshProUGUI _stateLabel;

        private static ServicesPanel _instance;
        public static ServicesPanel Instance { get => _instance; }
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            _initializeBtn.onClick.AddListener(OnInitialize);
            _stateLabel.text = UnityServices.State.ToString();
        }

        private async void OnInitialize()
        {
            StartWait();
            _stateLabel.text = UnityServices.State.ToString();
            Log("Initializing services...");
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Initialization failed.");
                return;
            }
            finally
            {
                _stateLabel.text = UnityServices.State.ToString();
                StopWait();
            }
            Log("Services initialized.");
        }
    }
}
