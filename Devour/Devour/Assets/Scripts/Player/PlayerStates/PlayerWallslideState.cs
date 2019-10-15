using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerWallslideState")]
public class PlayerWallslideState : PlayerBaseState {

    [Tooltip("The speed of the player sliding down on walls")]
    [SerializeField] private float wallSlideSpeed;

    public override void Enter() {
        owner.PlayerLog("WallslideState");
        owner.PlayerState = PlayerState.WALLSLIDE;
    }

    protected override void MovePlayer() {
        base.MovePlayer();
        if (owner.Rb2D.velocity.y < -wallSlideSpeed) {
            owner.Rb2D.velocity = new Vector2(owner.Rb2D.velocity.x, -wallSlideSpeed);
        }
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if (!owner.IsWallSliding) {
            owner.Transition<PlayerIdleState>();
        }
        base.HandleUpdate();
    }
}
