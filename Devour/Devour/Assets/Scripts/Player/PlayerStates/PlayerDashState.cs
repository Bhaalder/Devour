using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerDashState")]
public class PlayerDashState : PlayerBaseState {

    public override void Enter() {
        owner.PlayerLog("DashState");
        owner.PlayerState = PlayerState.DASH;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }
}
