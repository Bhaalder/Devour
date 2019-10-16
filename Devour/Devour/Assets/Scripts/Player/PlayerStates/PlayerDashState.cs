using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerDashState")]
public class PlayerDashState : PlayerBaseState {

    [SerializeField] private float dashForce;
    [SerializeField] private float startDashTime;
    private float dashTime;

    public override void Enter() {
        owner.PlayerLog("DashState");
        owner.PlayerState = PlayerState.DASH;
        dashTime = startDashTime;
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
    }

    protected override void MovePlayer() {
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

    protected override void Jump(float extra) {

    }

}
