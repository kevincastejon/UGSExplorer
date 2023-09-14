using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class TransportPanel : UIPanel
    {

        [SerializeField] private TMP_InputField _IPInput;
        [SerializeField] private TMP_InputField _IPListenInput;
        [SerializeField] private TMP_InputField _portInput;
        [SerializeField] private Button _setDirectTransport;
        [SerializeField] private Toggle _useCreateAllocToggle;
        [SerializeField] private Toggle _useJoinAllocToggle;
        [SerializeField] private TMP_InputField _connectionTypeInput;
        [SerializeField] private Button _setServerRelayTransport;
        UnityTransport _transport;

        private static TransportPanel _instance;
        public static TransportPanel Instance { get => _instance; }
        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _setDirectTransport.onClick.AddListener(SetDirectTransport);
            _setServerRelayTransport.onClick.AddListener(SetServerRelayTransport);
        }
        private void Start()
        {
            _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        }

        private void SetDirectTransport()
        {
            StartWait();
            Log("Setting direct transport...");
            try
            {
                _transport.SetConnectionData(_IPInput.text.Length == 0 ? null : _IPInput.text, 7777, _IPListenInput.text.Length == 0 ? null : _IPListenInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Setting direct transport failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Direct transport set.");
        }

        private void SetServerRelayTransport()
        {
            StartWait();
            Log("Setting relay transport...");
            try
            {
                _transport.SetRelayServerData(_useCreateAllocToggle.isOn ? new RelayServerData(RelayPanel.Instance.CreateAlloc, _connectionTypeInput.text) : new RelayServerData(RelayPanel.Instance.JoinAlloc, _connectionTypeInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Setting relay transport failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Relay transport set.");
        }
    }
}