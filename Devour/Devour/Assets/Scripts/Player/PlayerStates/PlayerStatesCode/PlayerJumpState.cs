//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerJumpState")]
public class PlayerJumpState : PlayerBaseState {

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
