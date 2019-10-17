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
