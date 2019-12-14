//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEvent : Event<PlayerAttackEvent> {

    public Player player;
    public BoxCollider2D attackCollider;
    public Vector3 playerPosition;
    public float damage;
    public bool isMeleeAttack;

}

public class PlayerTakeDamageEvent : Event<PlayerTakeDamageEvent> {

    public Vector3 enemyPosition;
    public float damage;
    public bool isSelfInflicted;

}

public class PlayerHealEvent : Event<PlayerHealEvent> {

    public bool isLifeLeech;
    public float amount;

}

public class PlayerGetAbilityEvent : Event<PlayerGetAbilityEvent> {

    public PlayerAbility playerAbility;

}

public class PlayerCollectibleChangeEvent : Event<PlayerCollectibleChangeEvent> {

    public Collectible collectible;

}

public class PlayerVoidChangeEvent : Event<PlayerVoidChangeEvent> {

    public float amount;

}

public class PlayerVoidIsFullEvent : Event<PlayerVoidIsFullEvent> {


}

public class PlayerBounceEvent : Event<PlayerBounceEvent> {

    public Vector2 amountOfForce;

}

public class PlayerTouchKillzoneEvent : Event<PlayerTouchKillzoneEvent> {

    public float damage;

}

public class PlayerBusyEvent : Event<PlayerBusyEvent> {
    public bool playerIsBusy;
}

public class PlayerDiedEvent : Event<PlayerDiedEvent> {

    public Player player;
    public Collectible collectibleLifeforceLost;

}
public class PlayerTookLastEssenceEvent : Event<PlayerTookLastEssenceEvent>
{

}