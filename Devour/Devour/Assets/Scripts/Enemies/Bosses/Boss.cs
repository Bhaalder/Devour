//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy{

    public Animator Animator { get; set; }
    public GameObject AbilityEssence { get; set; }
    public string BossName { get; set; }
    public float MaxHealth { get; set; }
    [SerializeField] protected PlayerAbility bossGivesAbility;
    [SerializeField] protected GameObject abilityEssence;
    [SerializeField] protected string bossName;
    [SerializeField] protected float maxHealth;

    private void OnEnable() {
        if(GameController.Instance.KilledBosses != null) {
            if (GameController.Instance.KilledBosses.Contains(BossName)) {
                Destroy(gameObject);
            }
        }
    }

    protected override void Awake() {
        base.Awake();
        MaxHealth = maxHealth;
        Health = MaxHealth;
        AbilityEssence = abilityEssence;
        BossName = bossName;
        Animator = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void TakeDamage(PlayerAttackEvent attackEvent) {
        if (invulnerabilityTimer <= 0) {
            try {
                if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds)) {
                    ChangeEnemyHealth(-attackEvent.damage);
                    HurtSoundAndParticles();
                    if (attackEvent.isMeleeAttack) {
                        PlayerHealEvent phe = new PlayerHealEvent {
                            isLifeLeech = true
                        };
                        if (attackEvent.player.HasAbility(PlayerAbility.VOIDMEND)) {
                            PlayerVoidEvent voidEvent = new PlayerVoidEvent {
                                amount = attackEvent.player.MeleeVoidLeech
                            };
                            voidEvent.FireEvent();
                        }
                        phe.FireEvent();
                        if (!attackEvent.player.IsGrounded && attackEvent.player.IsAttackingDown && attackEvent.isMeleeAttack) {
                            attackEvent.player.ExtraJumpsLeft = attackEvent.player.ExtraJumps;
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, 0);
                            attackEvent.player.Rb2D.velocity = new Vector2(attackEvent.player.Rb2D.velocity.x, attackEvent.player.BounceForce);
                            return;
                        }
                    }
                }
            } catch (System.NullReferenceException) {
                Debug.LogWarning("A missing reference in PlayerAttackEvent, check Log!");
            }
        }
    }

    public override void ChangeEnemyHealth(float amount) {
        Health += amount;
        Debug.Log(bossName + ": " + Health + " health left");
        if (Health <= 0) {
            EnemyDeath();
        }
        invulnerabilityTimer = startInvulnerability;
    }

    public override void EnemyDeath() {
        //Basic ifall bossen inte har en egen deathstate
        Destroy(gameObject);
    }

    public void SpawnAbilityEssence() {
        GameObject essence;
        AbilityEssence abilityEssence;
        essence = Instantiate(AbilityEssence, transform.position, Quaternion.identity);
        abilityEssence = essence.GetComponent<AbilityEssence>();
        abilityEssence.Ability = bossGivesAbility;
    }

    protected override void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
        }
    }

    public void BossLog(string message) {
        Debug.Log(bossName + ": " + message);
    }
}
