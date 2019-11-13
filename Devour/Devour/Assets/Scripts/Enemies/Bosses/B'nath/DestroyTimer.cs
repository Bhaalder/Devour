using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float destructionTime;
    public float DestructionTime { get; set; } = 1f;

    void Start()
    {
        if(destructionTime > 0)
        {
            DestructionTime = destructionTime;
        }
        else
        {
            destructionTime = DestructionTime;
        }
    }

    void Update()
    {
        DestroyCooldown();
    }

    private void DestroyCooldown()
    {
        DestructionTime -= Time.deltaTime;

        if (DestructionTime > 0)
        {
            return;
        }

        Destroy(gameObject);
    }
}
