using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 v;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        v = new Vector2 (x, y).normalized;
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(v.x * moveSpeed * Time.deltaTime, v.y * moveSpeed * Time.deltaTime);
    }
}
