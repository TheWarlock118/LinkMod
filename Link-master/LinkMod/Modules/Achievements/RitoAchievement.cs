using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_RITO_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class RitoAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_RITO_";
        //the name of the sprite in your bundle
        public override string AchievementSpriteName => "RitoSkin";
        //the token of your character's unlock achievement if you have one
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
            if (base.localUser.cachedBody.characterMotor.isGrounded && !base.localUser.cachedBody.inputBank.jump.down)
            {
                glideTime = 0f;
            }
            else
            {
                glideTime += Time.fixedDeltaTime;                
                if(glideTime > 30f)
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