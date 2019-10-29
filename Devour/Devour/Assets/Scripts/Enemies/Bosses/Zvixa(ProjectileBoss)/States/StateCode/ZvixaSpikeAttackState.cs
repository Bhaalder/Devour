//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaSpikeAttackState")]
public class ZvixaSpikeAttackState : ZvixaBaseState {

    [Tooltip("How much damage the spikes does on contact")]
    [SerializeField] private float spikeDamage;
    [Tooltip("How long time before the spikes come up")]
    [SerializeField] private float spikeAttackWindUpTime;
    [Tooltip("How long time the spikeAttack lasts")]
    [SerializeField] private float spikeAttackTime;
    
    private float windUpLeft;
    private float attackTimeLeft;
    private bool spikesAreUp;
    private SpriteRenderer spriteRenderer;

    public override void Enter() {
        owner.State = BossZvixaState.SPIKE_ATTACK;
        owner.BossLog("SpikeAttackState");
        spikesAreUp = false;
        windUpLeft = spikeAttackWindUpTime;
        attackTimeLeft = spikeAttackTime;
        spriteRenderer = owner.LowArea.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(255, 0, 0, 25);
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && !spikesAreUp) {
            spriteRenderer.color = new Color32(255, 0, 0, 255);
            spikesAreUp = true;
        }
        if (spikesAreUp) {
            if (owner.Player.PlayerHorizontalMeleeCollider.bounds.Intersects(owner.LowArea.bounds)) {
                PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                    damage = spikeDamage,
                    enemyPosition = owner.LowArea.transform.position
                };
                playerTakeDamage.FireEvent();
            }
        }
        if (attackTimeLeft <= 0) {
            spikesAreUp = false;
            owner.Transition<ZvixaIdleState>();
        }
        base.HandleUpdate();
    }

    public override void Exit() {
        spriteRenderer.color = new Color32(255, 0, 0, 0);
        base.Exit();
    }
}
