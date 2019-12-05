using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBombExplosion : MonoBehaviour {

    public float Damage { get; set; }

    [SerializeField] private float explosionTime;
    private CircleCollider2D circleCollider2D;

    private void Update() {
        explosionTime -= Time.deltaTime;
        if(explosionTime <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                damage = Damage,
                enemyPosition = transform.position
            };
            playerTakeDamage.FireEvent();
        }
    }

}
