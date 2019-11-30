﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour{

    [SerializeField] private Player player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float delay;

    private Transform playerTransform;
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
        CameraBoundsChangeEvent.RegisterListener(SetCameraBounds);
        CameraTiltEvent.RegisterListener(OnTiltCamera);
        CameraZoomEvent.RegisterListener(OnChangeZoom);
    }

    private void Start() {
        player = GameController.Instance.Player;
        playerTransform = player.transform;
    }

    private void SetCameraBounds(CameraBoundsChangeEvent cameraBounds) {
        sceneBoxCollider = cameraBounds.cameraBounds;
        cameraBoundsIsFound = true;
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
        return new Vector3(playerTransform.position.x + (cameraOffset.x * player.FacingDirection), playerTransform.position.y + cameraOffset.y + cameraTiltValue, cameraOffset.z + cameraZoomValue);
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
        CameraBoundsChangeEvent.UnRegisterListener(SetCameraBounds);
        CameraTiltEvent.UnRegisterListener(OnTiltCamera);
        CameraZoomEvent.UnRegisterListener(OnChangeZoom);
    }
}
