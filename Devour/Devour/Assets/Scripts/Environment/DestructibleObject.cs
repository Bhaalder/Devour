using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private float health = 100;

    private BoxCollider2D boxCollider2D;

    void Start()
    {
        PlayerAttackEvent.RegisterListener(TakeDamage);
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }

    private void TakeDamage(PlayerAttackEvent attackEvent)
    {
        if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds))
        {
            health -= attackEvent.damage;
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
