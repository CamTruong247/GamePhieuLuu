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
        if (collision.gameObject.tag == "Enemy")
        {
            SlimeMovement slimeMovement = collision.gameObject.GetComponent<SlimeMovement>();
            slimeMovement.UpdateHealthServerRpc(3);
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
