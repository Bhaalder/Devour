using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossZvixaState {
    NONE, INTRO, IDLE, LIGHTORB_ATTACK, 
}

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaIdleState")]
public class ZvixaBaseState : State {

    protected Boss owner;
    protected BossZvixaState State;

    public override void Enter() {
        if(State == BossZvixaState.NONE) {
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
