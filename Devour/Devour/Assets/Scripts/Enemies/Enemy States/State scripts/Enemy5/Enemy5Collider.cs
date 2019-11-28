using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5Collider : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponentInParent<Enemy5>().rb.velocity = new Vector2(0, GetComponentInParent<Enemy5>().rb.velocity.y);
            GetComponentInParent<Enemy5>().rb.gravityScale = 6;
            GetComponentInParent<Enemy5>().JumpCollision = true;
            GetComponentInParent<Enemy5>().Transition<Enemy5Idle>();
        }
    }
}
