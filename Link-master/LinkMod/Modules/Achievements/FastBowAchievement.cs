using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_FASTBOW_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class FastBowAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_FASTBOW_";
        public override string AchievementSpriteName => "FalconBow";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";        

        public string RequiredCharacterBody = "LinkBody";

        private int killCount;

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            killCount = 0;
            RoR2Application.onFixedUpdate += OnFixedUpdate;
            GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
        }
        public override void OnBodyRequirementBroken()
        {            
            base.OnBodyRequirementBroken();            
            RoR2Application.onFixedUpdate -= OnFixedUpdate;
            GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
        }
        
        private void OnFixedUpdate()
        {
            killCount = 0;
        }

        private void OnCharacterDeath(DamageReport damageReport)
        {            
            if ((int)damageReport.damageInfo.force.magnitude == 41 && damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
            {                
                killCount++;                
            }
            if (killCount >= 3)
            {
                Grant();
            }
        }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
        
    }
}