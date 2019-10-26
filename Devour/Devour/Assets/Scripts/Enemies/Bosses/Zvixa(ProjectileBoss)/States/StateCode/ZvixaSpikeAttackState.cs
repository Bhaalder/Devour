//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaSpikeAttackState")]
public class ZvixaSpikeAttackState : ZvixaBaseState {

    [Tooltip("How long time before the spikes come up")]
    [SerializeField] private float spikeAttackWindUpTime;
    [Tooltip("How long time the spikeAttack lasts")]
    [SerializeField] private float spikeAttackTime;
    [Tooltip("The projectile that Zvixa shoots out")]
    [SerializeField] private GameObject ballAttackGameObject;

    public override void Enter() {
        owner.State = BossZvixaState.SPIKE_ATTACK;
        owner.BossLog("SpikeAttackState");
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        
        base.HandleUpdate();
    }
}
