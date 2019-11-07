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

    //public Enemy enemy; //fienden som gav skada
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

public class PlayerVoidEvent : Event<PlayerVoidEvent> {

    public float amount;

}

public class PlayerTouchKillzoneEvent : Event<PlayerTouchKillzoneEvent> {

    public float damage;

}

public class PlayerDiedEvent : Event<PlayerDiedEvent> {

    public Player player;
    //info var man dog (typ vilken scen m.m.)

}