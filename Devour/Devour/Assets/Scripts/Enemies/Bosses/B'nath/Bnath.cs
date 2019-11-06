using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bnath : Boss
{

    [SerializeField] private GameObject[] voidGroundLocation;
    [SerializeField] private GameObject bossFightBlock;
    
    public GameObject[] VoidGroundLocation { get; set; }
    public bool BossFightStart { get; set; } = false;

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

    }

    protected override void Update()
    {
        base.Update();
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
        }
        //rb.velocity = new Vector2(0, 0);
    }

    public override void EnemyDeath()
    {
        //Transition till DeathState
        isDead = true;
        Destroy(gameObject);//FÖR TILLFÄLLET
    }

    protected override void OnDestroy()
    {
        bossFightBlock.SetActive(false);
    }
}
