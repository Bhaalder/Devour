using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTrigger : MonoBehaviour{

    [TextArea(0, 5)]
    [SerializeField] protected string tipText;

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            ShowTipTextEvent withinBoundsShowTextEvent = new ShowTipTextEvent {
                tipText = tipText
            };
            withinBoundsShowTextEvent.FireEvent();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            HideTipTextEvent hideTextEvent = new HideTipTextEvent { };
            hideTextEvent.FireEvent();
        }
    }

}
