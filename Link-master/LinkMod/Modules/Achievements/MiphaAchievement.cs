using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_MIPHA_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_MIPHA_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class MiphaAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_MIPHA_";
        public override string AchievementSpriteName => "MiphasGrace";        
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
            if (base.localUser.cachedBody.bodyIndex == BodyCatalog.FindBodyIndex(RequiredCharacterBody))
            {
                if (base.localUser.cachedBody.GetComponent<UpdateValues>())
                {
                    if (base.localUser.cachedBody.GetComponent<UpdateValues>().darukBlockedAttacks >= 5)
                    {
                        Grant();
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