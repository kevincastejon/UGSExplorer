using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class ProfilePanel : UIPanel
    {
        [SerializeField] private Button _setProfileBtn;
        [SerializeField] private TextMeshProUGUI _profileLabel;
        [SerializeField] private TMP_InputField _profileInput;

        private static ProfilePanel _instance;
        public static ProfilePanel Instance { get => _instance; }
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            _setProfileBtn.onClick.AddListener(OnSetProfile);
        }

        private void OnSetProfile()
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
    }
}
