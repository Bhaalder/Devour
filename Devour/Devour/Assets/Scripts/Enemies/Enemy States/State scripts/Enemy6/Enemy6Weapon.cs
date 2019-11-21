using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6Weapon : MonoBehaviour
{

    private float damage;
    private SpriteRenderer sprite;

    void Start()
    {
        damage = GetComponentInParent<Enemy6>().WeaponDamage;
        sprite = GetComponent<SpriteRenderer>();
    }


}
