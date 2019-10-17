using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : StateMachine
{
    [SerializeField] private float enemyHealth;

    // Start is called before the first frame update
    void Start()
    {

    }
    protected override void Awake()
    {
       base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth = -damage;
        if(enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
    }

}
