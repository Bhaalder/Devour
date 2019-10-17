using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : StateMachine
{
    [SerializeField] private float enemyHealth;
    private BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    protected override void Awake()
    {
        EnemyTakeDamageEvent.RegisterListener(TakeDamage);
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

    public void TakeDamage(EnemyTakeDamageEvent damageEvent){
        if (damageEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
            enemyHealth = -damageEvent.damage;
            if(damageEvent.player.Rb2D.velocity.y <= 0) {//denna ska ta in så player studsar upp när man hugger ner, just nu studsar man alltid upp
                damageEvent.player.Rb2D.velocity = new Vector2(damageEvent.player.Rb2D.velocity.x, 5);
            }
        }
        
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
