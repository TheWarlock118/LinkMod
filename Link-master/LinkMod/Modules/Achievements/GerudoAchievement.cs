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

            private int killsNotCounted;

            private float resetDelay;
            public override void OnInstall()
            {
                base.OnInstall();
                killCount = 0;
                killsNotCounted = 0;
                resetDelay = 0f;
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
                if (resetDelay >= 0.5f)
                {
                    killCount = 0;
                    killsNotCounted = 0;
                    resetDelay = 0f;
                }
                else
                {
                    resetDelay += Time.fixedDeltaTime;
                }
            }
            private void OnCharacterDeath(DamageReport damageReport)
            {
                if (killCount == 0)
                    resetDelay = 0f;

                CharacterBody currentBody = this.serverAchievementTracker.networkUser.GetCurrentBody();                              
                if (damageReport.damageInfo.damageType.HasFlag(DamageType.Shock5s) && damageReport.attackerBody == currentBody && currentBody.bodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
                {                    
                    killCount++;                    
                }
                else
                {
                    killsNotCounted++;
                }
                if (killCount >= 20)
                {
                    Grant();
                }

                //Log.LogDebug("Has Shock: " + damageReport.damageInfo.damageType.HasFlag(DamageType.Shock5s).ToString());
                //Log.LogDebug("Attacker is Link: " + (damageReport.attackerBody == currentBody).ToString());
                //Log.LogDebug("Kill Count: " + killCount.ToString());
                //Log.LogDebug("Kills Not Counted: " + killsNotCounted.ToString());
            }

            
        }
    }
}