//Author: Patrik Ahlgren

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NazroSecondPhaseEvent : Event<NazroSecondPhaseEvent> {

}

public class ZvixaSelfDamageEvent : Event<ZvixaSelfDamageEvent> {

    public float damage;
    public CircleCollider2D circleCollider2D;

}

public class EnemyHealEvent : Event<EnemyHealEvent> {

    public bool isLifeLeech;
    public float amount;

}

public class EnemyTouchKillzoneEvent : Event<EnemyTouchKillzoneEvent> {

    public Enemy enemy;

}

public class BossIntroEvent : Event<BossIntroEvent> {

    public string bossName;
    public string bossTitle;

}

public class BossDiedEvent : Event<BossDiedEvent> {

    public Boss boss;

}

public class ArenaEnemyDiedEvent : Event<ArenaEnemyDiedEvent>
{
    public int enemiesLeft;
}

public class ArenaTriggerEvent: Event<ArenaTriggerEvent>
{

}