//using RoR2;
//using RoR2.Achievements;
//using System;
//using UnityEngine;

//namespace LinkMod.Modules.Achievements
//{
//    [RegisterAchievement("ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_ACHIEVEMENT_ID", "ACHIEVEMENT_LINK_BODY_DARUK_UNLOCKABLE_REWARD_ID", null, 5, typeof(DarukServerAchievement))]    
//    internal class DarukAchievement : GenericModdedUnlockable
//    {
//        public override string AchievementTokenPrefix => "ACHIEVEMENT_LINK_BODY_DARUK_";
//        public override string AchievementSpriteName => "DaruksProtection";        
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

//        private class DarukServerAchievement : BaseServerAchievement
//        {
//            public override void OnInstall()
//            {
//                base.OnInstall();
//                RoR2Application.onFixedUpdate += OnFixedUpdate;
//            }
//            public override void OnUninstall()
//            {
//                base.OnUninstall();
//                RoR2Application.onFixedUpdate -= OnFixedUpdate;
//            }
//            private void OnFixedUpdate()
//            {
//                CharacterBody currentBody = serverAchievementTracker.networkUser.GetCurrentBody();
//                if (currentBody)
//                {
//                    if (currentBody.bodyIndex == BodyCatalog.FindBodyIndex("LinkBody"))
//                    {
//                        if (currentBody.GetComponent<UpdateValues>())
//                        {
//                            if (currentBody.GetComponent<UpdateValues>().blockedAttacks >= 50)
//                            {
//                                Grant();
//                            }
//                        }
//                    }
//                }
//            }
//        }
        
//    }
//}