//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroIntroState")]
public class NazroIntroState : NazroBaseState {

    [Tooltip("How long time the introsequence lasts")]
    [SerializeField] private float introTime;
    private float introTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.INTRO;
        owner.BossLog("IntroState");
        owner.BossDoor.SetActive(true);
        introTimeLeft = introTime;
        battleStart = false;

        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if (introTimeLeft > 0) {
            introTimeLeft -= Time.deltaTime;
            return;
        }
        owner.Transition<NazroIdleState>();
        base.HandleUpdate();
    }
}
