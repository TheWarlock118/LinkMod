using RoR2;
using System;
using UnityEngine;

namespace LinkMod.Modules.Achievements
{
    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_MASTERY_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_MASTERY_UNLOCKABLE_REWARD_ID", null, 10, null)]    
    internal class MasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_MASTERY_";        
        public override string AchievementSpriteName => "MasterySkin";        
        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => "LinkBody";        
        public override float RequiredDifficultyCoefficient => 3;
    }
}