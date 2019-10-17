//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerDashState")]
public class PlayerDashState : PlayerBaseState {

    [Tooltip("How fast you dash")]
    [SerializeField] private float dashForce;
    [Tooltip("How long time the dash lasts")]
    [SerializeField] private float startDashTime;
    private float dashTime;
    
    public override void Enter() {
        //owner.PlayerLog("DashState");
        owner.PlayerState = PlayerState.DASH;
        dashTime = startDashTime;
        owner.UntilNextDash = owner.DashCooldown;
        if (owner.IsTouchingWall) {
            Flip(owner.XScale * -owner.FacingDirection);
            owner.FacingDirection *= -1;
        }
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        Dash();
        base.HandleUpdate();
    }

    protected void Dash() {
        //base.MovePlayer();
   
        owner.Rb2D.velocity = new Vector2((dashForce * owner.FacingDirection), 0);

        if (dashTime <= 0) {
            if (owner.IsGrounded) {
                owner.Transition<PlayerIdleState>();
            } else {
                owner.Transition<PlayerAirState>();
            }
        }
        dashTime -= Time.deltaTime;
        
    }

    protected override void GetMovementInput() {
        owner.XInput = 0;
    }

    protected override void MovePlayer() {

    }

    protected override void Jump(float extra) {

    }

}
