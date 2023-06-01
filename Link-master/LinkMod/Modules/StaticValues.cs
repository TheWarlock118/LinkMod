using System;

namespace LinkMod.Modules
{
    internal static class StaticValues
    {
        internal static float swordDamageCoefficient = Config.SwordDamageCoeffConfig.Value / 100;

        internal static float bowDamageCoefficient = Config.BowDamageCoeffConfig.Value / 100;

        internal static float bombDamageCoefficient = Config.BowDamageCoeffConfig.Value / 100;

        internal static float bombArrowDamageCoefficient = Config.BombArrowDamageCoeffConfig.Value / 100;

        internal static float urbosaDamageCoefficient = Config.UrbosaDamageCoeffConfig.Value / 100;

        internal static float revaliDamageCoefficient = Config.RevaliDamageCoeffConfig.Value / 100;

        internal static float cryonisDamageCoefficient = Config.CryonisDamageCoeffConfig.Value / 100;
    }
}