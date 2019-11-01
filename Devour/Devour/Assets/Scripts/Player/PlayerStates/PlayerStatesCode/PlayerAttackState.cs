//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerAttackState")]
public class PlayerAttackState : PlayerBaseState {

    public override void Enter() {//denna animation som ska vara här måste nog ha exit-time (jag kollade på hollowknight)
        //owner.PlayerLog("AttackState");
        owner.PlayerState = PlayerState.ATTACK;
        owner.UntilNextMeleeAttack = owner.MeleeCooldown;
        AudioPlaySoundEvent attackAudio = new AudioPlaySoundEvent {
            name = "Attack",
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1f
        };
        attackAudio.FireEvent();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(owner.UntilNextMeleeAttack-0.1f <= 0) {
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
        base.HandleUpdate();
    }

    public override void Exit() {
        owner.IsAttackingUp = false;
        owner.Animator.SetBool("IsAttackingUp", false);
        //AudioStopSoundEvent stopSwoosh = new AudioStopSoundEvent {
        //    name = "Attack",
        //};
        //stopSwoosh.FireEvent();
        AudioFadeSoundEvent fadeSwoosh = new AudioFadeSoundEvent {
            name = "Attack",
            isFadeOut = true,
            fadeDuration = 0.08f,
            soundVolumePercentage = 0
        };
        fadeSwoosh.FireEvent();
        base.Exit();
    }

}
