﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossNazroState {
    NONE, INTRO, IDLE, VOID_BOMB, VOID_WALL, VOID_COMET, VOID_OBS, WAIT, DEATH
}

public class Nazro : Boss {
    public BossNazroState State { get; set; }

    public BoxCollider2D RightArea { get; set; }
    public BoxCollider2D HighArea { get; set; }
    public BoxCollider2D StartFightArea { get; set; }

    public Transform TeleportAreaLeft { get; set; }
    public Transform TeleportAreaMiddle { get; set; }
    public Transform TeleportAreaRight { get; set; }
    public GameObject BossDoor { get; set; }

    public float DistanceToPlayer { get; set; }

    [SerializeField] private BoxCollider2D rightArea;
    [SerializeField] private BoxCollider2D highArea;
    [SerializeField] private BoxCollider2D startFightArea;
    [SerializeField] private GameObject bossDoor;

    public static bool IsDead { get; set; }

    protected override void Awake() {
        if (IsDead) {
            Destroy(gameObject);
        }
        base.Awake();
        RightArea = rightArea;
        HighArea = highArea;
        StartFightArea = startFightArea;
        BossDoor = bossDoor;

        PlayerDiedEvent.RegisterListener(Reset);
    }

    protected override void Update() {
        base.Update();
        //Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate() {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        base.FixedUpdate();
    }

    protected override void EnemyTouchKillzone(EnemyTouchKillzoneEvent killzoneEvent) {

    }

    protected override void OnTriggerStay2D(Collider2D collision) {

    }

    public virtual void SelfDamage(ZvixaSelfDamageEvent selfDamageEvent) {
        ChangeEnemyHealth(-selfDamageEvent.damage);
    }

    private void Reset(PlayerDiedEvent playerDied) {
        Health = MaxHealth;
        State = BossNazroState.NONE;
        //Transition<ZvixaBaseState>();
    }

    public override void EnemyDeath() {
        if (!IsDead) {
            IsDead = true;
        }
        if (State != BossNazroState.DEATH) {
            //Transition<ZvixaDeathState>(); DEATHSTATE
        }
    }

    protected override void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        PlayerDiedEvent.UnRegisterListener(Reset);
    }
}
