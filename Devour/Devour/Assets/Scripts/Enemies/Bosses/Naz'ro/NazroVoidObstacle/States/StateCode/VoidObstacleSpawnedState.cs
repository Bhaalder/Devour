//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/VoidObstacle/VoidObstacleSpawnedState")]
public class VoidObstacleSpawnedState : State {

    protected NazroVoidObstacle owner;

    public override void Enter() {
        owner.VoidObstacleState = VoidObstacleState.SPAWNED;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if (owner.ArmingTime > 0) {
            owner.ArmingTime -= Time.deltaTime;
            return;
        }
        owner.Transition<VoidObstacleArmedState>();
        base.HandleUpdate();
    }

    public override void Initialize(StateMachine owner) {
        this.owner = (NazroVoidObstacle)owner;
    }

}
