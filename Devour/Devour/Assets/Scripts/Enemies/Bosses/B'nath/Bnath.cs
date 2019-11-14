using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossBnathState
{
    NONE, INTRO, IDLE, BODYSLAM, CLIMBING, DASH_TELEGRAPH, DASHING, VOID_ASSAULT, DEATH
}
public class Bnath : Boss
{


    [SerializeField] private GameObject[] voidGroundLocation;
    [SerializeField] private GameObject bossFightBlock;
    [SerializeField] GameObject startPosition;
    [SerializeField] GameObject yPoint;

    public BossBnathState State { get; set; }
    public GameObject[] VoidGroundLocation { get; set; }
    public bool BossFightStart { get; set; } = false;
    public GameObject Blocker { get; set; }
    public GameObject StartPosition { get; set; }
    public GameObject YPoint { get; set; }

    private static bool isDead;

    protected override void Awake()
    {
        if (isDead)
        {
            Destroy(gameObject);
        }
        VoidGroundLocation = voidGroundLocation;
        base.Awake();
        BossFightStart = false;
        Blocker = bossFightBlock;
        StartPosition = startPosition;
        YPoint = yPoint;
        IsAlive = !isDead;

        PlayerDiedEvent.RegisterListener(Reset);

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

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public override void EnemyDeath()
    {
        //Transition till DeathState
        isDead = true;
        SpawnAbilityEssence();
        GiveLifeforce();
        Transition<BnathDeathState>();
    }
    private void Reset(PlayerDiedEvent playerDied)
    {
        Health = MaxHealth;
        Transition<BnathIntro>();
        bossFightBlock.SetActive(false);
        BossFightStart = false;
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
