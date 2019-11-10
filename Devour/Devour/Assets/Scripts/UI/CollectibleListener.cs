//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectibleListener : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI voidEssenceText;
    [SerializeField] private TextMeshProUGUI lifeforceText;

    private int voidEssenceCount;
    private int lifeforceCount;

    private void Start() {
        foreach (Collectible collectible in GameController.Instance.Player.Collectibles) {
            CheckCollectible(collectible);
        }
        voidEssenceText.text = voidEssenceCount + "";
        lifeforceText.text = lifeforceCount + "";
        PlayerCollectibleChange.RegisterListener(OnPlayerGainCollectible);
    }

    private void OnPlayerGainCollectible(PlayerCollectibleChange collectibleEvent) {
        CheckCollectible(collectibleEvent.collectible);
    }

    private void CheckCollectible(Collectible collectible) {
        switch (collectible.collectibleType) {
            case CollectibleType.VOIDESSENCE:
                voidEssenceCount += collectible.amount;
                voidEssenceText.text = voidEssenceCount + "";
                break;
            case CollectibleType.LIFEFORCE:
                lifeforceCount += collectible.amount;
                lifeforceText.text = lifeforceCount + "";
                break;
        } 
    }

    private void OnDestroy() {
        PlayerCollectibleChange.UnRegisterListener(OnPlayerGainCollectible);
    }

}
