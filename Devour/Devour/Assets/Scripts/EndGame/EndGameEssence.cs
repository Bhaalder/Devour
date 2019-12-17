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
    [SerializeField] private float timeBeforeEndParticles;
    [SerializeField] private float timeBetweenEndParticles;
    [SerializeField] private GameObject startLocation;
    [SerializeField] private GameObject endLocation;
    [SerializeField] private GameObject voidEssence;
    [SerializeField] private GameObject playerDummy;
    [SerializeField] private Vector3 endGameCameraOffset;
    [SerializeField] private NazroDeathState nazroDeathState;
    [SerializeField] private string endGameScene;
    [SerializeField] private GameObject gameEndVoid;
    [SerializeField] private GameObject gameEndVoidReverse;
    [SerializeField] private GameObject rightWall;

    private GameObject endGameEssence;
    private GameObject gameEndVoidParticle;
    private GameObject gameEndVoidReverseParticle;
    private bool bossDied;
    private bool deathSequenceDone;
    private bool endParticleCountdown;
    private bool endReverseParticleCountdown;
    private float countUp = 0f;
    private float deathSequnceTime;
    
    void Start()
    {
        bossDied = false;
        deathSequnceTime = nazroDeathState.DeathTime;
        BossDiedEvent.RegisterListener(OnBossDiedEvent);
        PlayerTookLastEssenceEvent.RegisterListener(OnPlayerTookLastEssenceEvent);
        if(GameController.Instance.KilledBosses.Contains(bossName))
        {
            OnBossIsAlreadyDead();
        }
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
                else
                {
                    countUp = 1;
                }
                return;
            }
            deathSequenceDone = false;
        }
        GameEndVoidParticle();
        GameEndVoidReverseParticle();
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
        endGameEssence.transform.position = startLocation.transform.position;
        deathSequenceDone = true;
        bossDied = false;
    }

    private void GameEndVoidParticle()
    {
        if (endParticleCountdown)
        {
            timeBeforeEndParticles -= Time.deltaTime;

            if (timeBeforeEndParticles > 0)
            {
                return;
            }

            gameEndVoidParticle = Instantiate(gameEndVoid, null);
            gameEndVoidParticle.transform.position = endLocation.transform.position;
            endParticleCountdown = false;
            endReverseParticleCountdown = true;
        }

    }

    private void GameEndVoidReverseParticle()
    {
        if (endReverseParticleCountdown)
        {
            timeBetweenEndParticles -= Time.deltaTime;

            if (timeBetweenEndParticles > 0)
            {
                return;
            }

            gameEndVoidReverseParticle = Instantiate(gameEndVoidReverse, null);
            gameEndVoidReverseParticle.transform.position = endLocation.transform.position;
            endReverseParticleCountdown = false;
            Invoke("endGameSceneFade", timeBeforeSceneSwitch);
        }

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
        AudioStopSoundEvent stopSound = new AudioStopSoundEvent
        {
            name = "LowHealth"
        };
        stopSound.FireEvent();

        endParticleCountdown = true;
    }

    private void OnBossIsAlreadyDead()
    {
        endGameEssence = Instantiate(voidEssence, null);
        endGameEssence.transform.position = endLocation.transform.position;
        rightWall.SetActive(false);
    }

    private void endGameSceneFade()
    {
        FadeScreenEvent fadeScreen = new FadeScreenEvent
        {
            isFadeOut = true,
            fadeSpeed = fadeSpeed
        };
        fadeScreen.FireEvent();
        SwitchSceneEvent switchScene = new SwitchSceneEvent
        {
            enteringSceneName = endGameScene,
            leavingSceneName = SceneManager.GetActiveScene().name
        };
        switchScene.FireEvent();

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
