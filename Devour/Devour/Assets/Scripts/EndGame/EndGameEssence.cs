using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEssence : MonoBehaviour
{
    [SerializeField] private string bossName;
    [SerializeField] private float count;
    [SerializeField] private GameObject startLocation;
    [SerializeField] private GameObject endLocation;
    [SerializeField] private GameObject voidEssence;

    private bool bossDied;
    private float countUp = 0f;
    
    void Start()
    {
        bossDied = false;
        BossDiedEvent.RegisterListener(OnBossDiedEvent);
    }

    void Update()
    {
        if (bossDied)
        {
            countUp += count * Time.deltaTime;
            if (countUp < 1)
            {
                voidEssence.transform.position = Vector3.Lerp(startLocation.transform.position, endLocation.transform.position, countUp);
            }
        }
    }

    private void OnBossDiedEvent(BossDiedEvent bossDiedEvent)
    {
        if(bossDiedEvent.boss.BossName == bossName)
        {
            voidEssence = Instantiate(voidEssence, null);
            voidEssence.transform.position = startLocation.transform.position;
            bossDied = true;
        }
    }

    private void OnDestroy()
    {
        BossDiedEvent.UnRegisterListener(OnBossDiedEvent);
    }
}
