using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss2State
{
    NONE, INTRO, IDLE, SONIC_THRUST_MOVEMENT, SONIC_THRUST_TELEGRAPH, SONIC_THRUST_ATTACK, SONIC_THRUST_EXIT, SONIC_SNIPE_MOVEMENT, SONIC_SNIPE_TELEGRAPH, SONIC_SNIPE_ATTACK, SONIC_SNIPE_EXIT, SONIC_DASH_MOVEMENT, SONIC_DASH_TELEGRAPH, SONIC_DASH_ATTACK, SONIC_DASH_EXIT, DEATH
}
public class Boss2 : Boss
{
    [SerializeField] private GameObject dashPattern1;
    [SerializeField] private GameObject dashPattern2;
    [SerializeField] private GameObject dashPattern3;
    [SerializeField] private GameObject sonicSnipeBeam;
    [SerializeField] private GameObject bossFightBlock;
    [SerializeField] private float sonicSnipeBeamDamage = 25f;


    public Boss2State State { get; set; }
    public GameObject DashPattern1 { get; set; }
    public GameObject DashPattern2 { get; set; }
    public GameObject DashPattern3 { get; set; }
    public GameObject ChosenPattern { get; set; }
    public GameObject SonicSnipeBeam { get; set; }
    public SpriteRenderer SnipeBeamSprite { get; set; }
    public float SonicSnipeBeamDamage { get; set; }
    public bool IntroStarted { get; set; }
    public Vector2 dashStartDirection { get; set; }

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
        DashPattern1 = dashPattern1;
        DashPattern2 = dashPattern2;
        DashPattern3 = dashPattern3;
        SonicSnipeBeam = sonicSnipeBeam;
        SonicSnipeBeamDamage = sonicSnipeBeamDamage;
        SnipeBeamSprite = SonicSnipeBeam.GetComponentInChildren<SpriteRenderer>();
        IntroStarted = false;
        PlayerDiedEvent.RegisterListener(Reset);
        Transition<Boss2Intro>();
        bossFightBlock.SetActive(true);
        IsAlive = !isDead;
    }

    protected override void Update()
    {
        base.Update();
        //Animator.SetInteger("State", (int)State);

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();

        }
    }

    //protected override void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Collided with Player");
    //        PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
    //        {
    //            damage = damageToPlayerOnContact,
    //            enemyPosition = rb.position
    //        };
    //        ptde.FireEvent();

    //    }
    //}

    public override void EnemyDeath()
    {
        //Transition till DeathState
        
        SpawnAbilityEssence();
        GiveLifeforce();
        
        BossDiedEvent boss2Died = new BossDiedEvent
        {
            boss = this
        };
        boss2Died.FireEvent();
        if (!isDead)
        {
            isDead = true;
            State = Boss2State.DEATH;
            Transition<Boss2DeathState>();            
        }
        
    }
    private void Reset(PlayerDiedEvent playerDied)
    {
        Health = MaxHealth;
        SnipeBeamSprite.enabled = false;
        Transition<Boss2Intro>();
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
