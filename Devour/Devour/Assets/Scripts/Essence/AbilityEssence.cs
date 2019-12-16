//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEssence : MonoBehaviour{

    public PlayerAbility Ability { get; set; }
    public string BossNameIsDead { get; set; }

    [SerializeField] private PlayerAbility ability;
    [Tooltip("If the boss with this name is dead (and the player does not have the ability) this object will stay")]
    [SerializeField] private string bossNameIsDead;

    private float timer;
    private bool hasChecked;

    private void Awake() {
        if (ability > 0) {
            Ability = ability;
        }
        if (bossNameIsDead != null || bossNameIsDead != "") {
            BossNameIsDead = bossNameIsDead;
        }
    }
    private void Start() {
        if (GameController.Instance.Player.HasAbility(Ability)) {
            Destroy(gameObject);
        }
        if (!GameController.Instance.KilledBosses.Contains(BossNameIsDead)) {
            Destroy(gameObject);
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
