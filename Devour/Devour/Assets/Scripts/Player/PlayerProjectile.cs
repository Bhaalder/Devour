//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour{

    public float Damage { get; set; }
    public Player Player { get; set; }
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }

    private float lifespan = 4f;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        

    }

    private void Update() {
        transform.position += (Vector3)Direction * Speed * Time.deltaTime;
        if(lifespan > 0) {
            lifespan -= Time.deltaTime;
            return;
        }
        Destroy(gameObject);
    }

    private void FixedUpdate() {
        


    }

    private void OnTriggerEnter2D(Collider2D collision) {
        try {
            collision.gameObject.GetComponent<Enemy>().ChangeEnemyHealth(-Damage);
        } catch (System.Exception) {

        }
        //Debug.Log("COLLISION!");
        //PlayerAttackEvent pae = new PlayerAttackEvent {
        //    attackCollider = boxCollider2D,
        //    damage = Damage,
        //    player = Player,
        //    isLifeLeechAttack = false
        //};
        if (collision.gameObject.layer == 8) {
            Destroy(gameObject);
        }
    }

}
