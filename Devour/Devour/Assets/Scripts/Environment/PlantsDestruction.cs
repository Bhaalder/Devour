using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlantsDestruction : MonoBehaviour
{
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject destructionParticle;
    [SerializeField] private Vector3 hitParticleOffset;
    [SerializeField] private Vector3 destructionParticleOffset;
    [SerializeField] private int hitsToDestroy;

    private BoxCollider2D boxCollider2D;
    private int hits;

    void Start()
    {
        PlayerAttackEvent.RegisterListener(TakeDamage);
        boxCollider2D = GetComponent<BoxCollider2D>();
        if(hitsToDestroy < 1)
        {
            hitsToDestroy = 1;
        }
        hits = 0;
    }

    public void TakeDamage(PlayerAttackEvent attackEvent)
    {
        try
        {
            if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds) && boxCollider2D.enabled == true)
            {
                hits++;
                if (hits >= hitsToDestroy)
                {
                    Instantiate(destructionParticle, gameObject.transform.position + destructionParticleOffset, Quaternion.identity);
                    Destroy(gameObject);
                    return;
                }
                Instantiate(hitParticle, gameObject.transform.position + hitParticleOffset, Quaternion.identity);
            }
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("A missing reference in PlayerAttackEvent, check Log!");
        }
    }

    private void OnDestroy()
    {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
    }
}
