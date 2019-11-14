//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaDeathState")]
public class ZvixaDeathState : ZvixaBaseState {

    [Tooltip("How long time the deathState lasts")]
    [SerializeField] private float deathTime;
    private float deathTimeLeft;

    public override void Enter() {
        owner.State = BossZvixaState.DEATH;
        owner.BossLog("DeathState");
        deathTimeLeft = deathTime;
        BossDiedEvent zvixaDied = new BossDiedEvent {
            boss = owner
        };
        zvixaDied.FireEvent();
        Destroy(owner.LowArea.gameObject);
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if (deathTimeLeft > 0) {
            deathTimeLeft -= Time.deltaTime;
            return;
        }
        owner.SpawnAbilityEssence();
        owner.GiveLifeforce();
        Destroy(owner.gameObject);//FÖR TILLFÄLLET
        base.HandleUpdate();
    }
}
