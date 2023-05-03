using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class DarkAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_DARK_";        
        public override string AchievementSpriteName => "DarkSkin";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

        public string RequiredCharacterBody = "LinkBody";                

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
        }
        public override void OnBodyRequirementBroken()
        {            
            base.OnBodyRequirementBroken();
            GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
        }
        
        private void OnCharacterDeath(DamageReport damageReport)
        {
            if (damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("LinkBody") && damageReport.victimBodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
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