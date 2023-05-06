using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class FastBowAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_FASTBOW_";
        public override string AchievementSpriteName => "FalconBow";        
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

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
        
        private class FastBowServerAchievement : BaseServerAchievement
        {
            private int killCount;
            public override void OnInstall()
            {
                base.OnInstall();
                killCount = 0;
                RoR2Application.onFixedUpdate += OnFixedUpdate;
                GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
            }
            public override void OnUninstall()
            {
                base.OnUninstall();
                RoR2Application.onFixedUpdate -= OnFixedUpdate;
                GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
            }

            private void OnFixedUpdate()
            {
                killCount = 0;
            }

            private void OnCharacterDeath(DamageReport damageReport)
            {
                if ((int)damageReport.damageInfo.force.magnitude == 41 && damageReport.attackerBody == this.serverAchievementTracker.networkUser.GetCurrentBody() && damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
                {
                    killCount++;
                }
                if (killCount >= 3)
                {
                    Grant();
                }
            }
        }
    }
}