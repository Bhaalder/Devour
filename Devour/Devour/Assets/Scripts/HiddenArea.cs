using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HiddenArea : MonoBehaviour {

    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private string soundName;
    [SerializeField] private bool playSound;
    [SerializeField] private int hiddenAreaID;
    private bool isDisabled;

    private void Start() {
        if (GameController.Instance.HiddenAreasFound.ContainsKey(SceneManager.GetActiveScene().name)) {
            if (GameController.Instance.HiddenAreasFound[SceneManager.GetActiveScene().name].Contains(hiddenAreaID)) {
                SetActiveAreas(false);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isDisabled) {
            if (GameController.Instance.HiddenAreasFound.ContainsKey(SceneManager.GetActiveScene().name)) {
                if (GameController.Instance.HiddenAreasFound[SceneManager.GetActiveScene().name].Contains(hiddenAreaID)) {
                    Debug.LogWarning("A hiddenArea with the same ID [" + hiddenAreaID + "] has already been found in this scene [" + SceneManager.GetActiveScene().name + "]");
                    SetActiveAreas(false);
                    return;
                }
                GameController.Instance.HiddenAreasFound[SceneManager.GetActiveScene().name].Add(hiddenAreaID);
            } else {
                List<int> newHiddenAreaList = new List<int> { hiddenAreaID };
                GameController.Instance.HiddenAreasFound.Add(SceneManager.GetActiveScene().name, newHiddenAreaList);
            }
            if(soundName == "") {

            } else {
                AudioPlaySoundEvent playAreaFoundSound = new AudioPlaySoundEvent {
                    name = soundName,
                    soundType = SoundType.DEFAULT,
                    isRandomPitch = false
                };
                playAreaFoundSound.FireEvent();
            }
            SetActiveAreas(false);
        }

    }
    private void SetActiveAreas(bool b) {
        for (int i = 0; i < gameObjects.Length; i++) {
            gameObjects[i].SetActive(b);
        }
        isDisabled = b;
    }
    //private void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.CompareTag("Player") && !isPermanent) {
    //        SetActiveAreas(true);
    //    }
    //}
}
