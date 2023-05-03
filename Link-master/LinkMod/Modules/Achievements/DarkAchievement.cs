using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_DARK_UNLOCKABLE_REWARD_ID", null, null)]    
    internal class DarkAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_DARK_";
        //the name of the sprite in your bundle
        public override string AchievementSpriteName => "DarkSkin";
        //the token of your character's unlock achievement if you have one
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
            Log.LogDebug("Checking character death.");
            Log.LogDebug("Victim Index = " + damageReport.victimBodyIndex.ToString());            
            Log.LogDebug("Umbra Index = " + BodyCatalog.FindBodyIndex("LinkBody").ToString());
            

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