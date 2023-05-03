using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_GERUDO_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class GerudoAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_GERUDO_";        
        public override string AchievementSpriteName => "GerudoSkin";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

        private int killCount;

        public string RequiredCharacterBody = "LinkBody";                

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            killCount = 0;
            GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
            RoR2Application.onFixedUpdate += OnFixedUpdate;
        }
        public override void OnBodyRequirementBroken()
        {            
            base.OnBodyRequirementBroken();
            GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
            RoR2Application.onFixedUpdate -= OnFixedUpdate;
        }
        
        private void OnFixedUpdate()
        {
            killCount = 0;
        }
        private void OnCharacterDeath(DamageReport damageReport)
        {            
            if(damageReport.damageInfo.force.magnitude == 50f && damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
            {
                killCount++;                
            }
            if(killCount >= 20)
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