using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class NetcodeForGameobjectsPanel : UIPanel
    {
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private TMP_InputField _IPInput;
        [SerializeField] private TMP_InputField _IPListenInput;
        [SerializeField] private TMP_InputField _portInput;
        [SerializeField] private Button _setDirectTransport;
        [SerializeField] private Toggle _useCreateAllocToggle;
        [SerializeField] private Toggle _useJoinAllocToggle;
        [SerializeField] private TMP_InputField _connectionTypeInput;
        [SerializeField] private Button _setServerRelayTransport;
        [SerializeField] private Button _startServerBtn;
        [SerializeField] private Button _startHostBtn;
        [SerializeField] private Button _startClientBtn;
        [SerializeField] private TextMeshProUGUI[] _slotIDLabels;
        [SerializeField] private TMP_InputField _disconnectClientIDInput;
        [SerializeField] private Button _disconnectClientBtn;
        [SerializeField] private Button _shutdownBtn;
        [SerializeField] private CanvasGroup _authGroup;
        [SerializeField] private Toggle _authoritativeMovements;
        [SerializeField] private Toggle _authoritativeBullets;
        UnityTransport _transport;

        private static NetcodeForGameobjectsPanel _instance;
        public static NetcodeForGameobjectsPanel Instance { get => _instance; }
        public bool AuthoritativeMovements { get => _authoritativeMovements.isOn; }
        public bool AuthoritativeBullets { get => _authoritativeBullets.isOn; }

        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _setDirectTransport.onClick.AddListener(SetDirectTransport);
            _setServerRelayTransport.onClick.AddListener(SetServerRelayTransport);
            _startServerBtn.onClick.AddListener(StartServer);
            _startHostBtn.onClick.AddListener(StartHost);
            _startClientBtn.onClick.AddListener(StartClient);
            _disconnectClientBtn.onClick.AddListener(DisconnectClient);
            _shutdownBtn.onClick.AddListener(Shutdown);
            _authoritativeMovements.onValueChanged.AddListener((bool value) =>
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                    {
                        if (client.ClientId == NetworkManager.Singleton.LocalClientId)
                        {
                            continue;
                        }
                        Vector2 pos = client.PlayerObject.transform.position;
                        Color col = client.PlayerObject.GetComponent<PlayerController>().PlayerColor.Value;
                        Destroy(client.PlayerObject.gameObject);
                        PlayerController refreshedPlayerObject = Instantiate(_playerPrefab);
                        refreshedPlayerObject.transform.position = pos;
                        refreshedPlayerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                        refreshedPlayerObject.PlayerColor.Value = col;
                    }
                }
            });
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
        private void StartServer()
        {
            //_authGroup.interactable = false;
            StartWait();
            Log("Starting server...");
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnTransportFailure += OnServerStartFailed;
            bool success;
            try
            {
                success = NetworkManager.Singleton.StartServer();
            }
            catch (Exception e)
            {
                NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
                NetworkManager.Singleton.OnTransportFailure -= OnServerStartFailed;
                LogException(e);
                Log("Starting server failed.");
                //_authGroup.interactable = true;
                StopWait();
                return;
            }
            if (!success)
            {
                NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
                NetworkManager.Singleton.OnTransportFailure -= OnServerStartFailed;
                Log("Starting server failed.");
                //_authGroup.interactable = true;
                StopWait();
            }
        }

        private void OnServerStartFailed()
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnTransportFailure -= OnServerStartFailed;
            StopWait();
            //_authGroup.interactable = true;
            Log("Starting server failed.");
        }

        private void OnServerStarted()
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnTransportFailure -= OnServerStartFailed;
            NetworkManager.Singleton.OnServerStopped += OnServerStopped;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            GameManager.Instance.enabled = true;
            StopWait();
            Log("Server started.");
        }
        private void StartHost()
        {
            //_authGroup.interactable = false;
            StartWait();
            Log("Starting host...");
            NetworkManager.Singleton.OnServerStarted += OnHostStarted;
            NetworkManager.Singleton.OnTransportFailure += OnHostStartFailed;
            bool success;
            try
            {
                success = NetworkManager.Singleton.StartHost();
            }
            catch (Exception e)
            {
                NetworkManager.Singleton.OnServerStarted -= OnHostStarted;
                NetworkManager.Singleton.OnTransportFailure -= OnHostStartFailed;
                LogException(e);
                Log("Starting host failed.");
                //_authGroup.interactable = true;
                StopWait();
                return;
            }
            if (!success)
            {
                NetworkManager.Singleton.OnServerStarted -= OnHostStarted;
                NetworkManager.Singleton.OnTransportFailure -= OnHostStartFailed;
                Log("Starting host failed.");
                //_authGroup.interactable = true;
                StopWait();
            }
        }

        private void OnHostStartFailed()
        {
            NetworkManager.Singleton.OnServerStarted -= OnHostStarted;
            NetworkManager.Singleton.OnTransportFailure -= OnHostStartFailed;
            //_authGroup.interactable = true;
            StopWait();
            Log("Starting host failed.");
        }

        private void OnHostStarted()
        {
            NetworkManager.Singleton.OnServerStarted -= OnHostStarted;
            NetworkManager.Singleton.OnTransportFailure -= OnHostStartFailed;
            NetworkManager.Singleton.OnServerStopped += OnServerStopped;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            NetworkManager.Singleton.OnTransportFailure += OnHostTransportFailure;
            StopWait();
            GameManager.Instance.enabled = true;
            Log("Host started.");
        }

        private void OnHostTransportFailure()
        {
            NetworkManager.Singleton.OnServerStopped -= OnServerStopped;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            GameManager.Instance.enabled = false;
            LogError("Host transport failure");
        }

        private void OnClientConnected(ulong obj)
        {
            RefreshPlayersList();
        }

        private void OnClientDisconnected(ulong obj)
        {
            RefreshPlayersList();
        }

        private void RefreshPlayersList()
        {
            for (int i = 0; i < _slotIDLabels.Length; i++)
            {
                TextMeshProUGUI slot = _slotIDLabels[i];
                slot.text = NetworkManager.Singleton.ConnectedClientsIds.Count <= i ? "" : NetworkManager.Singleton.ConnectedClientsIds[i].ToString();
            }
        }

        private void OnServerStopped(bool obj)
        {
            NetworkManager.Singleton.OnServerStopped -= OnServerStopped;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            GameManager.Instance.enabled = false;
            //_authGroup.interactable = true;
            Log((obj ? "Host" : "Server") + " stopped.");
        }

        private void StartClient()
        {
            _authGroup.interactable = false;
            StartWait();
            Log("Starting client...");
            NetworkManager.Singleton.OnClientStarted += OnClientStarted;
            NetworkManager.Singleton.OnTransportFailure += OnClientStartFailed;
            bool success;
            try
            {
                success = NetworkManager.Singleton.StartClient();
            }
            catch (Exception e)
            {
                NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
                NetworkManager.Singleton.OnTransportFailure -= OnClientStartFailed;
                LogException(e);
                Log("Starting client failed.");
                _authGroup.interactable = true;
                StopWait();
                return;
            }
            if (!success)
            {
                NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
                NetworkManager.Singleton.OnTransportFailure -= OnClientStartFailed;
                Log("Starting client failed.");
                _authGroup.interactable = true;
                StopWait();
            }
        }

        private void OnClientStartFailed()
        {
            NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
            NetworkManager.Singleton.OnTransportFailure -= OnClientStartFailed;
            _authGroup.interactable = true;
            StopWait();
            Log("Starting client failed.");
        }

        private void OnClientStarted()
        {
            NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
            NetworkManager.Singleton.OnTransportFailure -= OnClientStartFailed;
            NetworkManager.Singleton.OnTransportFailure += OnClientTransportFailure;
            NetworkManager.Singleton.OnClientStopped += OnClientStopped;
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
            Log("Client started.");
        }

        private void OnClientTransportFailure()
        {
            NetworkManager.Singleton.OnTransportFailure -= OnClientTransportFailure;
            NetworkManager.Singleton.OnClientStopped -= OnClientStopped;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnected;
            _authGroup.interactable = true;
            StopWait();
            Log("Transport failure.");
        }

        private void OnConnected(ulong obj)
        {
            StopWait();
            Log("Connected to server.");
            _slotIDLabels[0].text = obj.ToString();
        }

        private void OnDisconnected(ulong obj)
        {
            _slotIDLabels[0].text = "";
        }

        private void OnClientStopped(bool obj)
        {
            NetworkManager.Singleton.OnTransportFailure -= OnClientTransportFailure;
            NetworkManager.Singleton.OnClientStopped -= OnClientStopped;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnected;
            _authGroup.interactable = true;
            StopWait();
            Log((obj ? "Host" : "Client") + " stopped.");
        }
        private void DisconnectClient()
        {
            StartWait();
            Log("Disconnecting client...");
            try
            {
                NetworkManager.Singleton.DisconnectClient(ulong.Parse(_disconnectClientIDInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Disconnecting client failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            RefreshPlayersList();
            Log("Client disconnected.");
        }
        private void Shutdown()
        {
            StartWait();
            Log("Shuting down...");
            try
            {
                NetworkManager.Singleton.Shutdown();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Shuting down failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            if (NetworkManager.Singleton.IsClient)
            {
                _slotIDLabels[0].text = "";
            }
            else
            {
                RefreshPlayersList();
            }
            Log("Shut down.");
        }
    }
}