//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidWallCollide : MonoBehaviour {

    public float Damage { get; set; }

    private BoxCollider2D boxCollider2D;

    private void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                damage = Damage,
                enemyPosition = GetComponent<Rigidbody2D>().position
            };
            playerTakeDamage.FireEvent();
        }
    }
}
