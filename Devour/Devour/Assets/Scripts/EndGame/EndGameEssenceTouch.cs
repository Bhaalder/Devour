using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEssenceTouch : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioPlaySoundEvent voidEssenceGainSound = new AudioPlaySoundEvent
            {
                name = "GainVoidEssence",
                isRandomPitch = false,
                soundType = SoundType.SFX
            };
            PlayerBusyEvent playerBusy = new PlayerBusyEvent
            {
                playerIsBusy = true
            };
            PlayerTookLastEssenceEvent endGame = new PlayerTookLastEssenceEvent { };
            playerBusy.FireEvent();
            voidEssenceGainSound.FireEvent();
            endGame.FireEvent();
            Destroy(gameObject);
        }
    }
}
