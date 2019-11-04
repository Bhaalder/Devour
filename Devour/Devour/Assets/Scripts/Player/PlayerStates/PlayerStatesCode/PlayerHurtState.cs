//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerHurtState")]
public class PlayerHurtState : PlayerBaseState {

    [Tooltip("How long time hurtstate lasts")]
    [SerializeField] private float startHurtTime;
    private float hurtTime;

    public override void Enter() {
        //owner.PlayerLog("HurtState");
        owner.IsInvulnerable = true;
        owner.PlayerState = PlayerState.HURT;
        hurtTime = startHurtTime;
        AudioPlaySoundEvent hurtAudio = new AudioPlaySoundEvent {
            name = "Hurt",
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1f
        };
        hurtAudio.FireEvent();
    }

    public override void HandleFixedUpdate() {

        //base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        CollisionCheck();
        hurtTime -= Time.deltaTime;
        if(hurtTime <= 0) {
            owner.Transition<PlayerIdleState>();
        }
        //base.HandleUpdate();
    }
}
