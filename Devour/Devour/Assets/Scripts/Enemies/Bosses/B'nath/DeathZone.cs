//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerTouchKillzoneEvent playerTouchKillzone = new PlayerTouchKillzoneEvent
            {
                damage = 5000
            };
            playerTouchKillzone.FireEvent();
        }
        if (collision.gameObject.tag == "Enemy") {
            PlayerTouchKillzoneEvent playerTouchKillzone = new PlayerTouchKillzoneEvent {
                damage = 5000
            };
            playerTouchKillzone.FireEvent();
        }
    }
}
