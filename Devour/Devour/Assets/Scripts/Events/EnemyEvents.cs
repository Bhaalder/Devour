//Author: Patrik Ahlgren

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageEvent : Event<EnemyTakeDamageEvent> {

    public Player player;
    public BoxCollider2D attackCollider;
    public Vector3 playerPosition;
    public float damage;

}

public class EnemyHealEvent : Event<EnemyHealEvent> {

    public bool isLifeLeech;
    public float amount;

}

public class EnemyTouchKillzoneEvent : Event<EnemyTouchKillzoneEvent> {

    public float damage;

}
