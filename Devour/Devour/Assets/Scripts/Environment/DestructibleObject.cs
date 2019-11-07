//Main Author: Marcus Söderberg
//Secondary Author: Patrik Ahlgren (destructibleID, en stor del i Start() och DestroyObject() då den kollar om den redan har förstörts annars adderar den till dictionaryn)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private bool isUsingStageDamagedHealth;
    [SerializeField] private bool isUsingStageHalfHealth;
    [SerializeField] private bool isUsingDestroyCooldown;
    [SerializeField] GameObject fullHealth;
    [SerializeField] GameObject damagedHealth;
    [SerializeField] GameObject halfHealth;
    [SerializeField] GameObject particles;
    [SerializeField] float destroyCooldown = 2f;
    [SerializeField] Animator anim;

    [SerializeField] bool isWall;
    [SerializeField] bool isFloor;

    private BoxCollider2D boxCollider2D;

    private bool isOnCooldown;

    private float originalHealth;
    private float currentCooldown;

    [SerializeField] private int destructibleID;

    private void Awake() {

    }

    void Start()
    {
        if (GameController.Instance.DestroyedDestructibles.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach(KeyValuePair<string, List<int>> destructible in GameController.Instance.DestroyedDestructibles) {
                if(destructible.Key == SceneManager.GetActiveScene().name) {
                    if (destructible.Value.Contains(destructibleID)) {
                        Destroy(gameObject);
                        return;
                    }
                }
            }          
        }
        PlayerAttackEvent.RegisterListener(TakeDamage);
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalHealth = health;
        currentCooldown = destroyCooldown;
        isOnCooldown = false;
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            DestroyCooldown();
        }
    }

    private void TakeDamage(PlayerAttackEvent attackEvent)
    {
        if (attackEvent.attackCollider.bounds.Intersects(boxCollider2D.bounds))
        {
            health -= attackEvent.damage;
            if (isWall)
            {
                anim.Play("DestructibleWall1");
            }
            if (isFloor)
            {
                anim.Play("floorbreak");
            }
            

            if (particles != null)
            {
                GameObject instantiatedParticle = Instantiate(particles, null);
                instantiatedParticle.transform.position = transform.position;
                AudioPlaySoundAtLocationEvent rockAttackSound = new AudioPlaySoundAtLocationEvent {
                    name = "HitRockWall",
                    isRandomPitch = true,
                    minPitch = 0.95f,
                    maxPitch = 1,
                    soundType = SoundType.SFX,
                    gameObject = instantiatedParticle
                };
                rockAttackSound.FireEvent();
            }

            if (health < originalHealth && health > originalHealth/2 && isUsingStageDamagedHealth)
            {
                fullHealth.SetActive(false);
                damagedHealth.SetActive(true);
                halfHealth.SetActive(false);
            }
            if (health <= originalHealth / 2 && isUsingStageHalfHealth)
            {
                fullHealth.SetActive(false);
                damagedHealth.SetActive(false);
                halfHealth.SetActive(true);
            }
            if (health <= 0)
            {
                if (isUsingDestroyCooldown)
                {
                    isOnCooldown = true;
                    //start destroy animation?
                }
                else
                {
                    DestroyObject();
                }
            }
        }
    }

    private void DestroyCooldown()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        DestroyObject();
    }

    private void DestroyObject()
    {
        if (particles != null)
        {
            GameObject instantiatedParticle = Instantiate(particles, null);
            instantiatedParticle.transform.position = transform.position;
            string[] breakRockWall = { "BreakRockWall1", "BreakRockWall1"};
            AudioPlayRandomSoundAtLocationEvent rockBreakSound = new AudioPlayRandomSoundAtLocationEvent {
                name = breakRockWall,
                isRandomPitch = true,
                minPitch = 0.95f,
                maxPitch = 1,
                soundType = SoundType.SFX,
                gameObject = instantiatedParticle
            };
            rockBreakSound.FireEvent();
        }
        if (GameController.Instance.DestroyedDestructibles.ContainsKey(SceneManager.GetActiveScene().name)) {
            foreach (KeyValuePair<string, List<int>> destructible in GameController.Instance.DestroyedDestructibles) {
                if (destructible.Key == SceneManager.GetActiveScene().name) {
                    if (destructible.Value.Contains(destructibleID)) {
                        Debug.LogWarning("A destructible with the same ID [" + destructibleID + "] has already been destroyed in this scene [" + SceneManager.GetActiveScene().name + "]");
                        Destroy(gameObject);
                        return;
                    }
                    destructible.Value.Add(destructibleID);
                }
            }
        } else {
            List<int> newDestructibleList = new List<int> {destructibleID};
            GameController.Instance.DestroyedDestructibles.Add(SceneManager.GetActiveScene().name, newDestructibleList);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PlayerAttackEvent.UnRegisterListener(TakeDamage);
    }
}
