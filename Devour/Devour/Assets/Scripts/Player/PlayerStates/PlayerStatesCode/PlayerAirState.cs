//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerAirState")]
public class PlayerAirState : PlayerBaseState {

    [SerializeField] private float justInTimeJump;
    private float justInTimeJumpLeft;

    public override void Enter() {
        //owner.PlayerLog("AirState");
        owner.PlayerState = PlayerState.AIR;
        owner.IsAttackingUp = false;
        owner.Animator.SetBool("IsAttackingUp", false);
        justInTimeJumpLeft = justInTimeJump;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(justInTimeJumpLeft > 0) {
            justInTimeJumpLeft -= Time.deltaTime;
        }
        if(!hasPressedJump && !owner.IsGrounded && Input.GetButtonDown("Jump") && justInTimeJumpLeft > 0) {
            Jump(0);
            return;
        }
        if (owner.IsGrounded) {           
            owner.Animator.SetBool("IsLanding", true);
            if (Input.GetButton("Horizontal")) {
                owner.Transition<PlayerWalkState>();
            } else {
                owner.Transition<PlayerIdleState>();
            }    
        }
        base.HandleUpdate();
    }

    public override void Exit() {
        owner.IsAttackingDown = false;
        base.Exit();
    }

}
