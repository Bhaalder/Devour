//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour{

    private void Start() {
        if(GameController.Instance.KilledBosses.Count == 3) {
            Destroy(gameObject);
        }
    }
}
