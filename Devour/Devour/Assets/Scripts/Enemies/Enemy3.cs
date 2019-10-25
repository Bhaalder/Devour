using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    public bool ChargeEnemy { get; set; }
    public bool PatrolEnemy { get; set; }
    public Vector2 ChargeTarget { get; set; }
    public float PatrolMoveRange { get; set; }

    [SerializeField] private bool chargeEnemy = false;
    [SerializeField] private bool patrolEnemy = false;
    [SerializeField] private float patrolMoveRange = 5f;


    protected override void Awake()
    {
        base.Awake();
        ChargeEnemy = chargeEnemy;
        PatrolEnemy = patrolEnemy;
        PatrolMoveRange = patrolMoveRange;
        Debug.Log("Charge=" + ChargeEnemy + " & " + "Patrol=" + PatrolEnemy);
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
