﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class PlayerData
{
    public List<PlayerAbility> PlayerAbilities { get; set; }

    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float MaxPlayerVoid { get; set; }
    public float PlayerVoid { get; set; }
    public float DamageReduction { get; set; }
    public float MeleeDamage { get; set; }
    public float ProjectileDamage { get; set; }
    public float KnockbackForce { get; set; }
    public float BounceForce { get; set; }
    public float MeleeCooldown { get; set; }
    public float UntilNextMeleeAttack { get; set; }
    public float MeleeLifeLeech { get; set; }
    public float MeleeVoidLeech { get; set; }
    public float ProjectileCooldown { get; set; }
    public float UntilNextProjectileAttack { get; set; }
    public float ProjectileHealthcost { get; set; }

    public int ExtraJumps { get; set; }
    public float MovementSpeed { get; set; }
    public float DashCooldown { get; set; }


    public float TalentMeleeDamage { get; set; }
    public float TalentProjectileDamage { get; set; }
    public float TalentHealth { get; set; }
    public float TalentLifeLeech { get; set; }
    public float TalentMovementSpeed { get; set; }
    public float TalentDashCooldown { get; set; }
    public float TalentPlayerVoid { get; set; }
    public float TalentVoidLeech { get; set; }

    public List<TalentPoint> TalentPoints { get; set; }
    public List<Collectible> Collectibles { get; set; }

    public PlayerData(Player playerData)
    {
        PlayerAbilities = playerData.PlayerAbilities;

        MaxHealth = playerData.MaxHealth;
        Health = playerData.Health;
        MaxPlayerVoid = playerData.MaxPlayerVoid;

        MeleeDamage = playerData.MeleeDamage;
        ProjectileDamage = playerData.ProjectileDamage;

        MeleeLifeLeech = playerData.MeleeLifeLeech;
        MeleeVoidLeech = playerData.MeleeVoidLeech;

        MovementSpeed = playerData.MovementSpeed;
        DashCooldown = playerData.DashCooldown;

        TalentPoints = playerData.TalentPoints;
        Collectibles = playerData.Collectibles;
    }
}