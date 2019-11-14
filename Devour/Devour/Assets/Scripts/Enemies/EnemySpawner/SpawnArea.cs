using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    //Author: Marcus Söderberg
    [SerializeField] private float spawnArea;
    // Start is called before the first frame update

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnArea);
    }
}
