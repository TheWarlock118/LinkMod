//using RoR2;
//using RoR2.Achievements;
//using System;
//using UnityEngine;

//namespace LinkMod.Modules.Achievements
//{
//    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_SPEEDRUN_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_SPEEDRUN_UNLOCKABLE_REWARD_ID", null, 5, typeof(SpeedrunServerAchievement))]    
//    internal class SpeedrunAchievement : GenericModdedUnlockable
//    {
//        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_SPEEDRUN_";
//        public override string AchievementSpriteName => "NakedSkin";        
//        public override string PrerequisiteUnlockableIdentifier => LinkPlugin.developerPrefix + "_LINK_BODY_UNLOCKABLE_REWARD_ID";        

//        public string RequiredCharacterBody = "LinkBody";

        

//        public override void OnInstall()
//        {
//            base.OnInstall();
//            base.SetServerTracked(true);
//        }
//        public override void OnUninstall()
//        {
//            base.OnUninstall();
//        }
        
//        private class SpeedrunServerAchievement : BaseServerAchievement
//        {            
//            public override void OnInstall()
//            {
//                base.OnInstall();
//                Run.onClientGameOverGlobal += OnClientGameOverGlobal;
//            }
//            public override void OnUninstall()
//            {
//                base.OnUninstall();
//                Run.onClientGameOverGlobal -= OnClientGameOverGlobal;
//            }

//            private void OnClientGameOverGlobal(Run run, RunReport runReport)
//            {
//                if ((bool)runReport.gameEnding && (runReport.gameEnding.isWin))
//                {
//                    if(runReport.snapshotRunTime.t < 1800f && serverAchievementTracker.networkUser.GetCurrentBody().bodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
//                        Grant();                    
//                }
//            }
//        }        
//    }
//}