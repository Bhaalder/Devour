﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaIntroState")]
public class ZvixaIntroState : ZvixaBaseState {

    [Tooltip("How long time the introsequence lasts")]
    [SerializeField] private float introTime;
    private float introTimeLeft;

    public override void Enter() {
        owner.State = BossZvixaState.INTRO;
        owner.BossLog("IntroState");
        owner.BossDoor.SetActive(true);
        if (!GameController.Instance.BossIntroPlayed.Contains(owner.BossName)) {
            introTimeLeft = introTime;
        } else {
            introTimeLeft = 1;
        }
        battleStart = false;
        if (teleportLocation != owner.TeleportAreaMiddle) {
            owner.rb.velocity = new Vector2(0, 0);
            owner.transform.position = owner.TeleportAreaMiddle.position;
            lastTeleport = owner.TeleportAreaMiddle;
        }
        owner.BossIntroSequence();
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    protected override void Movement() {

    }

    public override void HandleUpdate() {
        if(introTimeLeft > 0) {
            introTimeLeft -= Time.deltaTime;
            return;
        }
        owner.Transition<ZvixaIdleState>();
        base.HandleUpdate();
    }

    public override void Exit() {
        owner.BossIntroEnd();
        base.Exit();
    }
}
