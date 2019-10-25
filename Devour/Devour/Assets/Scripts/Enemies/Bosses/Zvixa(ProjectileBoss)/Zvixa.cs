using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossZvixaState {
    NONE, INTRO, IDLE, PREPARE_BASICATTACK, BASICATTACK, PREPARE_SONAR_EXPEL, SONAR_EXPEL, SPIKE_ATTACK
}

public class Zvixa : Boss{
    public BossZvixaState State { get; set; }
    public Rigidbody2D Rb2d { get; set; }
    public BoxCollider2D HighArea { get; set; }
    public BoxCollider2D LowArea { get; set; }
    public Player Player { get; set; }
    public float DistanceToPlayer { get; set; }

    public Color colorTest; //För tillfället för test

    [SerializeField] private BoxCollider2D highArea;
    [SerializeField] private BoxCollider2D lowArea;

    private void Start() {
        Player = GameController.Instance.Player;       
        Rb2d = GetComponent<Rigidbody2D>();
    }

    protected override void Awake() {
        base.Awake();
        HighArea = highArea;
        LowArea = lowArea;
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        base.FixedUpdate();
    }

    public override void EnemyDeath() {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        //Bossen gör en animation som tar X lång tid
        //Bossen dör
        Destroy(gameObject);
    }
}
