//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerBusyState")]
public class PlayerBusyState : PlayerBaseState {

    public override void Enter() {
        //owner.PlayerLog("BaseState");
        owner.PlayerState = PlayerState.BUSY;    
    }

    public override void HandleFixedUpdate() {
        
    }

    public override void HandleUpdate() {
        if (!GameController.Instance.GameIsPaused) {
            if (!owner.IsGrounded) {
                owner.Transition<PlayerAirState>();
            } else {
                owner.Transition<PlayerIdleState>();
            }
        }
    }

    public override void Exit() {
        base.Exit();
    }
}
