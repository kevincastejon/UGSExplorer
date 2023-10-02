using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class CloudSavePanel : UIPanel
    {
        [SerializeField] private Button _getAllKeysBtn;
        [SerializeField] private TMP_InputField _getDataInput;
        [SerializeField] private Button _getDataBtn;
        [SerializeField] private TMP_InputField _saveDataKeyInput;
        [SerializeField] private TMP_InputField _saveDataValueInput;
        [SerializeField] private Button _saveDataBtn;
        [SerializeField] private TMP_InputField _deleteDataInput;
        [SerializeField] private Button _deleteDataBtn;


        private static CloudSavePanel _instance;
        public static CloudSavePanel Instance { get => _instance; }
        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _getAllKeysBtn.onClick.AddListener(GetAllKeys);
            _getDataBtn.onClick.AddListener(GetData);
            _saveDataBtn.onClick.AddListener(SaveData);
            _deleteDataBtn.onClick.AddListener(DeleteData);
        }

        private async void GetAllKeys()
        {
            StartWait();
            Log("Fetching player cloud save keys...");
            List<string> keys;
            try
            {
                keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching player cloud save keys failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player cloud save keys fetched.");
            Log("Keys :");
            foreach (string key in keys)
            {
                Log("    " + key);
            }
        }

        private async void GetData()
        {
            StartWait();
            Log("Fetching player cloud save data...");
            Dictionary<string, string> data;
            try
            {
                data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>() { _getDataInput.text });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching player cloud save data failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player cloud save data fetched.");
            if (data.Count == 0)
            {
                Log("No data found for this key.");
            }
            else
            {
                string key = data.Keys.ElementAt(0);
                string value = data.Values.ElementAt(0);
                Log(key + " = " + value);
            }
        }

        private async void SaveData()
        {
            StartWait();
            Log("Saving player cloud save data...");
            try
            {
                await CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object>() { { _saveDataKeyInput.text, _saveDataValueInput.text } });
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Saving player cloud save keys failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player cloud save data saved.");
        }

        private async void DeleteData()
        {
            StartWait();
            Log("Deleting player cloud save data...");
            try
            {
                await CloudSaveService.Instance.Data.ForceDeleteAsync(_deleteDataInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting player cloud save keys failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player cloud save data deleted.");
        }
    }
}
