using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData) {

    }

}
