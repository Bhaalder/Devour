using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEssence : MonoBehaviour{

    public PlayerAbility Ability { get; set; }

    [SerializeField] private PlayerAbility ability;

    private void Awake() {
        if (ability > 0) {
            Ability = ability;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerGetAbilityEvent getAbility = new PlayerGetAbilityEvent {
                playerAbility = Ability
            };
            getAbility.FireEvent();
            Destroy(gameObject);
        }
    }
}
