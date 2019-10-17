using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform enemyGFX;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float distanceBeforeTurning = 1f;

    private Rigidbody2D rb;
    
    private Vector2 direction;
    private Vector2 force;
    [SerializeField] private bool movingRight = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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

        force = direction.normalized * speed * Time.deltaTime;

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

}
