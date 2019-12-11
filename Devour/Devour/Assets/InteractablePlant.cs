using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlant : MonoBehaviour
{
    private Animator anim;
    private bool isTouching;
    void Start()
    {
        anim = GetComponent<Animator>();
        isTouching = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isTouching)
            {
                anim.SetTrigger("Type1FromLeftTrigger");
                isTouching = true;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouching = false;
        }
    }
}
