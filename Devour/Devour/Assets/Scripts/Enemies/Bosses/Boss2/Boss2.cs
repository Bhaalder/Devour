using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss2State
{
    NONE, INTRO, IDLE, SONIC_THRUST_MOVEMENT, SONIC_THRUST_TELEGRAPH, SONIC_THRUST_ATTACK, SONIC_THRUST_EXIT, SONIC_SNIPE_MOVEMENT, SONIC_SNIPE_TELEGRAPH, SONIC_SNIPE_ATTACK, SONIC_SNIPE_EXIT, SONIC_DASH_MOVEMENT, SONIC_DASH_TELEGRAPH, SONIC_DASH_ATTACK, SONIC_DASH_EXIT, DEATH
}
public enum Boss2Attacks
{
    NONE, THRUST, SNIPE, DASH
}
public class Boss2 : Boss
{

    [SerializeField] private GameObject[] dashPatterns;
    [SerializeField] private GameObject sonicSnipeBeam;
    [SerializeField] private GameObject bossFightBlock;
    [SerializeField] private float sonicSnipeBeamDamage = 25f;

    [SerializeField] private GameObject hitBoxVertical;
    [SerializeField] private GameObject hitBoxHorizontal;


    public Boss2State State { get; set; }
    public Boss2Attacks LastAttack { get; set; }
    public GameObject DashPattern1 { get; set; }
    public GameObject DashPattern2 { get; set; }
    public GameObject DashPattern3 { get; set; }
    public GameObject ChosenPattern { get; set; }
    public GameObject[] DashPatterns { get; set; }
    public GameObject SonicSnipeBeam { get; set; }
    public SpriteRenderer SnipeBeamSprite { get; set; }
    public float SonicSnipeBeamDamage { get; set; }
    public bool IntroStarted { get; set; }
    public bool IsCausingDamage { get; set; }
    public Vector2 dashStartDirection { get; set; }

    public GameObject HitBoxVertical { get => hitBoxVertical; set => hitBoxVertical = value; }
    public GameObject HitBoxHorizontal { get => hitBoxHorizontal; set => hitBoxHorizontal = value; }
    public List<GameObject> SonicDashParticles { get; set; }
    public bool Transitioned { get; set; }

    private static bool isDead;

    protected override void Awake()
    {
        base.Awake();
        if (isDead)
        {
            Destroy(bossFightBlock);
            Destroy(gameObject);
        }
        Animator = GetComponent<Animator>();
        DashPatterns = dashPatterns;
        SonicSnipeBeam = sonicSnipeBeam;
        sonicSnipeBeam.GetComponentInChildren<BoxCollider2D>().enabled = false;
        SonicSnipeBeamDamage = sonicSnipeBeamDamage;
        SnipeBeamSprite = SonicSnipeBeam.GetComponentInChildren<SpriteRenderer>();
        SnipeBeamSprite.enabled = false;
        IntroStarted = false;
        IsCausingDamage = true;
        PlayerDiedEvent.RegisterListener(Reset);
        Transition<Boss2BaseState>();
        bossFightBlock.SetActive(true);
        IsAlive = !isDead;
        hitBoxHorizontal.SetActive(false);
        SonicDashParticles = new List<GameObject>();
        Transitioned = false;
    }

    protected override void Update()
    {
        base.Update();
        Animator.SetInteger("State", (int)State);

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && IsCausingDamage && IsAlive) {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();

        }
    }

    public override void EnemyDeath()
    {
        //Transition till DeathState
        
        GiveCollectibles();
        
        BossDiedEvent boss2Died = new BossDiedEvent
        {
            boss = this
        };
        boss2Died.FireEvent();
        if (!isDead)
        {
            isDead = true;
            IsAlive = false;
            State = Boss2State.DEATH;
            Transition<Boss2DeathState>();            
        }
        
    }
    private void Reset(PlayerDiedEvent playerDied)
    {
        Health = MaxHealth;
        SnipeBeamSprite.enabled = false;
        Transitioned = false;
        IntroStarted = false;
        Transition<Boss2BaseState>();
        FadeBossMusic_PlayerDied();
    }

    protected override void OnDestroy()
    {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
        EnemyTouchKillzoneEvent.UnRegisterListener(EnemyTouchKillzone);
        PlayerDiedEvent.UnRegisterListener(Reset);
        if (bossFightBlock != null)
        {
            Destroy(bossFightBlock);
        }
    }
}
