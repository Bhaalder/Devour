//Author: Patrik Ahlgren
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
        introTimeLeft = introTime;
        battleStart = false;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(introTimeLeft > 0) {
            introTimeLeft -= Time.deltaTime;
            return;
        }
        owner.Transition<ZvixaIdleState>();
        base.HandleUpdate();
    }
}
