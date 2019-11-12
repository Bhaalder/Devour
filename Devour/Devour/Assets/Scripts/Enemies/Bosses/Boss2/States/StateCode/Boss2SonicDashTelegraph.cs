﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicDashTelegraphState")]
public class Boss2SonicDashTelegraph : Boss2BaseState
{
    [SerializeField] private GameObject positionTelegraph;
    [SerializeField] private float telegraphTime = 2f;

    private GameObject chosenPattern;
    private float currentCooldown;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_DASH_TELEGRAPH;
        ChooseDashPattern();
        DashTelegraph();
        currentCooldown = telegraphTime;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        TelegraphTime();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void ChooseDashPattern()
    {
        int pattern = Random.Range(1, 4);
        chosenPattern = null;
        owner.ChosenPattern = null;
        switch (pattern)
        {
            case 3:
                chosenPattern = owner.DashPattern2;
                break;
            case 2:
                chosenPattern = owner.DashPattern2;
                break;
            case 1:
                chosenPattern = owner.DashPattern1;
                break;
            default:
                chosenPattern = owner.DashPattern1;
                break;
        }
        owner.ChosenPattern = chosenPattern;
    }

    private void DashTelegraph()
    {
        foreach (GameObject gameObject in chosenPattern.GetComponent<SonicDashPositions>().Positions)
        {
            GameObject position = Instantiate(positionTelegraph, null);
            positionTelegraph.transform.position = gameObject.transform.position;
        }
    }

    private void TelegraphTime()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = telegraphTime;
        owner.Transition<Boss2SonicDashAttack>();
    }
}