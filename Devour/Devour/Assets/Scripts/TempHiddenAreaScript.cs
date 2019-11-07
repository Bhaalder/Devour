using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHiddenAreaScript : MonoBehaviour
{

    public GameObject [] theSprite;
    public bool isPermanent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {

            for (int i = 0; i < theSprite.Length; i++)
            {
                theSprite[i].SetActive(false);
            }
            
            Debug.Log("Enter");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !isPermanent)
        {
            for (int i = 0; i < theSprite.Length; i++)
            {
                theSprite[i].SetActive(true);
            }
            Debug.Log("Exit");
        }
        
            
    }
}
