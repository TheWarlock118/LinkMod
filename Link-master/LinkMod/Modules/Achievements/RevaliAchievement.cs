//using RoR2;
//using System;
//using UnityEngine;

//namespace LinkMod.Modules.Achievements
//{
//    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_REVALI_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_REVALI_UNLOCKABLE_REWARD_ID", null, 5, null)]    
//    internal class RevaliAchievement : GenericModdedUnlockable
//    {
//        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_REVALI_";
//        public override string AchievementSpriteName => "RevalisGale";        
//        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

//        private float bowTime;

//        public string RequiredCharacterBody = "LinkBody";                

//        public override void OnBodyRequirementMet()
//        {
//            base.OnBodyRequirementMet();
//            bowTime = 0f;
//            RoR2Application.onFixedUpdate += OnFixedUpdate;
//        }
//        public override void OnBodyRequirementBroken()
//        {            
//            base.OnBodyRequirementBroken();            
//            RoR2Application.onFixedUpdate -= OnFixedUpdate;
//        }
        
//        private void OnFixedUpdate()
//        {
//            if (base.localUser.cachedBody.bodyIndex == BodyCatalog.FindBodyIndex(RequiredCharacterBody))
//            {
//                SkillLocator skillLocator = base.localUser.cachedBody.skillLocator;
                
                
//                if (!base.localUser.cachedBody.characterMotor.isGrounded && base.localUser.cachedBody.inputBank.skill2.down && (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_3BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME"))
//                {
//                    bowTime += Time.fixedDeltaTime;
//                    if (bowTime > 10f)
//                    {
//                        Grant();
//                    }
//                }
//                else                
//                {
//                    bowTime = 0f;
//                }                
//            }
//        }        
            
//        public override BodyIndex LookUpRequiredBodyIndex()
//        {
//            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
//        }
        
//    }
//}