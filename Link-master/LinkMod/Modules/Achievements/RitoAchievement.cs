using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_REWARD_ID", null, typeof(RitoServerAchievement))]    
    internal class RitoAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_RITO_";        
        public override string AchievementSpriteName => "RitoSkin";        
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

        private class RitoServerAchievement : BaseServerAchievement
        {
            private float glideTime;
            public override void OnInstall()
            {
                base.OnInstall();
                glideTime = 0f;
                RoR2Application.onFixedUpdate += OnFixedUpdate;
            }
            public override void OnUninstall()
            {
                base.OnUninstall();
                RoR2Application.onFixedUpdate -= OnFixedUpdate;
            }

            private void OnFixedUpdate()
            {
                CharacterBody currentBody = serverAchievementTracker.networkUser.GetCurrentBody();
                if (currentBody)
                {
                    if (currentBody.bodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
                    {
                        if (currentBody.HasBuff(Modules.Buffs.paragliderBuff))
                        {
                            glideTime += Time.fixedDeltaTime;
                            Log.LogDebug("GlideTime: " + glideTime.ToString());
                            if (glideTime > 30f)
                            {
                                Grant();
                            }
                        }
                        else
                        {
                            glideTime = 0f;
                        }
                    }
                }
            }
        }
        
    }
}