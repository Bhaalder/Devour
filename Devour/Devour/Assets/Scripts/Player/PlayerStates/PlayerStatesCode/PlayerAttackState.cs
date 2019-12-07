//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerAttackState")]
public class PlayerAttackState : PlayerBaseState {

    [SerializeField] private string[] attackSounds;
    private int attackSoundAlternate;

    public override void Enter() {
        //owner.PlayerLog("AttackState");
        owner.PlayerState = PlayerState.ATTACK;
        owner.UntilNextMeleeAttack = owner.MeleeCooldown;
        AudioPlayRandomSoundEvent attackAudio = new AudioPlayRandomSoundEvent {
            name = attackSounds,
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.93f,
            maxPitch = 1.02f
        };
        attackAudio.FireEvent();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        if (owner.ExtraJumpsLeft > 0 && Input.GetButtonDown("Jump")) {
            owner.ExtraJumpsLeft--;
            owner.Rb2D.velocity = new Vector2(0, 0);
            Jump(0);
        }
        if (owner.UntilNextMeleeAttack-0.1f <= 0) {
            if (Input.GetButton("Horizontal") && owner.IsGrounded) {
                owner.Transition<PlayerWalkState>();
                return;
            }
            if (!owner.IsGrounded) {
                owner.Transition<PlayerAirState>();
                return;
            }
            if (owner.IsGrounded) {
                owner.Transition<PlayerIdleState>();
            }
        }
    }

    public override void Exit() {
        owner.IsAttackingUp = false;
        owner.Animator.SetBool("IsAttackingUp", false);
        base.Exit();
    }

}
