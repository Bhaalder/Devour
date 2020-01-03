﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour{

    [SerializeField] private GameObject voidTalentScreen;
    [SerializeField] private GameObject inGameMenuScreen;

    private TextMeshProUGUI tipText;
    private Player player;
    private Animator uiAnimator;

    private void Awake() {
        VoidTalentScreenEvent.RegisterListener(OnVoidTalentScreen);
        InGameMenuEvent.RegisterListener(OnInGameMenuEvent);
        ShowTipTextEvent.RegisterListener(OnShowTipText);
        HideTipTextEvent.RegisterListener(OnHideTipText);
        PlayerTakeDamageEvent.RegisterListener(OnTakeDamage);
        PlayerHealEvent.RegisterListener(OnPlayerHealed);
        PlayerVoidIsFullEvent.RegisterListener(OnVoidMendFullEvent);
        PlayerCollectibleChangeEvent.RegisterListener(OnPlayerCollectibleChange);
    }

    private void Start() {
        player = GameController.Instance.Player;
        uiAnimator = GetComponent<Animator>();
        tipText = player.PlayerCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        try
        {
            if (player.PlayerState != PlayerState.BUSY && player.PlayerState != PlayerState.DEATH)
            {
                MenuInput();
            }
            else if (GameController.Instance.MenuIsOpen)
            {
                MenuInput();
            }
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Cannot find: GameController");
        }

    }

    private void MenuInput() {
        if (Input.GetButtonDown("Menu")) {
            InGameMenuEvent e = new InGameMenuEvent { };
            e.FireEvent();
        }
        if (Input.GetButtonDown("Back"))
        {
            //BackButtonEvent back = new BackButtonEvent { };
            //back.FireEvent();
            //if (voidTalentScreen.activeSelf)
            //{
            //    VoidTalentScreenEvent closeScreen = new VoidTalentScreenEvent { };
            //    closeScreen.FireEvent();
            //}
        }
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

    private void OnPlayerCollectibleChange(PlayerCollectibleChangeEvent collectibleChangeEvent) {
        if(collectibleChangeEvent.collectible.amount > 0) {
            if(collectibleChangeEvent.collectible.collectibleType == CollectibleType.VOIDESSENCE) {
                uiAnimator.SetTrigger("VECollected");
            } else {
                uiAnimator.SetTrigger("LFCollected");
            }
        }
    }

    private void OnInGameMenuEvent(InGameMenuEvent inGameMenuScreenEvent)
    {
        try
        {
            inGameMenuScreen.GetComponent<InGameMenuController>().InGameMenuGO.SetActive(true);
            inGameMenuScreen.GetComponent<InGameMenuController>().OptionsGO.SetActive(false);
            inGameMenuScreen.GetComponent<InGameMenuController>().SoundOptionsGO.SetActive(false);
            inGameMenuScreen.GetComponent<InGameMenuController>().VisualOptionsGO.SetActive(false);
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("GameObject Could Not Be Found: " + e);
        }

        inGameMenuScreen.SetActive(!inGameMenuScreen.activeSelf);

        PlayerBusyEvent playerBusy = new PlayerBusyEvent
        {
            playerIsBusy = inGameMenuScreen.activeSelf
        };
        playerBusy.FireEvent();
        GameController.Instance.GamePaused(inGameMenuScreen.activeSelf);
        Cursor.visible = !Cursor.visible;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    private void OnTakeDamage(PlayerTakeDamageEvent damageEvent) {
        uiAnimator.SetTrigger("Healing");
    }

    private void OnPlayerHealed(PlayerHealEvent healEvent) {
        if(player.Health >= player.MaxHealth) {
            return;
        }
        uiAnimator.SetTrigger("Healing");
        //ljud
    }

    private void OnVoidMendFullEvent(PlayerVoidIsFullEvent voidEvent) {
        uiAnimator.SetTrigger("VMfull");
    }

    private void OnShowTipText(ShowTipTextEvent showTextEvent) {
        tipText.text = showTextEvent.tipText;
    }

    private void OnHideTipText(HideTipTextEvent hideTextEvent) {
        tipText.text = "";
    }

    private void OnDestroy() {
        VoidTalentScreenEvent.UnRegisterListener(OnVoidTalentScreen);
        InGameMenuEvent.UnRegisterListener(OnInGameMenuEvent);
        ShowTipTextEvent.UnRegisterListener(OnShowTipText);
        HideTipTextEvent.UnRegisterListener(OnHideTipText);
        PlayerTakeDamageEvent.UnRegisterListener(OnTakeDamage);
        PlayerHealEvent.UnRegisterListener(OnPlayerHealed);
        PlayerVoidIsFullEvent.UnRegisterListener(OnVoidMendFullEvent);
        PlayerCollectibleChangeEvent.UnRegisterListener(OnPlayerCollectibleChange);
    }
}
