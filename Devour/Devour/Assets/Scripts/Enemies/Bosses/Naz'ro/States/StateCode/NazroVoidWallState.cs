//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroVoidWallState")]
public class NazroVoidWallState : NazroBaseState {

    [Tooltip("How long time before the wall spawns and starts moving")]
    [SerializeField] private float voidWallWindUpTime;
    [Tooltip("How long time the wallattack lasts")]
    [SerializeField] private float voidWallAttackTime;
    [Tooltip("(Second Phase only) How long before the HorizontalVoidWall spawns")]
    [SerializeField] private float voidWallWaitTime;
    private float windUpLeft;
    private float waitTimeLeft;
    private float attackTimeLeft;
    private bool wallIsSpawned;

    public override void Enter() {
        owner.State = BossNazroState.VOID_WALL;
        owner.BossLog("VoidWallState");
        windUpLeft = voidWallWindUpTime;
        waitTimeLeft = voidWallWaitTime;
        attackTimeLeft = voidWallAttackTime;
        wallIsSpawned = false;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && !wallIsSpawned) {
            SpawnWall();
            wallIsSpawned = true;
        }
        if(owner.IsSecondPhase) {
            SpawnWall();
        }
        if (attackTimeLeft <= 0) {
            owner.Transition<NazroIdleState>();
        }
        base.HandleUpdate();
    }

    private void SpawnWall() {
        if (owner.IsSecondPhase) {
            if (!owner.VerticalVoidWall.activeSelf) {
                owner.VerticalVoidWall.SetActive(true);
            }
            if(!owner.HorizontalVoidWall.activeSelf && waitTimeLeft <= 0) {
                owner.HorizontalVoidWall.SetActive(true);
            }
            waitTimeLeft -= Time.deltaTime;
            return;
        }
        int i = Random.Range(0, 2) + 1;
        switch (i) {
            case 1:
                if (owner.VerticalVoidWall.activeSelf) {
                    owner.HorizontalVoidWall.SetActive(true);
                } else {
                    owner.VerticalVoidWall.SetActive(true);
                }
                break;
            case 2:
                if (owner.HorizontalVoidWall.activeSelf) {
                    owner.VerticalVoidWall.SetActive(true);
                } else {
                    owner.HorizontalVoidWall.SetActive(true);
                }
                break;
            default:
                break;
        }
    }



}
