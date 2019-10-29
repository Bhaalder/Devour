using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour{

    private Slider healthSlider;
    private float playerMaxHealth;
    private float playerHealth;

    private void Awake() {
        PlayerTakeDamageEvent.RegisterListener(TakeDamage);
        PlayerHealEvent.RegisterListener(GainHealth);
    }

    private void Start() {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = GameController.Instance.Player.MaxHealth;
        Debug.Log(GameController.Instance.Player.MaxHealth);
        healthSlider.value = GameController.Instance.Player.MaxHealth;
    }

    private void TakeDamage(PlayerTakeDamageEvent takeDamageEvent) {
        if (takeDamageEvent.isSelfInflicted) {
            healthSlider.value -= takeDamageEvent.damage;
            return;
        }
        if (!GameController.Instance.Player.IsInvulnerable) {
            healthSlider.value -= takeDamageEvent.damage;
        }     
    }

    private void GainHealth(PlayerHealEvent healEvent) {
        if (healEvent.isLifeLeech) {
            healEvent.amount = GameController.Instance.Player.MeleeLifeLeech;
        }
        healthSlider.value += healEvent.amount;
    }

    private void OnDestroy() {
        PlayerTakeDamageEvent.UnRegisterListener(TakeDamage);
        PlayerHealEvent.UnRegisterListener(GainHealth);
    }

}
