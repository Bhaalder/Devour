//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy{

    public Animator Animator { get; set; }
    public GameObject AbilityEssence { get; set; }
    public string BossName { get; set; }
    public float MaxHealth { get; set; }
    public GameObject AudioVoiceGO { get; set; }

    [SerializeField] protected PlayerAbility bossGivesAbility;
    [SerializeField] protected GameObject abilityEssence;
    [SerializeField] protected string bossName;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected Vector2 offsetIntroZoom;
    [SerializeField] protected float introZoomInValue;
    [SerializeField] protected float introEndZoomOutValue;

    private void OnEnable() {
        if(GameController.Instance.KilledBosses != null) {
            if (GameController.Instance.KilledBosses.Contains(BossName)) {
                Destroy(gameObject);
            }
        }
        AudioVoiceGO = transform.Find("AudioVoice").gameObject;
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
                            attackEvent.player.DashesLeft = attackEvent.player.NumberOfDashes;
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

    public void BossIntroSequence() {
        string[] names = { "BossStart", "BossLoop" };
        AudioPlaySequence bossMusic = new AudioPlaySequence {
            name = names,
            soundType = SoundType.MUSIC
        };
        bossMusic.FireEvent();
        CameraChangeTargetEvent cameraTarget = new CameraChangeTargetEvent {
            newTarget = transform
        };
        cameraTarget.FireEvent();
        CameraZoomEvent cameraZoom = new CameraZoomEvent {
            zoomValue = introZoomInValue
        };
        cameraZoom.FireEvent();
    }

    public void BossIntroEnd() {
        CameraChangeTargetEvent cameraTarget = new CameraChangeTargetEvent {
            playerTarget = true
        };
        cameraTarget.FireEvent();
        CameraZoomEvent cameraZoom = new CameraZoomEvent {
            zoomValue = introEndZoomOutValue
        };
        cameraZoom.FireEvent();
    }

    public void FadeBossMusic() {
        AudioFadeSoundEvent fadeSoundEvent = new AudioFadeSoundEvent {
            isFadeOut = true,
            name = "BossLoop",
            fadeDuration = 2f,
            soundType = SoundType.MUSIC,
            soundVolumePercentage = 0,
            stopValue = 0.01f
        };
        fadeSoundEvent.FireEvent();
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

    public virtual void PlayVoice(string sound) { }

    protected override void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            playerTakeDamage.FireEvent();
        }
    }

    public void BossLog(string message) {
        Debug.Log(bossName + ": " + message);
    }
}
