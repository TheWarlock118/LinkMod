using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_REWARD_ID", null, typeof(FastBowServerAchievement))]    
    internal class FastBowAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_FASTBOW_";
        public override string AchievementSpriteName => "RoyalGuardBow";        
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

            private float resetDelay;
            public override void OnInstall()
            {
                base.OnInstall();
                killCount = 0;
                resetDelay = 0;
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
                if (resetDelay >= 2f)
                {
                    killCount = 0;
                    resetDelay = 0f;
                }
                else
                {
                    resetDelay += Time.fixedDeltaTime;
                }
            }

            private void OnCharacterDeath(DamageReport damageReport)
            {
                Log.LogDebug("KillCount = " + killCount.ToString());
                if (killCount == 0)
                    resetDelay = 0f;

                CharacterBody currentBody = this.serverAchievementTracker.networkUser.GetCurrentBody();                
                if (damageReport.damageInfo.damageType.HasFlag(DamageType.Freeze2s) && damageReport.attackerBody == currentBody && damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
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