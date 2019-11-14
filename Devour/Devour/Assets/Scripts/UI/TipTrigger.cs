using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTrigger : MonoBehaviour{

    [TextArea(0, 5)]
    [SerializeField] private string tipText;
    [SerializeField] private float tipDuration;
    [SerializeField] private bool isOneTimeTip;

    [SerializeField] private bool isWithinBoundsTip;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if (isWithinBoundsTip) {
                ShowTipTextEvent withinBoundsShowTextEvent = new ShowTipTextEvent {
                    tipText = tipText
                };
                withinBoundsShowTextEvent.FireEvent();
            } else {
                ShowTipTextEvent showTextEvent = new ShowTipTextEvent {
                    tipText = tipText,
                    tipDuration = tipDuration,
                    isOneTimeTip = isOneTimeTip
                };
                showTextEvent.FireEvent();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (isWithinBoundsTip) {
                HideTipTextEvent hideTextEvent = new HideTipTextEvent { };
                hideTextEvent.FireEvent();
            }
        }
    }

}
