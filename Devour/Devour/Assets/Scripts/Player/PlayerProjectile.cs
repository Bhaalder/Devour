//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour{

    [SerializeField] private bool isBounce;

    public float Damage { get; set; }
    public Player Player { get; set; }
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }

    private float lifespan = 4f;
    private bool hitObject;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (!hitObject) {
            transform.position += (Vector3)Direction * Speed * Time.deltaTime;
        }
        if (lifespan > 0) {
            lifespan -= Time.deltaTime;
            return;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        try {
            PlayerAttackEvent playerAttack = new PlayerAttackEvent {
                attackCollider = boxCollider2D,
                isMeleeAttack = false,
                damage = Damage,
                player = Player,
                playerPosition = Player.transform.position
            };
            playerAttack.FireEvent();
        } catch (System.Exception) {

        }
        if (!isBounce) {
            if (collision.gameObject.layer == 8) {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag != "Player") {
            hitObject = true;
        }
        
    }

}
