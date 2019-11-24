﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroDeathState")]
public class NazroDeathState : NazroBaseState {

    [Tooltip("How long time the deathState lasts")]
    [SerializeField] private float deathTime;
    private float deathTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.DEATH;
        owner.BossLog("DeathState");
        deathTimeLeft = deathTime;
        BossDiedEvent nazroDied = new BossDiedEvent {
            boss = owner
        };
        nazroDied.FireEvent();
        foreach (GameObject obs in owner.NazroVoidObstacles) {
            Destroy(obs);
        }
        owner.NazroVoidObstacles.Clear();
        base.Enter();
    }

    public override void HandleFixedUpdate() {

    }

    public override void HandleUpdate() {
        if (deathTimeLeft > 0) {
            deathTimeLeft -= Time.deltaTime;
            return;
        }
        Destroy(owner.gameObject);//FÖR TILLFÄLLET
    }
}