//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    [SerializeField] private Transform player;
    [SerializeField] private float cameraZoom;

    private static bool exists;

    private void Awake() {
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
            return;
        }
    }

    private void Start() {

    }
    
    void Update() {
        transform.position = new Vector3(player.position.x, player.position.y, cameraZoom);
    }
}
