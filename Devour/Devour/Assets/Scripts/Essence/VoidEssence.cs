//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidEssence : MonoBehaviour {

    [SerializeField] private int voidEssenceID;

    private void Start() {
        if (GameController.Instance.CollectedVoidEssences.ContainsKey(SceneManager.GetActiveScene().name)) {
            if (GameController.Instance.CollectedVoidEssences[SceneManager.GetActiveScene().name].Contains(voidEssenceID)) {
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Collectible voidEssence = new Collectible(CollectibleType.VOIDESSENCE, 1);
            PlayerCollectibleChange gainCollectibleEvent = new PlayerCollectibleChange {
                collectible = voidEssence
            };
            if (GameController.Instance.CollectedVoidEssences.ContainsKey(SceneManager.GetActiveScene().name)) {
                if (GameController.Instance.CollectedVoidEssences[SceneManager.GetActiveScene().name].Contains(voidEssenceID)) {
                    Debug.LogWarning("A voidessence with the same ID [" + voidEssenceID + "] has already been collected in this scene [" + SceneManager.GetActiveScene().name + "]");
                    Destroy(gameObject);
                    return;
                }
                GameController.Instance.CollectedVoidEssences[SceneManager.GetActiveScene().name].Add(voidEssenceID);
            } else {
                List<int> newEssenceList = new List<int> { voidEssenceID };
                GameController.Instance.CollectedVoidEssences.Add(SceneManager.GetActiveScene().name, newEssenceList);
            }
            gainCollectibleEvent.FireEvent();
            Destroy(gameObject);
        }
    }
}
