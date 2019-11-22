//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroVoidCometState")]
public class NazroVoidCometState : NazroBaseState {

    [Tooltip("How long time before the comets comes out")]
    [SerializeField] private float voidCometWindUpTime;
    [Tooltip("How long time before the comets starts moving after they have spawned")]
    [SerializeField] private float voidCometMoveWindUpTime;
    [Tooltip("How long time the cometattack lasts")]
    [SerializeField] private float voidCometAttackTime;
    [Tooltip("How much damage the comets does")]
    [SerializeField] private float cometDamage;
    [Tooltip("How fast the comets moves")]
    [SerializeField] private float voidCometSpeed;
    [Tooltip("The minimum number of comets that Nazro shoots out")]
    [SerializeField] private int minNumberOfComets;
    [Tooltip("The maximum number of comets that Nazro shoots out")]
    [SerializeField] private int maxNumberOfComets;
    [Tooltip("The additional number of minimum and maximum comets that Nazro shoots out")]
    [SerializeField] private int secondPhaseAdditionalComets;
    [Tooltip("The Horizontal VoidComet prefab that Nazro shoots out")]
    [SerializeField] private GameObject horizontalVoidCometPrefab;
    [Tooltip("The Vertical VoidComet prefab that Nazro shoots out")]
    [SerializeField] private GameObject verticalVoidCometPrefab;
    private int totalNumberOfComets;
    private float windUpLeft;
    private float attackTimeLeft;

    public override void Enter() {
        owner.State = BossNazroState.VOID_COMET;
        owner.BossLog("VoidCometState");
        windUpLeft = voidCometWindUpTime;
        attackTimeLeft = voidCometAttackTime;
        if (owner.IsSecondPhase) {
            totalNumberOfComets = Random.Range(minNumberOfComets, maxNumberOfComets + 1) + secondPhaseAdditionalComets;
        } else {
            totalNumberOfComets = Random.Range(minNumberOfComets, maxNumberOfComets + 1);
        }
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        Debug.Log(Random.Range(minNumberOfComets, maxNumberOfComets + 1));
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && totalNumberOfComets > 0) {
            SpawnComet(Random.Range(0, 2));
            totalNumberOfComets--;
            windUpLeft = voidCometWindUpTime;
        }
        if (attackTimeLeft <= 0) {
            owner.Transition<NazroIdleState>();
        }
        base.HandleUpdate();
    }

    private void SpawnComet(int cometType) {
        GameObject bomb = null;
        NazroVoidComet nazroVoidComet;
        if(cometType == 0) {
            bomb = Instantiate(horizontalVoidCometPrefab, owner.VoidCometSpawnLocations[0].position, Quaternion.identity);
        } else {
            bomb = Instantiate(verticalVoidCometPrefab, owner.VoidCometSpawnLocations[1].position, Quaternion.identity);          
        }
        nazroVoidComet = bomb.GetComponent<NazroVoidComet>();
        nazroVoidComet.StartPosition = owner.VoidCometSpawnLocations[cometType];
        nazroVoidComet.Damage = cometDamage;
        nazroVoidComet.Speed = voidCometSpeed;
        nazroVoidComet.WindUp = voidCometMoveWindUpTime;
        
    }
}
