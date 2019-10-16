using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    IDLE, AIR, DASH, WALLSLIDE, WALLJUMP, WALK
}

public class Player : StateMachine {

    public PlayerState PlayerState { get; set; }

    public Rigidbody2D Rb2D { get; set; }

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

    [Tooltip("The area of the groundcheck, to see if the player is touching the ground")]
    [SerializeField] private float groundCheckDistance;
    [Tooltip("The length of the wallcheck, to see if the player is touching a wall")]
    [SerializeField] private float wallCheckDistance;
    private float wallCheckDistanceValue;
    
    [Tooltip("Is the player touching ground?")]
    [SerializeField] private bool isGrounded;
    [Tooltip("Is the player touching wall?")]
    [SerializeField] private bool isTouchingWall;
    [Tooltip("Is the player touching wallsliding?")]
    [SerializeField] private bool isWallSliding;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround;

    private void Start() {
        Rb2D = GetComponent<Rigidbody2D>();

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
        isGrounded = IsGrounded;//bara för att se i inspektorn atm, kan tas bort sen
        isTouchingWall = IsTouchingWall;//bara för att se i inspektorn atm, kan tas bort sen
        isWallSliding = IsWallSliding;//bara för att se i inspektorn atm, kan tas bort sen
        base.Update();
    }

    public void PlayerLog(string message) {
        Debug.Log("PLAYER: " + message);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistanceValue, wallCheck.position.y, wallCheck.position.z));
    }

}
