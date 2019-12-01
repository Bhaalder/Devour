//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroWaitState")]
public class NazroWaitState : NazroBaseState {

    [Tooltip("Minimum time before the comets spawns")]
    [SerializeField] private float voidCometWindUpTimeMin;
    [Tooltip("Maximum time before the comets spawns")]
    [SerializeField] private float voidCometWindUpTimeMax;
    [Tooltip("How long time before the comets starts moving after they have spawned")]
    [SerializeField] private float voidCometMoveWindUpTime;
    [Tooltip("How much damage the comets does")]
    [SerializeField] private float cometDamage;
    [Tooltip("How fast the comets moves")]
    [SerializeField] private float voidCometSpeed;
    [Tooltip("The VoidComet prefab that Nazro shoots out during the platforming segment")]
    [SerializeField] private GameObject platformingVoidCometPrefab;
    private float windUpLeft;

    public override void Enter() {
        owner.State = BossNazroState.WAIT;
        owner.BossLog("WaitState");
        base.Enter();
    }

    public override void HandleFixedUpdate() {

    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        if (windUpLeft <= 0) {
            if (!PlayerIsAtEndOfSegment()) {
                SpawnComet();
            }
            windUpLeft = Random.Range(voidCometWindUpTimeMin, voidCometWindUpTimeMax + 1);
        }
        if (PlayerIsInsideBossRoom() && owner.Player.PlayerState != PlayerState.HURT) {
            owner.Transition<NazroSecondPhaseIntroState>();
        }
    }

    private void SpawnComet() {
        GameObject comet = null;
        NazroVoidComet nazroVoidComet;
        comet = Instantiate(platformingVoidCometPrefab, owner.transform.position, Quaternion.identity);
        nazroVoidComet = comet.GetComponent<NazroVoidComet>();
        nazroVoidComet.StartPosition = owner.transform;
        nazroVoidComet.Damage = cometDamage;
        nazroVoidComet.Speed = voidCometSpeed;
        nazroVoidComet.WindUp = voidCometMoveWindUpTime;
    }

    private bool PlayerIsAtEndOfSegment() {
        if (owner.Player.BoxCollider2D.bounds.Intersects(owner.EndPlatformArea.bounds)) {
            return true;
        }
        return false;
    }

}
