using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //Author: Marcus Söderberg

    float projectileSpeed;
    float projectileDamage;
    float ProjectileTravelDistance;

    private Vector3 startPosition;
    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("InteractionPlayer").transform;
        
        startPosition = transform.position;

        transform.LookAt(player);
        
    

    }

    void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, startPosition) > ProjectileTravelDistance) { DestroyProjectile(); }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameController.Instance.TakeDamage((int)projectileDamage);
        }
        DestroyProjectile();

    }

    public void SetProjectileSpeed(float speed)
    {
        projectileSpeed = speed;
    }
    public void SetProjectileDamage(float damage)
    {
        projectileDamage = damage;
    }
    public void SetProjectileTravelDistance(float traveldistance)
    {
        ProjectileTravelDistance = traveldistance;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
