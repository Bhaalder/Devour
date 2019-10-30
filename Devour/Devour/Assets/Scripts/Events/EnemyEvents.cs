//Author: Patrik Ahlgren

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealEvent : Event<EnemyHealEvent> {

    public bool isLifeLeech;
    public float amount;

}

public class EnemyTouchKillzoneEvent : Event<EnemyTouchKillzoneEvent> {

    public float damage;

}

public class BossDiedEvent : Event<BossDiedEvent> {

    public Boss boss;

}