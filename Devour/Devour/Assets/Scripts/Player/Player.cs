//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    IDLE, AIR, DASH, WALLSLIDE, WALLJUMP, WALK, HURT
}

public enum PlayerAbility {
    DOUBLEJUMP, WALLSLIDE, DASH
}

public class Player : StateMachine {

    public PlayerState PlayerState { get; set; }
    public List<PlayerAbility> PlayerAbilities { get; set; }

    public Rigidbody2D Rb2D { get; set; }

    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float CombatDamage { get; set; }
    public float ProjectileDamage { get; set; }
    public float KnockbackForce { get; set; }
    public float CombatCooldown { get; set; }
    public float CombatLifeLeech { get; set; }
    public float ProjectileCooldown { get; set; }
    public float ProjectileHealthcost { get; set; }

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
    [Tooltip("Player damage (close combat)")]
    [SerializeField] private float combatDamage;
    [Tooltip("How much health the close combat attack leeches")]
    [SerializeField] private float combatLifeLeech;
    [Tooltip("Cooldown between attacks (close combat)")]
    [SerializeField] private float combatCooldown;
    [Tooltip("Player damage (projectile)")]
    [SerializeField] private float projectileDamage;
    [Tooltip("How much health the projectile-attack drains")]
    [SerializeField] private float projectileHealthcost;
    [Tooltip("Cooldown between attacks (projectile)")]
    [SerializeField] private float projectileCooldown;
    [Tooltip("Kockbackvalue applied to enemies")]
    [SerializeField] private float knockbackForce;
    [Tooltip("How long the player is invulnerable to damage after taking damage")]
    [SerializeField] private float invulnerableStateTime;
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
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Testing")]//TESTING
    [Tooltip("For testing if the player has certain abilities")]//
    [SerializeField] private PlayerAbility[] playerAbilities;//

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

        Rb2D = GetComponent<Rigidbody2D>();

        MaxHealth = maxHealth;
        Health = maxHealth;
        CombatDamage = combatDamage;
        CombatCooldown = combatCooldown;
        CombatLifeLeech = combatLifeLeech;
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
        if (Input.GetKeyDown(KeyCode.E)) {//TESTING
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {//
                damage = 5
            };
            ptde.FireEvent();//
        }//
        health = Health;//
        InvulnerableTimeCheck();
        base.Update();
    }

    private void OnTouchKillzone(PlayerTouchKillzoneEvent killzone) {
        ChangeHealth(-killzone.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }
        //respawn?
    }

    private void OnTakeDamage(PlayerTakeDamageEvent eventDamage) {//EJ KLART
        if (IsInvulnerable) {
            return;
        }
        ChangeHealth(-eventDamage.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }
        Transition<PlayerHurtState>();
    }

    private void OnHeal(PlayerHealEvent eventHeal) {
        if (eventHeal.isLifeLeech) {
            eventHeal.amount = CombatLifeLeech;
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
            Debug.Log(untilInvulnerableEnds);
            untilInvulnerableEnds -= Time.deltaTime;
            return;
        }
        untilInvulnerableEnds = invulnerableStateTime;
    }

    private void Respawn() {
        Transition<PlayerHurtState>();
        untilInvulnerableEnds = invulnerableStateTime + 1f;       
    }

    private void Die() { //EJ KLART
        //respawnEvent?
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
