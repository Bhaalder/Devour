//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEssence : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Collectible voidEssence = new Collectible(CollectibleType.VOIDESSENCE, 1);
            PlayerGainCollectibleEvent gainCollectibleEvent = new PlayerGainCollectibleEvent {
                collectible = voidEssence
            };
            gainCollectibleEvent.FireEvent();
            Destroy(gameObject);
        }
    }
}
