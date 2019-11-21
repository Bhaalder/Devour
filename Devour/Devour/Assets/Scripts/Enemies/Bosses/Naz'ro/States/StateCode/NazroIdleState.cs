//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroIdleState")]
public class NazroIdleState : NazroBaseState {

    [Tooltip("How long time the introsequence lasts")]
    [SerializeField] private float introTime;
    private float introTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.IDLE;
        owner.BossLog("IdleState");
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    protected override void Movement() {

    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }
}
