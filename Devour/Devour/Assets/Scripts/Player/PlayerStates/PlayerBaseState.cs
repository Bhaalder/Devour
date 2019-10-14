using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerBaseState")]
public class PlayerBaseState : State {

    protected Player owner;

    public override void Enter() {
        owner.PlayerLog("Initialized Playerstates!");
        owner.Transition<PlayerIdleState>();
        base.Enter();
    }

    public override void Initialize(StateMachine owner) {
        this.owner = (Player)owner;
    }
}
