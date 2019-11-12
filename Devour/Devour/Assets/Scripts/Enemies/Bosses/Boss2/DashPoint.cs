using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPoint : MonoBehaviour
{
    //Author: Marcus Söderberg
    [SerializeField] private float spawnArea;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnArea);
    }
}
