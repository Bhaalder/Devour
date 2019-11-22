//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroVoidBombState")]
public class NazroVoidBombState : NazroBaseState {

    [Tooltip("How long time before the bomb comes out")]
    [SerializeField] private float voidBombWindUpTime;
    [Tooltip("How long time the bomb attack lasts")]
    [SerializeField] private float voidBombAttackTime;
    [Tooltip("How much damage the bomb does")]
    [SerializeField] private float bombDamage;
    [Tooltip("How fast the bomb moves")]
    [SerializeField] private float voidBombSpeed;
    [Tooltip("How long the bomb is out before exploding")]
    [SerializeField] private float bombLifespan;
    [Tooltip("The bomb that Nazro shoots out")]
    [SerializeField] private GameObject voidBombPrefab;
    private int numberOfBombs;
    private float windUpLeft;
    private float attackTimeLeft;
    private List<int> spawnLocations;

    public override void Enter() {
        owner.State = BossNazroState.VOID_BOMB;
        owner.BossLog("VoidBombState");
        windUpLeft = voidBombWindUpTime;
        attackTimeLeft = voidBombAttackTime;
        if (owner.IsSecondPhase) {
            numberOfBombs = 2;
        } else {
            numberOfBombs = 1;
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
        if (windUpLeft <= 0 && numberOfBombs > 0) {
            SpawnBomb();
            numberOfBombs--;
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
        bomb = Instantiate(voidBombPrefab, RandomLocationSpawn().position, Quaternion.identity);//ska kanske spawnas i nått random hörn på banan?
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
