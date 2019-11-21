using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Enemy6State
{
    NONE, INTRO, IDLE, TELEGRAPH_ATTACK ,ATTACK, MOVEMENT, HURT, DEATH
}
public class Enemy6 : Enemy
{
    [SerializeField] private float attackDistance;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float weaponDamage;
    public Enemy6State State { get; set; }
    public Animator Animator { get; set; }
    public float AttackDistance { get; set; }
    public GameObject Weapon { get; set; }
    public float WeaponDamage { get; set; }
    

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        Weapon = weapon;
        AttackDistance = attackDistance;
        WeaponDamage = weaponDamage;
        Weapon.SetActive(false);
        Transition<Enemy6Idle>();
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

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Player");
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPlayerOnContact,
                enemyPosition = rb.position
            };
            ptde.FireEvent();
            Stunned = true;
            Transition<Enemy6Hurt>();
        }
    }

}
