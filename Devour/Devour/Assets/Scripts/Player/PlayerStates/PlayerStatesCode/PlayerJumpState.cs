﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerJumpState")]
public class PlayerJumpState : PlayerBaseState {

    [SerializeField] private float justInTimeJump;
    private float justInTimeJumpLeft;
    private float timeBeforeEnter = 0.00001f;
    private float timeLeft;

    public override void Enter() {
        //owner.PlayerLog("JumpState");
        owner.PlayerState = PlayerState.JUMP;
        owner.IsAttackingUp = false;
        owner.Animator.SetBool("IsAttackingUp", false);
        timeLeft = timeBeforeEnter;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if (justInTimeJumpLeft > 0) {
            justInTimeJumpLeft -= Time.deltaTime;
        }
        if (!hasPressedJump && !owner.IsGrounded && Input.GetButtonDown("Jump") && justInTimeJumpLeft > 0) {
            Jump(0);
            return;
        }
        if (Input.GetButton("Jump")) {
            hasPressedJump = true;
        }
        if (timeLeft <= 0) {
            owner.Transition<PlayerAirState>();
        }
        timeLeft -= Time.deltaTime;
        base.HandleUpdate();
    }

    public override void Exit() {
        base.Exit();
    }

}
