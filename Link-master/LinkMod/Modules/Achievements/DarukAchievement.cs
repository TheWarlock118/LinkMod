using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class DarukAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_DARUK_";
        public override string AchievementSpriteName => "DaruksProtection";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";        

        public string RequiredCharacterBody = "LinkBody";                

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();            
            RoR2Application.onFixedUpdate += OnFixedUpdate;
        }
        public override void OnBodyRequirementBroken()
        {            
            base.OnBodyRequirementBroken();            
            RoR2Application.onFixedUpdate -= OnFixedUpdate;
        }
        
        private void OnFixedUpdate()
        {
            if (base.localUser.cachedBody)
            {
                if (base.localUser.cachedBody.bodyIndex == BodyCatalog.FindBodyIndex(RequiredCharacterBody))
                {
                    if (base.localUser.cachedBody.GetComponent<UpdateValues>())
                    {
                        if (base.localUser.cachedBody.GetComponent<UpdateValues>().blockedAttacks >= 50)
                        {
                            Grant();
                        }
                    }
                }
            }
        }        

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
        
    }
}