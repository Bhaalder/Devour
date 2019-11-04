using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidGround : MonoBehaviour
{

    [SerializeField] private float timeBeforeDestruction = 2f;
    [SerializeField] private float damageToPlayerOnContact;
    private float currentCooldown;


    private void Start()
    {
        currentCooldown = timeBeforeDestruction;
    }
    private void Update()
    {
        DestroyTimer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collision:" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
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
}
