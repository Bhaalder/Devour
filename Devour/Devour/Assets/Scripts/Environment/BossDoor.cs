//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour{

    private void Start() {
        if(GameController.Instance.KilledBosses.Count >= 3) {
            if (!GameController.Instance.BarrierCutsceneHasPlayed) {
                Invoke("Destroy", 3);
                return;
            }
            Destroy(gameObject);
        }
    }

    private void Destroy() {
        Destroy(gameObject);
    }

}
