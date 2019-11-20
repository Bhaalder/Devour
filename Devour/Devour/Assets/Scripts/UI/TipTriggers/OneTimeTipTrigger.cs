using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneTimeTipTrigger : TipTrigger {

    [SerializeField] private int tipID;
    [SerializeField] private float tipDuration;

    private void Start() {
        if (GameController.Instance.OneTimeTips.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> essence in GameController.Instance.OneTimeTips) {
                if (essence.Key == SceneManager.GetActiveScene().name) {
                    if (essence.Value.Contains(tipID)) {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (GameController.Instance.OneTimeTips.ContainsKey(SceneManager.GetActiveScene().name)) {
                foreach (KeyValuePair<string, List<int>> tip in GameController.Instance.OneTimeTips) {
                    if (tip.Key == SceneManager.GetActiveScene().name) {
                        if (tip.Value.Contains(tipID)) {
                            Debug.LogWarning("A tip with the same ID [" + tipID + "] has already been seen in this scene [" + SceneManager.GetActiveScene().name + "]");
                            Destroy(gameObject);
                            return;
                        }
                        tip.Value.Add(tipID);
                    }
                }
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

    private IEnumerator HideTooltip() {
        yield return new WaitForSecondsRealtime(tipDuration);
        HideTipTextEvent hideTextEvent = new HideTipTextEvent { };
        hideTextEvent.FireEvent();
        Destroy(gameObject);
    }

}
