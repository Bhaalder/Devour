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
    public GameObject SpikeWarningParticles { get => spikeWarningParticles; set => spikeWarningParticles = value; }
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
    [SerializeField] private GameObject spikeWarningParticles;
    [SerializeField] private GameObject tipAfterZvixa;
    private Vector3 startPosition;


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
        startPosition = transform.position;

        PlayerDiedEvent.RegisterListener(Reset);
        ZvixaSelfDamageEvent.RegisterListener(SelfDamage);
    }

    protected override void Update() {
        base.Update();
        Animator.SetInteger("State", (int)State);
        if (!PlayerIsInBossArea()) {
            if(BossDoor.activeSelf){
                ResetBoss();
            }
        }
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

    public override void PlayVoice(string sound) {
        int i = Random.Range(1, 2 + 1);
        int playChance = Random.Range(1, 100 + 1);
        switch (sound) {
            case "BallAttack":
                sound = "ZvixaBallAttackVoice" + i;
                break;
            case "Die":
                sound = "ZvixaDeath";
                playChance = 100;
                break;
            case "ExpelChant":
                sound = "ZvixaSonarExpelChant" + i;
                break;
            case "SpikeVoice":
                sound = "ZvixaSpikeVoice";
                break;
            default:
                break;
        }
        if (playChance >= 40) {
            AudioPlaySoundAtLocationEvent soundEvent = new AudioPlaySoundAtLocationEvent {
                name = sound,
                soundType = SoundType.SFX,
                isRandomPitch = true,
                minPitch = 0.95f,
                maxPitch = 1f,
                gameObject = AudioVoiceGO
            };
            soundEvent.FireEvent();
        }
    }

    private bool PlayerIsInBossArea() {
        if (IsDead) {
            return true;
        }
        if (Player.BoxCollider2D.bounds.Intersects(LowArea.bounds) || Player.BoxCollider2D.bounds.Intersects(HighArea.bounds)) {
            return true;
        }
        return false;
    }

    private void Reset(PlayerDiedEvent playerDied) {
        ResetBoss();
    }

    private void ResetBoss() {
        Health = MaxHealth;
        State = BossZvixaState.NONE;
        Transition<ZvixaBaseState>();
        transform.position = startPosition;
        BossDoor.SetActive(false);
        StopBossMusic();
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
        MainMenuEvent.UnRegisterListener(OnMainMenuSwitch);
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        PlayerDiedEvent.UnRegisterListener(Reset);
        ZvixaSelfDamageEvent.UnRegisterListener(SelfDamage);
        EnemyTouchKillzoneEvent.UnRegisterListener(EnemyTouchKillzone);
    }
}
