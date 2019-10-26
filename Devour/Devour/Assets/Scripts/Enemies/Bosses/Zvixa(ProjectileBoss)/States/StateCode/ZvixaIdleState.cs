//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaIdleState")]
public class ZvixaIdleState : ZvixaBaseState {

    [Tooltip("The minimum amount of Idle time between actions")]
    [SerializeField] protected float minIdle;
    [Tooltip("The maximum amount of Idle time between actions")]
    [SerializeField] protected float maxIdle;
    [Tooltip("The chance in % that the boss will act immediately again after an action")]
    [SerializeField] protected int actAgainPercentage;
    private float untilNextAction;
    private int actAgain;

    public override void Enter() {
        owner.State = BossZvixaState.IDLE;
        owner.BossLog("IdleState");
        actAgain = Random.Range(0, 100) + 1;
        if(actAgain <= actAgainPercentage) {
            untilNextAction = 0;
        } else {
            untilNextAction = Random.Range(minIdle, maxIdle);
        }
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if(untilNextAction > 0) {
            untilNextAction -= Time.deltaTime;
            return;
        }
        DecideAction();
    }

    private void DecideAction() {
        int percentage = Random.Range(0, 100) + 1;
        switch (CheckPlayerPosition()) {
            case 1:
                if(percentage <= 60) {
                    owner.Transition<ZvixaBasicAttackState>();
                } else {
                    owner.Transition<ZvixaSonarExpelState>();
                }
                break;
            case 2:
                if (percentage <= 60) {
                    owner.Transition<ZvixaBasicAttackState>();
                } else {
                    owner.Transition<ZvixaSpikeAttackState>();
                }
                break;
            default:
                break;
        }
    }

}
