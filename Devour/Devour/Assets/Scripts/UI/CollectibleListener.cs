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
        PlayerGainCollectibleEvent.RegisterListener(OnPlayerGainCollectible);
    }

    private void OnPlayerGainCollectible(PlayerGainCollectibleEvent collectibleEvent) {
        CheckCollectible(collectibleEvent.collectible);
    }

    private void CheckCollectible(Collectible collectible) {
        switch (collectible.CollectibleType) {
            case CollectibleType.VOIDESSENCE:
                voidEssenceCount += collectible.Amount;
                voidEssenceText.text = voidEssenceCount + "";
                break;
            case CollectibleType.LIFEFORCE:
                lifeforceCount += collectible.Amount;
                lifeforceText.text = lifeforceCount + "";
                break;
        } 
    }

    private void OnDestroy() {
        PlayerGainCollectibleEvent.UnRegisterListener(OnPlayerGainCollectible);
    }

}
