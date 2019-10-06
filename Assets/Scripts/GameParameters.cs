﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameParameters
{
    // General
    public static readonly float DEFAULT_MOVEMENT_SPEED = 20;
    public static readonly float DEFAULT_HEALTH = 20;
    public static readonly float COLLISION_DAMAGE = 5;

    // Projectiles
    public static readonly float DEFAULT_PROJECTILE_SPEED = 20;
    public static readonly float DEFAULT_PROJECTILE_DAMAGE = 5;

    // Player
    public static float playerAttackSpeed = 1 / 2f; // The second number is the number of attack per seconds
    public static int playerStartHealth = 20;
    public static int playerSpeed = 20;
    public static float playerInvincibilityTime = 2;
    public static readonly float AIR_SHIELD_ACTIVATION_TIME = 1;
    public static RangeInt airShieldActivationRange = new RangeInt(3, 10);
    public static int numberOfRockShield = 3;

    // Player Projectiles
    public static float fireballSpeed = 20;
    public static float fireballDamage = 5;
    public static float iceballSpeed = 5;
    public static float iceballDamage = 3;

    // Shooter enemy
    public static readonly float SHOOTER_MOVEMENT_TIME = 2f;
    public static readonly float SHOOTER_TIME_BETWEEN_ATTACKS = 0.25f;
    public static readonly int SHOOTER_NUMBER_OF_SHOOTS = 3;
    public static readonly float SHOOTER_MOVEMENT_SPEED = 5;

    // Shooter Projectiles
    public static float shooterProjectileSpeed = 20;
    public static float shooterDamage = 5;
}
