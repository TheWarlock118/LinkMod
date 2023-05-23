using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_TRIBOW_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_TRIBOW_UNLOCKABLE_REWARD_ID", null, typeof(TriBowServerAchievement))]    
    internal class TriBowAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_TRIBOW_";
        public override string AchievementSpriteName => "GreatEagleBow";        
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
        
        private class TriBowServerAchievement : BaseServerAchievement
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
                CharacterBody currentBody = serverAchievementTracker.networkUser.GetCurrentBody();
                if (currentBody)
                {
                    if (currentBody.characterMotor.isGrounded && currentBody.bodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
                    {
                        killCount = 0;
                    }
                }
            }

            private void OnCharacterDeath(DamageReport damageReport)
            {                
                CharacterBody currentBody = this.serverAchievementTracker.networkUser.GetCurrentBody();                
                if (damageReport.damageInfo.damageType.HasFlag(DamageType.IgniteOnHit) && damageReport.attackerBody == currentBody && damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
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