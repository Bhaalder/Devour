//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroPhaseChangeState")]
public class NazroPhaseChangeState : NazroBaseState {

    [Tooltip("How fast Nazro escapes when he gets to half HP")]
    [SerializeField] private float escapeSpeed;
    [Tooltip("Time before he switches to wait state in another position")]
    [SerializeField] private float phaseChangeTime;

    private float waitTime = 0.5f;
    private float waitLeft;
    private bool wallIsBroken;
    private bool switchedPosition;

    public override void Enter() {
        owner.State = BossNazroState.PHASE_CHANGE;
        owner.BossLog("PhaseChangeState");
        owner.StopSounds();
        for (int i = 0; i < owner.NazroVoidObstacles.Count; i++) {
            Destroy(owner.NazroVoidObstacles[i]);
        }
        owner.NazroVoidObstacles.Clear();
        waitLeft = waitTime;
        NazroSecondPhaseEvent secondPhaseEvent = new NazroSecondPhaseEvent { };
        secondPhaseEvent.FireEvent();
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if (owner.RightWall != null && owner.BoxCollider2D.bounds.Intersects(owner.RightWall.GetComponent<BoxCollider2D>().bounds)) {
            owner.RightWall.GetComponent<NazroRightWall>().Break = true;
        }
        if (phaseChangeTime > 0) {
            phaseChangeTime -= Time.deltaTime;
            return;
        }
        if (!switchedPosition) {
            owner.BossArea.position = owner.SecondPhaseLocation.position;
            owner.transform.position = owner.MoveLocations[4].position;
            switchedPosition = true;
        }
        if(waitLeft > 0) {
            waitLeft -= Time.deltaTime;
            return;
        }
        owner.Transition<NazroWaitState>();
    }

    protected override void Movement() {
        owner.transform.position += new Vector3(escapeSpeed, 0) * Time.deltaTime;
    }
}

