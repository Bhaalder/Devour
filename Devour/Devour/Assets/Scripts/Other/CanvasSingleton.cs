//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton : MonoBehaviour{

    public bool Exists { get => exists; set => exists = value; }

    private static bool exists;

    private void Awake() {
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
            return;
        }
        GameController.Instance.Canvas = gameObject.transform;

        MainMenuEvent.RegisterListener(OnMainMenuSwitch);
    }

    private void OnMainMenuSwitch(MainMenuEvent menuEvent)
    {
        exists = false;
        Destroy(gameObject, 3f);
    }

    private void OnDestroy()
    {
        MainMenuEvent.UnRegisterListener(OnMainMenuSwitch);
    }
}
