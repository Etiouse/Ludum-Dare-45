using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameParameters
{
    // General
    public static readonly float DEFAULT_MOVEMENT_SPEED = 20;
    public static readonly float DEFAULT_HEALTH = 50;
    public static readonly float COLLISION_DAMAGE = 5;

    // Traps
    public static readonly float LAVA_DAMAGE = 5;

    // Shockwave
    public static readonly float DEFAULT_SHOCKWAVE_EXPANDING_SCALE = 2;
    public static readonly float DEFAULT_SHOCKWAVE_EXPANDING_SPEED = 3;
    public static readonly float DEFAULT_SHOCKWAVE_DAMAGE = 5;

    // Projectiles
    public static readonly float DEFAULT_PROJECTILE_SPEED = 20;
    public static readonly float DEFAULT_PROJECTILE_DAMAGE = 5;

    // Player
    public static float playerAttackSpeedDefault = 1 / 2f; // The second number is the number of attack per seconds
    public static float playerAttackSpeedUpgrade1 = 1 / 2.5f; 
    public static float playerAttackSpeedUpgrade2 = 1 / 3f; 
    public static int playerMaxHealthDefault = 20;
    public static int playerMaxHealthUpgrade1 = 25;
    public static int playerMaxHealthUpgrade2 = 40;
    public static int playerSpeedDefault = 10;
    public static int playerSpeedUpgrade1 = 15;
    public static int playerSpeedUpgrade2 = 20;
    public static float playerInvincibilityTime = 1;
    public static readonly float AIR_SHIELD_ACTIVATION_TIME = 1;
    public static RangeInt airShieldActivationRange = new RangeInt(3, 8);
    public static RangeInt airShieldActivationRangeDefault = new RangeInt(3, 8);
    public static RangeInt airShieldActivationRangeUpgrade = new RangeInt(3, 5);
    public static int numberOfRockShield = 2;
    public static int numberOfRockShieldDefault = 2;
    public static int numberOfRockShieldUpgrade = 3;
    public static float playerDamageDefault = 10;
    public static float playerDamageUpgrade1 = 15;
    public static float playerDamageUpgrade2 = 20;
    public static float lifeSteal = 2;

    // Player Projectiles
    public static float fireballSpeed = 40;
    public static float iceballSpeed = 25;

    // Shooter enemy
    public static readonly float SHOOTER_LIFE = 50f;
    public static readonly float SHOOTER_MOVEMENT_TIME = 1.5f;
    public static readonly float SHOOTER_TIME_BETWEEN_ATTACKS = 0.25f;
    public static readonly int SHOOTER_NUMBER_OF_SHOOTS = 3;
    public static readonly float SHOOTER_MOVEMENT_SPEED = 10;

    // Shooter Projectiles
    public static float shooterProjectileSpeed = 15;
    public static float shooterDamage = 5;

    // Water Splasher
    public static readonly float WATERSPLASHER_LIFE = 30f;
    public static readonly float WATERSPLASHER_MOVEMENT_TIME = 2f;
    public static readonly float WATERSPLASHER_TIME_BETWEEN_ATTACKS = 0.1f;
    public static readonly int WATERSPLASHER_NUMBER_OF_SHOOTS = 20;
    public static readonly float WATERSPLASHER_MOVEMENT_SPEED = 5;

    // Shooter Projectiles
    public static float waterSplasherProjectileSpeed = 15;
    public static float waterSplasherDamage = 5;

    // Cone
    public static readonly float CONE_LIFE = 40f;
    public static readonly float CONE_MOVEMENT_TIME = 1f;
    public static readonly float CONE_TIME_BETWEEN_ATTACKS = 0.25f;
    public static readonly float CONE_MOVEMENT_SPEED = 5;

    // Cone Projectiles
    public static float coneProjectileSpeed = 20;
    public static float coneDamage = 5;

    // Dasher
    public static readonly float DASHER_LIFE = 40f;
    public static readonly float DASHER_MOVEMENT_TIME = 0.5f;
    public static readonly float DASHER_WAIT_TIME = 3f;
    public static readonly float DASHER_MOVEMENT_SPEED = 25;

    // Dasher Shockwave
    public static float dasherShockwaveExpandingScale = 3;
    public static float dasherShockwaveExpandingSpeed = 0.25f;
    public static float dasherShockwaveDamage = 5;

    // Braum
    public static readonly float BRAUM_LIFE = 80f;
    public static readonly float BRAUM_MOVEMENT_TIME = 3f;
    public static readonly float BRAUM_WAIT_TIME = 3f;
    public static readonly float BRAUM_MOVEMENT_SPEED = 10;

    // Healer
    public static readonly float HEALER_LIFE = 80f;
    public static readonly float HEALER_MOVEMENT_TIME = 3f;
    public static readonly float HEALER_WAIT_TIME = 2f;
    public static readonly float HEALER_MOVEMENT_SPEED = 10;

    // Healer shockwave
    public static float healerhockwaveExpandingScale = 5;
    public static float healerShockwaveExpandingSpeed = 0.25f;
    public static float healerShockwaveHeal = 10;

    // Braum shield
    public static readonly float BRAUM_SHIELD_DEACTIVATION_TIME = 2;

    // Middle Boss
    public static readonly float MIDDLE_BOSS_LIFE = 1200f;
    public static readonly float MIDDLE_BOSS_MOVEMENT_TIME = 2f;
    public static readonly float MIDDLE_BOSS_TIME_BETWEEN_ATTACKS = 5f;
    public static readonly float MIDDLE_BOSS_MOVEMENT_SPEED = 5;
    public static readonly float MIDDLE_BOSS_PROJECTILE_TIME = 1;
    public static readonly float MIDDLE_BOSS_PROJECTILE_SPEED = 10;

    // FinalBoss
    public static readonly float FINAL_BOSS_MOVEMENT_TIME = 1f;

    // FinalBoss Projectiles

    public static Sprite PlayerSprite;
    public static Sprite BossSprite;
}
