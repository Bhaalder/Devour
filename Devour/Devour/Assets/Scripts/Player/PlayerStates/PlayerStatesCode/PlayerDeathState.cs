//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerDeathState")]
public class PlayerDeathState : PlayerBaseState {

    [SerializeField] private float deathAnimationTime;
    private float timeLeft;

    public override void Enter() {
        //owner.PlayerLog("DeathState");
        owner.PlayerState = PlayerState.DEATH;
        timeLeft = deathAnimationTime;
        owner.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        owner.UntilInvulnerableEnds = deathAnimationTime + 1f;
        owner.Rb2D.gravityScale = 0;
    }

    public override void HandleFixedUpdate() {
        //base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        owner.Rb2D.velocity = Vector2.zero;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) {
            owner.Transition<PlayerIdleState>();
        }
        //base.HandleUpdate();
    }

    public override void Exit() {
        owner.Rb2D.gravityScale = 6;
        PlayerDiedEvent diedEvent = new PlayerDiedEvent {
            Description = "Player died!",
            player = owner
        };
        owner.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        diedEvent.FireEvent();
        PlayerHealEvent healEvent = new PlayerHealEvent {
            amount = 500//FÖR TILLFÄLLET
        };
        healEvent.FireEvent();
        base.Exit();
    }
}
