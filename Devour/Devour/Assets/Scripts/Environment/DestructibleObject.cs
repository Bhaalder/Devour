using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private bool isUsingStageDamagedHealth;
    [SerializeField] private bool isUsingStageHalfHealth;
    [SerializeField] private bool isUsingDestroyCooldown;
    [SerializeField] GameObject fullHealth;
    [SerializeField] GameObject damagedHealth;
    [SerializeField] GameObject halfHealth;
    [SerializeField] float destroyCooldown = 2f;

    private BoxCollider2D boxCollider2D;

    private bool isOnCooldown;

    private float originalHealth;
    private float currentCooldown;

    void Start()
    {
        PlayerAttackEvent.RegisterListener(TakeDamage);
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalHealth = health;
        currentCooldown = destroyCooldown;
        isOnCooldown = false;
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            DestroyCooldown();
        }
    }

    private void TakeDamage(PlayerAttackEvent attackEvent)
    {
        if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds))
        {
            health -= attackEvent.damage;
            if (health < originalHealth && health > originalHealth/2 && isUsingStageDamagedHealth)
            {
                fullHealth.SetActive(false);
                damagedHealth.SetActive(true);
                halfHealth.SetActive(false);
            }
            if (health <= originalHealth / 2 && isUsingStageHalfHealth)
            {
                fullHealth.SetActive(false);
                damagedHealth.SetActive(false);
                halfHealth.SetActive(true);
            }
            if (health <= 0)
            {
                if (isUsingDestroyCooldown)
                {
                    isOnCooldown = true;
                    //start destroy animation?
                }
                else
                {
                    DestroyObject();
                }
            }
        }
    }

    private void DestroyCooldown()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
    }
}
