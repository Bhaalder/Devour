﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/BnathIntroState")]


public class BnathIntro : BnathBaseState
{

    public override void Enter()
    {
        base.Enter();
        owner.State = BossBnathState.INTRO;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        Debug.Log("BossFightStart" + owner.GetComponent<Bnath>().BossFightStart);
        if (owner.GetComponent<Bnath>().BossFightStart == true)
        {
            Movement();
        }
        owner.rb.position = owner.StartPosition.transform.position;
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
