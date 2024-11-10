using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class DropItem : NetworkBehaviour
{
    [SerializeField] private GameObject[] Items;

    [ServerRpc(RequireOwnership = false)]
    public void DropItemServerRpc()
    {
        DropItemClientRpc();
    }

    [ClientRpc]
    public void DropItemClientRpc()
    {
        int random = Random.Range(0, 4);
        if (random == 0)
        {
            if (IsServer)
            {
                var drop = Instantiate(Items[random], gameObject.transform.position, gameObject.transform.rotation);
                drop.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
