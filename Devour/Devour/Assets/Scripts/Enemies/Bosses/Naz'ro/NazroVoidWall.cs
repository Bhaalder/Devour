//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidWall : MonoBehaviour {

    public float Damage { get; set; }
    public float SelfDamage { get; set; }
    public float LifeSpan { get; set; }
    public float Speed { get; set; }

    [SerializeField] private float speed;
    [SerializeField] private Vector2 direction;

    private BoxCollider2D boxCollider2D;
    private bool gotHit;

    private void Start() {
        if(Speed == 0) {
            Speed = speed;
        }
        
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void FixedUpdate() {
        LifeSpan -= Time.deltaTime;
        transform.position += ((Vector3)direction * Speed) * Time.deltaTime;
        //if (LifeSpan <= 0) {
        //    Destroy(gameObject);
        //}
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

    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        BossDiedEvent.UnRegisterListener(BossDied);
    }

}
