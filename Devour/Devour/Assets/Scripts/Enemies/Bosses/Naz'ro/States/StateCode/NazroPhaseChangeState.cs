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

    public override void Enter() {
        owner.State = BossNazroState.PHASE_CHANGE;
        owner.BossLog("PhaseChangeState");
        foreach(GameObject obs in owner.NazroVoidObstacles) {
            Destroy(obs);
        }
        owner.NazroVoidObstacles.Clear();

        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if (phaseChangeTime > 0) {
            phaseChangeTime -= Time.deltaTime;
            return;
        }
        //FÖRSTÖR VÄGGEN TILL HÖGER NÄR HAN NUDDAR DEN
        //GO TO WAITSTATE AND TELEPORT
    }

    protected override void Movement() {
        
    }

}

