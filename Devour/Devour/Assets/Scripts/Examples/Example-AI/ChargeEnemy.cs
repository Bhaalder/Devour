//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargeEnemy : Enemy
{
    [Tooltip("Damage done to Player if hit by the charge.")]
    [SerializeField] private int damage;
    [Tooltip("Time in Seconds between two seperate charges.")]
    [SerializeField] private float chargeDowntime;
    private float currentCooldown;
    [SerializeField] private float enemyHealth;

    protected override void Awake()
    {
        DealtDamage = false;
        HasRecentlyCharged = false;
        currentCooldown = chargeDowntime;
        health = enemyHealth;
        base.Awake();
        
    }

    protected override void Update()
    {
        base.Update();
        if(HasRecentlyCharged == true)
        {
            RecentlyCharged();
        }
        
    }

    public override void TakeDamage(float damage)
    {

        if (damage - damageResistance < 0)
        {

        }
        else
        {
            health -= (damage - damageResistance);
        }
        if (health <= 0)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            Death();
        }
        if (isDamaged == false)
        {
            Transition<ChargeChaseState>();
            isDamaged = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !DealtDamage && animator.GetCurrentAnimatorStateInfo(0).IsTag("Charging"))
        {
            GameController.Instance.TakeDamage(damage);
            DealtDamage = true;
            Transition<ChargeStunnedState>();
        }
    }

    private void RecentlyCharged()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        HasRecentlyCharged = false;
        currentCooldown = chargeDowntime;
    }


}
