using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoidSlider : MonoBehaviour {

    private Slider voidSlider;

    private void Awake() {
        PlayerVoidEvent.RegisterListener(VoidEvent);
    }

    private void Start() {
        voidSlider = GetComponent<Slider>();
        voidSlider.maxValue = GameController.Instance.Player.MaxPlayerVoid;
        voidSlider.value = GameController.Instance.Player.MaxPlayerVoid;
    }

    private void VoidEvent(PlayerVoidEvent voidEvent) {
        ChangeSlider(voidEvent.amount);
    }

    private void ChangeSlider(float amount) {
        voidSlider.value += amount;
    }

    private void OnDestroy() {
        PlayerVoidEvent.UnRegisterListener(VoidEvent);
    }

}
