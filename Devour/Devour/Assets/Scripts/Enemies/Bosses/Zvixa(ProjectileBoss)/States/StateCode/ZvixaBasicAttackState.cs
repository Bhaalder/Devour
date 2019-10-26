//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaBasicAttackState")]
public class ZvixaBasicAttackState : ZvixaBaseState {

    [Tooltip("How long time before the ball comes out")]
    [SerializeField] private float basicAttackTimeWindUp;
    [Tooltip("How long time the basic attack lasts")]
    [SerializeField] private float basicAttackTime;
    [Tooltip("The projectile that Zvixa shoots out")]
    [SerializeField] private GameObject ballAttackGameObject;

    public override void Enter() {
        owner.State = BossZvixaState.BASICATTACK;
        owner.BossLog("BasicAttackState");
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        
        base.HandleUpdate();
    }
}
