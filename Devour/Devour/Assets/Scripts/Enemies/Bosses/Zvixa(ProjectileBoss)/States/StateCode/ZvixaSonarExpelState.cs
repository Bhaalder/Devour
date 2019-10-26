//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaSonarExpelState")]
public class ZvixaSonarExpelState : ZvixaBaseState {

    [Tooltip("How long time the introsequence lasts")]
    [SerializeField] private float introTime;
    [Tooltip("The projectile that Zvixa shoots out")]
    [SerializeField] private GameObject sonarExpelGameObject;

    public override void Enter() {
        owner.State = BossZvixaState.SONAR_EXPEL;
        owner.BossLog("SonarExpelState");
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        
        base.HandleUpdate();
    }
}
