//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour{

    [SerializeField] private GameObject VoidTalentScreen;


    private void Awake() {
        VoidTalentScreenEvent.RegisterListener(OnVoidTalentScreen);
    }

    private void OnVoidTalentScreen(VoidTalentScreenEvent screenEvent) {
        VoidTalentScreen.SetActive(!VoidTalentScreen.activeSelf);
        GameController.Instance.GamePaused(VoidTalentScreen.activeSelf);
        Cursor.visible = !Cursor.visible;
        if(Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    private void OnDestroy() {
        VoidTalentScreenEvent.UnRegisterListener(OnVoidTalentScreen);
    }
}
