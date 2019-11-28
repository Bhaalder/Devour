//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidBomb : MonoBehaviour {

    public float Damage { get; set; }
    public float LifeSpan { get; set; }
    public float Speed { get; set; }

    private Transform player;
    private CircleCollider2D circleCollider2D;

    private void Start() {
        player = GameController.Instance.Player.transform;
        circleCollider2D = GetComponent<CircleCollider2D>();
        PlayerAttackEvent.RegisterListener(GetHit);
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void FixedUpdate() {
        LifeSpan -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
        if (LifeSpan <= 0) {
            Destroy(gameObject);//FÖR TILLFÄLLET
        }
    }

    private void GetHit(PlayerAttackEvent attackEvent) {
        if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds) && attackEvent.isMeleeAttack) {
            Explode();
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
            Explode();
        }
    }

    private void Explode() {//FÖR TILLFÄLLET
        Debug.Log("PANG");
        PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
            damage = Damage,
            enemyPosition = GetComponent<Rigidbody2D>().position
        };
        playerTakeDamage.FireEvent();
        Destroy(gameObject);
    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);//borde inte explodera då
    }

    private void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(GetHit);
        BossDiedEvent.UnRegisterListener(BossDied);
    }

}
