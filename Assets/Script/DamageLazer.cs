using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageLazer : NetworkBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SlimeMovement slimeMovement = collision.gameObject.GetComponent<SlimeMovement>();
            slimeMovement.UpdateHealthServerRpc(5);

        }
    }

}
