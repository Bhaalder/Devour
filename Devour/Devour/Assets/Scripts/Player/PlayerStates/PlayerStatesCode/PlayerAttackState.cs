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
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        if(owner.UntilNextMeleeAttack <= 0) {
            owner.IsAttackingUp = false;
            owner.Animator.SetBool("IsAttackingUp", false);
            if (Input.GetButton("Horizontal")) {
                owner.Transition<PlayerWalkState>();
                return;
            }
            if (!owner.IsGrounded) {
                owner.Transition<PlayerAirState>();
                return;
            }
            owner.Transition<PlayerIdleState>();
        }
        base.HandleUpdate();
    }
}
