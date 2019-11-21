﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5AttackState")]
public class Enemy5Attack : EnemyMovement
{

    [SerializeField] private float Count = 1f;
    [SerializeField] private float telegraphTime = 1f;
    [SerializeField] private float middlePointCurve = 10f;
    [SerializeField] private float targetYOffset = -1.5f;
    [SerializeField] GameObject particles;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 middlePoint;

    private float countUp;
    private float currentTelegraphCooldown;

    private bool startAttack;
    private bool particleInstantiated;

    private bool initializeState;


    public override void Enter()
    {
        base.Enter();

        startAttack = false;
        initializeState = false;
        particleInstantiated = false;

        currentTelegraphCooldown = telegraphTime;
        countUp = 0;
    }

    public override void HandleUpdate()
    {

        if (!initializeState)
        {
            owner.GetComponent<Enemy5>().State = Enemy5State.ATTACK;
            startPoint = owner.rb.position;
            endPoint = new Vector2(owner.Player.transform.position.x, startPoint.y + targetYOffset);
            middlePoint = startPoint + (endPoint - startPoint) / 2 + Vector2.up * middlePointCurve;
            initializeState = true;
        }

        base.HandleUpdate();

        if (!startAttack)
        {
            TurnedRight();
            BodySlamTelegraph();
        }
        else
        {
            BodySlam();
        }
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void BodySlam()
    {
        if (countUp < Count)
        {
            countUp += Count * Time.deltaTime;

            Vector2 m1 = Vector2.Lerp(startPoint, middlePoint, countUp);
            Vector2 m2 = Vector2.Lerp(middlePoint, endPoint, countUp);
            owner.gameObject.transform.position = Vector3.Lerp(m1, m2, countUp);
        }
        else
        {
            //CameraShakeEvent shakeEvent = new CameraShakeEvent
            //{
            //    startDuration = 0.4f,
            //    startValue = 0.35f
            //};
            //shakeEvent.FireEvent();
            TurnedRight();
            owner.Transition<Enemy5Idle>();
        }
    }

    private void BodySlamTelegraph()
    {
        currentTelegraphCooldown -= Time.deltaTime;

        if (currentTelegraphCooldown > 0)
        {
            if (!particleInstantiated)
            {
                particleInstantiated = true;
                GameObject instantiatedParticle = Instantiate(particles, null);
                instantiatedParticle.transform.position = endPoint;
            }

            return;
        }

        startAttack = true;
        currentTelegraphCooldown = telegraphTime;
    }

}
