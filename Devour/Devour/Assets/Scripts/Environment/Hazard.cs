//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour{

    [Tooltip("How much damage the player takes on contact")]
    [SerializeField] private float playerDamage;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerTouchKillzoneEvent playerTouchKillzone = new PlayerTouchKillzoneEvent {
                damage = playerDamage
            };
            playerTouchKillzone.FireEvent();
        } else if (collision.gameObject.tag == "Enemy") {
            EnemyTouchKillzoneEvent enemyTouchKillzone = new EnemyTouchKillzoneEvent {
                enemy = collision.gameObject.GetComponent<Enemy>()
            };
            enemyTouchKillzone.FireEvent();
        }
    }
}
