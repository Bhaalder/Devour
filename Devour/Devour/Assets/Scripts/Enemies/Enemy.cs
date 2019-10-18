//Main Author: Marcus Söderberg
//Secondary Author: Patrik Ahlgren (TakeDamage)
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
        PlayerMeleeAttackEvent.RegisterListener(TakeDamage);
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

    public void TakeDamage(PlayerMeleeAttackEvent attackEvent){
        try {
            if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
                Debug.Log("I took damage! " + gameObject.name);
                ChangeEnemyHealth(-attackEvent.damage);
                PlayerHealEvent phe = new PlayerHealEvent {
                    isLifeLeech = true
                };
                phe.FireEvent();
                if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown) {
                    attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                    attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                    attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 15);
                }
            }
        } catch (NullReferenceException) {

        }
        if (enemyHealth <= 0){
            EnemyDeath();
        }
    }

    public void ChangeEnemyHealth(float amount) {
        enemyHealth += amount;
    }

    private void EnemyDeath()
    {
        PlayerMeleeAttackEvent.UnRegisterListener(TakeDamage);
        Destroy(gameObject);
    }

}
