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
    
    private float windUpLeft;
    private float attackTimeLeft;


    public override void Enter() {
        owner.State = BossZvixaState.SPIKE_ATTACK;
        owner.BossLog("SpikeAttackState");
        windUpLeft = spikeAttackWindUpTime;
        attackTimeLeft = spikeAttackTime;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0) {
            Debug.Log("Spikes spawns (not implemented)");
        }
        if (attackTimeLeft <= 0) {
            owner.Transition<ZvixaIdleState>();
        }
        base.HandleUpdate();
    }
}
