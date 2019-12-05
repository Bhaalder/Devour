//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidBomb : MonoBehaviour {

    public float Damage { get; set; }
    public float LifeSpan { get; set; }
    public float Speed { get; set; }

    [SerializeField] private GameObject explosion;

    private bool isStartingToExplode;
    private Transform player;
    private CircleCollider2D circleCollider2D;
    private Rigidbody2D rigidBody2D;
    private Animator animator;

    private void Start() {
        player = GameController.Instance.Player.transform;
        circleCollider2D = GetComponent<CircleCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerAttackEvent.RegisterListener(GetHit);
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void FixedUpdate() {
        LifeSpan -= Time.deltaTime;
        if (!isStartingToExplode) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
        }
        if (LifeSpan <= 0 && !isStartingToExplode) {
            Explode();
        }
    }

    private void GetHit(PlayerAttackEvent attackEvent) {
        try {
            if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
                //Vector2 knockBack;
                //if (attackEvent.isMeleeAttack) {
                //    if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack) {
                //        attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                //        attackEvent.player.DashesLeft = attackEvent.player.NumberOfDashes;
                //        attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                //        attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, attackEvent.player.BounceForce);
                //        return;
                //    }
                //    if (attackEvent.player.IsAttackingUp) {
                //        knockBack = new Vector2(0, attackEvent.player.KnockbackForce);
                //        rigidBody2D.velocity = knockBack;
                //        return;
                //    }
                //    if (attackEvent.player.IsAttackingDown) {
                //        knockBack = new Vector2(0, -attackEvent.player.KnockbackForce);
                //        rigidBody2D.velocity = knockBack;
                //        return;
                //    }
                //    knockBack = new Vector2(attackEvent.player.FacingDirection * attackEvent.player.KnockbackForce, 0);
                //    rigidBody2D.velocity = knockBack;
                //} else {
                //    Explode();
                //}
                Vector2 knockBack;
                if (attackEvent.player.IsAttackingDown) {
                    knockBack = new Vector2(0, -attackEvent.player.KnockbackForce * 1.3f);
                } else if (attackEvent.player.IsAttackingUp) {
                    knockBack = new Vector2(0, attackEvent.player.KnockbackForce * 1.3f);
                } else {
                    knockBack = new Vector2(attackEvent.player.FacingDirection * attackEvent.player.KnockbackForce * 1.3f, 0);
                }
                rigidBody2D.velocity = Vector2.zero;
                rigidBody2D.velocity = knockBack;
            }
        } catch (System.NullReferenceException) {
            Debug.LogWarning("A missing reference in PlayerAttackEvent, check Log!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            //Debug.Log("Collided with Player");
            //PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
            //    damage = Damage,
            //    enemyPosition = GetComponent<Rigidbody2D>().position
            //};
            //playerTakeDamage.FireEvent();
            Explode();
        }
    }

    private void Explode() {//FÖR TILLFÄLLET
        if (!isStartingToExplode) {
            Debug.Log("Börjar explodera");
            rigidBody2D.velocity = Vector2.zero;
            isStartingToExplode = true;
            animator.SetTrigger("WindUp");
            StartCoroutine(CountDown());
        }
    }

    private IEnumerator CountDown() {
        yield return new WaitForSecondsRealtime(2);
        Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<VoidBombExplosion>().Damage = Damage;
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
