//Author: Patrik Ahlgren
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
    [Tooltip("The extra chance in % that the boss will act immediately again after an action in second phase(whole number)")]
    [SerializeField] protected int actAgainPercentageExtraSecondPhase;
    private float untilNextAction;
    private int chanceToActAgain;
    private int actAgainRoll;

    public override void Enter() {
        owner.State = BossNazroState.IDLE;
        owner.BossLog("IdleState");
        if (!battleStart) {
            battleStart = true;
        }
        actAgainRoll = Random.Range(0, 100) + 1;
        if (owner.IsSecondPhase) {
            chanceToActAgain = actAgainPercentage + actAgainPercentageExtraSecondPhase;
        } else {
            chanceToActAgain = actAgainPercentage;
        }
        if (actAgainRoll <= actAgainPercentage) {
            untilNextAction = 0;
        } else {
            untilNextAction = Random.Range(minIdle, maxIdle + 1);
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
        ChooseAction();
    }

    private void ChooseAction() {
        int i = Random.Range(0, 3) + 1;
        switch (i) {
            case 1:
                owner.Transition<NazroVoidWallState>();
                break;
            case 2:
                owner.Transition<NazroVoidBombState>();
                break;
            case 3:
                owner.Transition<NazroVoidCometState>();
                break;
            default:
                Debug.Log("Inget");
                break;
        }
        
    }

}
