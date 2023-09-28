using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.PlayerAccounts;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class AccountPanel : UIPanel
    {
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
        private static AccountPanel _instance;
        public static AccountPanel Instance { get => _instance; }
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            _playerNameRefreshBtn.onClick.AddListener(RefreshPlayerName);
            _playerInfoRefreshBtn.onClick.AddListener(RefreshPlayerInfo);
            _updatePlayerNameBtn.onClick.AddListener(UpdatePlayerName);
            _linkWithUnityBtn.onClick.AddListener(LinkWithUnity);
            _unlinkUnityBtn.onClick.AddListener(UnlinkUnity);
            _signOutBtn.onClick.AddListener(SignOut);
            _deleteAccountBtn.onClick.AddListener(DeleteAccount);
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

        private void Update()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                string playerName = AuthenticationService.Instance.PlayerName;
                string playerId = AuthenticationService.Instance.PlayerId;
                bool playerLink = AuthenticationService.Instance.PlayerInfo != null ? AuthenticationService.Instance.PlayerInfo.Identities.Count > 0 : false;
                _playerNameLabel.text = !string.IsNullOrEmpty(playerName) ? playerName : "";
                _playerIDLabel.text = !string.IsNullOrEmpty(playerId) ? playerId : "";
                _playerLinkLabel.text = playerLink ? "YES" : "NO";
            }
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
                Log("Linking failed.");
                StopWait();
            }
        }

        private void OnUnitySignedInFailed(RequestFailedException e)
        {
            PlayerAccountService.Instance.SignedIn -= OnUnitySignedIn;
            PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInFailed;
            LogException(e);
            Log("Linking failed.");
            StopWait();
        }

        private async void OnUnitySignedIn()
        {
            PlayerAccountService.Instance.SignedIn -= OnUnitySignedIn;
            PlayerAccountService.Instance.SignInFailed -= OnUnitySignedInFailed;
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
