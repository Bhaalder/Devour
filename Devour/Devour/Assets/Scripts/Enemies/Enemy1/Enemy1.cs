using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : Enemy
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float distanceBeforeTurning = 1f;
    [SerializeField] private float enemySpeed = 400;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform enemyGFX;

    private Vector2 direction;
    private Vector2 force;
    private bool movingRight = true;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        Movement();
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
    }
    private void Movement()
    {
        if (movingRight)
        {
            direction = new Vector2(1f, 0f);
        }
        else if (!movingRight)
        {
            direction = new Vector2(-1f, 0f);
        }

        force = direction.normalized * enemySpeed * Time.deltaTime;

        rb.AddForce(force);

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }

        RaycastHit2D obstructed = Physics2D.Raycast(rb.position, direction, distanceBeforeTurning, layerMask);
        if (obstructed.collider == true)
        {
            movingRight = !movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision is made");
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("collision is player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent{
                damage = 5,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
        }
    }

}
