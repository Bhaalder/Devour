//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{

    [SerializeField] private string contactTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == contactTag)
        {
            Destroy(gameObject);
        }
    }
}
