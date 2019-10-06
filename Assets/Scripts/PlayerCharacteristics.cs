using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCharacteristics
{
    public static bool FireBall { get; set; }
    public static bool FireBallUp1 { get; set; }
    public static bool IceBall { get; set; }
    public static bool IceBallUp1 { get; set; }
    public static bool RockShield { get; set; }
    public static bool RockShieldUp1 { get; set; }
    public static bool AirShield { get; set; }
    public static bool AirShieldUp1 { get; set; }
    public static bool PrimMovement { get; set; }
    public static bool PrimNocVision { get; set; }
    public static bool PrimLifebarVision { get; set; }
    public static bool MaxHealth { get; set; }
    public static bool MaxHealthUp1 { get; set; }
    public static bool AttackSpeed { get; set; }
    public static bool AttackSpeedUp1 { get; set; }
    public static bool AttackDamage { get; set; }
    public static bool AttackDamageUp1 { get; set; }
    public static bool SpeedMovement { get; set; }
    public static bool SpeedMovementUp1 { get; set; }
    public static bool LifeSteal { get; set; }

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
}
