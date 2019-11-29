//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossZvixaState {
    NONE, INTRO, IDLE, BASICATTACK, SONAR_EXPEL, SPIKE_ATTACK, DEATH
}

public class Zvixa : Boss{
    public BossZvixaState State { get; set; }

    public BoxCollider2D HighArea { get; set; }
    public BoxCollider2D LowArea { get; set; }
    public BoxCollider2D StartFightArea { get; set; }

    public Transform TeleportAreaLeft { get; set; }
    public Transform TeleportAreaMiddle { get; set; }
    public Transform TeleportAreaRight { get; set; }
    public GameObject BossDoor { get; set; }
    public GameObject TipAfterZvixa { get => tipAfterZvixa; set => tipAfterZvixa = value; }

    public int FacingDirection { get; set; }

    public float DistanceToPlayer { get; set; }

    public float XScale { get; set; }

    [SerializeField] private BoxCollider2D highArea;
    [SerializeField] private BoxCollider2D lowArea;
    [SerializeField] private BoxCollider2D startFightArea;
    [SerializeField] private Transform teleportAreaLeft;
    [SerializeField] private Transform teleportAreaMiddle;
    [SerializeField] private Transform teleportAreaRight;
    [SerializeField] private GameObject bossDoor;
    [SerializeField] private GameObject tipAfterZvixa;

    public static bool IsDead { get; set; }

    protected override void Awake() {
        if (IsDead) {
            Destroy(gameObject);
        }
        XScale = transform.localScale.x;
        base.Awake();
        HighArea = highArea;
        LowArea = lowArea;
        StartFightArea = startFightArea;
        TeleportAreaLeft = teleportAreaLeft;
        TeleportAreaMiddle = teleportAreaMiddle;
        TeleportAreaRight = teleportAreaRight;
        BossDoor = bossDoor;

        PlayerDiedEvent.RegisterListener(Reset);
        ZvixaSelfDamageEvent.RegisterListener(SelfDamage);
    }

    protected override void Update() {
        base.Update();
        Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate() {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        base.FixedUpdate();
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!IsDead) {
                Debug.Log("Collided with Player");
                PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                    damage = damageToPlayerOnContact,
                    enemyPosition = rb.position
                };
                playerTakeDamage.FireEvent();
            }
        }
        rb.velocity = new Vector2(0, 0);
    }

    public virtual void SelfDamage(ZvixaSelfDamageEvent selfDamageEvent) {
        ChangeEnemyHealth(-selfDamageEvent.damage);
        HurtSoundAndParticles();
    }

    private void Reset(PlayerDiedEvent playerDied) {
        Health = MaxHealth;
        State = BossZvixaState.NONE;
        Transition<ZvixaBaseState>();
        transform.position = TeleportAreaMiddle.position;
        BossDoor.SetActive(false);
    }

    public override void EnemyDeath() {
        if (!IsDead) {
            IsDead = true;
        }
        if(State != BossZvixaState.DEATH) {
            Transition<ZvixaDeathState>();
        }
    }

    protected override void OnDestroy() {
        BossDoor.SetActive(false);
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        PlayerDiedEvent.UnRegisterListener(Reset);
        ZvixaSelfDamageEvent.UnRegisterListener(SelfDamage);
        EnemyTouchKillzoneEvent.UnRegisterListener(EnemyTouchKillzone);
    }
}
