//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    [SerializeField] private Transform player;
    [SerializeField] private float cameraZoom;

    void Start() {

    }
    
    void Update() {
        transform.position = new Vector3(player.position.x, player.position.y, cameraZoom);
    }
}
