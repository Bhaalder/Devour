//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroVoidWall : MonoBehaviour {

    [Tooltip("The end location of the wall")]
    [SerializeField] private Transform wallEndLocation;
    [Tooltip("The minimum and maximum location of Y on the wall (min first)")]
    [SerializeField] private Transform[] wallMinMax_YLocations;
    [Tooltip("The minimum and maximum location of X on the wall (min first)")]
    [SerializeField] private Transform[] wallMinMax_XLocations;
    [SerializeField] private float timeBeforeMoving;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool isVerticalWall;
    private float timeLeft;
    private Vector3 tempPosition;
    private Vector3 startPosition;

    private void OnEnable() {
        timeLeft = timeBeforeMoving;
        startPosition = transform.position;
        if (isVerticalWall) {
            tempPosition = transform.position + new Vector3(0, Random.Range(wallMinMax_YLocations[0].localPosition.y, wallMinMax_YLocations[1].localPosition.y), 0);
        } else {
            tempPosition = transform.position + new Vector3(Random.Range(wallMinMax_XLocations[0].localPosition.x, wallMinMax_XLocations[1].localPosition.x), 0, 0);
        }
        transform.position = tempPosition;
    }

    private void Start() {
        for (int i = 0; i < 2; i++) {
            transform.GetChild(i).GetComponent<NazroVoidWallCollide>().Damage = damage;
        }
        BossDiedEvent.RegisterListener(BossDied);
    }

    private void FixedUpdate() {
        if(timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            return;
        }
        transform.position += ((Vector3)direction * speed) * Time.deltaTime;
        if(isVerticalWall && transform.position.x <= wallEndLocation.position.x) {
            gameObject.SetActive(false);
        }
        if(!isVerticalWall && transform.position.y <= wallEndLocation.position.y) {
            gameObject.SetActive(false);
        }
    }

    private void BossDied(BossDiedEvent bossDied) {
        Destroy(gameObject);
    }

    private void OnDisable() {
        transform.position = startPosition;
    }

    private void OnDestroy() {
        BossDiedEvent.UnRegisterListener(BossDied);
    }

}
