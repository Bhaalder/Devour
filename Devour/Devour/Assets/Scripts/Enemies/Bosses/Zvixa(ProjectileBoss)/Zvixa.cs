//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossZvixaState {
    NONE, INTRO, IDLE, BASICATTACK, SONAR_EXPEL, SPIKE_ATTACK, STAGGER, DEATH
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

    public int FacingDirection { get; set; }

    
    public float DistanceToPlayer { get; set; }

    [SerializeField] private BoxCollider2D highArea;
    [SerializeField] private BoxCollider2D lowArea;
    [SerializeField] private BoxCollider2D startFightArea;
    [SerializeField] private Transform teleportAreaLeft;
    [SerializeField] private Transform teleportAreaMiddle;
    [SerializeField] private Transform teleportAreaRight;
    [SerializeField] private GameObject bossDoor;

    

    private static bool isDead;

    protected override void Awake() {
        if (isDead) {
            Destroy(gameObject);
        }
        base.Awake();
        HighArea = highArea;
        LowArea = lowArea;
        StartFightArea = startFightArea;
        TeleportAreaLeft = teleportAreaLeft;
        TeleportAreaMiddle = teleportAreaMiddle;
        TeleportAreaRight = teleportAreaRight;
        BossDoor = bossDoor;

        PlayerDiedEvent.RegisterListener(Reset);
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        base.FixedUpdate();
    }

    protected override void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
        }
        rb.velocity = new Vector2(0, 0);
    }

    private void Reset(PlayerDiedEvent playerDied) {
        Health = MaxHealth;
        State = BossZvixaState.NONE;
        Transition<ZvixaBaseState>();
        transform.position = TeleportAreaMiddle.position;
        BossDoor.SetActive(false);
    }

    public override void EnemyDeath() {
        //Transition till DeathState
        isDead = true;
        BossDiedEvent zvixaDied = new BossDiedEvent {
            boss = this
        };
        zvixaDied.FireEvent();
        Destroy(gameObject);//FÖR TILLFÄLLET
    }

    protected override void OnDestroy() {
        BossDoor.SetActive(false);
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        PlayerDiedEvent.UnRegisterListener(Reset);
    }
}
