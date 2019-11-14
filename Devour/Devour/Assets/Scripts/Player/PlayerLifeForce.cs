//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeForce {   

    public string SceneName { get; set; }
    public Vector3 Location { get; set; }
    public Collectible Collectible { get; set; }

    public PlayerLifeForce(string sceneName, Vector3 location, Collectible collectible) {
        SceneName = sceneName;
        Location = location;
        Collectible = collectible;
    }

}
