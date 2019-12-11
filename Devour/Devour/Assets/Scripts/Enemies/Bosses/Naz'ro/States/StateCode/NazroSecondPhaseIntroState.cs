//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroSecondPhaseIntroState")]
public class NazroSecondPhaseIntroState : NazroBaseState {

    [Tooltip("How long time the secondPhaseIntro lasts")]
    [SerializeField] private float secondPhaseIntroTime;
    private float introTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.SECOND_PHASE_INTRO;
        owner.BossLog("SecondPhaseIntroState");
        owner.transform.position = owner.MoveLocations[4].position;
        Instantiate(owner.BossDoor, owner.BossDoor.transform.position, Quaternion.identity);
        introTimeLeft = secondPhaseIntroTime;
        owner.IsSecondPhase = true;
        owner.SecondBossDoor.SetActive(true);
        NazroSecondPhaseEvent secondPhaseEvent = new NazroSecondPhaseEvent {};
        secondPhaseEvent.FireEvent();
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
