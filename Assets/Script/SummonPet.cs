using UnityEngine;
using Unity.Netcode;

public class SummonPet : NetworkBehaviour
{
    [SerializeField] private GameObject Pet;
    [SerializeField] private float summonDistance = 2.0f;

    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.E))
        {
           
            SummonPetServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SummonPetServerRpc()
    {
        if (IsServer)
        {
            Vector3 summonPosition = transform.position + new Vector3(Random.Range(-summonDistance, summonDistance), Random.Range(-summonDistance, summonDistance), 0);
            var pet = Instantiate(Pet, summonPosition, Quaternion.identity);
            pet.GetComponent<NetworkObject>().Spawn();

            
        }
    }
}
