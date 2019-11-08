//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType {

    VOIDESSENCE, LIFEFORCE

}

public class Collectible {
    
    public CollectibleType CollectibleType { get; set; }
    public int Amount { get; set; }

    public Collectible(CollectibleType collectibleType, int amount) {
        CollectibleType = collectibleType;
        Amount = amount;
    }

}
