using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootingPoint;

    public Animator animator;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttackServerRpc();
        }
    }

    private void FixedUpdate()
    {
        RotationWeapon();
    }

    [ServerRpc(RequireOwnership = false)]
    private void AttackServerRpc()
    {
        if (IsServer)
        {
            AttackClientRpc();
            var b = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
            b.GetComponent<NetworkObject>().Spawn();
        }
    }

    [ClientRpc]
    private void AttackClientRpc()
    {
        animator.SetTrigger("Attack");

    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }


    private void RotationWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 look = mousePos - transform.position;
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = rotation;

        /*if(transform.eulerAngles.z > 90 && transform.eulerAngles.z <270)
        {
            transform.localScale = new Vector3(1, -1, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 0);
        }*/

    }


}
