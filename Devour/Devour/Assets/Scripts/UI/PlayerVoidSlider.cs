//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoidSlider : MonoBehaviour {

    private Slider voidSlider;

    private void Awake() {
        PlayerVoidChangeEvent.RegisterListener(VoidEvent);
        PlayerGetAbilityEvent.RegisterListener(GetVoidMend);
        TalentPointGainEvent.RegisterListener(OnTalentPointGain);
        voidSlider = GetComponent<Slider>();
    }

    private void Start() {
        if (GameController.Instance.Player.HasAbility(PlayerAbility.VOIDMEND)) {
            voidSlider.maxValue = GameController.Instance.Player.MaxPlayerVoid;
            voidSlider.value = 0;
        } else {
            gameObject.SetActive(false);
        }      
    }

    private void GetVoidMend(PlayerGetAbilityEvent abilityEvent) {
        if(abilityEvent.playerAbility == PlayerAbility.VOIDMEND) {
            gameObject.SetActive(true);
        }
    }

    private void VoidEvent(PlayerVoidChangeEvent voidEvent) {
        ChangeSlider(voidEvent.amount);
    }

    private void ChangeSlider(float amount) {
        voidSlider.value += amount;
        voidSlider.value = GameController.Instance.Player.PlayerVoid;
    }

    private void OnTalentPointGain(TalentPointGainEvent pointGainEvent) {
        if (pointGainEvent.talentPoint.talentPointType == TalentPointType.VOID) {
            voidSlider.maxValue += pointGainEvent.talentPoint.variablesToChange[0].amount;
        }
    }

    private void OnDestroy() {
        PlayerVoidChangeEvent.UnRegisterListener(VoidEvent);
        PlayerGetAbilityEvent.UnRegisterListener(GetVoidMend);
        TalentPointGainEvent.UnRegisterListener(OnTalentPointGain);
    }

}
