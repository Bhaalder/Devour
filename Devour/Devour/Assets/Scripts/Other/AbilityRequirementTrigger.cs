using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRequirementTrigger : MonoBehaviour{

    [SerializeField] private PlayerAbility abilityRequired;

    private void Start() {
        if (!GameController.Instance.Player.HasAbility(abilityRequired)) {
            gameObject.SetActive(false);
        }
    }

}
