using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectRandomStart : MonoBehaviour {

    [SerializeField] private float minTimeBeforeActive;
    [SerializeField] private float maxTimeBeforeActive;

    private void Awake() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Start() {
        Invoke("ActivateEyes", Random.Range(minTimeBeforeActive, maxTimeBeforeActive + 1));
    }

    private void ActivateEyes() {
        GetComponent<Animator>().SetTrigger("");
    }
}
