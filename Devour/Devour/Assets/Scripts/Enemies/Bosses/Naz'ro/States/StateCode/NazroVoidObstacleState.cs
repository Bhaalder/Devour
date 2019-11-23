//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroVoidObstacleState")]
public class NazroVoidObstacleState : NazroBaseState {

    [Tooltip("How long time before the obstacle comes out")]
    [SerializeField] private float voidObsWindUpTime;
    [Tooltip("How long time before the obstacle gets armed")]
    [SerializeField] private float voidObsArmingTime;
    [Tooltip("How long time the obstacleAttack lasts")]
    [SerializeField] private float voidObsAttackTime;
    [Tooltip("How much damage the obstacle does")]
    [SerializeField] private float obsDamage;
    [Tooltip("The minimum number of obstacles that Nazro spawns at a time")]
    [SerializeField] private int minNumberOfObs;
    [Tooltip("The maximum number of obstacles that Nazro spawns at a time")]
    [SerializeField] private int maxNumberOfObs;
    [Tooltip("The additional number of minimum and maximum obstacles that Nazro spawns at a time")]
    [SerializeField] private int secondPhaseAdditionalObs;
    [Tooltip("The Vertical VoidComet prefab that Nazro shoots out")]
    [SerializeField] private GameObject voidObstaclePrefab;
    private int totalNumberOfObs;
    private float windUpLeft;
    private float attackTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.VOID_OBS;
        owner.BossLog("ObstacleState");
        windUpLeft = voidObsWindUpTime;
        attackTimeLeft = voidObsAttackTime;
        if (owner.IsSecondPhase) {
            totalNumberOfObs = Random.Range(minNumberOfObs, maxNumberOfObs + 1) + secondPhaseAdditionalObs;
        } else {
            totalNumberOfObs = Random.Range(minNumberOfObs, maxNumberOfObs + 1);
        }
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && totalNumberOfObs > 0) {
            if(owner.NazroVoidObstacles.Count < owner.MaximumNumberOfVoidObstacles) {
                SpawnObstacle();
            }
            totalNumberOfObs--;
            windUpLeft = voidObsWindUpTime;
        }
        if (attackTimeLeft <= 0) {
            owner.Transition<NazroIdleState>();
        }
        base.HandleUpdate();
    }

    private void SpawnObstacle() {
        GameObject obstacle = null;
        NazroVoidObstacle nazroVoidObs;
        obstacle = Instantiate(voidObstaclePrefab, RandomSpawnPoint(owner.VoidObsSpawnArea.bounds.center, owner.VoidObsSpawnArea.bounds.size), Quaternion.identity);
        nazroVoidObs = obstacle.GetComponent<NazroVoidObstacle>();
        nazroVoidObs.Damage = obsDamage;
        nazroVoidObs.ArmingTime = voidObsArmingTime;
        nazroVoidObs.Nazro = owner;
        owner.NazroVoidObstacles.Add(obstacle);
    }

    private Vector3 RandomSpawnPoint(Vector3 center, Vector3 size) {
        return center + new Vector3((Random.value - 0.5f) * size.x, (Random.value - 0.5f) * size.y, 0);
    }

}
