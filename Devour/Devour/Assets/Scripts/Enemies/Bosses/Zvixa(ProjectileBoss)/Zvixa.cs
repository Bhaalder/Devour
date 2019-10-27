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

    public Transform TeleportAreaLeft { get; set; }
    public Transform TeleportAreaMiddle { get; set; }
    public Transform TeleportAreaRight { get; set; }

    public int FacingDirection { get; set; }

    public Player Player { get; set; }
    public float DistanceToPlayer { get; set; }

    [SerializeField] private BoxCollider2D highArea;
    [SerializeField] private BoxCollider2D lowArea;
    [SerializeField] private Transform teleportAreaLeft;
    [SerializeField] private Transform teleportAreaMiddle;
    [SerializeField] private Transform teleportAreaRight;

    private void Start() {
        Player = GameController.Instance.Player;           
    }

    protected override void Awake() {
        base.Awake();
        HighArea = highArea;
        LowArea = lowArea;
        TeleportAreaLeft = teleportAreaLeft;
        TeleportAreaMiddle = teleportAreaMiddle;
        TeleportAreaRight = teleportAreaRight;
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        base.FixedUpdate();
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
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

    public override void EnemyDeath() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        //Transition till DeathState
        Destroy(gameObject);//FÖR TILLFÄLLET
    }
}
