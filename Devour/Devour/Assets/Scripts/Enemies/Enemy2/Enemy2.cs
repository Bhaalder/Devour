using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy2 : Enemy
{



    protected override void Awake() {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision is made");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("collision is player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = 5,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
        }
    }
}
