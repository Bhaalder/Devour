using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossZvixaState {
    NONE, INTRO, IDLE, PREPARE_BASICATTACK, BASICATTACK, PREPARE_SONAR_EXPEL, SONAR_EXPEL, SPIKE_ATTACK
}

//SPIKEATTACK är en followup av SonarExpel (%-chans)
[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaBaseState")]
public class ZvixaBaseState : State {

    protected Boss owner;
    protected BossZvixaState State;
    //protected bool isInSecondStage;
    protected Player player;

    public override void Enter() {
        owner.BossLog("Initialized Zvixas states!");
        if (State == BossZvixaState.NONE) {// och om player är inne i bossrummet
            player = GameController.Instance.Player;
            owner.Transition<ZvixaIdleState>();
        }
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }

    public override void Initialize(StateMachine owner) {
        this.owner = (Boss)owner;
    }

}
