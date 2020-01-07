using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Marcus Söderberg
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
            GameController.Instance.Player.transform.position = new Vector3(0, -5000, 0);

            Destroy(gameObject);
        }
    }
}
