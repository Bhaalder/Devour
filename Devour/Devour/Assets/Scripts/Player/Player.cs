//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    NONE, IDLE, WALK, AIR, DASH, WALLSLIDE, WALLJUMP, HURT, ATTACK, PROJECTILEATTACK, JUMP, DEATH
}

public enum PlayerAbility {
    DOUBLEJUMP = 1, WALLSLIDE, DASH, PROJECTILE, VOIDMEND
}

public class Player : StateMachine {

    public PlayerState PlayerState { get; set; }
    public List<PlayerAbility> PlayerAbilities { get; set; }

    public Rigidbody2D Rb2D { get; set; }

    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float MaxPlayerVoid { get; set; }
    public float PlayerVoid { get; set; }
    public float DamageReduction { get; set; }
    public float MeleeDamage { get; set; }
    public float ProjectileDamage { get; set; }
    public float KnockbackForce { get; set; }
    public float BounceForce { get; set; }
    public float MeleeCooldown { get; set; }
    public float UntilNextMeleeAttack { get; set; }
    public float MeleeLifeLeech { get; set; }
    public float MeleeVoidLeech { get; set; }
    public float ProjectileCooldown { get; set; }
    public float UntilNextProjectileAttack { get; set; }
    public float ProjectileHealthcost { get; set; }

    public BoxCollider2D BoxCollider2D { get; set; }
    public BoxCollider2D HorizontalMeleeCollider { get; set; }
    public BoxCollider2D DownMeleeCollider { get; set; }
    public BoxCollider2D UpMeleeCollider { get; set; }
    public bool IsAttackingDown { get; set; }
    public bool IsAttackingUp { get; set; }
    public bool IsInvulnerable { get; set; }
    public float UntilInvulnerableEnds { get; set; }

    public float MovementSpeed { get; set; }
    public float JumpForce { get; set; }
    public int ExtraJumps { get; set; }
    public int ExtraJumpsLeft { get; set; }
    public float VariableJumpHeight { get; set; }
    public float PermanentVariableJumpHeight { get; set; }
    public int NumberOfDashes { get; set; }
    public int DashesLeft { get; set; }
    public float DashCooldown { get; set; }
    public float UntilNextDash { get; set; }
    public float FallSpeed { get; set; }

    public float XInput { get; set; }
    public float YInput { get; set; }

    public float XScale { get; set; }

    public float GroundCheckDistance { get; set; }
    public float WallCheckDistance { get; set; }
    public int FacingDirection { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsTouchingWall { get; set; }
    public bool IsWallSliding { get; set; }

    public Transform GroundCheck { get; set; }
    public Transform WallCheck { get; set; }
    public Transform Aim { get; set; }
    public LayerMask WhatIsGround { get; set; }
    public Animator Animator { get; set; }

    [Header("Health & Combat")]
    [Tooltip("Player maxHealth")]
    [SerializeField] private float maxHealth;
    [Tooltip("Player current health")]
    [SerializeField] private float health;
    [Tooltip("Player maxHealth")]
    [SerializeField] private float maxPlayerVoid;
    [Tooltip("Player current health")]
    [SerializeField] private float playerVoid;
    [Tooltip("How much less damage the player takes from enemy attacks")]
    [SerializeField] private float damageReduction;
    [Tooltip("Player damage (close combat)")]
    [SerializeField] private float meleeDamage;
    [Tooltip("How much health the close combat attack leeches in health")]
    [SerializeField] private float meleeLifeLeech;
    [Tooltip("How much health the close combat attack leeches void")]
    [SerializeField] private float meleeVoidLeech;
    [Tooltip("Cooldown between attacks (close combat)")]
    [SerializeField] private float meleeCooldown;
    [Tooltip("Player damage (projectile)")]
    [SerializeField] private float projectileDamage;
    [Tooltip("How much health the projectile-attack drains")]
    [SerializeField] private float projectileHealthcost;
    [Tooltip("Cooldown between attacks (projectile)")]
    [SerializeField] private float projectileCooldown;
    [Tooltip("Knockbackvalue applied to enemies from the player")]
    [SerializeField] private float knockbackForce;
    [Tooltip("Bouncevalue applied when player is attacking down on enemies")]
    [SerializeField] private float bounceForce;
    [Tooltip("How long the player is invulnerable to damage after taking damage")]
    [SerializeField] private float invulnerableStateTime;
    [Tooltip("Knockbackvalue applied to player when hurt by enemies")]
    [SerializeField] private Vector2 hurtKnockbackForce;

    [Header("Camera")]
    [Tooltip("How long the camera will shake when taking damage")]
    [SerializeField] private float cameraShakeDuration;
    [Tooltip("How much the camera will shake when taking damagee")]
    [SerializeField] private float cameraShakeValue;
    
    [Header("Movement")]
    [Tooltip("How fast the player is moving")]
    [SerializeField] private float movementSpeed;   
    [Tooltip("How high the player can jump")]
    [SerializeField] private float jumpForce;
    [Tooltip("How many extra jumps the player has (when not on ground)")]
    [SerializeField] private int extraJumps;
    [Tooltip("How much the jump gets 'cut' if the player releases the jumpbutton")]
    [SerializeField] private float variableJumpHeight;
    [Tooltip("How many dashes the player can do between being on ground/wallslide or bounce on enemies")]
    [SerializeField] private int numberOfDashes;
    [Tooltip("The cooldown between dashes")]
    [SerializeField] private float dashCooldown;
    [Tooltip("The maxvalue the player can fall")]
    [SerializeField] private float fallSpeed;

    [Header("MovementCheckVariables")]
    [Tooltip("The area of the groundcheck, to see if the player is touching the ground")]
    [SerializeField] private float groundCheckDistance;
    [Tooltip("The length of the wallcheck, to see if the player is touching a wall")]
    [SerializeField] private float wallCheckDistance;
    private float wallCheckDistanceValue;

    [Header("Transforms & Layermask")]
    [SerializeField] private Transform horizontalAttack;
    [SerializeField] private Transform upAttack;
    [SerializeField] private Transform downAttack;
    [SerializeField] private Transform aim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Testing")]//TESTING
    [Tooltip("For testing if the player has certain abilities")]//
    [SerializeField] private PlayerAbility[] playerAbilities;//
    public Vector2 PlayerVelocity;//

    private static bool exists;

    protected override void Awake() {
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
            return;
        }

        GameController.Instance.Player = this;

        PlayerAbilities = new List<PlayerAbility>();

        FacingDirection = 1;

        Rb2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        HorizontalMeleeCollider = horizontalAttack.GetComponent<BoxCollider2D>();
        UpMeleeCollider = upAttack.GetComponent<BoxCollider2D>();
        DownMeleeCollider = downAttack.GetComponent<BoxCollider2D>();

        MaxHealth = maxHealth;
        Health = maxHealth;
        MaxPlayerVoid = maxPlayerVoid;
        PlayerVoid = playerVoid;
        DamageReduction = damageReduction;
        MeleeDamage = meleeDamage;
        MeleeCooldown = meleeCooldown;
        MeleeLifeLeech = meleeLifeLeech;
        MeleeVoidLeech = meleeVoidLeech;
        ProjectileDamage = projectileDamage;
        ProjectileCooldown = projectileCooldown;
        ProjectileHealthcost = projectileHealthcost;
        KnockbackForce = knockbackForce;
        BounceForce = bounceForce;

        MovementSpeed = movementSpeed;
        JumpForce = jumpForce;
        ExtraJumps = extraJumps;
        ExtraJumpsLeft = extraJumps;
        VariableJumpHeight = variableJumpHeight;
        PermanentVariableJumpHeight = variableJumpHeight;
        NumberOfDashes = numberOfDashes;
        DashCooldown = dashCooldown;
        FallSpeed = fallSpeed;
        XScale = transform.localScale.x;
        GroundCheckDistance = groundCheckDistance;
        WallCheckDistance = wallCheckDistance;
        wallCheckDistanceValue = wallCheckDistance;

        GroundCheck = groundCheck;
        WallCheck = wallCheck;
        Aim = aim;
        WhatIsGround = whatIsGround;
        Animator = GetComponent<Animator>();

        PlayerTakeDamageEvent.RegisterListener(OnTakeDamage);
        PlayerHealEvent.RegisterListener(OnHeal);
        PlayerVoidEvent.RegisterListener(OnVoidEvent);
        PlayerTouchKillzoneEvent.RegisterListener(OnTouchKillzone);
        PlayerGetAbilityEvent.RegisterListener(OnGetAbility);
        //foreach (PlayerAbility ability in playerAbilities) {//TEST
        //    PlayerAbilities.Add(ability);
        //}//TEST
        base.Awake();
    }

    private void Start() {

    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void Update() {
        PlayerVelocity = Rb2D.velocity;//TEST
        health = Health;//TEST
        InvulnerableTimeCheck();
        Animator.SetInteger("State", (int)PlayerState);
        base.Update();
    }

    private void OnTouchKillzone(PlayerTouchKillzoneEvent killzone) {
        CameraShakeEvent cse = new CameraShakeEvent {
            startDuration = cameraShakeDuration,
            startValue = cameraShakeValue
        };
        cse.FireEvent();
        ChangeHealth(-killzone.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }
        Respawn();
    }

    private void OnTakeDamage(PlayerTakeDamageEvent eventDamage) {//EJ KLART
        if (!eventDamage.isSelfInflicted) {
            if (IsInvulnerable) {
                return;
            }
            eventDamage.damage -= DamageReduction;
            if (eventDamage.damage <= 0) {
                eventDamage.damage = 0;
            }
            CameraShakeEvent cse = new CameraShakeEvent {
                startDuration = cameraShakeDuration,
                startValue = cameraShakeValue
            };
            cse.FireEvent();
            KnockBack(eventDamage.enemyPosition, 2);//knockback
            Transition<PlayerHurtState>();
        }       
        ChangeHealth(-eventDamage.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }  
    }

    private void OnVoidEvent(PlayerVoidEvent voidEvent) {
        if(PlayerVoid >= MaxPlayerVoid && voidEvent.amount > 0) {
            voidEvent.amount = 0;
            PlayerVoid = MaxPlayerVoid;
        }
        PlayerVoid += voidEvent.amount;
    }

    private void KnockBack(Vector3 enemyPosition, float amount) {
        int direction = 1;
        if (enemyPosition.x > transform.position.x) {
            direction = -direction;
        }
        Rb2D.velocity = new Vector2(hurtKnockbackForce.x*direction, hurtKnockbackForce.y);
    }

    private void OnHeal(PlayerHealEvent eventHeal) {
        if (eventHeal.isLifeLeech) {
            eventHeal.amount = MeleeLifeLeech;
        }
        ChangeHealth(eventHeal.amount);
        if (Health > MaxHealth) {
            Health = MaxHealth;
        }
    }

    public void ChangeHealth(float amount) {
        Health += amount;
    }

    public bool DamageWasDeadly() {
        if (Health <= 0) {
            Health = 0;
            return true;
        }
        return false;
    }

    private void InvulnerableTimeCheck() {
        if(UntilInvulnerableEnds <= 0) {
            IsInvulnerable = false;
        }
        if (IsInvulnerable) {
            UntilInvulnerableEnds -= Time.deltaTime;
            return;
        }
        UntilInvulnerableEnds = invulnerableStateTime;
    }

    private void Respawn() {
        Transition<PlayerHurtState>();
        Rb2D.velocity = new Vector2(0, 0);
        UntilInvulnerableEnds = invulnerableStateTime + 1f; //längre invulnerable när man fallit ner i killzone?
        try {
            transform.position = GameController.Instance.SceneCheckpoint;
        } catch(UnassignedReferenceException) {
            Debug.LogError("No 'SceneCheckpoint' assigned in GameController to be able to respawn!");
        }
    }

    private void Die() { //EJ KLART, just nu gör vi bara en respawn och får fullt HP
                         //MaxHP halveras, man hamnar på senaste "RestingPlace", ens "essence" hamnar där man dog      
        Transition<PlayerDeathState>();
        //Respawn();//FÖR TILLFÄLLET
    }

    public void PlaySound(string sound) {
        List<string> soundList = new List<string>();
        switch (sound) {
            case "Walk":
                soundList.Add("Step1");
                break;
            default:
                break;
        }
        AudioPlayRandomSoundEvent soundEvent = new AudioPlayRandomSoundEvent {
            name = soundList.ToArray(),
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.9f,
            maxPitch = 1f
        };
        soundEvent.FireEvent();
    }

    private void OnGetAbility(PlayerGetAbilityEvent abilityEvent) {
        if (!HasAbility(abilityEvent.playerAbility)) {
            PlayerAbilities.Add(abilityEvent.playerAbility);
        } else {
            PlayerLog("Already has " + abilityEvent.playerAbility.ToString());
        }
        
    }

    public bool HasAbility(PlayerAbility playerAbility) {
        foreach(PlayerAbility ability in PlayerAbilities) {
            if(ability == playerAbility) {
                return true;
            }
        }
        return false;
    }

    private void OnDestroy() {
        PlayerTakeDamageEvent.UnRegisterListener(OnTakeDamage);
        PlayerHealEvent.UnRegisterListener(OnHeal);
        PlayerVoidEvent.UnRegisterListener(OnVoidEvent);
        PlayerTouchKillzoneEvent.UnRegisterListener(OnTouchKillzone);
        PlayerGetAbilityEvent.UnRegisterListener(OnGetAbility);
    }

    public void PlayerLog(string message) {
        Debug.Log("PLAYER: " + message);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistanceValue, wallCheck.position.y, wallCheck.position.z));
    }

}
