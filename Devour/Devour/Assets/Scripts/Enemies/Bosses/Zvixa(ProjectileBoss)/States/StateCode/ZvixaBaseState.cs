//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//SPIKEATTACK är en followup av SonarExpel (%-chans)
[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaBaseState")]
public class ZvixaBaseState : State {

    protected Zvixa owner;
    //protected bool isInSecondStage;
    private float moveTime = 2f;
    private float timeUntilNextMove;

    public override void Enter() {
        owner.BossLog("Initialized Zvixas states!");
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
        Movement();
    }

    public override void HandleUpdate() {
        if (owner.State == BossZvixaState.NONE && CheckPlayerPosition() == 2) {// och om player är inne i bossrummet
            owner.Transition<ZvixaIntroState>();
        }
        base.HandleUpdate();
    }

    protected virtual void Movement() {
        if(timeUntilNextMove > 0) {
            timeUntilNextMove -= Time.deltaTime;
            return;
        }
        timeUntilNextMove = moveTime;
        owner.Rb2d.velocity = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
    }

    protected int CheckPlayerPosition() {
        if (owner.Player.PlayerHorizontalMeleeCollider.bounds.Intersects(owner.HighArea.bounds)) {
            return 1;
        }
        if (owner.Player.PlayerHorizontalMeleeCollider.bounds.Intersects(owner.LowArea.bounds)) {
            return 2;
        }
        return -1;
    }

    public override void Initialize(StateMachine owner) {
        this.owner = (Zvixa)owner;
    }

}
