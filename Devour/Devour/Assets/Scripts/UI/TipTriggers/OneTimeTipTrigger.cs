using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneTimeTipTrigger : TipTrigger {

    [SerializeField] private int tipID;
    [SerializeField] private float tipDuration;

    private void Start() {
        if (GameController.Instance.OneTimeTips.ContainsKey(SceneManager.GetActiveScene().name)) {
            if (GameController.Instance.OneTimeTips[SceneManager.GetActiveScene().name].Contains(tipID)) {
                Destroy(gameObject);
                return;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (GameController.Instance.OneTimeTips.ContainsKey(SceneManager.GetActiveScene().name)) {
                if (GameController.Instance.OneTimeTips[SceneManager.GetActiveScene().name].Contains(tipID)) {
                    Debug.LogWarning("A tip with the same ID [" + tipID + "] has already been seen in this scene [" + SceneManager.GetActiveScene().name + "]");
                    Destroy(gameObject);
                    return;
                }
                GameController.Instance.OneTimeTips[SceneManager.GetActiveScene().name].Add(tipID);
            } else {
                List<int> newTipList = new List<int> { tipID };
                GameController.Instance.OneTimeTips.Add(SceneManager.GetActiveScene().name, newTipList);
            }
            ShowTipTextEvent showTextEvent = new ShowTipTextEvent {
                tipText = tipText,
            };
            showTextEvent.FireEvent();
            StartCoroutine(HideTooltip());
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision) {

    }

    private IEnumerator HideTooltip() {
        yield return new WaitForSecondsRealtime(tipDuration);
        HideTipTextEvent hideTextEvent = new HideTipTextEvent { };
        hideTextEvent.FireEvent();
        Destroy(gameObject);
    }

}
