using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManTrigger : MonoBehaviour
{

    public GameObject theParticle;
    // Start is called before the first frame update
    void Start()
    {
        
        theParticle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            theParticle.SetActive(true);
        }
    }
}
