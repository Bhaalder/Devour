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
    [SerializeField] private GameObject dashParticle;
    private float dashTime;
    
    public override void Enter() {
        //owner.PlayerLog("DashState");
        owner.PlayerState = PlayerState.DASH;
        dashTime = startDashTime;
        owner.UntilNextDash = owner.DashCooldown;
        AudioPlaySoundEvent dashAudio = new AudioPlaySoundEvent {
            name = "Dash",
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1.05f
        };
        dashAudio.FireEvent();
        GameObject particle = Instantiate(dashParticle, owner.GroundChecks[0].position, Quaternion.identity);
        particle.transform.localScale = new Vector3(particle.transform.localScale.x * owner.FacingDirection, particle.transform.localScale.y, particle.transform.localScale.z);
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        Dash();
        base.HandleUpdate();
    }

    protected void Dash() {
        if (owner.IsWallSliding) {
            Flip(owner.XScale * -owner.FacingDirection);
            owner.FacingDirection *= -1;
        }
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
