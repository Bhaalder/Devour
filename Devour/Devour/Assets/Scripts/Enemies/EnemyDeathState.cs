﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/EnemyDeathState")]

public class EnemyDeathState : EnemyBaseState
{

    [SerializeField] protected GameObject particleOnDeath;
    [SerializeField] protected float deathTimer = 1f;
    protected float currentCooldown;
    protected GameObject[] childrenToDisable;

    public override void Enter()
    {
        if (owner.GetComponent<BoxCollider2D>() != null)
        {
            owner.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (owner.GetComponent<CircleCollider2D>() != null)
        {
            owner.GetComponent<CircleCollider2D>().enabled = false;
        }

        base.Enter();

        owner.rb.gravityScale = 0;

        currentCooldown = deathTimer;

        childrenToDisable = owner.ChildrenToDisable;

        if (childrenToDisable != null)
        {
            foreach (GameObject gameObject in childrenToDisable)
            {
                gameObject.SetActive(false);
            }
        }

        owner.IsAlive = false;

        GameObject instantiatedParticle = Instantiate(particleOnDeath, null);
        instantiatedParticle.transform.position = owner.rb.position;
        try {
            owner.DeathSound();
        } catch (System.IndexOutOfRangeException) {
            Debug.Log("This enemy does not have a deathsound declared");
        }


        
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        DestroyTimer();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    protected void DestroyTimer()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }
        
        Destroy(owner.gameObject);
    }
}
