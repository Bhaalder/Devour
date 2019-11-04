﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerAirState")]
public class PlayerAirState : PlayerBaseState {

    public override void Enter() {
        //owner.PlayerLog("AirState");
        owner.PlayerState = PlayerState.AIR;
        owner.IsAttackingUp = false;
        owner.Animator.SetBool("IsAttackingUp", false);
        owner.Animator.SetBool("IsDJ", false);
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        //if(Input.GetAxisRaw("Vertical") < 0) {
        //    owner.IsAttackingDown = true;
        //    owner.Animator.SetBool("IsAttackingDown", true);
        //} else {
        //    owner.IsAttackingDown = false;
        //    owner.Animator.SetBool("IsAttackingDown", false);
        //}

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
