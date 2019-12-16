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

    [Header("Infotext that pops up")]
    [TextArea(0, 5)]
    [SerializeField] private string dashInfo;
    [TextArea(0, 5)]
    [SerializeField] private string doubleJumpInfo;
    [TextArea(0, 5)]
    [SerializeField] private string projectileInfo;
    [TextArea(0, 5)]
    [SerializeField] private string wallclimbInfo;
    [TextArea(0, 5)]
    [SerializeField] private string voidMendInfo;

    private void Awake() {
        PlayerGetAbilityEvent.RegisterListener(OnGetAbility);
    }

    private void OnGetAbility(PlayerGetAbilityEvent abilityEvent) {
        textAnimator.SetTrigger("GainAbility");
        abilityNameText.text = abilityEvent.playerAbility.ToString();
        string description = "";
        switch (abilityEvent.playerAbility) {
            case PlayerAbility.DASH:
                description = dashInfo;
                imageAnimator.SetTrigger("Dash");
                break;
            case PlayerAbility.DOUBLEJUMP:
                description = doubleJumpInfo;
                imageAnimator.SetTrigger("DoubleJump");
                break;
            case PlayerAbility.PROJECTILE:
                description = projectileInfo;
                imageAnimator.SetTrigger("Projectile");
                break;
            case PlayerAbility.WALLCLIMB:
                description = wallclimbInfo;
                imageAnimator.SetTrigger("WallJump");
                break;
            case PlayerAbility.VOIDMEND:
                description = voidMendInfo;
                imageAnimator.SetTrigger("VoidMend");
                break;
        }
        descriptionText.text = description;
    }


    private void OnDestroy() {
        PlayerGetAbilityEvent.UnRegisterListener(OnGetAbility);
    }

}
