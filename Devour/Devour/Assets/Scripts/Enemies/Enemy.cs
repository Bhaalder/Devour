using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : StateMachine
{
    [SerializeField] private float enemyHealth;
    private CircleCollider2D circleCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void Awake(){
        base.Awake();
        circleCollider2D = GetComponent<CircleCollider2D>();
        EnemyTakeDamageEvent.RegisterListener(TakeDamage);

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

    public void TakeDamage(EnemyTakeDamageEvent damageEvent){//funkar ej

        try {
            if (damageEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
                Debug.Log("I took damage!");
                Debug.Log(damageEvent.attackCollider.gameObject.name);
                enemyHealth -= damageEvent.damage;

                if (!damageEvent.player.IsGrounded && damageEvent.player.IsAttackingDown) {//denna ska ta in så player studsar upp när man hugger ner, just nu studsar man alltid upp
                    damageEvent.player.Rb2D.velocity = new Vector2(damageEvent.player.Rb2D.velocity.x, 0);
                    damageEvent.player.Rb2D.velocity = new Vector2(damageEvent.player.Rb2D.velocity.x, 15);
                }
            }
        } catch (System.NullReferenceException) {

        }


        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        EnemyTakeDamageEvent.UnRegisterListener(TakeDamage);
        Destroy(gameObject);
    }

}
