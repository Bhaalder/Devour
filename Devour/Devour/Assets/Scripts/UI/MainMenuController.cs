using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour{

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    private void Awake() {
        if (GameController.Instance.gameObject) {
            Destroy(GameController.Instance.Player.gameObject);
            Destroy(GameController.Instance.Canvas.gameObject);
            Destroy(GameController.Instance.gameObject);
        }



    }


}
