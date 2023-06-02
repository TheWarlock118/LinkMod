using System;

namespace LinkMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "Link is the Hero of Hyrule, weilder of the Triforce of Courage.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > The Master Sword is Link's primary weapon against the forces of evil." + Environment.NewLine + Environment.NewLine
             + "< ! > His bows allows Link to deal high damage at range, with a variety of special effects." + Environment.NewLine + Environment.NewLine
             + "< ! > Link's Sheikah Runes allow for a wide variety of utility options." + Environment.NewLine + Environment.NewLine
             + "< ! > Link's Champion Abilities offer extra mobility, protection, high damage, or a second life." + Environment.NewLine + Environment.NewLine;

        internal static float swordDamageCoefficient = Config.SwordDamageCoeffConfig.Value / 100;

        internal static float bowDamageCoefficient = Config.BowDamageCoeffConfig.Value / 100;

        internal static float bombDamageCoefficient = Config.BowDamageCoeffConfig.Value / 100;

        internal static float bombArrowDamageCoefficient = Config.BombArrowDamageCoeffConfig.Value / 100;

        internal static float urbosaDamageCoefficient = Config.UrbosaDamageCoeffConfig.Value / 100;

        internal static float revaliDamageCoefficient = Config.RevaliDamageCoeffConfig.Value / 100;

        internal static float cryonisDamageCoefficient = Config.CryonisDamageCoeffConfig.Value / 100;
    }
}