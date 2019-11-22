﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidComet : MonoBehaviour {

    public float Damage { get; set; }
    public float Speed { get; set; }
    public float WindUp { get; set; }
    public Transform StartPosition { get; set; }

    [SerializeField] private Vector2 direction;
    [SerializeField] private float lifeSpan;
    [SerializeField] private bool isVerticalComet;
    private bool isMoving;

    private Transform player;
    private CircleCollider2D circleCollider2D;

    private void Start() {
        player = GameController.Instance.Player.transform;
        circleCollider2D = GetComponent<CircleCollider2D>();
        if(isVerticalComet) {
            transform.position = new Vector3(player.position.x, StartPosition.position.y);  
        } else {
            transform.position = new Vector3(StartPosition.position.x, player.position.y);
        }
        PlayerAttackEvent.RegisterListener(GetHit);
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void Update() {
        if (WindUp > 0 && !isMoving) {
            WindUp -= Time.deltaTime;
            return;
        }
        isMoving = true;
    }

    private void FixedUpdate() {
        if (isMoving) {
            lifeSpan -= Time.deltaTime;
            transform.position += ((Vector3)direction * Speed) * Time.deltaTime;
            if (lifeSpan <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void GetHit(PlayerAttackEvent attackEvent) {
        if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
            //SKA NÅGOT HÄNDA DÅ?
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                damage = Damage
            };
            playerTakeDamage.FireEvent();
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
