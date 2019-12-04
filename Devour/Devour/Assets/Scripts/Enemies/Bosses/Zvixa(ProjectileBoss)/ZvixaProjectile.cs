//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZvixaProjectile : MonoBehaviour{

    public float Damage { get; set; }
    public float SelfDamage { get; set; }
    public float LifeSpan { get; set; }
    public float Speed { get; set; }

    private Transform player;
    private Rigidbody2D rigidBody2D;
    private CircleCollider2D circleCollider2D;
    private bool gotHit;

    private void Start(){
        player = GameController.Instance.Player.transform;
        rigidBody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        PlayerAttackEvent.RegisterListener(GetHit);
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void FixedUpdate() {
        LifeSpan -= Time.deltaTime;
        if (!gotHit) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
        }
        if(LifeSpan <= 0) {
            Destroy(gameObject);
        }
    }

    private void GetHit(PlayerAttackEvent attackEvent) {
        if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
            Vector2 knockBack;
            if (attackEvent.player.IsAttackingDown) {
                knockBack = new Vector2(0, -attackEvent.player.KnockbackForce * 1.3f);
            } else if (attackEvent.player.IsAttackingUp) {
                knockBack = new Vector2(0, attackEvent.player.KnockbackForce * 1.3f);
            } else {
                knockBack = new Vector2(attackEvent.player.FacingDirection * attackEvent.player.KnockbackForce * 1.3f, 0);
            }
            gotHit = true;
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.velocity = knockBack;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                damage = Damage,
                enemyPosition = GetComponent<Rigidbody2D>().position
            };
            playerTakeDamage.FireEvent();
            Destroy(gameObject);
        }
        if (gotHit && collision.gameObject.tag == "Enemy") {
            ZvixaSelfDamageEvent zvixaSelfDamage = new ZvixaSelfDamageEvent {
                circleCollider2D = circleCollider2D,
                damage = SelfDamage
            };
            zvixaSelfDamage.FireEvent();
            Destroy(gameObject);
        }
    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(GetHit);
        BossDiedEvent.UnRegisterListener(BossDied);
    }

}
