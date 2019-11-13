using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy3State
{
    NONE, IDLE, MOVEMENT, CHARGE_TELEGRAPH, CHARGE, HURT, DEATH
}
public class Enemy3 : Enemy
{
    public bool ChargeEnemy { get; set; }
    public bool PatrolEnemy { get; set; }
    public Vector2 ChargeTarget { get; set; }
    public float PatrolMoveRange { get; set; }
    public bool IWasStuck { get; set; }
    public Enemy3State State { get; set; }
    public Animator Animator { get; set; }

    [SerializeField] private bool chargeEnemy = false;
    [SerializeField] private bool patrolEnemy = false;
    [SerializeField] private float patrolMoveRange = 5f;


    protected override void Awake()
    {
        base.Awake();
        ChargeEnemy = chargeEnemy;
        PatrolEnemy = patrolEnemy;
        PatrolMoveRange = patrolMoveRange;
        IWasStuck = false;
        Animator = GetComponent<Animator>();

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
