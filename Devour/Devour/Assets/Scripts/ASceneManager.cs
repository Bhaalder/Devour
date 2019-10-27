using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASceneManager : MonoBehaviour
{


   
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   public void LoadLevelOne()
    {
        SceneManager.LoadScene("Scene01");
    }
    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("Scene02");
    }
    public void LoadBossArea1()
    {
        SceneManager.LoadScene("BossArea");
    }



    
}
