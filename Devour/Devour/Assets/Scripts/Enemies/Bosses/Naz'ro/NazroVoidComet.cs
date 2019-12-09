//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidComet : MonoBehaviour {

    public float Damage { get; set; }
    public float Speed { get; set; }
    public float WindUp { get; set; }
    public Transform StartPosition { get; set; }

    [SerializeField] private GameObject warningParticlePrefab;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float lifeSpan;
    [SerializeField] private bool isVerticalComet;
    [SerializeField] private bool isPlatformingComet;

    private float windUpLeft;
    private float lastKnownPlayerYPos;
    private bool isMoving;
    
    private GameObject particleSystem;
    private Transform player;
    private GameObject warningParticle;
    private CircleCollider2D circleCollider2D;   

    private void Start() {
        particleSystem = transform.GetChild(0).gameObject;
        player = GameController.Instance.Player.transform;
        circleCollider2D = GetComponent<CircleCollider2D>();
        if (isVerticalComet) {
            transform.position = new Vector3(player.position.x, StartPosition.position.y);
        } else {
            transform.position = new Vector3(StartPosition.position.x, player.position.y);
        }
        warningParticle = Instantiate(warningParticlePrefab, transform.localPosition, Quaternion.identity);
        windUpLeft = WindUp;
        PlayerAttackEvent.RegisterListener(GetHit);
        PlayerTouchKillzoneEvent.RegisterListener(OnPlayerTouchKillzone);
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void Update() {
        if (windUpLeft > 0 && !isMoving) {
            windUpLeft -= Time.deltaTime;
            if (isPlatformingComet) {
                transform.position = PlatformCometPosition();
                warningParticle.transform.position = transform.position;
            }
            return;
        }
        isMoving = true;
        if (!particleSystem.activeSelf) {
            particleSystem.SetActive(true);
        }
        Destroy(warningParticle);
    }

    private void FixedUpdate() {
        if (isMoving) {
            lifeSpan -= Time.deltaTime;
            transform.position += ((Vector3)direction * Speed) * Time.deltaTime;
            if (lifeSpan <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void GetHit(PlayerAttackEvent attackEvent) {
        if (attackEvent.attackCollider.bounds.Intersects(circleCollider2D.bounds)) {
            //SKA NÅGOT HÄNDA DÅ?
        }
    }

    private void OnPlayerTouchKillzone(PlayerTouchKillzoneEvent killzoneEvent) {
        if (isPlatformingComet) {
            if(warningParticle != null) {
                Destroy(warningParticle);
            }
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (isMoving) {
            if (collision.gameObject.tag == "Player") {
                Debug.Log("Collided with Player");
                PlayerTakeDamageEvent playerTakeDamage = new PlayerTakeDamageEvent {
                    damage = Damage
                };
                playerTakeDamage.FireEvent();
            }
        }
    }

    private Vector3 PlatformCometPosition() {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp(position.x, 1, 1);
        Vector3 cometPos;
        if(windUpLeft > WindUp / 2) {
            cometPos = new Vector3(Camera.main.ViewportToWorldPoint(position).x, player.transform.position.y, 0);
            lastKnownPlayerYPos = cometPos.y;
            return cometPos;
        }
        cometPos = new Vector3(Camera.main.ViewportToWorldPoint(position).x, lastKnownPlayerYPos, 0);
        return cometPos;
    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        PlayerAttackEvent.UnRegisterListener(GetHit);
        PlayerTouchKillzoneEvent.UnRegisterListener(OnPlayerTouchKillzone);
        BossDiedEvent.UnRegisterListener(BossDied);
    }

}
