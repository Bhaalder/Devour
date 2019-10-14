﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerIdleState")]
public class PlayerIdleState : PlayerBaseState {

    public override void Enter() {

        owner.PlayerLog("IdleState");
    }


}
