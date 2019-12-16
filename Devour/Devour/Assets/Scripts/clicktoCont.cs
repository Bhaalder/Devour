using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clicktoCont : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel1()
    {
        SwitchSceneEvent sceneEvent = new SwitchSceneEvent {
            enteringSceneName = "Scene01",
            leavingSceneName = SceneManager.GetActiveScene().name
        };
        sceneEvent.FireEvent();
        FadeScreenEvent fadeOutScreen = new FadeScreenEvent
        {
            isFadeOut = true,
            fadeSpeed = fadeSpeed
        };
        fadeOutScreen.FireEvent();
        Invoke("Switch", 1f);
    }

    private void Switch() {
        SceneManager.LoadScene("Scene01");
    }
}
