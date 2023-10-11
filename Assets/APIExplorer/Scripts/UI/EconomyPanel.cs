using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.CloudSave;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class EconomyPanel : UIPanel
    {
        [SerializeField] private Button _syncConfBtn;
        [SerializeField] private Button _getCurrenciesBtn;
        [SerializeField] private Button _getItemsBtn;
        [SerializeField] private Button _getVirtualPurchasesBtn;
        [SerializeField] private Button _getBalancesBtn;
        [SerializeField] private TMP_InputField _setBalanceIDInput;
        [SerializeField] private TMP_InputField _setBalanceValueInput;
        [SerializeField] private Button _setBalanceBtn;
        [SerializeField] private TMP_InputField _incrementBalanceIdInput;
        [SerializeField] private TMP_InputField _incrementBalanceValueInput;
        [SerializeField] private Button _incrementBalanceBtn;
        [SerializeField] private TMP_InputField _decrementBalanceIdInput;
        [SerializeField] private TMP_InputField _decrementBalanceValueInput;
        [SerializeField] private Button _decrementBalanceBtn;
        [SerializeField] private Button _getInventoryBtn;
        [SerializeField] private TMP_InputField _addItemInput;
        [SerializeField] private Button _addItemBtn;
        [SerializeField] private TMP_InputField _deleteItemInput;
        [SerializeField] private Button _deleteItemBtn;
        [SerializeField] private TMP_InputField _makePurchaseIDInput;
        [SerializeField] private Button _makePurchaseBtn;

        private static EconomyPanel _instance;
        public static EconomyPanel Instance { get => _instance; }
        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _syncConfBtn.onClick.AddListener(SyncConfiguration);
            _getCurrenciesBtn.onClick.AddListener(GetCurrencies);
            _getItemsBtn.onClick.AddListener(GetItems);
            _getVirtualPurchasesBtn.onClick.AddListener(GetVirtualPurchases);
            _getBalancesBtn.onClick.AddListener(GetBalances);
            _setBalanceBtn.onClick.AddListener(SetBalance);
            _incrementBalanceBtn.onClick.AddListener(IncrementBalances);
            _decrementBalanceBtn.onClick.AddListener(DecrementBalances);
            _getInventoryBtn.onClick.AddListener(GetInventory);
            _addItemBtn.onClick.AddListener(AddItem);
            _deleteItemBtn.onClick.AddListener(DeleteItem);
            _makePurchaseBtn.onClick.AddListener(MakePurchase);
        }

        private async void SyncConfiguration()
        {
            StartWait();
            Log("Synchronizing configurations...");
            List<ConfigurationItemDefinition> configs;
            try
            {
                configs = await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Synchronizing configurations failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Configurations synchronized.");
        }

        private void GetCurrencies()
        {
            StartWait();
            Log("Fetching currencies definitions...");
            List<CurrencyDefinition> currencies;
            try
            {
                currencies = EconomyService.Instance.Configuration.GetCurrencies();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching currencies definitions failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Currencies definitions fetched.");
            Log("Currencies :");
            foreach (CurrencyDefinition currency in currencies)
            {
                Log("    ID : " + currency.Id + "    Name : " + currency.Name);
            }
        }

        private void GetItems()
        {
            StartWait();
            Log("Fetching items definitions...");
            List<InventoryItemDefinition> items;
            try
            {
                items = EconomyService.Instance.Configuration.GetInventoryItems();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching items definitions failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Items definitions fetched.");
            Log("Items :");
            foreach (InventoryItemDefinition item in items)
            {
                Log("    ID : " + item.Id + "    Name : " + item.Name);
            }
        }

        private void GetVirtualPurchases()
        {
            StartWait();
            Log("Fetching virtual purchases definitions...");
            List<VirtualPurchaseDefinition> virtualPurchases;
            try
            {
                virtualPurchases = EconomyService.Instance.Configuration.GetVirtualPurchases();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching virtual purchases definitions failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Virtual purchases definitions fetched.");
            Log("Virtual purchases :");
            foreach (VirtualPurchaseDefinition virtualPurchase in virtualPurchases)
            {
                Log("    ID : " + virtualPurchase.Id + "    Name : " + virtualPurchase.Name);
                Log("        Costs :");
                foreach (PurchaseItemQuantity item in virtualPurchase.Costs)
                {
                    Log("                Type : " + item.Item.GetReferencedConfigurationItem().Type+ "    ID : " + item.Item.GetReferencedConfigurationItem().Id + "    Amount : " + item.Amount);
                }
                Log("        Rewards :");
                foreach (PurchaseItemQuantity item in virtualPurchase.Rewards)
                {
                    Log("                Type : " + item.Item.GetReferencedConfigurationItem().Type + "    ID : " + item.Item.GetReferencedConfigurationItem().Id + "    Amount : " + item.Amount);
                }
            }
        }

        private async void GetBalances()
        {
            StartWait();
            Log("Fetching player balances...");
            GetBalancesResult balances;
            try
            {
                balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching player balances failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player balances fetched.");
            Log("Balances :");
            foreach (PlayerBalance balance in balances.Balances)
            {
                Log("    Currency : " + balance.CurrencyId + "    Amount : " + balance.Balance);
            }
        }

        private async void SetBalance()
        {
            StartWait();
            Log("Updating player balance...");
            try
            {
                await EconomyService.Instance.PlayerBalances.SetBalanceAsync(_setBalanceIDInput.text, long.Parse(_setBalanceValueInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Updating player balance failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player balance updated.");
        }

        private async void IncrementBalances()
        {
            StartWait();
            Log("Incrementing player balance...");
            try
            {
                await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(_incrementBalanceIdInput.text, int.Parse(_incrementBalanceValueInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Incrementing player balance failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player balance incremented.");
        }

        private async void DecrementBalances()
        {
            StartWait();
            Log("Decrementing player balance...");
            try
            {
                await EconomyService.Instance.PlayerBalances.DecrementBalanceAsync(_decrementBalanceIdInput.text, int.Parse(_decrementBalanceValueInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Decrementing player balance failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player balance decremented.");
        }

        private async void GetInventory()
        {
            StartWait();
            Log("Fetching player inventory...");
            GetInventoryResult inventory;
            try
            {
                inventory = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching player inventory failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player inventory fetched.");
            Log("Inventory :");
            foreach (PlayersInventoryItem playersInventoryItem in inventory.PlayersInventoryItems)
            {
                Log("    Item ID : " + playersInventoryItem.InventoryItemId + "    Player Item ID : " + playersInventoryItem.PlayersInventoryItemId);
            }
        }

        private async void AddItem()
        {
            StartWait();
            Log("Adding player item...");
            try
            {
                await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync(_addItemInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Adding player item failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player item added.");
        }

        private async void DeleteItem()
        {
            StartWait();
            Log("Deleting player item...");
            try
            {
                await EconomyService.Instance.PlayerInventory.DeletePlayersInventoryItemAsync(_deleteItemInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting player item failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player item deleted.");
        }

        private async void MakePurchase()
        {
            StartWait();
            Log("Purchasing item...");
            MakeVirtualPurchaseResult result;
            try
            {
                result = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(_makePurchaseIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Purchasing item failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Item purchased.");
            Log("Purchase info :");
            Log("    Costs :");
            Log("        Currencies :");
            foreach (CurrencyExchangeItem item in result.Costs.Currency)
            {
                Log("            ID : " + item.Id + "    Amount : " + item.Amount);
            }
            Log("        Items :");
            foreach (InventoryExchangeItem item in result.Costs.Inventory)
            {
                Log("            ID : " + item.Id + "    Amount : " + item.Amount);
            }
            Log("");
            Log("    Rewards :");
            Log("        Currencies :");
            foreach (CurrencyExchangeItem item in result.Rewards.Currency)
            {
                Log("            ID : " + item.Id + "    Amount : " + item.Amount);
            }
            Log("        Items :");
            foreach (InventoryExchangeItem item in result.Rewards.Inventory)
            {
                Log("            ID : " + item.Id + "    Amount : " + item.Amount);
            }
        }
    }
}