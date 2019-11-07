﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidGround : MonoBehaviour
{

    [SerializeField] private float timeBeforeDestruction = 2f;
    [SerializeField] private float damageToPlayerOnContact;
    [SerializeField] private GameObject telegraphPartice;
    private float currentCooldown;
    private float currentStartCooldown;

    private SpriteRenderer sprite;

    private bool particleInstantiated;
    private bool canDoDamage;

    public float StartCooldown { get; set; }


    private void Start()
    {
        currentCooldown = timeBeforeDestruction;
        currentStartCooldown = StartCooldown;
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        particleInstantiated = false;
        canDoDamage = false;
    }
    private void Update()
    {
        if (!canDoDamage)
        {
            StartTimer();
        }
        else if (canDoDamage)
        {
            DestroyTimer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && canDoDamage == true)
        {
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent
            {
                damage = damageToPlayerOnContact,
                enemyPosition = transform.position
            };
            ptde.FireEvent();
        }
    }

    private void DestroyTimer()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        Destroy(gameObject);
    }

    private void StartTimer()
    {
        currentStartCooldown -= Time.deltaTime;
        if (!particleInstantiated)
        {
            particleInstantiated = true;
            GameObject instantiatedParticle = Instantiate(telegraphPartice, null);
            instantiatedParticle.transform.position = transform.position;
            instantiatedParticle.GetComponent<DestroyTimer>().DestructionTime = currentStartCooldown;
        }

        if (currentStartCooldown > 0)
        {
            return;
        }

        canDoDamage = true;
        sprite.enabled = true;
        
    }
}
