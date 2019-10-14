using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine {

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    private float xScale;
    private int extraJumping;
    private float XInput;
    private float YInput;
    [SerializeField] private float fakeExtraGravity;

    [SerializeField] private bool isGrounded;

    private bool isFacingRight;
    private Rigidbody2D rb2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        isGrounded = false;
        xScale = transform.localScale.x;
    }
    
    protected override void FixedUpdate() {
        FakeGravity();
        rb2D.velocity = new Vector2(XInput * movementSpeed, rb2D.velocity.y + fakeExtraGravity);//
    }

    protected override void Update() {
        GetInput();
        FacingDirection();
        JumpCheck();
    }

    private void GetInput() {
        XInput = Input.GetAxisRaw("Horizontal");
    }


    public void FacingDirection() {
        if (XInput < 0) {
            Flip(-xScale);
        } else if (XInput > 0) {
            Flip(xScale);
        }
    }
    private void Flip(float direction) {
        Vector3 myScale = transform.localScale;
        myScale.x = direction;
        transform.localScale = myScale;
    }

    private void FakeGravity() {
        if (!isGrounded) {
            fakeExtraGravity -= 2 * Time.deltaTime;
        } else {
            fakeExtraGravity = 0;
        }
        if (fakeExtraGravity <= -0.5f) {
            fakeExtraGravity = -0.5f;
        }
    }

    private void CollisionCheck() {

    }

    private void JumpCheck() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, whatIsGround);
        if (isGrounded) {
            extraJumping = extraJumps;
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            Jump(0);
        }
        if (!isGrounded && extraJumping > 0 && Input.GetKeyDown(KeyCode.Space)) {
            extraJumping--;
            rb2D.velocity = new Vector2(0, 0);
            fakeExtraGravity = 0;
            Jump(5);
        }
    }
    
    private void Jump(float extra) {
        rb2D.velocity = Vector2.up * (jumpForce + extra);
    }

    public void PlayerLog(string message) {
        Debug.Log("PLAYER: " + message);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, 0.15f);
    }

}
