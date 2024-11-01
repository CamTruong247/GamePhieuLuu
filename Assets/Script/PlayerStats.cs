using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private Image healthbar;

    private float health = 100;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        HealthBarServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void HealthBarServerRpc()
    {
        HealthBarClientRpc();
    }

    [ClientRpc]
    private void HealthBarClientRpc()
    {
        healthbar.fillAmount = health / 100f;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHealthServerRpc(float health)
    {
        UpdateHealthClientRpc(health);
    }

    [ClientRpc]
    public void UpdateHealthClientRpc(float health)
    {
        this.health -= health;
        if (this.health <= 0)
        {
            Destroy(gameObject);
        }
    }


}