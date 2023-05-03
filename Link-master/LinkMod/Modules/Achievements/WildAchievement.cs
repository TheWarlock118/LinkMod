using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_WILD_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_WILD_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class WildAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_WILD_";        
        public override string AchievementSpriteName => "WildSkin";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

        private float glideTime;

        public string RequiredCharacterBody = "LinkBody";                

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            glideTime = 0f;
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
                if (base.localUser.cachedBody.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("UseAmbientLevel")))
                {
                    Grant();
                }
            }
        }        

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
        
    }
}