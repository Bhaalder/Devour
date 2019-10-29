using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private bool isUsingStages;
    [SerializeField] GameObject fullHealth;
    [SerializeField] GameObject damagedHealth;
    [SerializeField] GameObject halfHealth;

    private BoxCollider2D boxCollider2D;
    private float originalHealth;

    void Start()
    {
        PlayerAttackEvent.RegisterListener(TakeDamage);
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalHealth = health;
    }

    private void TakeDamage(PlayerAttackEvent attackEvent)
    {
        if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds))
        {
            health -= attackEvent.damage;
            if (health < originalHealth && health > originalHealth/2 && isUsingStages)
            {
                fullHealth.SetActive(false);
                damagedHealth.SetActive(true);
                halfHealth.SetActive(false);
            }
            if (health <= originalHealth / 2 && isUsingStages)
            {
                fullHealth.SetActive(false);
                damagedHealth.SetActive(false);
                halfHealth.SetActive(true);
            }
            if (health <= 0)
            {
                DestroyObject();
            }
        }
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
