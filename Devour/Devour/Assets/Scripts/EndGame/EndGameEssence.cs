using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameEssence : MonoBehaviour
{
    [SerializeField] private string bossName;
    [SerializeField] private float count;
    [SerializeField] private float timeBeforeSceneSwitch;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private GameObject startLocation;
    [SerializeField] private GameObject endLocation;
    [SerializeField] private GameObject voidEssence;
    [SerializeField] private GameObject playerDummy;
    [SerializeField] private Vector3 endGameCameraOffset;
    [SerializeField] private NazroDeathState nazroDeathState;
    [SerializeField] private string endGameScene;

    private GameObject endGameEssence;
    private bool bossDied;
    private bool deathSequenceDone;
    private float countUp = 0f;
    private float deathSequnceTime;
    
    void Start()
    {
        bossDied = false;
        deathSequnceTime = nazroDeathState.DeathTime;
        BossDiedEvent.RegisterListener(OnBossDiedEvent);
        PlayerTookLastEssenceEvent.RegisterListener(OnPlayerTookLastEssenceEvent);
    }

    void Update()
    {
        if (bossDied)
        {
            bossDiedDeathsequenceCountdown();
        }

        if (deathSequenceDone)
        {
            countUp += count * Time.deltaTime;
            if (countUp < 1)
            {
                if(endGameEssence != null)
                {
                    endGameEssence.transform.position = Vector3.Lerp(startLocation.transform.position, endLocation.transform.position, countUp);
                }
            }
        }
    }

    private void OnBossDiedEvent(BossDiedEvent bossDiedEvent)
    {
        if(bossDiedEvent.boss.BossName == bossName)
        {
            bossDied = true;
        }
    }

    private void bossDiedDeathsequenceCountdown()
    {
        deathSequnceTime -= Time.deltaTime;

        if(deathSequnceTime > 0)
        {
            return;
        }

        endGameEssence = Instantiate(voidEssence, null);
        voidEssence.transform.position = startLocation.transform.position;
        deathSequenceDone = true;
        bossDied = false;
    }

    private void OnPlayerTookLastEssenceEvent(PlayerTookLastEssenceEvent lastEssenceTaken)
    {
        Destroy(GameController.Instance.Player.gameObject);
        playerDummy.SetActive(true);
        CameraChangeTargetEvent cameraTarget = new CameraChangeTargetEvent
        {
            newTarget = playerDummy.transform
        };
        cameraTarget.FireEvent();
        CameraOffsetEvent cameraOffset = new CameraOffsetEvent
        {
            newOffset = endGameCameraOffset,
            setBoundsInactive = true
        };
        cameraOffset.FireEvent();

        Invoke("endGameSceneFade", timeBeforeSceneSwitch);
    }

    private void endGameSceneFade()
    {
        FadeScreenEvent fadeScreen = new FadeScreenEvent
        {
            isFadeOut = true,
            fadeSpeed = fadeSpeed
        };
        fadeScreen.FireEvent();

        Invoke("loadScene", 1.5f);
    }

    private void loadScene()
    {
        SceneManager.LoadScene(endGameScene);
    }

    private void OnDestroy()
    {
        BossDiedEvent.UnRegisterListener(OnBossDiedEvent);
        PlayerTookLastEssenceEvent.UnRegisterListener(OnPlayerTookLastEssenceEvent);
    }
}
