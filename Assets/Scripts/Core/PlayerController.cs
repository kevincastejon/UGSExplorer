using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class PlayerController : NetworkBehaviour
    {
        private Camera _camera;
        private SpriteRenderer _sprite;
        private NetworkVariable<Color> _color = new(Color.white);
        private NetworkVariable<bool> _serverAuthoritativeMovement = new(false);
        private NetworkVariable<bool> _serverAuthoritativeShooting = new(false);
        private NetworkVariable<Vector2> _clientPosition = new(Vector2.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<bool> ServerAuthoritativeMovement { get => _serverAuthoritativeMovement; }
        public NetworkVariable<Color> PlayerColor { get => _color; }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _color.Value = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);
                _sprite.color = _color.Value;
                _serverAuthoritativeMovement.Value = NetcodeForGameobjectsPanel.Instance.AuthoritativeMovements;
                if (_serverAuthoritativeMovement.Value)
                {
                    _clientPosition.OnValueChanged += (Vector2 prev, Vector2 next) => transform.position = next;
                }
            }
            else
            {
                _sprite.color = _color.Value;
            }
            _color.OnValueChanged += (Color prev, Color next) => _sprite.color = next;
        }
        private void Awake()
        {
            _camera = Camera.main;
            _sprite = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            if (!IsOwner) { return; }
            if (_serverAuthoritativeMovement.Value)
            {
                Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _clientPosition.Value = new Vector2(Mathf.Clamp(mouseWorldPos.x, -5f, 5f), Mathf.Clamp(mouseWorldPos.y, -5f, 5f));
            }
            else
            {
                Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector2(Mathf.Clamp(mouseWorldPos.x, -5f, 5f), Mathf.Clamp(mouseWorldPos.y, -5f, 5f));
            }
            if (_serverAuthoritativeShooting.Value)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    ShootServerRpc();
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(new Vector3(transform.position.x, transform.position.y, -10f), Vector3.forward * 20f));
                    if (hit.collider && hit.collider.CompareTag("Mobs"))
                    {
                        // TO DO SOME PREDICTION HERE...
                        KillMobServerRpc(hit.collider.GetComponent<NetworkObject>().NetworkObjectId);
                    }
                }
            }
        }
        [ServerRpc]
        public void ShootServerRpc()
        {
            if (!_serverAuthoritativeShooting.Value)
            {
                return;
            }
            RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(new Vector3(transform.position.x, transform.position.y, -10f), Vector3.forward * 20f));
            if (hit && hit.collider.CompareTag("Mobs"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        [ServerRpc]
        public void KillMobServerRpc(ulong mobId)
        {
            if (_serverAuthoritativeShooting.Value)
            {
                return;
            }

            Destroy(NetworkManager.Singleton.SpawnManager.SpawnedObjects[mobId].gameObject);
        }
        private void FixedUpdate()
        {
            //if (IsOwner)
            //{
            //    Debug.Log("TRY MOVING");
            //    Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            //    _rigidbody.velocity = moveDir * _moveSpeed;

            //    Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            //    _rigidbody.rotation = Quaternion.LookRotation(new Vector3(mouseWorldPos.x, 0f, mouseWorldPos.z) - new Vector3(transform.position.x, 0f, transform.position.z));
            //}
        }
    }
}
