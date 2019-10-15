using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerAirState")]
public class PlayerAirState : PlayerBaseState {

    public override void Enter() {
        owner.PlayerLog("AirState");
        owner.PlayerState = PlayerState.AIR;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }
}
