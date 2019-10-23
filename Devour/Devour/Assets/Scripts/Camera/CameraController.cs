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

    private BoxCollider2D sceneBoxCollider;
    private bool cameraBoundsIsFound;
    float checkBoundsTimer = 1f;
    float untilNextBoundsCheck;

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
        try {
            sceneBoxCollider = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
            cameraBoundsIsFound = true;
        } catch (System.NullReferenceException) {
            Debug.LogWarning("SceneCameraBounds cannot be found in scene!");
            cameraBoundsIsFound = false;
        }
        untilNextBoundsCheck = checkBoundsTimer;
    }
    
    private void FixedUpdate() {
        desiredPosition = new Vector3(playerTransform.position.x + (cameraOffset.x * player.FacingDirection), playerTransform.position.y + cameraOffset.y, cameraOffset.z);

        Vector3 currentPosition = transform.position;

        transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, delay);
        if (cameraBoundsIsFound) {
            transform.position = new Vector3(CameraBoundsX(), CameraBoundsY(), cameraOffset.z);
        } else { 
            CheckCameraBounds();
        }
    }

    #region CameraBounds
    private float CameraBoundsX() {
        if (CameraBoundsExist()) {
            return Mathf.Clamp(transform.position.x, sceneBoxCollider.bounds.min.x, sceneBoxCollider.bounds.max.x);
        }
        return -1;
    }
    private float CameraBoundsY() {
        if (CameraBoundsExist()) {
            return Mathf.Clamp(transform.position.y, sceneBoxCollider.bounds.min.y, sceneBoxCollider.bounds.max.y);
        }
        return -1;
        
    }

    private void CheckCameraBounds() {
        if(untilNextBoundsCheck <= 0) {
            if (CameraBoundsExist()) {
                sceneBoxCollider = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
            }
            untilNextBoundsCheck = checkBoundsTimer;
            return;
        }
        untilNextBoundsCheck -= Time.deltaTime;
    }

    private bool CameraBoundsExist() {
        try {
            if (GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>()) {
                cameraBoundsIsFound = true;
                return true;
            }
        } catch (System.NullReferenceException) {
            Debug.LogWarning("SceneCameraBounds cannot be found in scene!");
        }
        cameraBoundsIsFound = false;
        return false;
    }
    #endregion

}
