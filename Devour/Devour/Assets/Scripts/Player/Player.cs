using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine {

    [SerializeField] private float movementSpeed;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    private float variableJumpHeight = 0.4f;
    private float xScale;
    private int extraJumping;
    private float XInput;
    private float YInput;
    
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    private float wallCheckValue;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private bool isWallSliding;

    private bool isFacingRight;
    private Rigidbody2D rb2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        isGrounded = false;
        xScale = transform.localScale.x;
        wallCheckValue = wallCheckDistance;
    }
    
    protected override void FixedUpdate() {
        FacingDirection();
        
        MovePlayer();
    }

    protected override void Update() {
        CollisionCheck();
        GetInput();
        JumpCheck();        
    }

    private void GetInput() {
        XInput = Input.GetAxisRaw("Horizontal");
    }


    private void MovePlayer() {
        rb2D.velocity = new Vector2(XInput * movementSpeed, rb2D.velocity.y );
        if (isWallSliding) {
            if(rb2D.velocity.y < -wallSlideSpeed) {
                rb2D.velocity = new Vector2(rb2D.velocity.x, -wallSlideSpeed);
            }
        }
    }

    public void FacingDirection() {
        if (XInput < 0) {
            Flip(-xScale);
            wallCheckValue = -wallCheckDistance;
        } else if (XInput > 0) {
            Flip(xScale);
            wallCheckValue = wallCheckDistance;
        }
    }
    private void Flip(float direction) {
        Vector3 myScale = transform.localScale;
        myScale.x = direction;
        transform.localScale = myScale;
    }

    private void CollisionCheck() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckValue, whatIsGround);
        WallSlideCheck();
    }

    private void WallSlideCheck() {
        if(isTouchingWall && !isGrounded && rb2D.velocity.y < 0) {
            isWallSliding = true;
        } else {
            isWallSliding = false;
        }
    }

    private void JumpCheck() {
        
        if (isGrounded || isWallSliding) {
            extraJumping = extraJumps;
        }
        if (isWallSliding && Input.GetButtonDown("Jump")) {
            Jump(0);
            return;
        }
        if (isGrounded && Input.GetButtonDown("Jump")) {
            Jump(0);
            return;
        }
        if (!isGrounded && extraJumping > 0 && Input.GetButtonDown("Jump")) {
            extraJumping--;
            rb2D.velocity = new Vector2(0, 0);
            Jump(5);
        }
        if (Input.GetButtonUp("Jump") && rb2D.velocity.y > 0) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * variableJumpHeight);
        }
    }
    
    private void Jump(float extra) {
        rb2D.velocity = Vector2.up * (jumpForce + extra);
    }

    public void PlayerLog(string message) {
        Debug.Log("PLAYER: " + message);
    }

    //private void OnDrawGizmos() {
    //    Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
    //    Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckValue, wallCheck.position.y, wallCheck.position.z));
    //}

}
