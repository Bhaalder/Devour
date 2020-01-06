using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTriggerScript : MonoBehaviour
{


    public bool isLastAreaEnter;
    public bool isTutorialBP;
    //public bool isLastAreaEnter;


    public GameObject lastAreaText;
    
    // Start is called before the first frame update
    void Start()
    {
        lastAreaText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isLastAreaEnter)
        {
            lastAreaText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isLastAreaEnter)
        {
            lastAreaText.SetActive(false);
        }
    }
    
}
