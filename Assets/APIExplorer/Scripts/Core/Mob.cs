using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class Mob : NetworkBehaviour
    {
        private static int _mobCount;
        [SerializeField] private float _moveSpeed = 0.5f;
        [SerializeField] private float _changeDirectionDelay = 2f;

        private float _nextChangeDirectionTime;
        private Vector2 _moveDir;
        private Rigidbody2D _rigidbody;

        public static int MobCount { get => _mobCount; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _mobCount++;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            _mobCount--;            
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            enabled = IsOwner;
        }
        private void Update()
        {
            if(Time.time > _nextChangeDirectionTime)
            {
                _moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                _nextChangeDirectionTime = Time.time + _changeDirectionDelay;
            }
        }
        private void FixedUpdate()
        {
            _rigidbody.velocity = _moveDir * _moveSpeed;
        }
    }
}