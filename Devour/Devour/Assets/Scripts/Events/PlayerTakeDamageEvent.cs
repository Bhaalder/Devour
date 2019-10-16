using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamageEvent : Event<PlayerTakeDamageEvent> {

    public float damage;
    public bool isKillzone;

}
