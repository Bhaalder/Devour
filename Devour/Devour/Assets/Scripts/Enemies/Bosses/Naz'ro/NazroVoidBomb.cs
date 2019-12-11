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
        NazroSecondPhaseEvent.RegisterListener(OnPhaseChange);
    }

    private void FixedUpdate() {
        LifeSpan -= Time.deltaTime;
        if (!isStartingToExplode) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
            if (LifeSpan <= 0) {
                Explode();
            }
        }
    }

    private void GetHit(PlayerAttackEvent attackEvent) {
        try {
            if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
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
            Explode();
        }
    }

    private void Explode() {
        if (!isStartingToExplode) {
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

    private void OnPhaseChange(NazroSecondPhaseEvent phaseChange) {
        Destroy(gameObject);
    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(GetHit);
        BossDiedEvent.UnRegisterListener(BossDied);
        NazroSecondPhaseEvent.UnRegisterListener(OnPhaseChange);
    }

}
