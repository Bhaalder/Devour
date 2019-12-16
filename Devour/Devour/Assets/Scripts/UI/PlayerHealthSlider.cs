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
        TalentPointGainEvent.RegisterListener(OnTalentPointGain);
        healthSlider = GetComponent<Slider>();
    }

    private void Start() {
        healthSlider.maxValue = GameController.Instance.Player.MaxHealth;
        healthSlider.value = GameController.Instance.Player.Health;
    }

    private void TouchKillzone(PlayerTouchKillzoneEvent touchKillzoneEvent) {
        if (!GameController.Instance.Player.IsInvulnerable) {
            Invoke("ChangeSlider", 0.01f);
            //ChangeSlider(touchKillzoneEvent.damage);
        }
    }

    private void TakeDamage(PlayerTakeDamageEvent takeDamageEvent) {
        if (takeDamageEvent.isSelfInflicted) {
            Invoke("ChangeSlider", 0.01f);
            //ChangeSlider(takeDamageEvent.damage);
            return;
        }
        if (!GameController.Instance.Player.IsInvulnerable) {
            Invoke("ChangeSlider", 0.01f);
            //ChangeSlider(takeDamageEvent.damage);
        }     
    }

    private void ChangeSlider() {
        healthSlider.value = GameController.Instance.Player.Health;
        //healthSlider.value -= amount;
    }

    private void GainHealth(PlayerHealEvent healEvent) {
        if (healEvent.isLifeLeech) {
            healEvent.amount = GameController.Instance.Player.MeleeLifeLeech;
        }
        Invoke("ChangeSlider", 0.01f);
        //ChangeSlider(-healEvent.amount);
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
