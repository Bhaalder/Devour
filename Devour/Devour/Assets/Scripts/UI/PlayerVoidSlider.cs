//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoidSlider : MonoBehaviour {

    private Slider voidSlider;

    private void Awake() {
        PlayerVoidEvent.RegisterListener(VoidEvent);
        PlayerGetAbilityEvent.RegisterListener(GetVoidMend);
        TalentPointGainEvent.RegisterListener(OnTalentPointGain);
    }

    private void Start() {
        voidSlider = GetComponent<Slider>();
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

    private void VoidEvent(PlayerVoidEvent voidEvent) {
        ChangeSlider(voidEvent.amount);
    }

    private void ChangeSlider(float amount) {
        voidSlider.value += amount;
    }

    private void OnTalentPointGain(TalentPointGainEvent pointGainEvent) {
        if (pointGainEvent.talentPoint.talentPointType == TalentPointType.VOID) {
            voidSlider.maxValue += pointGainEvent.talentPoint.variablesToChange[0].amount;
        }
    }

    private void OnDestroy() {
        PlayerVoidEvent.UnRegisterListener(VoidEvent);
        PlayerGetAbilityEvent.UnRegisterListener(GetVoidMend);
        TalentPointGainEvent.UnRegisterListener(OnTalentPointGain);
    }

}
