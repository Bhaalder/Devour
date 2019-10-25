//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaIntroState")]
public class ZvixaIntroState : ZvixaBaseState {

    [Tooltip("How long time the introsequence lasts")]
    [SerializeField] private float introTime;

    public override void Enter() {
        owner.State = BossZvixaState.INTRO;
        owner.BossLog("IntroState");
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(introTime > 0) {
            introTime -= Time.deltaTime;
            return;
        }
        owner.Transition<ZvixaIdleState>();
        base.HandleUpdate();
    }
}
