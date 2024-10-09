using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        v = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(v.x * moveSpeed, v.y * moveSpeed);
    }
}
