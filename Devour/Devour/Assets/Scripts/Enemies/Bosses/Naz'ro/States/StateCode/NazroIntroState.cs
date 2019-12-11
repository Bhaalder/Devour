//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroIntroState")]
public class NazroIntroState : NazroBaseState {

    [Tooltip("How fast he moves during the introsequence")]
    [SerializeField] private float introMovementSpeed;
    [Tooltip("How long time the introsequence lasts")]
    [SerializeField] private float introTime;
    private float introTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.INTRO;
        owner.BossLog("IntroState");
        Instantiate(owner.BossDoor, owner.BossDoor.transform.position, Quaternion.identity);
        if (!GameController.Instance.BossIntroPlayed.Contains(owner.BossName)) {
            introTimeLeft = introTime;
        } else {
            introTimeLeft = 1;
        }
        battleStart = false;
        owner.BossIntroSequence();
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if (introTimeLeft > 0) {
            introTimeLeft -= Time.deltaTime;
            return;
        }
        owner.Transition<NazroIdleState>();
    }

    protected override void Movement() {
        owner.transform.position = Vector2.MoveTowards(owner.transform.position, owner.MoveLocations[4].position, introMovementSpeed * Time.deltaTime);
    }

    public override void Exit() {
        owner.BossIntroEnd();
        base.Exit();
    }

}
