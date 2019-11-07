//Author: Patrik Ahlgren
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
    }

    private void Start() {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = GameController.Instance.Player.MaxHealth;
        healthSlider.value = GameController.Instance.Player.MaxHealth;
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
        healthSlider.value -= amount;
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
