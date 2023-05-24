using R2API;
using System;

namespace LinkMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Link
            string prefix = LinkPlugin.developerPrefix + "_LINK_BODY_";

            string desc = "Link is the Hero of Hyrule, weilder of the Triforce of Courage.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > The Master Sword is Link's primary weapon against the forces of evil." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > His shield allows Link to block projectiles, while his bows allows him to deal high damage at range." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Link's Sheikah Bombs allow for high-damage crowd control." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Link's Champion Abilities offer extra mobility, protection, high damage, or a second life." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left for Hyrule, another kingdom saved.";
            string outroFailure = "..and so he perished, another kingdom left to ruin.";

            LanguageAPI.Add(prefix + "NAME", "Link");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Hero's Shade");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Hylian Set");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Champion's Tunic");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Paraglider");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "An item that you received from the king on the Great Plateau.\nIt allows you to sail through the sky.\n" + $"<style=cIsUtility>Hold space while you're in the air to use it</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SWORD_NAME", "The Master Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SWORD_DESCRIPTION", "The legendary sword that seals the darkness. " + $"<style=cIsUtility>Swing forward</style> for " + $"<style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>." + $" At over " + $"<style=cIsHealth>90% health</style>, fire a sword beam for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient / 2}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BOW_NAME", "Royal Guard Bow");
            LanguageAPI.Add(prefix + "SECONDARY_BOW_DESCRIPTION", Helpers.agilePrefix + $"<style=cIsUtility>Loose a bomb arrow</style>, which explodes to fling enemies and " + $"deal <style=cIsDamage>{100f * StaticValues.bombArrowDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_3BOW_NAME", "Great Eagle Bow");
            LanguageAPI.Add(prefix + "SECONDARY_3BOW_DESCRIPTION", Helpers.agilePrefix + $"<style=cIsUtility>Loose three ice arrows at once</style>," + $" each dealing <style=cIsDamage>{100f * StaticValues.bowDamageCoefficient / 3}% damage</style> and " + $"<style=cIsHealth> freezing enemies for 2 seconds</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_FASTBOW_NAME", "Falcon Bow");
            LanguageAPI.Add(prefix + "SECONDARY_FASTBOW_DESCRIPTION", Helpers.agilePrefix + $"<style=cIsUtility>Loose a hasty fire arrow</style> for <style=cIsDamage>{100f * StaticValues.bowDamageCoefficient / 2}% fire damage</style>. The specially engineered bowstring allows for faster drawing and a short cooldown.");

            LanguageAPI.Add(prefix + "SECONDARY_SHIELD_NAME", "Hylian Shield");
            LanguageAPI.Add(prefix + "SECONDARY_SHIELD_DESCRIPTION", "A shield passed down through the Hyrulean royal family, along with the legend of the hero who wielded it." + $" <style=cIsUtility>Hold to block all damage from the front</style>. Can attack with your sword while blocking.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_BOMB_NAME", "Remote Bomb");
            LanguageAPI.Add(prefix + "UTILITY_BOMB_DESCRIPTION", $"<style=cIsUtility>Hold to draw a bomb and let go to throw</style>. Explodes on impact to fling enemies and deal <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>. While gliding, bombs will drop straight down.");

            LanguageAPI.Add(prefix + "UTILITY_MAG_NAME", "Magnesis");
            LanguageAPI.Add(prefix + "UTILITY_MAG_DESCRIPTION", "Manipulate metallic objects using magnetism.");

            LanguageAPI.Add(prefix + "UTILITY_STAS_NAME", "Stasis");
            LanguageAPI.Add(prefix + "UTILITY_STAS_DESCRIPTION", "Stop the flow of time for an enemy.");

            LanguageAPI.Add(prefix + "UTILITY_CRY_NAME", "Cryonis");
            LanguageAPI.Add(prefix + "UTILITY_CRY_DESCRIPTION", "Create a pillar of ice.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_MIPHA_NAME", "Mipha's Grace");
            LanguageAPI.Add(prefix + "SPECIAL_MIPHA_DESCRIPTION", "When your hearts run out, call on the Champion Mipha to " + $"<style=cIsUtility>bring you back to life</style>. Usable once per stage.");

            LanguageAPI.Add(prefix + "SPECIAL_DARUK_NAME", "Daruk's Protection");
            LanguageAPI.Add(prefix + "SPECIAL_DARUK_DESCRIPTION", "Call on the Champion Daruk to " + $"<style=cIsUtility>enforce your defenses</style>. The next attack is automatically deflected. Cooldown begins after attack deflection.");

            LanguageAPI.Add(prefix + "SPECIAL_REVALI_NAME", "Revali's Gale");
            LanguageAPI.Add(prefix + "SPECIAL_REVALI_DESCRIPTION", "Call on the Champion Revali and " + $"<style=cIsUtility>soar into the sky</style>, dealing " + $"<style=cIsDamage>{100f * StaticValues.revaliDamageCoefficient}% damage</style> and pushing back enemies around you.");

            LanguageAPI.Add(prefix + "SPECIAL_URBOSA_NAME", "Urbosa's Fury");
            LanguageAPI.Add(prefix + "SPECIAL_URBOSA_DESCRIPTION", "Call on the Champion Urbosa to " + $"<style=cIsUtility>summon powerful lightning</style>, dealing " + $"<style=cIsDamage>{100f * StaticValues.urbosaDamageCoefficient}% damage</style> and " + $"<style=cIsHealth>shocking enemies for 2 seconds</style>.");
            #endregion

            #region Achievements            
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_MASTERY_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Mastery");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_MASTERY_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, beat the game or obliterate on Monsoon");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_MASTERY_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Mastery");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_MASTERY_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, beat the game or obliterate on Monsoon");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Inner Conflict");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, find and defeat the Hero's Shade");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Inner Conflict");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, find and defeat the Hero's Shade");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Stormcaller");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, kill 20 enemies at once with Urbosa's Fury");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Stormcaller");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, kill 20 enemies at once with Urbosa's Fury");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Windrider");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, glide for 30 seconds");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Windrider");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, glide for 30 seconds");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_WILD_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Reborn");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_WILD_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, call on Mipha's Grace to be reborn");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_WILD_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Reborn");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_WILD_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, call on Mipha's Grace to be reborn");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_REVALI_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: The Slow-mo Guy");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_REVALI_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, hold an arrow drawn for 10 seconds while falling");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_REVALI_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: The Slow-mo Guy");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_REVALI_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, hold an arrow drawn for 10 seconds while falling");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Great Protector");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, block 50 attacks with your Hylian Shield in one life");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Great Protector");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, block 50 attacks with your Hylian Shield in one life");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_MIPHA_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Protected");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_MIPHA_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, block 5 attacks with Daruk's Protection in one life");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_MIPHA_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Protected");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_MIPHA_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, block 5 attacks with Daruk's Protection in one life");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Triple Threat");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, kill 3 enemies with a single Great Eagle Bow shot");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Triple Threat");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, kill 3 enemies with a single Great Eagle Bow shot");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_TRIBOW_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Max Pain");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_TRIBOW_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, kill 3 enemies with your Falcon Bow without touching the ground");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_TRIBOW_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Max Pain");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_TRIBOW_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, kill 3 enemies with your Falcon Bow without touching the ground");

            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_SPEEDRUN_UNLOCKABLE_ACHIEVEMENT_NAME", "Link: Speedrunner");
            LanguageAPI.Add("ACHIEVEMENT_LINK_BODY_SPEEDRUN_UNLOCKABLE_ACHIEVEMENT_DESCRIPTION", "As Link, beat the game within 30 minutes");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_SPEEDRUN_UNLOCKABLE_ACHIEVEMENT_ID_NAME", "Link: Speedrunner");
            LanguageAPI.Add("ACHIEVEMENT_ACHIEVEMENT_LINK_BODY_SPEEDRUN_UNLOCKABLE_ACHIEVEMENT_ID_DESCRIPTION", "As Link, beat the game within 30 minutes");
            #endregion
            #endregion
        }
    }
}