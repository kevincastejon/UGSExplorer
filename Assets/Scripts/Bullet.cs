using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class Bullet : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            enabled = IsOwner;
        }
        public void Shoot()
        {
            GetComponent<NetworkObject>().Spawn();
            GetComponent<Rigidbody>().AddForce(transform.forward * 15f, ForceMode.Impulse);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!enabled) { return; }
            if (collision.gameObject.CompareTag("Mobs"))
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}