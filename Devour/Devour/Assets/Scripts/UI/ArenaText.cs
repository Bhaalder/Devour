//Author: Marcus Söderberg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaText : MonoBehaviour
{
    [SerializeField] private GameObject ArenaTextGO;
    [SerializeField] private TextMeshProUGUI arenaEnemiesText;
    [SerializeField] private string arenaText;

    void Start()
    {
        ArenaEnemyDiedEvent.RegisterListener(OnArenaEnemyDiedEvent);
        ArenaTriggerEvent.RegisterListener(OnArenaTriggerEvent);
        arenaEnemiesText.text = arenaText + 0;
    }

    private void OnArenaEnemyDiedEvent(ArenaEnemyDiedEvent enemyDied)
    {
        arenaEnemiesText.text = arenaText + enemyDied.enemiesLeft;
    }

    private void OnArenaTriggerEvent(ArenaTriggerEvent arenaTrigger)
    {
        ArenaTextGO.SetActive(!ArenaTextGO.activeSelf);
    }

    private void OnDestroy()
    {
        ArenaEnemyDiedEvent.UnRegisterListener(OnArenaEnemyDiedEvent);
        ArenaTriggerEvent.UnRegisterListener(OnArenaTriggerEvent);
    }

}
