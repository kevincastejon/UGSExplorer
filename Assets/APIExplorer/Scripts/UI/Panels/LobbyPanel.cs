using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{

    public class LobbyPanel : UIPanel
    {
        [SerializeField] private Button _getJoinedLobbiesBtn;
        [SerializeField] private TMP_InputField _createLobbyNameInput;
        [SerializeField] private TMP_InputField _createAllocIDInput;
        [SerializeField] private TMP_InputField _createRelayCodeInput;
        [SerializeField] private Button _createBtn;
        [SerializeField] private TMP_InputField _sendHeartbeatIDInput;
        [SerializeField] private Button _sendHeartbeatBtn;
        [SerializeField] private TMP_InputField _updateLobbyIDInput;
        [SerializeField] private TMP_InputField _updateLobbyNameInput;
        [SerializeField] private TMP_InputField _updateLobbyHostIDInput;
        [SerializeField] private Button _updateLobbyBtn;
        [SerializeField] private TMP_InputField _deleteIDInput;
        [SerializeField] private Button _deleteBtn;
        [SerializeField] private TMP_InputField _queryNameInput;
        [SerializeField] private Button _queryBtn;
        [SerializeField] private TMP_InputField _getLobbyIDInput;
        [SerializeField] private Button _getLobbyBtn;
        [SerializeField] private TMP_InputField _quickJoinNameInput;
        [SerializeField] private TMP_InputField _quickJoinAllocIDInput;
        [SerializeField] private TMP_InputField _quickJoinRelayCodeInput;
        [SerializeField] private Button _quickJoinBtn;
        [SerializeField] private TMP_InputField _joinByCodeCodeInput;
        [SerializeField] private TMP_InputField _joinByCodeAllocIDInput;
        [SerializeField] private TMP_InputField _joinByCodeRelayCodeInput;
        [SerializeField] private Button _joinByCodeBtn;
        [SerializeField] private TMP_InputField _joinByIDIDInput;
        [SerializeField] private TMP_InputField _joinByIDAllocIDInput;
        [SerializeField] private TMP_InputField _joinByIDRelayCodeInput;
        [SerializeField] private Button _joinByIDBtn;
        [SerializeField] private TMP_InputField _reconnectIDInput;
        [SerializeField] private Button _reconnectBtn;
        [SerializeField] private TMP_InputField _removePlayerLobbyIDInput;
        [SerializeField] private TMP_InputField _removePlayerPlayerIDInput;
        [SerializeField] private Button _removePlayerBtn;
        [SerializeField] private TMP_InputField _updatePlayerLobbyIDInput;
        [SerializeField] private TMP_InputField _updatePlayerPlayerIDInput;
        [SerializeField] private TMP_InputField _updatePlayerAllocIDInput;
        [SerializeField] private TMP_InputField _updatePlayerRelayCodeInput;
        [SerializeField] private Button _updatePlayerBtn;

        private static LobbyPanel _instance;
        public static LobbyPanel Instance { get => _instance; }
        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _createBtn.onClick.AddListener(CreateLobby);
            _sendHeartbeatBtn.onClick.AddListener(SendHeartbeat);
            _updateLobbyBtn.onClick.AddListener(UpdateLobby);
            _deleteBtn.onClick.AddListener(Delete);
            _queryBtn.onClick.AddListener(QueryLobbies);
            _getJoinedLobbiesBtn.onClick.AddListener(GetJoinedLobbies);
            _getLobbyBtn.onClick.AddListener(GetLobby);
            _quickJoinBtn.onClick.AddListener(QuickJoin);
            _joinByCodeBtn.onClick.AddListener(JoinByCode);
            _joinByIDBtn.onClick.AddListener(JoinByID);
            _reconnectBtn.onClick.AddListener(Reconnect);
            _removePlayerBtn.onClick.AddListener(RemovePlayer);
            _updatePlayerBtn.onClick.AddListener(UpdatePlayer);
        }

        private async void GetJoinedLobbies()
        {
            StartWait();
            Log("Fetching joined lobbies...");
            List<string> ids;
            try
            {
                ids = await LobbyService.Instance.GetJoinedLobbiesAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching joined lobbies failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Joined lobbies fetched.");
            LogJoinedLobby(ids);
        }
        private async void CreateLobby()
        {
            StartWait();
            Log("Creating lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.CreateLobbyAsync(_createLobbyNameInput.text, 4, new CreateLobbyOptions() { Player = new Player(null, _createRelayCodeInput.text.Length > 0 ? _createRelayCodeInput.text : null, null, _createAllocIDInput.text.Length > 0 ? _createAllocIDInput.text : null, default, default, new PlayerProfile(AuthenticationService.Instance.PlayerName)) });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Creating lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby created.");
            LogLobby(lobby);
        }
        private void SendHeartbeat()
        {
            StartWait();
            Log("Sending heartbeat...");
            try
            {
                LobbyService.Instance.SendHeartbeatPingAsync(_sendHeartbeatIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Sending heartbeat failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Heartbeat sent.");
        }
        private async void UpdateLobby()
        {
            StartWait();
            Log("Updating lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.UpdateLobbyAsync(_updateLobbyIDInput.text, new UpdateLobbyOptions() { Name = _updateLobbyNameInput.text.Length > 0 ? _updateLobbyNameInput.text : null, HostId = _updateLobbyHostIDInput.text.Length > 0 ? _updateLobbyHostIDInput.text : null });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Updating lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby updated.");
            LogLobby(lobby);
        }
        private void Delete()
        {
            StartWait();
            Log("Deleting lobby...");
            try
            {
                LobbyService.Instance.DeleteLobbyAsync(_deleteIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby deleted.");
        }

        private async void QueryLobbies()
        {
            StartWait();
            Log("Fetching lobbies...");
            QueryResponse res;
            try
            {
                res = await LobbyService.Instance.QueryLobbiesAsync(_queryNameInput.text.Length == 0 ? null : new QueryLobbiesOptions() { Filters = new List<QueryFilter>() { new QueryFilter(QueryFilter.FieldOptions.Name, _queryNameInput.text, QueryFilter.OpOptions.CONTAINS) } });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching lobbies failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobbies fetched.");
            LogLobbies(res);
        }
        private async void GetLobby()
        {
            StartWait();
            Log("Fetching lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.GetLobbyAsync(_getLobbyIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby fetched.");
            LogLobby(lobby);
        }
        private async void QuickJoin()
        {
            StartWait();
            Log("Quick joining lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions() { Filter = _quickJoinNameInput.text.Length == 0 ? null : new List<QueryFilter>() { new QueryFilter(QueryFilter.FieldOptions.Name, _quickJoinNameInput.text, QueryFilter.OpOptions.CONTAINS) }, Player = new Player(null, _quickJoinRelayCodeInput.text.Length > 0 ? _quickJoinRelayCodeInput.text : null, null, _quickJoinAllocIDInput.text.Length > 0 ? _quickJoinAllocIDInput.text : null, default, default, new PlayerProfile(AuthenticationService.Instance.PlayerName)) });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Quick joining lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby found and joined.");
            LogLobby(lobby);
        }
        private async void JoinByCode()
        {
            StartWait();
            Log("Joining lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(_joinByCodeCodeInput.text, new JoinLobbyByCodeOptions() { Player = new Player(null, _joinByCodeRelayCodeInput.text.Length > 0 ? _joinByCodeRelayCodeInput.text : null, null, _joinByCodeAllocIDInput.text.Length > 0 ? _joinByCodeAllocIDInput.text : null, default, default, new PlayerProfile(AuthenticationService.Instance.PlayerName)) });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Joining lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby joined.");
            LogLobby(lobby);
        }
        private async void JoinByID()
        {
            StartWait();
            Log("Joining lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.JoinLobbyByIdAsync(_joinByIDIDInput.text, new JoinLobbyByIdOptions() { Player = new Player(null, _joinByIDRelayCodeInput.text.Length > 0 ? _joinByIDRelayCodeInput.text : null, null, _joinByIDAllocIDInput.text.Length > 0 ? _joinByIDAllocIDInput.text : null, default, default, new PlayerProfile(AuthenticationService.Instance.PlayerName)) });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Joining lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby joined.");
            LogLobby(lobby);
        }
        private async void Reconnect()
        {
            StartWait();
            Log("Reconnecting lobby...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.ReconnectToLobbyAsync(_reconnectIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Reconnecting lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Lobby Reconnected.");
            LogLobby(lobby);
        }

        private void RemovePlayer()
        {
            StartWait();
            Log("Removing player from lobby...");
            try
            {
                LobbyService.Instance.RemovePlayerAsync(_removePlayerLobbyIDInput.text, _removePlayerPlayerIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Removing player from lobby failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player removed from lobby.");
        }
        private async void UpdatePlayer()
        {
            StartWait();
            Log("Updating player...");
            Lobby lobby;
            try
            {
                lobby = await LobbyService.Instance.UpdatePlayerAsync(_updatePlayerLobbyIDInput.text, _updatePlayerPlayerIDInput.text, new UpdatePlayerOptions() { AllocationId = _updatePlayerAllocIDInput.text.Length == 0 ? null : _updatePlayerAllocIDInput.text, ConnectionInfo = _updatePlayerRelayCodeInput.text.Length == 0 ? null : _updatePlayerRelayCodeInput.text });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Updating player failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player updated.");
            LogLobby(lobby);
        }

        private void LogLobbies(QueryResponse res)
        {
            Log("-------------------------------------");
            Log("Lobbies found");
            Log("");
            foreach (Lobby lobby in res.Results)
            {
                Log("    Lobby " + lobby.Id);
                Log("");
                foreach (Player player in lobby.Players)
                {
                    Log("        - Player " + player.Id + "    Relay code : " + player.ConnectionInfo);
                }
                Log("");
                Log("");
            }
            Log("-------------------------------------");
        }
        private void LogLobby(Lobby lobby)
        {
            Log("-------------------------------------");
            Log("Lobby " + lobby.Id);
            Log("");
            foreach (Player player in lobby.Players)
            {
                Log("    Player " + player.Id + "    Relay code : " + (player.ConnectionInfo != null ? player.ConnectionInfo : "NONE") + "    Name : " + (player.Profile != null ? player.Profile.Name : "NONE"));
            }
            Log("-------------------------------------");
        }
        private void LogJoinedLobby(List<string> lobbiesIds)
        {
            Log("-------------------------------------");
            Log("Joined lobbies");
            Log("");
            foreach (string id in lobbiesIds)
            {
                Log("    Lobby " + id);
            }
            Log("-------------------------------------");
        }
    }
}