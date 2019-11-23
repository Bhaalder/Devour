//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/VoidObstacle/VoidObstacleArmedState")]
public class VoidObstacleArmedState : VoidObstacleSpawnedState {

    public override void Enter() {
        owner.GetComponent<SpriteRenderer>().color = new Color(120, 0, 255, 255);
        owner.VoidObstacleState = VoidObstacleState.ARMED;
    }

    public override void HandleFixedUpdate() {

    }

    public override void HandleUpdate() {

    }

}
