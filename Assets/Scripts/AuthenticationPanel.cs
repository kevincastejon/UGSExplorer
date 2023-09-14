using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.PlayerAccounts;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class AuthenticationPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _statusLabel;
        [SerializeField] private TextMeshProUGUI _sessionTokenLabel;
        [SerializeField] private Button _signInAnonBtn;
        [SerializeField] private Button _signInUnityBtn;
        [SerializeField] private Button _clearSessionBtn;

        private static AuthenticationPanel _instance;
        public static AuthenticationPanel Instance { get => _instance; }
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            _signInAnonBtn.onClick.AddListener(SignInAnonymously);
            _signInUnityBtn.onClick.AddListener(SignInWithUnity);
            _clearSessionBtn.onClick.AddListener(ClearSession);
        }
        private void Update()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                _statusLabel.text = AuthenticationService.Instance.IsSignedIn ? "Signed in" : "Not signed in";
                _sessionTokenLabel.text = AuthenticationService.Instance.SessionTokenExists ? "Session token : YES" : "Session token : NO";
            }
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
    }
}
