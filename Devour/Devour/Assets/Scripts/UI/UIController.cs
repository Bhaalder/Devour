//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour{

    [SerializeField] private GameObject voidTalentScreen;

    private TextMeshProUGUI tipText;
    private Player player;
    [SerializeField] private Animator uiAnimator;

    private void Awake() {
        VoidTalentScreenEvent.RegisterListener(OnVoidTalentScreen);
        ShowTipTextEvent.RegisterListener(OnShowTipText);
        HideTipTextEvent.RegisterListener(OnHideTipText);
        PlayerTakeDamageEvent.RegisterListener(OnTakeDamage);
    }

    private void Start() {
        player = GameController.Instance.Player;
        tipText = player.PlayerCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnVoidTalentScreen(VoidTalentScreenEvent screenEvent) {
        voidTalentScreen.SetActive(!voidTalentScreen.activeSelf);
        PlayerBusyEvent playerBusy = new PlayerBusyEvent {
            playerIsBusy = voidTalentScreen.activeSelf
        };
        playerBusy.FireEvent();
        GameController.Instance.GamePaused(voidTalentScreen.activeSelf);
        Cursor.visible = !Cursor.visible;
        if(Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    private void OnTakeDamage(PlayerTakeDamageEvent damageEvent) {
        uiAnimator.SetTrigger("Healing");
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
        PlayerTakeDamageEvent.UnRegisterListener(OnTakeDamage);
    }
}
