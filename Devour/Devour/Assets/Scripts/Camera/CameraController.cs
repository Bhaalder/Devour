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
    private Transform targetTransform;
    private Vector3 velocity;
    private float cameraTiltValue;
    private float cameraZoomValue;

    public BoxCollider2D sceneBoxCollider;
    private bool cameraBoundsIsFound;

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
        CameraChangeTargetEvent.RegisterListener(OnChangeTarget);
        CameraBoundsChangeEvent.RegisterListener(SetCameraBounds);
        CameraTiltEvent.RegisterListener(OnTiltCamera);
        CameraZoomEvent.RegisterListener(OnChangeZoom);
    }

    private void Start() {
        player = GameController.Instance.Player;
        targetTransform = player.transform;
        playerTransform = player.transform;
    }

    private void SetCameraBounds(CameraBoundsChangeEvent cameraBounds) {
        sceneBoxCollider = cameraBounds.cameraBounds;
        cameraBoundsIsFound = true;
    }

    private void OnChangeTarget(CameraChangeTargetEvent targetEvent) {
        if (targetEvent.playerTarget) {
            targetTransform = playerTransform;
            return;
        }
        targetTransform = targetEvent.newTarget;
    }

    private void OnTiltCamera(CameraTiltEvent cameraTilt) {
        cameraTiltValue = cameraTilt.tiltValue;
    }

    private void OnChangeZoom(CameraZoomEvent cameraZoom) {
        cameraZoomValue = cameraZoom.zoomValue;
    }

    private void FixedUpdate() {
        Vector3 currentPosition = transform.position;

        transform.position = Vector3.SmoothDamp(currentPosition, DesiredPosition(), ref velocity, delay);
        if (sceneBoxCollider != null) {
            transform.position = new Vector3(CameraBoundsX(), CameraBoundsY(), cameraOffset.z + cameraZoomValue);
        }
    }

    private Vector3 DesiredPosition() {
        return new Vector3(targetTransform.position.x + (cameraOffset.x * player.FacingDirection), targetTransform.position.y + cameraOffset.y + cameraTiltValue, cameraOffset.z + cameraZoomValue);
    }

    #region CameraBounds
    private float CameraBoundsX() {
        return Mathf.Clamp(transform.position.x, sceneBoxCollider.bounds.min.x, sceneBoxCollider.bounds.max.x);
    }
    private float CameraBoundsY() {
        return Mathf.Clamp(transform.position.y, sceneBoxCollider.bounds.min.y, sceneBoxCollider.bounds.max.y);

    }

    #endregion

    public void SetCameraToPlayer() {
        transform.position = DesiredPosition();
    }

    private void OnDestroy() {
        CameraChangeTargetEvent.UnRegisterListener(OnChangeTarget);
        CameraBoundsChangeEvent.UnRegisterListener(SetCameraBounds);
        CameraTiltEvent.UnRegisterListener(OnTiltCamera);
        CameraZoomEvent.UnRegisterListener(OnChangeZoom);
    }
}
