//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroVoidBombState")]
public class NazroVoidBombState : NazroBaseState {

    [Tooltip("How long time before a bomb comes out (aswell as between each bomb)")]
    [SerializeField] private float voidBombWindUpTime;
    [Tooltip("How long time the bombattack lasts")]
    [SerializeField] private float voidBombAttackTime;
    [Tooltip("How much damage a bomb does")]
    [SerializeField] private float bombDamage;
    [Tooltip("How fast a bomb moves")]
    [SerializeField] private float voidBombSpeed;
    [Tooltip("How long a bomb is out before exploding")]
    [SerializeField] private float bombLifespan;
    [Tooltip("The number of bombs that Nazro shoots out during the attackTime")]
    [SerializeField] private int numberOfBombs;
    [Tooltip("The number of extra bombs that Nazro shoots out during second phase")]
    [SerializeField] private int secondPhaseExtraNumberOfBombs;
    [Tooltip("The bomb that Nazro shoots out")]
    [SerializeField] private GameObject voidBombPrefab;
    private int totalNumberOfBombs;
    private float windUpLeft;
    private float attackTimeLeft;
    private List<int> spawnLocations;

    public override void Enter() {
        owner.State = BossNazroState.VOID_BOMB;
        owner.BossLog("VoidBombState");
        windUpLeft = voidBombWindUpTime;
        attackTimeLeft = voidBombAttackTime;
        if (owner.IsSecondPhase) {
            totalNumberOfBombs = numberOfBombs + secondPhaseExtraNumberOfBombs;
        } else {
            totalNumberOfBombs = numberOfBombs;
        }
        spawnLocations = new List<int>();
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && totalNumberOfBombs > 0) {
            SpawnBomb();
            totalNumberOfBombs--;
            windUpLeft = voidBombWindUpTime;
        }
        if (attackTimeLeft <= 0) {
            owner.Transition<NazroIdleState>();
        }
        base.HandleUpdate();
    }

    private void SpawnBomb() {
        GameObject bomb;
        NazroVoidBomb nazroVoidBomb;
        bomb = Instantiate(voidBombPrefab, RandomLocationSpawn().position, Quaternion.identity);
        nazroVoidBomb = bomb.GetComponent<NazroVoidBomb>();
        nazroVoidBomb.Damage = bombDamage;
        nazroVoidBomb.Speed = voidBombSpeed;
        nazroVoidBomb.LifeSpan = bombLifespan;
    }

    private Transform RandomLocationSpawn() {
        int random = Random.Range(0, 4);
        for(int i = 0; i < owner.VoidBombSpawnLocations.Length; i++){
            if(i == random) {
                if (HasSpawnedHere(i)) {
                    if(spawnLocations.Count >= 4) {
                        spawnLocations.Clear();
                    } else {
                        while (HasSpawnedHere(i)) {
                            i = Random.Range(0, 4);
                        }
                    }
                }
                spawnLocations.Add(i);
                return owner.VoidBombSpawnLocations[i];
            }
        }
        return owner.transform;
    }

    private bool HasSpawnedHere(int i) {
        if (spawnLocations.Contains(i)) {
            return true;
        }
        return false;
    }

}
