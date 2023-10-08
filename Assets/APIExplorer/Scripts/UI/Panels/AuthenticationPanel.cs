using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.PlayerAccounts;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class AuthenticationPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _profileLabel;
        [SerializeField] private TMP_InputField _profileInput;
        [SerializeField] private Button _setProfileBtn;
        [SerializeField] private TextMeshProUGUI _statusLabel;
        [SerializeField] private TextMeshProUGUI _sessionTokenLabel;
        [SerializeField] private Button _signInAnonBtn;
        [SerializeField] private Button _signInUnityBtn;
        [SerializeField] private Button _clearSessionBtn;
        [SerializeField] private TextMeshProUGUI _playerNameLabel;
        [SerializeField] private TextMeshProUGUI _playerIDLabel;
        [SerializeField] private TextMeshProUGUI _playerLinkLabel;
        [SerializeField] private Button _playerNameRefreshBtn;
        [SerializeField] private Button _playerInfoRefreshBtn;
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private Button _updatePlayerNameBtn;
        [SerializeField] private Button _linkWithUnityBtn;
        [SerializeField] private Button _unlinkUnityBtn;
        [SerializeField] private Button _signOutBtn;
        [SerializeField] private Button _deleteAccountBtn;

        private static AuthenticationPanel _instance;
        public static AuthenticationPanel Instance { get => _instance; }
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            _setProfileBtn.onClick.AddListener(SetProfile);
            _signInAnonBtn.onClick.AddListener(SignInAnonymously);
            _signInUnityBtn.onClick.AddListener(SignInWithUnity);
            _clearSessionBtn.onClick.AddListener(ClearSession);
            _playerNameRefreshBtn.onClick.AddListener(RefreshPlayerName);
            _playerInfoRefreshBtn.onClick.AddListener(RefreshPlayerInfo);
            _updatePlayerNameBtn.onClick.AddListener(UpdatePlayerName);
            _linkWithUnityBtn.onClick.AddListener(LinkWithUnity);
            _unlinkUnityBtn.onClick.AddListener(UnlinkUnity);
            _signOutBtn.onClick.AddListener(SignOut);
            _deleteAccountBtn.onClick.AddListener(DeleteAccount);
        }
        private void Update()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                _statusLabel.text = AuthenticationService.Instance.IsSignedIn ? "Signed in" : "Not signed in";
                _sessionTokenLabel.text = AuthenticationService.Instance.SessionTokenExists ? "Session token : YES" : "Session token : NO";
                string playerName = AuthenticationService.Instance.PlayerName;
                string playerId = AuthenticationService.Instance.PlayerId;
                bool playerLink = AuthenticationService.Instance.PlayerInfo != null ? AuthenticationService.Instance.PlayerInfo.Identities.Count > 0 : false;
                _playerNameLabel.text = !string.IsNullOrEmpty(playerName) ? playerName : "";
                _playerIDLabel.text = !string.IsNullOrEmpty(playerId) ? playerId : "";
                _playerLinkLabel.text = playerLink ? "YES" : "NO";
            }
        }
        private void SetProfile()
        {
            StartWait();
            Log("Switching profile...");
            try
            {
                AuthenticationService.Instance.SwitchProfile(_profileInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Profile switching failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            _profileLabel.text = AuthenticationService.Instance.Profile;
            Log("Profile switched.");
        }
        private async void SignInAnonymously()
        {
            StartWait();
            Log("Authenticating...");
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Authentication failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Authenticated.");
        }

        private async void SignInWithUnity()
        {
            StartWait();
            Log("Authenticating...");
            PlayerAccountService.Instance.SignedIn += OnUnitySignedIn;
            PlayerAccountService.Instance.SignInFailed += OnUnitySignedInFailed;
            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (Exception e)
            {
                PlayerAccountService.Instance.SignedIn -= OnUnitySignedIn;
                PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInFailed;
                LogException(e);
                Log("Authentication failed.");
                StopWait();
            }
        }

        private async void OnUnitySignedIn()
        {
            PlayerAccountService.Instance.SignedIn -= OnUnitySignedIn;
            PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInFailed;
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Authentication failed.");
                PlayerAccountService.Instance.SignOut();
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Authenticated.");
        }

        private void OnUnitySignedInFailed(RequestFailedException e)
        {
            PlayerAccountService.Instance.SignedIn -= OnUnitySignedIn;
            PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInFailed;
            LogException(e);
            Log("Authentication failed.");
            StopWait();
        }

        private void ClearSession()
        {
            StartWait();
            Log("Clearing session...");
            try
            {
                AuthenticationService.Instance.ClearSessionToken();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Session clearing failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Session cleared.");
        }
        private async void RefreshPlayerName()
        {
            StartWait();
            Log("Refreshing player name...");
            try
            {
                await AuthenticationService.Instance.GetPlayerNameAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Player name refreshing failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            _playerNameLabel.text = AuthenticationService.Instance.PlayerName;
            Log("Player name refreshed.");
        }

        private async void RefreshPlayerInfo()
        {
            StartWait();
            Log("Refreshing player info...");
            PlayerInfo pi;
            try
            {
                pi = await AuthenticationService.Instance.GetPlayerInfoAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Player info refreshing failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            _playerIDLabel.text = pi.Id;
            _playerLinkLabel.text = pi.Identities.Count > 0 ? "YES" : "NO";
            Log("Player info refreshed.");
        }


        private async void UpdatePlayerName()
        {
            StartWait();
            Log("Updating player name...");
            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(_playerNameInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Player name update failed.");
                return;
            }
            finally
            {
                _playerNameInput.text = AuthenticationService.Instance.PlayerName;
                StopWait();
            }
            Log("Player name updated.");
        }

        private async void LinkWithUnity()
        {
            StartWait();
            Log("Linking anonymous account with Unity Player Account...");
            PlayerAccountService.Instance.SignedIn += OnUnitySignedInForLink;
            PlayerAccountService.Instance.SignInFailed += OnUnitySignedInForLinkFailed;
            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (Exception e)
            {
                PlayerAccountService.Instance.SignedIn -= OnUnitySignedInForLink;
                PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInForLinkFailed;
                LogException(e);
                Log("Linking failed.");
                StopWait();
            }
        }

        private void OnUnitySignedInForLinkFailed(RequestFailedException e)
        {
            PlayerAccountService.Instance.SignedIn -= OnUnitySignedInForLink;
            PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInForLinkFailed;
            LogException(e);
            Log("Linking failed.");
            StopWait();
        }

        private async void OnUnitySignedInForLink()
        {
            PlayerAccountService.Instance.SignedIn -= OnUnitySignedInForLink;
            PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInForLinkFailed;
            try
            {
                await AuthenticationService.Instance.LinkWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Linking failed.");
                PlayerAccountService.Instance.SignOut();
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Account linked. Authenticated with Unity.");
        }

        private async void UnlinkUnity()
        {
            StartWait();
            Log("Unlinking Unity Player Account...");
            try
            {
                await AuthenticationService.Instance.UnlinkUnityAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Unlinking failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            PlayerAccountService.Instance.SignOut();
            Log("Account unlinked. Authenticated anonymously.");
        }

        private void SignOut()
        {
            StartWait();
            Log("Signing out...");
            try
            {
                AuthenticationService.Instance.SignOut();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Signing out failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            if (PlayerAccountService.Instance.IsSignedIn)
            {
                PlayerAccountService.Instance.SignOut();
            }
            Log("Signed out.");
        }

        private async void DeleteAccount()
        {
            StartWait();
            Log("Deleting account...");
            try
            {
                await AuthenticationService.Instance.DeleteAccountAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Account deletion failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            if (PlayerAccountService.Instance.IsSignedIn)
            {
                PlayerAccountService.Instance.SignOut();
            }
            Log("Account deleted.");
        }
    }
}
