//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerJumpState")]
public class PlayerJumpState : PlayerBaseState {

    public override void Enter() {
        //owner.PlayerLog("JumpState");
        owner.PlayerState = PlayerState.JUMP;
        owner.IsAttackingUp = false;
        owner.Animator.SetBool("IsAttackingUp", false);
        owner.Transition<PlayerAirState>();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }

    public override void Exit() {
        base.Exit();
    }

}
