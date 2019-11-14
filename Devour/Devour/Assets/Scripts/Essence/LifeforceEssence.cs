//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeforceEssence : MonoBehaviour {

    public PlayerLifeForce PlayerLifeForce { get; set; }

    private Player player;
    private BoxCollider2D boxCollider2D;
    private bool isInRange;

    private void Awake() {
        if (GameController.Instance.PlayerLifeForce == null) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        PlayerLifeForce = GameController.Instance.PlayerLifeForce;
        if(SceneManager.GetActiveScene().name != PlayerLifeForce.SceneName) {
            Destroy(gameObject);
            return;
        }
        transform.position = PlayerLifeForce.Location;
        player = GameController.Instance.Player;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (isInRange) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 3 * Time.deltaTime);
        }
        if (boxCollider2D.bounds.Intersects(player.BoxCollider2D.bounds)) {
            PlayerCollectibleChange retrieveLifeforce = new PlayerCollectibleChange {
                collectible = PlayerLifeForce.Collectible
            };
            retrieveLifeforce.FireEvent();
            GameController.Instance.PlayerLifeForce = null;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            isInRange = false;
        }
    }

}
