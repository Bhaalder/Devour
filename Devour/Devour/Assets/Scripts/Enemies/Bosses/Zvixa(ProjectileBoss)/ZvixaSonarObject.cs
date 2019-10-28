using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZvixaSonarObject : MonoBehaviour {

    public float Damage { get; set; }
    public float LifeSpan { get; set; }
    public float Growth { get; set; }

    private float growing;

    private void Start() {

    }

    private void FixedUpdate() {
        LifeSpan -= Time.deltaTime;
        if (LifeSpan > 0.5f) {
            if(growing <= 1) {
                growing = 1.01f;
            }
            transform.localScale *= Growth;
        } else {
            transform.localScale *= Growth / 1.2f;
        }
        if(LifeSpan <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("SonarExpel hit Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                damage = Damage,
                enemyPosition = GetComponent<Rigidbody2D>().position
            };
            ptde.FireEvent();
        }
    }

}
