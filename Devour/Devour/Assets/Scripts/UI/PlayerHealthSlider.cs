﻿//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour{

    private Slider healthSlider;

    private void Awake() {
        PlayerTakeDamageEvent.RegisterListener(TakeDamage);
        PlayerHealEvent.RegisterListener(GainHealth);
        PlayerTouchKillzoneEvent.RegisterListener(TouchKillzone);
        TalentPointGainEvent.RegisterListener(OnTalentPointGain);
        healthSlider = GetComponent<Slider>();
    }

    private void Start() {
        healthSlider.maxValue = GameController.Instance.Player.MaxHealth;
        healthSlider.value = GameController.Instance.Player.Health;
    }

    private void TouchKillzone(PlayerTouchKillzoneEvent touchKillzoneEvent) {
        InvokeSliderChange();
        //if (!GameController.Instance.Player.IsInvulnerable) {
        //    //Invoke("ChangeSlider", 0.01f);
        //    ChangeSliderValue(touchKillzoneEvent.damage);
        //}
    }

    private void TakeDamage(PlayerTakeDamageEvent takeDamageEvent) {
        InvokeSliderChange();
        //if (takeDamageEvent.isSelfInflicted) {
        //    Invoke("ChangeSlider", 0.01f);
        //    //ChangeSlider(takeDamageEvent.damage);
        //    return;
        //}
        //if (!GameController.Instance.Player.IsInvulnerable) {
        //    Invoke("ChangeSlider", 0.01f);
        //    //ChangeSlider(takeDamageEvent.damage);
        //}     
    }

    private void GainHealth(PlayerHealEvent healEvent) {
        InvokeSliderChange();
        //if (healEvent.isLifeLeech) {
        //    healEvent.amount = GameController.Instance.Player.MeleeLifeLeech;
        //}
        //ChangeSlider(-healEvent.amount);
    }

    private void InvokeSliderChange() {
        Invoke("ChangeSlider", 0.01f);
    }

    private void ChangeSlider() {
        healthSlider.value = GameController.Instance.Player.Health;
        //healthSlider.value -= amount;
    }



    private void OnTalentPointGain(TalentPointGainEvent pointGainEvent) {
        if(pointGainEvent.talentPoint.talentPointType == TalentPointType.SURVIVAL) {
            healthSlider.maxValue += pointGainEvent.talentPoint.variablesToChange[0].amount;
        }
    }

    private void OnDestroy() {
        PlayerTakeDamageEvent.UnRegisterListener(TakeDamage);
        PlayerHealEvent.UnRegisterListener(GainHealth);
        PlayerTouchKillzoneEvent.UnRegisterListener(TouchKillzone);
        TalentPointGainEvent.UnRegisterListener(OnTalentPointGain);
    }

}
