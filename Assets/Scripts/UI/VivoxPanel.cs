using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using Unity.Services.Lobbies;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class VivoxPanel : UIPanel
    {
        [SerializeField] private Button _setDefaultTokenProviderBtn;
        [SerializeField] private Button _setLobbyTokenProviderBtn;
        [SerializeField] private Button _initializeBtn;
        [SerializeField] private Toggle _ttsToggle;
        [SerializeField] private Button _loginBtn;
        [SerializeField] private Button _logoutBtn;
        [SerializeField] private TMP_InputField _joinChannelNameInput;
        [SerializeField] private Button _joinChannelBtn;
        [SerializeField] private TMP_InputField _leaveChannelNameInput;
        [SerializeField] private Button _leaveChannelBtn;
        [SerializeField] private TMP_InputField _sendChannelNameInput;
        [SerializeField] private TMP_InputField _sendChannelMessageInput;
        [SerializeField] private Button _sendChannelBtn;
        [SerializeField] private TMP_InputField _sendDirectUserIDInput;
        [SerializeField] private TMP_InputField _sendDirectMessageInput;
        [SerializeField] private Button _sendDirectBtn;

        private static VivoxPanel _instance;
        public static VivoxPanel Instance { get => _instance; }

        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _setDefaultTokenProviderBtn.onClick.AddListener(SetDefaultTokenProviderBtn);
            _setLobbyTokenProviderBtn.onClick.AddListener(SetLobbyTokenProviderBtn);
            _initializeBtn.onClick.AddListener(Initialize);
            _loginBtn.onClick.AddListener(Login);
            _logoutBtn.onClick.AddListener(Logout);
            _joinChannelBtn.onClick.AddListener(JoinChannel);
            _leaveChannelBtn.onClick.AddListener(LeaveChannel);
            _sendChannelBtn.onClick.AddListener(SendChannelMessage);
            _sendDirectBtn.onClick.AddListener(SendDirectMessage);
        }

        private void SetDefaultTokenProviderBtn()
        {
            StartWait();
            Log("Settng default VivoxTokenProvider...");
            try
            {
                VivoxService.Instance.SetTokenProvider(null);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Settng default VivoxTokenProvider failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Default VivoxTokenProvider set.");
        }

        private void SetLobbyTokenProviderBtn()
        {
            StartWait();
            Log("Setting LobbyVivoxTokenProvider...");
            try
            {
                VivoxService.Instance.SetTokenProvider(null);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Setting LobbyVivoxTokenProvider failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("LobbyVivoxTokenProvider set.");
        }

        private async void Initialize()
        {
            StartWait();
            Log("Initializing Vivox...");
            try
            {
                await VivoxService.Instance.InitializeAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Initializing Vivox failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Vivox initialized.");
        }

        private async void Login()
        {
            StartWait();
            Log("Logging in Vivox...");
            try
            {
                await VivoxService.Instance.LoginAsync(new LoginOptions() { EnableTTS = _ttsToggle.isOn, DisplayName = AuthenticationService.Instance.PlayerName });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Logging in Vivox failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Logged in Vivox.");
        }

        private async void Logout()
        {
            StartWait();
            Log("Logging out Vivox...");
            try
            {
                await VivoxService.Instance.LogoutAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Logging out Vivox failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Logged out Vivox.");
        }

        private async void JoinChannel()
        {
            StartWait();
            Log("Joining Vivox channel...");
            try
            {
                await VivoxService.Instance.JoinGroupChannelAsync(_joinChannelNameInput.text, ChatCapability.TextAndAudio);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Joining Vivox channel failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Vivox channel joined.");
        }

        private async void LeaveChannel()
        {
            StartWait();
            Log("Leaving Vivox channel...");
            try
            {
                await VivoxService.Instance.LeaveChannelAsync(_leaveChannelNameInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Leaving Vivox channel failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Vivox channel left.");
        }

        private async void SendChannelMessage()
        {
            StartWait();
            Log("Sending Vivox channel message...");
            try
            {
                await VivoxService.Instance.SendChannelTextMessageAsync(_sendChannelNameInput.text, _sendChannelMessageInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Sending Vivox channel message failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Vivox channel message sent.");
        }

        private async void SendDirectMessage()
        {
            StartWait();
            Log("Sending Vivox direct message...");
            try
            {
                await VivoxService.Instance.SendDirectTextMessageAsync(_sendDirectUserIDInput.text, _sendDirectMessageInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Sending Vivox direct message failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Vivox direct message sent.");
        }
    }
}
