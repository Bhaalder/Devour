//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaBasicAttackState")]
public class ZvixaBasicAttackState : ZvixaBaseState {

    [Tooltip("How long time before the ball comes out")]
    [SerializeField] private float basicAttackTimeWindUp;
    [Tooltip("How long time the basic attack lasts")]
    [SerializeField] private float basicAttackTime;
    [Tooltip("How much damage the ball does on contact to player")]
    [SerializeField] private float ballDamage;
    [Tooltip("How much damage the ball does on contact to Zvixa (if the player bounces the ball back)")]
    [SerializeField] private float ballSelfDamage;
    [Tooltip("How fast the ball moves")]
    [SerializeField] private float ballSpeed;
    [Tooltip("How long the ball is out before disappearing")]
    [SerializeField] private float ballLifespan;
    [Tooltip("The projectile that Zvixa shoots out")]
    [SerializeField] private GameObject ballAttackPrefab;
    private bool ballSpawned;
    private float windUpLeft;
    private float attackTimeLeft;


    public override void Enter() {
        owner.State = BossZvixaState.BASICATTACK;
        owner.BossLog("BasicAttackState");
        windUpLeft = basicAttackTimeWindUp;
        attackTimeLeft = basicAttackTime;
        ballSpawned = false;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && !ballSpawned) {
            SpawnBall();
            ballSpawned = true;
        }
        if(attackTimeLeft <= 0) {
            owner.Transition<ZvixaIdleState>();
        }
        base.HandleUpdate();       
    }

    private void SpawnBall() {
        GameObject projectile;
        ZvixaProjectile zvixaProjectile;
        projectile = Instantiate(ballAttackPrefab, owner.transform.position + new Vector3(owner.FacingDirection * 4, 0, 0), Quaternion.identity);
        zvixaProjectile = projectile.GetComponent<ZvixaProjectile>();
        zvixaProjectile.Damage = ballDamage;
        zvixaProjectile.SelfDamage = ballSelfDamage;
        zvixaProjectile.Speed = ballSpeed;
        zvixaProjectile.LifeSpan = ballLifespan;
    }

}
