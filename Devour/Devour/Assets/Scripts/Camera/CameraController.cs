//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour{

    [SerializeField] private Player player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float delay;
    private Transform playerTransform;
    private Vector3 desiredPosition;
    private Vector3 velocity;

    [SerializeField] private BoxCollider2D sceneCamerabounds;

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

        if (Input.GetKeyDown(KeyCode.P)) {
            Destroy(sceneCamerabounds.gameObject);
        }

        desiredPosition = new Vector3(playerTransform.position.x + (cameraOffset.x * player.FacingDirection), playerTransform.position.y + cameraOffset.y, cameraOffset.z);

        Vector3 currentPosition = transform.position;

        transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, delay);
        try {
            transform.position = new Vector3(CameraBoundsX(), CameraBoundsY(), cameraOffset.z);
        } catch (System.Exception) {
            
        }
    }

    private float CameraBoundsX() {
        try {
            return Mathf.Clamp(transform.position.x, sceneCamerabounds.bounds.min.x, sceneCamerabounds.bounds.max.x);
        } catch (System.Exception) {
            try {
                sceneCamerabounds = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
            } catch (System.Exception) {
                Debug.LogWarning("SceneCameraBounds is not defined in CameraHolder and cannot be found in scene!");
            }
            return -1;
        }
    }
    private float CameraBoundsY() {
        try {
            return Mathf.Clamp(transform.position.y, sceneCamerabounds.bounds.min.y, sceneCamerabounds.bounds.max.y);
        } catch (System.Exception) {
            return -1;
        }       
    }
}
