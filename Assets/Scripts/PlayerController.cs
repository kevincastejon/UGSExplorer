using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class PlayerController : NetworkBehaviour
    {
        private Camera _camera;
        private SpriteRenderer _sprite;
        private NetworkVariable<Color> _color = new(Color.white);
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _color.Value = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);
                _sprite.color = _color.Value;
            }
            else
            {
                _sprite.color = _color.Value;
                _color.OnValueChanged += (Color prev, Color next) => _sprite.color = next;
            }
        }
        private void Awake()
        {
            _camera = Camera.main;
            _sprite = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            if (IsOwner)
            {
                Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector2(Mathf.Clamp(mouseWorldPos.x, -5f, 5f), Mathf.Clamp(mouseWorldPos.y, -5f, 5f));
                if (Input.GetMouseButtonDown(0))
                {
                    ShootServerRpc(transform.position);
                }
            }
        }
        [ServerRpc]
        public void ShootServerRpc(Vector2 pos)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(new Vector3(pos.x, pos.y, -10f), Vector3.forward * 20f));
            if (hit && hit.collider.CompareTag("Mobs"))
            {
                Destroy(hit.collider.gameObject);
            }
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
