using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy4 : Enemy
{
    [SerializeField] private bool isIdle;

    public bool IsIdle { get; set; }

    protected override void Awake()
    {
        base.Awake();
        IsIdle = isIdle;

        if (isIdle)
        {
            Transition<Enemy4Idle>();
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
