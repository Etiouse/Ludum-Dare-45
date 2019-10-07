using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCharacteristics
{
    public static bool FireBall { get; private set; }
    public static bool FireBallUp1 { get; private set; }
    public static bool IceBall { get; private set; }
    public static bool IceBallUp1 { get; private set; }
    public static bool RockShield { get; private set; }
    public static bool RockShieldUp1 { get; private set; }
    public static bool AirShield { get; private set; }
    public static bool AirShieldUp1 { get; private set; }
    public static bool PrimMovement { get; private set; }
    public static bool PrimNocVision { get; private set; }
    public static bool PrimLifebarVision { get; private set; }
    public static bool MaxHealth { get; private set; }
    public static bool MaxHealthUp1 { get; private set; }
    public static bool AttackSpeed { get; private set; }
    public static bool AttackSpeedUp1 { get; private set; }
    public static bool AttackDamage { get; private set; }
    public static bool AttackDamageUp1 { get; private set; }
    public static bool SpeedMovement { get; private set; }
    public static bool SpeedMovementUp1 { get; private set; }
    public static bool LifeSteal { get; private set; }

    public static void ResetAll()
    {
        FireBall = false;
        FireBallUp1 = false;
        IceBall = false;
        IceBallUp1 = false;
        RockShield = false;
        RockShieldUp1 = false;
        AirShield = false;
        AirShieldUp1 = false;
        PrimMovement = false;
        PrimNocVision = false;
        PrimLifebarVision = false;
        MaxHealth = false;
        MaxHealthUp1 = false;
        AttackSpeed = false;
        AttackSpeedUp1 = false;
        AttackDamage = false;
        AttackDamageUp1 = false;
        SpeedMovement = false;
        SpeedMovementUp1 = false;
        LifeSteal = false;
    }

    public static bool GetValue(PowerShape.Type powerShapeType)
    {
        switch (powerShapeType)
        {
            case PowerShape.Type.FIRE_BALL:
                return FireBall;
            case PowerShape.Type.FIRE_BALL_UP1:
                return FireBallUp1;
            case PowerShape.Type.ICE_BALL:
                return IceBall;
            case PowerShape.Type.ICE_BALL_UP1:
                return IceBallUp1;
            case PowerShape.Type.ROCK_SHIELD:
                return RockShield;
            case PowerShape.Type.ROCK_SHIELD_UP1:
                return RockShieldUp1;
            case PowerShape.Type.AIR_SHIELD:
                return AirShield;
            case PowerShape.Type.AIR_SHIELD_UP1:
                return AirShieldUp1;
            case PowerShape.Type.PRIM_MOVEMENT:
                return PrimMovement;
            case PowerShape.Type.PRIM_NOC_VISION:
                return PrimNocVision;
            case PowerShape.Type.PRIM_LIFEBAR_VISION:
                return PrimLifebarVision;
            case PowerShape.Type.MAX_HEALTH:
                return MaxHealth;
            case PowerShape.Type.MAX_HEALTH_UP1:
                return MaxHealthUp1;
            case PowerShape.Type.ATTACK_SPEED:
                return AttackSpeed;
            case PowerShape.Type.ATTACK_SPEED_UP1:
                return AttackSpeedUp1;
            case PowerShape.Type.ATTACK_DAMAGE:
                return AttackDamage;
            case PowerShape.Type.ATTACK_DAMAGE_UP1:
                return AttackDamageUp1;
            case PowerShape.Type.SPEED_MOVEMENT:
                return SpeedMovement;
            case PowerShape.Type.SPEED_MOVEMENT_UP1:
                return SpeedMovementUp1;
            case PowerShape.Type.LIFE_STEAL:
                return LifeSteal;
        }

        return false;
    }

    public static void ChangeValue(PowerShape.Type powerShapeType, bool value)
    {
        switch (powerShapeType)
        {
            case PowerShape.Type.FIRE_BALL:
                FireBall = value;
                break;
            case PowerShape.Type.FIRE_BALL_UP1:
                FireBallUp1 = value;
                break;
            case PowerShape.Type.ICE_BALL:
                IceBall = value;
                break;
            case PowerShape.Type.ICE_BALL_UP1:
                IceBallUp1 = value;
                break;
            case PowerShape.Type.ROCK_SHIELD:
                RockShield = value;
                break;
            case PowerShape.Type.ROCK_SHIELD_UP1:
                RockShieldUp1 = value;
                break;
            case PowerShape.Type.AIR_SHIELD:
                AirShield = value;
                break;
            case PowerShape.Type.AIR_SHIELD_UP1:
                AirShieldUp1 = value;
                break;
            case PowerShape.Type.PRIM_MOVEMENT:
                PrimMovement = value;
                break;
            case PowerShape.Type.PRIM_NOC_VISION:
                PrimNocVision = value;
                break;
            case PowerShape.Type.PRIM_LIFEBAR_VISION:
                PrimLifebarVision = value;
                break;
            case PowerShape.Type.MAX_HEALTH:
                MaxHealth = value;
                break;
            case PowerShape.Type.MAX_HEALTH_UP1:
                MaxHealthUp1 = value;
                break;
            case PowerShape.Type.ATTACK_SPEED:
                AttackSpeed = value;
                break;
            case PowerShape.Type.ATTACK_SPEED_UP1:
                AttackSpeedUp1 = value;
                break;
            case PowerShape.Type.ATTACK_DAMAGE:
                AttackDamage = value;
                break;
            case PowerShape.Type.ATTACK_DAMAGE_UP1:
                AttackDamageUp1 = value;
                break;
            case PowerShape.Type.SPEED_MOVEMENT:
                SpeedMovement = value;
                break;
            case PowerShape.Type.SPEED_MOVEMENT_UP1:
                SpeedMovementUp1 = value;
                break;
            case PowerShape.Type.LIFE_STEAL:
                LifeSteal = value;
                break;
        }
    }
}
