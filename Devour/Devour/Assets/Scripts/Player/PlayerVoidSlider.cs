﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoidSlider : MonoBehaviour {

    private Slider voidSlider;
    private float playerMaxVoid;
    private float playerVoid;

    private void Awake() {
        
    }

    private void Start() {
        voidSlider = GetComponent<Slider>();
        voidSlider.maxValue = GameController.Instance.Player.MaxHealth;
        voidSlider.value = GameController.Instance.Player.MaxHealth;
    }

    private void TouchKillzone(PlayerTouchKillzoneEvent touchKillzoneEvent) {
        ChangeSlider(touchKillzoneEvent.damage);
    }

    private void TakeDamage(PlayerTakeDamageEvent takeDamageEvent) {
        if (takeDamageEvent.isSelfInflicted) {
            ChangeSlider(takeDamageEvent.damage);
            //healthSlider.value -= takeDamageEvent.damage;
            return;
        }
        if (!GameController.Instance.Player.IsInvulnerable) {
            ChangeSlider(takeDamageEvent.damage);
            //healthSlider.value -= takeDamageEvent.damage;
        }
    }

    private void ChangeSlider(float amount) {
        voidSlider.value -= amount;
    }

    private void GainHealth(PlayerHealEvent healEvent) {
        if (healEvent.isLifeLeech) {
            healEvent.amount = GameController.Instance.Player.MeleeLifeLeech;
        }
        ChangeSlider(-healEvent.amount);
        //healthSlider.value += healEvent.amount;
    }

    private void OnDestroy() {
        PlayerTakeDamageEvent.UnRegisterListener(TakeDamage);
        PlayerHealEvent.UnRegisterListener(GainHealth);
        PlayerTouchKillzoneEvent.UnRegisterListener(TouchKillzone);
    }

}
