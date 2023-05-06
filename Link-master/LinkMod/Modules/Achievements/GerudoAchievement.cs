using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_REWARD_ID", null, typeof(GerudoServerAchievement))]    
    internal class GerudoAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_GERUDO_";        
        public override string AchievementSpriteName => "GerudoSkin";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

        

        public string RequiredCharacterBody = "LinkBody";                

        public override void OnInstall()
        {
            base.OnInstall();
            base.SetServerTracked(true);

        }
        public override void OnBodyRequirementBroken()
        {
            base.OnUninstall();            
        }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
        private class GerudoServerAchievement : BaseServerAchievement
        {
            private int killCount;
            public override void OnInstall()
            {
                base.OnInstall();
                killCount = 0;
                GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
                RoR2Application.onFixedUpdate += OnFixedUpdate;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
                RoR2Application.onFixedUpdate -= OnFixedUpdate;
            }

            private void OnFixedUpdate()
            {
                killCount = 0;
            }
            private void OnCharacterDeath(DamageReport damageReport)
            {
                CharacterBody currentBody = this.serverAchievementTracker.networkUser.GetCurrentBody();
                if (damageReport.damageInfo.force.magnitude == 50f && damageReport.attackerBody == currentBody && currentBody.bodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
                {
                    killCount++;
                }
                if (killCount >= 20)
                {
                    Grant();
                }
            }

            
        }
    }
}