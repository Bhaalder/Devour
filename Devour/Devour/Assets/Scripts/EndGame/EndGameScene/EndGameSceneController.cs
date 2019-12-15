using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSceneController : MonoBehaviour
{
    [SerializeField] private GameObject[] endGameCards;
    [SerializeField] [Range(1, 10)] private float timeBetweenCards;
    [SerializeField] private string sceneExit;

    private float fadeTime;
    private int currentCard;
    private float CurrentTimer;
    private bool canActivateButton;
    private bool exitDirectlyToMainMenu;

    void Start()
    {
        fadeTime = 1 / (timeBetweenCards / 2 * 3);
        CurrentTimer = timeBetweenCards;
        exitDirectlyToMainMenu = true;
    }

    void Update()
    {
        timer();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitDirectlyToMainMenu)
            {
                Invoke("sceneChange", timeBetweenCards);
                MainMenuEvent goToMainMenuEvent = new MainMenuEvent { };
                goToMainMenuEvent.FireEvent();
                return;
            }
        }
    }

    public void OnButtonClick()
    {
        if (canActivateButton)
        {
            if (currentCard >= endGameCards.Length)
            {
                return;
            }
            FadeScreenEvent fadeOutScreen = new FadeScreenEvent
            {
                isFadeOut = true,
                fadeSpeed = fadeTime
            };
            fadeOutScreen.FireEvent();
            if (currentCard == endGameCards.Length - 1)
            {
                Invoke("sceneChange", timeBetweenCards);
                MainMenuEvent goToMainMenuEvent = new MainMenuEvent { };
                goToMainMenuEvent.FireEvent();
                SwitchSceneEvent switchScene = new SwitchSceneEvent
                {
                    enteringSceneName = sceneExit,
                    leavingSceneName = SceneManager.GetActiveScene().name
                };
                switchScene.FireEvent();
                return;
            }
            else
            {

                Invoke("nextCard", timeBetweenCards / 2);

            }
            canActivateButton = false;
            CurrentTimer = timeBetweenCards;
        }
    }

    private void nextCard()
    {
        endGameCards[currentCard].SetActive(false);
        currentCard++;
        endGameCards[currentCard].SetActive(true);
        FadeScreenEvent fadeInScreen = new FadeScreenEvent
        {
            isFadeIn = true,
            fadeSpeed = fadeTime
        };
        fadeInScreen.FireEvent();
    }

    private void sceneChange()
    {
        SceneManager.LoadScene(sceneExit);
    }

    private void timer()
    {
        CurrentTimer -= Time.deltaTime;
        if(CurrentTimer > 0)
        {
            return;
        }

        canActivateButton = true;
    }
}
