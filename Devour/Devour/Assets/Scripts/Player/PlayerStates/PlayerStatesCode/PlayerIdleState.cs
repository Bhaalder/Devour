//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerIdleState")]
public class PlayerIdleState : PlayerBaseState {

    [Tooltip("How much the camera will tilt up or down")]
    [SerializeField] private float cameraTiltValue;
    [Tooltip("The time before tilting camera up or down when pressing a vertical input")]
    [SerializeField] private float cameraTiltTime;
    private float tempCameraTiltValue;
    private float cameraTiltTimeLeft;
    private bool isTiltingCamera;

    public override void Enter() {
        //owner.PlayerLog("IdleState");
        owner.PlayerState = PlayerState.IDLE;
        owner.Animator.SetBool("IsLanding", false);
        cameraTiltTimeLeft = cameraTiltTime;
        hasPressedJump = false;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        CameraTilt();
        if (owner.XInput != 0 && owner.IsGrounded) {
            ResetCameraTilt();
            owner.Transition<PlayerWalkState>();
        }
        if (!owner.IsGrounded) {
            ResetCameraTilt();
            owner.Transition<PlayerJumpState>();
        }
    }

    private void CameraTilt() {
        if(isTiltingCamera && !TiltUp() && !TiltDown()) {
            ResetCameraTilt();
            isTiltingCamera = false;
        }
        if (TiltUp()) {
            cameraTiltTimeLeft -= Time.deltaTime;
            tempCameraTiltValue = cameraTiltValue;
        } else if (TiltDown()) {
            cameraTiltTimeLeft -= Time.deltaTime;
            tempCameraTiltValue = -cameraTiltValue;
        } else {
            cameraTiltTimeLeft = cameraTiltTime;
            isTiltingCamera = false;
        }
        if (cameraTiltTimeLeft <= 0 && !isTiltingCamera) {
            CameraTiltEvent cameraTilt = new CameraTiltEvent {
                tiltValue = tempCameraTiltValue
            };
            cameraTilt.FireEvent();
            isTiltingCamera = true;
        }
        
        //Debug.Log(owner.XInput);
    }

    private bool TiltUp() {
        if (Input.GetAxisRaw("Vertical") > 0f) {
            return true;
        }
        return false;
    }
    private bool TiltDown() {
        if (Input.GetAxisRaw("Vertical") < 0f) {
            return true;
        }
        return false;
    }

    private void ResetCameraTilt() {
        CameraTiltEvent cameraTilt = new CameraTiltEvent {
            tiltValue = 0
        };
        cameraTilt.FireEvent();
    }

}
