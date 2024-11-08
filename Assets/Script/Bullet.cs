using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(Deplay());
        }

    }

    private IEnumerator Deplay()
    {
        yield return new WaitForSeconds(5);
        GetComponent<NetworkObject>().Despawn();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * 4;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Kiểm tra nếu đối tượng là Werewolf
            werewolfmovement werewolfMovement = collision.gameObject.GetComponent<werewolfmovement>();
            if (werewolfMovement != null)
            {
                werewolfMovement.UpdateHealthServerRpc(3);
            }

            // Kiểm tra nếu đối tượng là Slime
            SlimeMovement slimeMovement = collision.gameObject.GetComponent<SlimeMovement>();
            if (slimeMovement != null)
            {
                slimeMovement.UpdateHealthServerRpc(3);
            }

            // Kiểm tra nếu đối tượng là Golem
            GolemBoss golemMovement = collision.gameObject.GetComponent<GolemBoss>();
            if (golemMovement != null)
            {
                golemMovement.UpdateHealthServerRpc(3); 
            }

            // Kiểm tra nếu đối tượng là Slime King
            SlimeKingMovement slimeKingMovement = collision.gameObject.GetComponent<SlimeKingMovement>();
            if (slimeKingMovement != null)
            {
                slimeKingMovement.UpdateHealthServerRpc(3); 
            }

            // Despawn the bullet after hitting an enemy if on the server
            if (IsServer)
            {
                NetworkObject networkObject = GetComponent<NetworkObject>();
                if (networkObject != null && networkObject.IsSpawned)
                {
                    networkObject.Despawn(); // Hủy đối tượng
                }
            }
        }

    }

}
