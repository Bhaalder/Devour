using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Enemy4State
{
    NONE, IDLE, MOVEMENT, RANGE_ATTACK, HURT, DEATH
}
public class Enemy4 : Enemy
{
    [SerializeField] private bool isIdle;

    public bool IsIdle { get; set; }
    public Enemy4State State { get; set; }
    public Animator Animator { get; set; }

    protected override void Awake()
    {
        base.Awake();
        IsIdle = isIdle;
        Animator = GetComponent<Animator>();

        if (isIdle)
        {
            Transition<Enemy4Idle>();
        }
    }

    protected override void Update()
    {
        base.Update();
        //Animator.SetInteger("State", (int)State);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
