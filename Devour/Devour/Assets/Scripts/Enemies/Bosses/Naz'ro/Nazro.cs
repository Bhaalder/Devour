//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossNazroState {
    NONE, INTRO, IDLE, VOID_BOMB, VOID_WALL, VOID_COMET, VOID_OBS, PHASE_CHANGE, WAIT, SECOND_PHASE_INTRO, DEATH
}

public class Nazro : Boss {
    public BossNazroState State { get; set; }

    public Transform BossArea { get; set; }

    public float Speed { get; set; }

    public BoxCollider2D LeftArea { get; set; }
    public BoxCollider2D RightArea { get; set; }
    public BoxCollider2D HighArea { get; set; }
    public BoxCollider2D LowCenterArea { get; set; }
    public BoxCollider2D StartFightArea { get; set; }
    public BoxCollider2D VoidObsSpawnArea { get; set; }
    public BoxCollider2D EndPlatformArea { get => endPlatformArea; set => endPlatformArea = value; }

    public GameObject VerticalVoidWall { get; set; }
    public GameObject HorizontalVoidWall { get; set; }
    public GameObject BossDoor { get; set; }
    public GameObject RightWall { get; set; }
    public GameObject SecondBossDoor { get => secondBossDoor; set => secondBossDoor = value; }
    
    public Transform[] VoidBombSpawnLocations { get; set; }
    public Transform[] VoidCometSpawnLocations { get; set; }
    public Transform[] MoveLocations { get; set; }
    public Transform SecondPhaseLocation { get; set; }

    public int MaximumNumberOfVoidObstacles { get; set; }
    public int CurrentLocation { get; set; }
    public int NewLocation { get; set; }

    public float DistanceToPlayer { get; set; }
    public bool IsSecondPhase { get; set; }

    public List<GameObject> NazroVoidObstacles { get; set; }

    [SerializeField] private Transform bossArea;

    [SerializeField] private float speed;
    [SerializeField] private int maximumNumberOfVoidObstacles;

    [SerializeField] private BoxCollider2D leftArea;
    [SerializeField] private BoxCollider2D rightArea;
    [SerializeField] private BoxCollider2D highArea;
    [SerializeField] private BoxCollider2D lowCenterArea;
    [SerializeField] private BoxCollider2D startFightArea;
    [SerializeField] private BoxCollider2D voidObsSpawnArea;
    [SerializeField] private BoxCollider2D endPlatformArea;
    [SerializeField] private GameObject verticalVoidWall;
    [SerializeField] private GameObject horizontalVoidWall;
    [SerializeField] private GameObject bossDoor;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject secondBossDoor;
    [SerializeField] private Transform[] voidBombSpawnLocations;
    [SerializeField] private Transform[] voidCombetSpawnLocations;
    [SerializeField] private Transform[] moveLocations;
    [SerializeField] private Transform secondPhaseLocation; 

    public static bool IsDead { get; set; }

    protected override void Awake() {
        if (IsDead) {
            Destroy(gameObject);
        }
        base.Awake();
        BossArea = bossArea;
        Speed = speed;
        LeftArea = leftArea;
        RightArea = rightArea;
        HighArea = highArea;
        LowCenterArea = lowCenterArea;
        StartFightArea = startFightArea;
        VerticalVoidWall = verticalVoidWall;
        HorizontalVoidWall = horizontalVoidWall;
        VoidObsSpawnArea = voidObsSpawnArea;
        BossDoor = bossDoor;
        RightWall = rightWall;
        VoidBombSpawnLocations = voidBombSpawnLocations;
        VoidCometSpawnLocations = voidCombetSpawnLocations;
        MoveLocations = moveLocations;
        MaximumNumberOfVoidObstacles = maximumNumberOfVoidObstacles;
        SecondPhaseLocation = secondPhaseLocation;
        NazroVoidObstacles = new List<GameObject>();

        PlayerDiedEvent.RegisterListener(Reset);
    }

    protected override void Update() {
        base.Update();
        Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate() {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        base.FixedUpdate();
    }

    protected override void EnemyTouchKillzone(EnemyTouchKillzoneEvent killzoneEvent) {
        //Can't touch killzone
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        //No damage on contact
    }

    private void Reset(PlayerDiedEvent playerDied) {
        Health = MaxHealth;
        State = BossNazroState.NONE;
        Transition<NazroBaseState>();
        StopBossMusic();
    }

    public override void EnemyDeath() {
        if (!IsDead) {
            IsDead = true;
        }
        if (State != BossNazroState.DEATH) {
            Transition<NazroDeathState>();
        }
    }

    protected override void OnDestroy() {
        MainMenuEvent.UnRegisterListener(OnMainMenuSwitch);
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        PlayerDiedEvent.UnRegisterListener(Reset);
    }
}
