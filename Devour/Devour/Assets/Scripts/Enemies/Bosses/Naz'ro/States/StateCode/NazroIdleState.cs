﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroIdleState")]
public class NazroIdleState : NazroBaseState {

    [Tooltip("The minimum amount of Idle time between actions")]
    [SerializeField] protected float minIdle;
    [Tooltip("The maximum amount of Idle time between actions")]
    [SerializeField] protected float maxIdle;
    [Tooltip("The chance in % that the boss will act immediately again after an action (whole number)")]
    [SerializeField] protected int actAgainPercentage;
    private float untilNextAction;
    private int actAgain;

    public override void Enter() {
        owner.State = BossNazroState.IDLE;
        owner.BossLog("IdleState");
        if (!battleStart) {
            battleStart = true;
        }
        actAgain = Random.Range(0, 100) + 1;
        if (actAgain <= actAgainPercentage) {
            untilNextAction = 0;
        } else {
            untilNextAction = Random.Range(minIdle, maxIdle)+1;
        }
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if (untilNextAction > 0) {
            untilNextAction -= Time.deltaTime;
            return;
        }
        owner.Transition<NazroVoidWallState>();
    }
}
