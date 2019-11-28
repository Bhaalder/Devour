using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TempHiddenAreaScript : MonoBehaviour
{

    public GameObject [] theSprite;
    public bool isPermanent;
    [SerializeField] private int hiddenAreaID;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.HiddenAreasFound.ContainsKey(SceneManager.GetActiveScene().name)) {
            if (GameController.Instance.HiddenAreasFound[SceneManager.GetActiveScene().name].Contains(hiddenAreaID)) {
                Destroy(gameObject);
                return;
            }
        }
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
            if (GameController.Instance.HiddenAreasFound.ContainsKey(SceneManager.GetActiveScene().name)) {
                if (GameController.Instance.HiddenAreasFound[SceneManager.GetActiveScene().name].Contains(hiddenAreaID)) {
                    Debug.LogWarning("A hiddenArea with the same ID [" + hiddenAreaID + "] has already been found in this scene [" + SceneManager.GetActiveScene().name + "]");
                    Destroy(gameObject);
                    return;
                }
                GameController.Instance.HiddenAreasFound[SceneManager.GetActiveScene().name].Add(hiddenAreaID);
            } else {
                List<int> newHiddenAreaList = new List<int> { hiddenAreaID };
                GameController.Instance.HiddenAreasFound.Add(SceneManager.GetActiveScene().name, newHiddenAreaList);
            }
            Destroy(gameObject);
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
