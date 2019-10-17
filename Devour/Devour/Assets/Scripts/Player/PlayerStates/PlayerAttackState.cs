//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerAttackState")]
public class PlayerAttackState : PlayerBaseState {

    public override void Enter() {
        owner.PlayerLog("AttackState");
        owner.PlayerState = PlayerState.ATTACK;

    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }
}
