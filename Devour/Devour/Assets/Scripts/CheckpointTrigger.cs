//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTrigger : MonoBehaviour {
    //Glöm ej att sätta ut spawnpoint vid rätt plats, det är dess child

    private void Awake() {
        Destroy(transform.GetChild(0).GetComponent<SpriteRenderer>());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameController.Instance.SceneCheckpoint = transform.GetChild(0).position;
        }
    }
}
