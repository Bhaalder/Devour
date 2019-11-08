//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidEssence : MonoBehaviour {

    [SerializeField] private int voidEssenceID;

    private void Start() {
        if (GameController.Instance.CollectedVoidEssences.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> essence in GameController.Instance.CollectedVoidEssences) {
                if (essence.Key == SceneManager.GetActiveScene().name) {
                    if (essence.Value.Contains(voidEssenceID)) {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Collectible voidEssence = new Collectible(CollectibleType.VOIDESSENCE, 1);
            PlayerGainCollectibleEvent gainCollectibleEvent = new PlayerGainCollectibleEvent {
                collectible = voidEssence
            };
            if (GameController.Instance.CollectedVoidEssences.ContainsKey(SceneManager.GetActiveScene().name)) {
                foreach (KeyValuePair<string, List<int>> essence in GameController.Instance.CollectedVoidEssences) {
                    if (essence.Key == SceneManager.GetActiveScene().name) {
                        if (essence.Value.Contains(voidEssenceID)) {
                            Debug.LogWarning("A voidessence with the same ID [" + voidEssenceID + "] has already been collected in this scene [" + SceneManager.GetActiveScene().name + "]");
                            Destroy(gameObject);
                            return;
                        }
                        essence.Value.Add(voidEssenceID);
                    }
                }
            } else {
                List<int> newEssenceList = new List<int> { voidEssenceID };
                GameController.Instance.DestroyedDestructibles.Add(SceneManager.GetActiveScene().name, newEssenceList);
            }
            gainCollectibleEvent.FireEvent();
            Destroy(gameObject);
        }
    }
}
