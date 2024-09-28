using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_REWARD_ID", null, 5, typeof(DarkServerAchievement))]    
    internal class DarkAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_DARK_";        
        public override string AchievementSpriteName => "DarkSkin";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

        public string RequiredCharacterBody = "LinkBody";

        public override void OnInstall()
        {
            base.OnInstall();
            base.SetServerTracked(true);
        }
        public override void OnUninstall()
        {
            base.OnUninstall();
        }
        private class DarkServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
                GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
            }

            private void OnCharacterDeath(DamageReport damageReport)
            {
                CharacterBody currentBody = this.serverAchievementTracker.networkUser.GetCurrentBody();
                if (damageReport.attackerBody == currentBody && damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody") && damageReport.victimBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
                {
                    Grant();
                }
            }
        }
    }
}