//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour{

    [SerializeField] private GameObject VoidTalentScreen;

    private TextMeshProUGUI tipText;
    private Player player;

    private void Awake() {
        VoidTalentScreenEvent.RegisterListener(OnVoidTalentScreen);
        ShowTipTextEvent.RegisterListener(OnShowTipText);
        HideTipTextEvent.RegisterListener(OnHideTipText);
    }

    private void Start() {
        player = GameController.Instance.Player;
        tipText = player.PlayerCanvas.GetComponentInChildren<TextMeshProUGUI>();
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

    private void OnShowTipText(ShowTipTextEvent showTextEvent) {
        tipText.text = showTextEvent.tipText;
    }

    private void OnHideTipText(HideTipTextEvent hideTextEvent) {
        tipText.text = "";
    }

    private void OnDestroy() {
        VoidTalentScreenEvent.UnRegisterListener(OnVoidTalentScreen);
        ShowTipTextEvent.UnRegisterListener(OnShowTipText);
        HideTipTextEvent.UnRegisterListener(OnHideTipText);
    }
}
