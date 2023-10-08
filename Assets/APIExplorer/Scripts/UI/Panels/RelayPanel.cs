using KevinCastejon.MultiplayerAPIExplorer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class RelayPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _createAllocLabel;
        [SerializeField] private TextMeshProUGUI _joinAllocLabel;
        [SerializeField] private TextMeshProUGUI _joinCodeLabel;
        [SerializeField] private TMP_InputField _joinCodeInput;
        [SerializeField] private Button _createAllocBtn;
        [SerializeField] private Button _getJoinCodeBtn;
        [SerializeField] private Button _joinAllocBtn;

        private Allocation _createAlloc;
        private JoinAllocation _joinAlloc;

        private static RelayPanel _instance;
        public static RelayPanel Instance { get => _instance; }
        public Allocation CreateAlloc { get => _createAlloc; }
        public JoinAllocation JoinAlloc { get => _joinAlloc; }

        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            _createAllocBtn.onClick.AddListener(CreateAllocation);
            _getJoinCodeBtn.onClick.AddListener(GetJoinCode);
            _joinAllocBtn.onClick.AddListener(JoinAllocation);
        }

        private async void CreateAllocation()
        {
            StartWait();
            Log("Creating relay allocation...");
            try
            {
                _createAlloc = await RelayService.Instance.CreateAllocationAsync(4);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Relay allocation creation failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            _createAllocLabel.text = "YES";
            Log("Relay allocation created.");
        }

        private async void GetJoinCode()
        {
            StartWait();
            Log("Getting relay join code...");
            string joinCode;
            try
            {
                joinCode = await RelayService.Instance.GetJoinCodeAsync(_createAlloc.AllocationId);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Getting relay join code failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            _joinCodeLabel.text = joinCode;
            Log("Got relay join code. " + joinCode);
        }

        private async void JoinAllocation()
        {
            StartWait();
            Log("Joining relay allocation...");
            try
            {
                _joinAlloc = await RelayService.Instance.JoinAllocationAsync(_joinCodeInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Relay allocation joining failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            _joinAllocLabel.text = "YES";
            Log("Relay allocation joined.");
        }
    }
}