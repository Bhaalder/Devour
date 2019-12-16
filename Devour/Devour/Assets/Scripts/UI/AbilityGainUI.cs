//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityGainUI : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI gainNewAbilityText;
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator imageAnimator;

    private void Awake() {
        PlayerGetAbilityEvent.RegisterListener(OnGetAbility);
    }

    private void OnGetAbility(PlayerGetAbilityEvent abilityEvent) {
        textAnimator.SetTrigger("GainAbility");
        abilityNameText.text = abilityEvent.playerAbility.ToString();
        string description = "";
        switch (abilityEvent.playerAbility) {
            case PlayerAbility.DASH:
                description = "Press [LEFT SHIFT] or [RT] to dash horizontally";
                imageAnimator.SetTrigger("Dash");
                break;
            case PlayerAbility.DOUBLEJUMP:
                description = "Press [SPACE] or [A] while in the air to jump again";
                imageAnimator.SetTrigger("DoubleJump");
                break;
            case PlayerAbility.PROJECTILE:
                description = "Hold [RIGHT MOUSEBUTTON] or [B] and aim with the directional input to fire a void projectile";
                imageAnimator.SetTrigger("Projectile");
                break;
            case PlayerAbility.WALLCLIMB:
                description = "Slide against walls and press [SPACE] or [A] to \n jump from walls";
                imageAnimator.SetTrigger("WallJump");
                break;
            case PlayerAbility.VOIDMEND:
                description = "Attack to fill your voidbar and press [F] or [Y] to heal \n a portion of your damage taken";
                imageAnimator.SetTrigger("VoidMend");
                break;
        }
        descriptionText.text = description;
    }


    private void OnDestroy() {
        PlayerGetAbilityEvent.UnRegisterListener(OnGetAbility);
    }

}
