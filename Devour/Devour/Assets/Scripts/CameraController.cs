//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    [SerializeField] private Player player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float delay;
    private Transform playerTransform;
    private Vector3 desiredPosition;
    private Vector3 zero = Vector3.zero;

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
        player = GameController.Instance.Player;
        playerTransform = player.transform;
    }
    
    private void FixedUpdate() {
        desiredPosition = new Vector3(playerTransform.position.x + (cameraOffset.x*player.FacingDirection), playerTransform.position.y + cameraOffset.y, cameraOffset.z);
        Vector3 currentPosition = transform.position;
        transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref zero, delay);
    }
}
