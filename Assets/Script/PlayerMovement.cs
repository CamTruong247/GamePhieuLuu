using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] public SpriteRenderer avatar;

    private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 v;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveServerRpc();
    }

    [ClientRpc]
    private void MoveClientRpc()
    {
        if (IsOwner)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            v = new Vector2(x, y).normalized;
            if (x > 0)
            {
                gameObject.transform.GetChild(0).localScale = new Vector3(1, 1, 0);
            }
            else if (x < 0)
            {
                gameObject.transform.GetChild(0).localScale = new Vector3(-1, 1, 0);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveServerRpc()
    {
        MoveClientRpc();
    }

    [ClientRpc]
    private void FUMoveClientRpc()
    {
        rb.velocity = new Vector2(v.x * moveSpeed, v.y * moveSpeed);
    }


    [ServerRpc(RequireOwnership = false)]
    public void FUMoveServerRpc()
    {
        FUMoveClientRpc();
    }

    private void FixedUpdate()
    {
        FUMoveServerRpc();
    }
}
