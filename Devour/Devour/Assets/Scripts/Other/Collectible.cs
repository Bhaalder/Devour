//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType {

    VOIDESSENCE, LIFEFORCE

}

[System.Serializable]
public class Collectible {

    public CollectibleType collectibleType;
    public int amount;

    public Collectible(CollectibleType collectibleType, int amount) {
        this.collectibleType = collectibleType;
        this.amount = amount;
    }

}
