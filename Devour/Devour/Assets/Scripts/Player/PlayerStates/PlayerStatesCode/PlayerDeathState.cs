//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerDeathState")]
public class PlayerDeathState : PlayerBaseState {

    [SerializeField] private float deathAnimationTime;
    [SerializeField] private GameObject deathParticles;
    private float timeLeft;
    private bool startedFade;

    public override void Enter() {
        //owner.PlayerLog("DeathState");
        owner.PlayerState = PlayerState.DEATH;
        timeLeft = deathAnimationTime;
        owner.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        GameObject deathGO;
        deathGO = Instantiate(deathParticles, owner.transform.position, Quaternion.identity);
        owner.UntilInvulnerableEnds = deathAnimationTime + 1f;
        owner.Rb2D.gravityScale = 0;
        startedFade = false;
        owner.IsDead = true;
    }

    public override void HandleFixedUpdate() {
        //base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        owner.Rb2D.velocity = Vector2.zero;
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0.5f && !startedFade) {
            FadeScreenEvent fadeScreen = new FadeScreenEvent {
                isFadeOut = true
            };
            fadeScreen.FireEvent();
            startedFade = true;
        }
        if (timeLeft <= 0) {
            owner.Transition<PlayerIdleState>();
        }
        //base.HandleUpdate();
    }

    public override void Exit() {
        owner.Rb2D.gravityScale = 6;
        int amountOfLifeforceLost = 0;
        
        foreach(Collectible collectible in owner.Collectibles) {
            if(collectible.collectibleType == CollectibleType.LIFEFORCE) {
                amountOfLifeforceLost = collectible.amount/2;
                break;
            }
        }
        
        PlayerCollectibleChange lifeForceLossEvent = new PlayerCollectibleChange {
            collectible = new Collectible(CollectibleType.LIFEFORCE, -amountOfLifeforceLost)
        };
        lifeForceLossEvent.FireEvent();
        PlayerDiedEvent diedEvent = new PlayerDiedEvent {
            Description = "Player died!",
            player = owner,
            collectibleLifeforceLost = new Collectible(CollectibleType.LIFEFORCE, amountOfLifeforceLost)
        };
        diedEvent.FireEvent();
        PlayerHealEvent healEvent = new PlayerHealEvent {
            amount = owner.MaxHealth
        };
        healEvent.FireEvent();
        owner.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        startedFade = false;
        owner.IsDead = false;
        base.Exit();
    }
}
