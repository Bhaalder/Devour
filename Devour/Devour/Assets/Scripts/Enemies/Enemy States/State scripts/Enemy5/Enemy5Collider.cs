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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            GetComponentInParent<Enemy5>().rb.velocity = new Vector2(0, 0);
            GetComponentInParent<Enemy5>().rb.gravityScale = 6;
            GetComponentInParent<Enemy5>().jumpCollision = true;
            GetComponentInParent<Enemy5>().Transition<Enemy5Idle>();
        }
    }
}
