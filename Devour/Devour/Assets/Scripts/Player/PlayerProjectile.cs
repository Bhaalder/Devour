//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour{

    public float Damage { get; set; }
    public Player Player { get; set; }
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public float ProjectileHealthcost { get; set; }

    private float lifespan = 4f;
    private bool hitObject;
    private bool canDealDamage;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        AudioPlaySoundAtLocationEvent projectileSound = new AudioPlaySoundAtLocationEvent {
            name = "Projectile",
            isRandomPitch = true,
            minPitch = 0.90f,
            maxPitch = 1,
            soundType = SoundType.SFX,
            gameObject = gameObject
        };
        projectileSound.FireEvent();
    }

    private void Start() {
        PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
            damage = ProjectileHealthcost,
            isSelfInflicted = true
        };
        playerTakeDamage.FireEvent();
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
        if(collision.gameObject.tag == "Enemy") {
            if (boxCollider2D.isActiveAndEnabled) {
                PlayerAttackEvent playerAttack = new PlayerAttackEvent {
                    attackCollider = boxCollider2D,
                    isMeleeAttack = false,
                    damage = Damage,
                    player = Player,
                    playerPosition = Player.transform.position
                };
                playerAttack.FireEvent();
            }
        }
        if (collision.gameObject.layer == 8) {
            Destroy(transform.GetChild(0).gameObject);
            hitObject = true;
            boxCollider2D.enabled = false;
        }
    }
}
