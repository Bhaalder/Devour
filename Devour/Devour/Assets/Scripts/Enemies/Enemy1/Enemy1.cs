using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : Enemy
{


    // Start is called before the first frame update
    void Start()
    {

    }

    protected override void Awake() {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        //Movement();
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
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
