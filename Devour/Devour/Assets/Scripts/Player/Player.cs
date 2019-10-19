//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    IDLE, AIR, DASH, WALLSLIDE, WALLJUMP, WALK, HURT, ATTACK, PROJECTILEATTACK
}

public enum PlayerAbility {
    DOUBLEJUMP, WALLSLIDE, DASH, PROJECTILE
}

public class Player : StateMachine {

    public PlayerState PlayerState { get; set; }
    public List<PlayerAbility> PlayerAbilities { get; set; }

    public Rigidbody2D Rb2D { get; set; }

    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float DamageReduction { get; set; }
    public float MeleeDamage { get; set; }
    public float ProjectileDamage { get; set; }
    public float KnockbackForce { get; set; }
    public float MeleeCooldown { get; set; }
    public float UntilNextMeleeAttack { get; set; }
    public float MeleeLifeLeech { get; set; }
    public float ProjectileCooldown { get; set; }
    public float UntilNextProjectileAttack { get; set; }
    public float ProjectileHealthcost { get; set; }

    public BoxCollider2D PlayerHorizontalMeleeCollider { get; set; }
    public BoxCollider2D PlayerDownMeleeCollider { get; set; }
    public BoxCollider2D PlayerUpMeleeCollider { get; set; }
    public bool IsAttackingDown { get; set; }
    public bool IsAttackingUp { get; set; }
    public bool IsInvulnerable { get; set; }

    public float MovementSpeed { get; set; }
    public float JumpForce { get; set; }
    public int ExtraJumps { get; set; }
    public int ExtraJumpsLeft { get; set; }
    public float VariableJumpHeight { get; set; }
    public float PermanentVariableJumpHeight { get; set; }
    public float DashCooldown { get; set; }
    public float UntilNextDash { get; set; }

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
    public LayerMask WhatIsGround { get; set; }

    [Header("Health & Combat")]
    [Tooltip("Player maxHealth")]
    [SerializeField] private float maxHealth;
    [Tooltip("Player current health")]
    [SerializeField] private float health;
    [Tooltip("How much less damage the player takes from enemy attacks")]
    [SerializeField] private float damageReduction;
    [Tooltip("Player damage (close combat)")]
    [SerializeField] private float meleeDamage;
    [Tooltip("How much health the close combat attack leeches")]
    [SerializeField] private float meleeLifeLeech;
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
    [Tooltip("How long the player is invulnerable to damage after taking damage")]
    [SerializeField] private float invulnerableStateTime;
    [Tooltip("Knockbackvalue applied to player when hurt by enemies")]
    [SerializeField] private Vector2 playerHurtKnockbackForce;
    private float untilInvulnerableEnds;

    [Header("Movement")]
    [Tooltip("How fast the player is moving")]
    [SerializeField] private float movementSpeed;   
    [Tooltip("How high the player can jump")]
    [SerializeField] private float jumpForce;
    [Tooltip("How many extra jumps the player has (when not on ground)")]
    [SerializeField] private int extraJumps;
    [Tooltip("How much the jump gets 'cut' if the player releases the jumpbutton")]
    [SerializeField] private float variableJumpHeight;
    [Tooltip("The cooldown between dashes")]
    [SerializeField] private float dashCooldown;

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
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Testing")]//TESTING
    [Tooltip("For testing if the player has certain abilities")]//
    [SerializeField] private PlayerAbility[] playerAbilities;//
    public Vector2 PlayerVelocity;

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
        PlayerTakeDamageEvent.RegisterListener(OnTakeDamage);
        PlayerHealEvent.RegisterListener(OnHeal);
        PlayerTouchKillzoneEvent.RegisterListener(OnTouchKillzone);
        base.Awake();
    }

    private void Start() {
        PlayerAbilities = new List<PlayerAbility>();
        foreach(PlayerAbility ability in playerAbilities) {
            PlayerAbilities.Add(ability);
        }
        FacingDirection = 1;

        Rb2D = GetComponent<Rigidbody2D>();
        PlayerHorizontalMeleeCollider = horizontalAttack.GetComponent<BoxCollider2D>();
        PlayerUpMeleeCollider = upAttack.GetComponent<BoxCollider2D>();
        PlayerDownMeleeCollider = downAttack.GetComponent<BoxCollider2D>();

        MaxHealth = maxHealth;
        Health = maxHealth;
        DamageReduction = damageReduction;
        MeleeDamage = meleeDamage;
        MeleeCooldown = meleeCooldown;
        MeleeLifeLeech = meleeLifeLeech;
        ProjectileDamage = projectileDamage;       
        ProjectileCooldown = projectileCooldown;
        ProjectileHealthcost = projectileHealthcost;
        KnockbackForce = knockbackForce;
        

        MovementSpeed = movementSpeed;
        JumpForce = jumpForce;
        ExtraJumps = extraJumps;
        ExtraJumpsLeft = extraJumps;
        VariableJumpHeight = variableJumpHeight;
        PermanentVariableJumpHeight = variableJumpHeight;
        DashCooldown = dashCooldown;
        XScale = transform.localScale.x;
        GroundCheckDistance = groundCheckDistance;
        WallCheckDistance = wallCheckDistance;
        wallCheckDistanceValue = wallCheckDistance;

        GroundCheck = groundCheck;
        WallCheck = wallCheck;
        WhatIsGround = whatIsGround;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void Update() {
        PlayerVelocity = Rb2D.velocity;//TEST
        health = Health;//TEST
        InvulnerableTimeCheck();
        base.Update();
    }

    private void OnTouchKillzone(PlayerTouchKillzoneEvent killzone) {
        ChangeHealth(-killzone.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }
        Respawn();
    }

    private void OnTakeDamage(PlayerTakeDamageEvent eventDamage) {//EJ KLART
        if (IsInvulnerable) {
            return;
        }
        eventDamage.damage -= DamageReduction;
        if (eventDamage.damage <= 0) {
            eventDamage.damage = 0;
        }
        ChangeHealth(-eventDamage.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }
        KnockBack(eventDamage.enemyPosition, 2);//knockback
        Transition<PlayerHurtState>();
    }

    private void KnockBack(Vector3 enemyPosition, float amount) {
        int direction = 1;
        if (enemyPosition.x > transform.position.x) {
            direction = -direction;
        }
        Rb2D.velocity = new Vector2(playerHurtKnockbackForce.x*direction, playerHurtKnockbackForce.y);
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
            return true;
        }
        return false;
    }

    private void InvulnerableTimeCheck() {
        if(untilInvulnerableEnds <= 0) {
            IsInvulnerable = false;
        }
        if (IsInvulnerable) {
            untilInvulnerableEnds -= Time.deltaTime;
            return;
        }
        untilInvulnerableEnds = invulnerableStateTime;
    }

    private void Respawn() {
        Transition<PlayerHurtState>();
        untilInvulnerableEnds = invulnerableStateTime + 1f; //längre invulnerable när man fallit ner i killzone?
        try {
            transform.position = GameController.Instance.SceneCheckpoint.position;
        } catch(System.NullReferenceException) {
            Debug.LogError("Ingen 'SceneCheckpoint' deklarerad i GameController för att kunna respawna!");
        }
        
    }

    private void Die() { //EJ KLART, just nu gör vi bara en respawn och får fullt HP
        Health = MaxHealth;
        Respawn();//FÖR TILLFÄLLET
    }

    public void PlayerLog(string message) {
        Debug.Log("PLAYER: " + message);
    }

    public bool HasAbility(PlayerAbility playerAbility) {
        foreach(PlayerAbility ability in PlayerAbilities) {
            if(ability == playerAbility) {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistanceValue, wallCheck.position.y, wallCheck.position.z));
    }

}
