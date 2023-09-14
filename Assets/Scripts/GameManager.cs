using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NetworkObject _mobPrefab;
        [SerializeField] private float _spawnDelay = 0.5f;
        private float _nextSpawnTime;
        private static GameManager _instance;
        public static GameManager Instance { get => _instance; }

        private void Awake()
        {
            _instance = this;
            enabled = false;
        }
        private void Update()
        {
            if (Time.time >= _nextSpawnTime && Mob.MobCount < 20)
            {
                Instantiate(_mobPrefab, new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity).Spawn();
                _nextSpawnTime = Time.time + _spawnDelay;
            }
        }
    }
}