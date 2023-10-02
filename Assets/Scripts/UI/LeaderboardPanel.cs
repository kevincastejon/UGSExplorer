using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class LeaderboardPanel : UIPanel
    {

        [SerializeField] private Button _getPlayerScoreBtn;
        [SerializeField] private Button _getScoresBtn;
        [SerializeField] private TMP_InputField _addScoreInput;
        [SerializeField] private Button _addScoreBtn;

        private const string LEADERBOARD_ID = "Kills";

        private static LeaderboardPanel _instance;
        public static LeaderboardPanel Instance { get => _instance; }
        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            _getPlayerScoreBtn.onClick.AddListener(GetPlayerScore);
            _getScoresBtn.onClick.AddListener(GetScores);
            _addScoreBtn.onClick.AddListener(AddScore);
        }

        private async void GetPlayerScore()
        {
            StartWait();
            Log("Fetching player score...");
            LeaderboardEntry entry;
            try
            {
                entry = await LeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching player score failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player score fetched.");
            Log(entry.PlayerName + " : " + entry.Score);
        }

        private async void GetScores()
        {
            StartWait();
            Log("Fetching scores...");
            LeaderboardScoresPage scores;
            try
            {
                scores = await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Fetching scores failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Scores fetched.");
            Log("Leaderboard scores : ");
            foreach (LeaderboardEntry entry in scores.Results)
            {
                Log("    "+entry.PlayerName + " : " + entry.Score);
            }
        }

        private async void AddScore()
        {
            StartWait();
            Log("Adding player score...");
            LeaderboardEntry entry;
            try
            {
                entry = await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, int.Parse(_addScoreInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Adding player score failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Player score added.");
            Log(entry.PlayerName + " : " + entry.Score);
        }
    }
}
