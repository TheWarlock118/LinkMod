using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LinkMod.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        internal override string bodyName { get; set; } = "Link";

        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override float sortPosition { get; set; } = 100f;

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 0f,
            bodyName = "LinkBody",
            bodyNameToken = LinkPlugin.developerPrefix + "_LINK_BODY_NAME",
            bodyColor = new Color(0.12f, 0.39f, 0.25f, 1),
            characterPortrait = Modules.Assets.LoadCharacterIcon("Link"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 12f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 1,
            maxHealth = 110f,
            subtitleNameToken = LinkPlugin.developerPrefix + "_LINK_BODY_SUBTITLE",
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        // internal static Material LinkMat = Modules.Assets.CreateMaterial("matLink");
        internal override int mainRendererIndex { get; set; } = 0;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {
                //Too convoluted with a model made of up of this many meshes
                //new CustomRendererInfo
                //{
                //    childName = "Boots_001__Mt_Lower_001",
                //},
        };


        internal override Type characterMainState { get; set; } = typeof(SkillStates.BaseStates.LinkMain);

        // item display stuffs
        internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal override UnlockableDef characterUnlockableDef { get; set; }
        private static UnlockableDef masterySkinUnlockableDef;
        private static UnlockableDef darkSkinUnlockableDef;
        private static UnlockableDef gerudoSkinUnlockableDef;
        private static UnlockableDef ritoSkinUnlockableDef;
        private static UnlockableDef wildSkinUnlockableDef;
        private static UnlockableDef nakedSkinUnlockableDef;

        private static UnlockableDef revaliUnlockableDef;
        private static UnlockableDef darukUnlockableDef;
        private static UnlockableDef miphaUnlockableDef;

        private static UnlockableDef fastBowUnlockableDef;
        private static UnlockableDef triBowUnlockableDef;

        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        internal override void InitializeUnlockables()
        {            
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
            darkSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.DarkAchievement>(true);
            gerudoSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.GerudoAchievement>(true);
            ritoSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.RitoAchievement>(true);
            wildSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.WildAchievement>(true);
            nakedSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.SpeedrunAchievement>(true);

            revaliUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.RevaliAchievement>(true);
            darukUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.DarukAchievement>(true);
            miphaUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MiphaAchievement>(true);

            fastBowUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.FastBowAchievement>(true);
            triBowUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.TriBowAchievement>(true);
        }

        internal override void InitializeDoppelganger()
        {
            base.InitializeDoppelganger();
        }

        internal override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");            
        }               

        internal override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            string prefix = LinkPlugin.developerPrefix;

            #region Primary
            Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)), "Weapon", prefix + "_LINK_BODY_PRIMARY_SWORD_NAME", prefix + "_LINK_BODY_PRIMARY_SWORD_DESCRIPTION", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("MasterSword"), true), null);
            #endregion

            #region Secondary
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SECONDARY_BOW_NAME",
                skillNameToken = prefix + "_LINK_BODY_SECONDARY_BOW_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SECONDARY_BOW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("RoyalGuardBow"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef shootTriSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SECONDARY_3BOW_NAME",
                skillNameToken = prefix + "_LINK_BODY_SECONDARY_3BOW_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SECONDARY_3BOW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("GreatEagleBow"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShootTri)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef shootFastSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SECONDARY_FASTBOW_NAME",
                skillNameToken = prefix + "_LINK_BODY_SECONDARY_FASTBOW_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SECONDARY_FASTBOW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("FalconBow"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShootFast)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = true,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SECONDARY_SHIELD_NAME",
                skillNameToken = prefix + "_LINK_BODY_SECONDARY_SHIELD_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SECONDARY_SHIELD_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("HylianShieldIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shield)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = true,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef[] secondaries = { shieldSkillDef, shootFastSkillDef, shootTriSkillDef, shootSkillDef };
            UnlockableDef[] secondaryUnlockables = { null, null, triBowUnlockableDef, fastBowUnlockableDef };
            Modules.Skills.AddSecondarySkills(bodyPrefab, secondaryUnlockables, secondaries);
            #endregion

            #region Utility
            SkillDef bombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_UTILITY_BOMB_NAME",
                skillNameToken = prefix + "_LINK_BODY_UTILITY_BOMB_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_UTILITY_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("RemoteBomb"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,                
            });

            SkillDef[] utilities = { bombSkillDef};
            UnlockableDef[] utilityUnlockables = { null };
            Modules.Skills.AddUtilitySkills(bodyPrefab, utilityUnlockables, utilities);
            #endregion

            #region Special
            SkillDef miphaSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SPECIAL_MIPHA_NAME",
                skillNameToken = prefix + "_LINK_BODY_SPECIAL_MIPHA_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SPECIAL_MIPHA_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("MiphasGrace"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.MiphasGrace)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 69420f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1            
            });

            SkillDef darukSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SPECIAL_DARUK_NAME",
                skillNameToken = prefix + "_LINK_BODY_SPECIAL_DARUK_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SPECIAL_DARUK_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("DaruksProtection"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.DaruksProtection)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 24f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef revaliSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SPECIAL_REVALI_NAME",
                skillNameToken = prefix + "_LINK_BODY_SPECIAL_REVALI_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SPECIAL_REVALI_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("RevalisGale"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.RevalisGale)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef urbosaSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SPECIAL_URBOSA_NAME",
                skillNameToken = prefix + "_LINK_BODY_SPECIAL_URBOSA_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SPECIAL_URBOSA_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("UrbosasFury"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.UrbosasFury)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef[] specials = { urbosaSkillDef, revaliSkillDef, darukSkillDef, miphaSkillDef};
            UnlockableDef[] specialUnlockables = { null, revaliUnlockableDef, darukUnlockableDef, miphaUnlockableDef };
            Modules.Skills.AddSpecialSkills(bodyPrefab, specialUnlockables, specials);
            #endregion

            SkillLocator passSkillLocator = bodyPrefab.GetComponent<SkillLocator>();
            passSkillLocator.passiveSkill.enabled = true;
            passSkillLocator.passiveSkill.skillNameToken = prefix + "_LINK_BODY_PASSIVE_NAME";
            passSkillLocator.passiveSkill.skillDescriptionToken = prefix + "_LINK_BODY_PASSIVE_DESCRIPTION";
            passSkillLocator.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Paraglider");            
        }



        internal override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;            

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("Hylian Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("HylianSkin"),
                defaultRenderers,                
                model.gameObject);

            // defaultSkin.rendererInfos = Modules.Skins.getRendererMaterials(defaultSkin.rendererInfos, Modules.Materials.CreateHopooMaterial("Mt_armor_upper"));
            // defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultSkin.rendererInfos, "Metal_001__Mt_Upper_001");

            /* ALL gameObjectActivations - copy and remove the ones you want to keep for each skin
             * defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HylianHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Boots_001__Mt_Lower_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Earring__Mt_Earring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Eye_L__Mt_Eyeball_L"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Eye_R__Mt_Eyeball_R"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Eyelashes__Mt_Eyelashes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Face__Mt_Face"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair_A_001_A__Mt_Hair_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin_001__Mt_Upper_Skin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Face__Mt_Head"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair1"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
            };
            */

            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEyes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Face__Mt_Head"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair1"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
            };

            skins.Add(defaultSkin);
            #endregion

            #region RitoSkin            
            SkinDef ritoSkin = Modules.Skins.CreateSkinDef("Snowquill Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("RitoSkin"),
                defaultRenderers,                
                model.gameObject,
                ritoSkinUnlockableDef);

            ritoSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEyes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HylianHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Boots_001__Mt_Lower_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair_A_001_A__Mt_Hair_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin_001__Mt_Upper_Skin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair1"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                },

            };

            skins.Add(ritoSkin);
            #endregion

            #region GerudoSkin
            SkinDef gerudoSkin = Modules.Skins.CreateSkinDef("Gerudo Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("GerudoSkin"),
                defaultRenderers,                
                model,
                gerudoSkinUnlockableDef);

            gerudoSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEyes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HylianHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Boots_001__Mt_Lower_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair_A_001_A__Mt_Hair_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin_001__Mt_Upper_Skin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair1"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                },

            };

            skins.Add(gerudoSkin);
            #endregion

            #region WildSkin
            SkinDef wildSkin = Modules.Skins.CreateSkinDef("Wild Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("WildSkin"),
                defaultRenderers,                
                model,
                wildSkinUnlockableDef);

            wildSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEyes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HylianHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Boots_001__Mt_Lower_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair_A_001_A__Mt_Hair_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin_001__Mt_Upper_Skin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair1"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
            };

            skins.Add(wildSkin);
            #endregion

            #region DarkSkin            
            SkinDef darkSkin = Modules.Skins.CreateSkinDef("Dark Link",
                Assets.mainAssetBundle.LoadAsset<Sprite>("DarkSkin"),
                defaultRenderers,                
                model,
                darkSkinUnlockableDef);

            darkSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Eye_L__Mt_Eyeball_L"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Eye_R__Mt_Eyeball_R"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HylianHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Boots_001__Mt_Lower_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Eyelashes__Mt_Eyelashes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Face__Mt_Face"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair_A_001_A__Mt_Hair_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin_001__Mt_Upper_Skin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Face__Mt_Head"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair1"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
            };

            skins.Add(darkSkin);
            #endregion

            #region NakedSkin
            SkinDef nakedSkin = Modules.Skins.CreateSkinDef("Birthday Suit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("NakedSkin"),
                defaultRenderers,
                model,
                nakedSkinUnlockableDef);

            nakedSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEyes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HylianHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Boots_001__Mt_Lower_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Circlet"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CircletJewel"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGauntlets"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionGlove"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ChampionUndershirts"),
                    shouldActivate = false
                }
            };

            skins.Add(nakedSkin);
            #endregion

            #region MasterySkin            
            SkinDef masterySkin = Modules.Skins.CreateSkinDef("Champion's Tunic",
                Assets.mainAssetBundle.LoadAsset<Sprite>("MasterySkin"),
                defaultRenderers,                
                model.gameObject,
                masterySkinUnlockableDef);

            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEyes"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_A_Buckle__Mt_Belt_A_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_B_001_Buckle__Mt_Belt_B_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Belt_C_Buckle__Mt_Belt_C_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Beltmini_001__Mt_Upper_Belt_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Chain_001__Mt_Upper_kusari_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hair_A_001_A__Mt_Hair_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_A_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Hood_B_001_Metal__Mt_Hood_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Metal_001__Mt_Upper_001"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin_001__Mt_Upper_Skin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Skin__Mt_Underwear"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBody2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkEarring"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkFace"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("DarkSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBelt"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildHat"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildTunic2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("WildLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoFeather"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoTunic"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoBoots"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHair2"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHairband"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoHook"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoParts"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("RitoSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoArmBands"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHair"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoHeadcloth"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoLegacc"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoPants"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinHand"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GerudoSkinLeg"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainUpperSkin"),
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MainLowerSkin"),
                    shouldActivate = false
                },
            };

            skins.Add(masterySkin);
            #endregion
            
            skinController.skins = skins.ToArray();
        }

        internal override void SetItemDisplays()
        {
            itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            #region Item Displays
            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Jetpack,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
                            childName = "Spine_2",
localPos = new Vector3(-0.00132F, -0.00043F, -0.00013F),
localAngles = new Vector3(291.7152F, 251.2411F, 201.9476F),
localScale = new Vector3(0.00141F, 0.00141F, 0.00141F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.GoldGat,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldGat"),
childName = "Root",
localPos = new Vector3(-0.00257F, 0.01482F, 0.00113F),
localAngles = new Vector3(311.249F, 267.6794F, 324.1024F),
localScale = new Vector3(-0.00118F, -0.00118F, -0.00118F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.BFG,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
childName = "Spine_2",
localPos = new Vector3(-0.00185F, 0.00001F, -0.00009F),
localAngles = new Vector3(277.2284F, 88.49117F, 47.82944F),
localScale = new Vector3(0.00283F, 0.00283F, 0.00283F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.CritGlasses,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
childName = "Head",
localPos = new Vector3(-0.00058F, 0.00138F, 0F),
localAngles = new Vector3(274.0445F, 118.1071F, 333.7419F),
localScale = new Vector3(0.00197F, 0.00186F, 0.00186F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Syringe,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
childName = "Spine_1",
localPos = new Vector3(0.00086F, 0.00007F, -0.00159F),
localAngles = new Vector3(79.91241F, 111.9505F, 90.67212F),
localScale = new Vector3(-0.0012F, -0.0012F, -0.0012F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Behemoth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
childName = "Head",
localPos = new Vector3(0.00002F, -0.00117F, -0.00186F),
localAngles = new Vector3(4.73043F, 256.4166F, 176.757F),
localScale = new Vector3(0.00119F, 0.00119F, 0.00119F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Missile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
childName = "Spine_2",
localPos = new Vector3(-0.00479F, 0.00007F, -0.00263F),
localAngles = new Vector3(280.3456F, 66.67073F, 6.73252F),
localScale = new Vector3(0.00075F, 0.00075F, 0.00075F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Dagger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
childName = "Head",
localPos = new Vector3(0.00195F, -0.00112F, 0.00063F),
localAngles = new Vector3(355.3441F, 275.6597F, 172.0008F),
localScale = new Vector3(0.00946F, 0.00946F, 0.00936F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Hoof,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
childName = "Ankle_R",
localPos = new Vector3(0.00022F, -0.00096F, -0.00001F),
localAngles = new Vector3(301.3307F, 68.25417F, 201.2285F),
localScale = new Vector3(-0.00092F, -0.00115F, -0.00078F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ChainLightning,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
childName = "Spine_2",
localPos = new Vector3(-0.00032F, -0.00185F, 0.00002F),
localAngles = new Vector3(286.6041F, 88.56171F, 151.7254F),
localScale = new Vector3(-0.00569F, -0.00569F, -0.00569F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.GhostOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
childName = "Head",
localPos = new Vector3(-0.00065F, 0.00104F, 0F),
localAngles = new Vector3(273.7123F, 225.3471F, 221.8175F),
localScale = new Vector3(0.00566F, 0.00566F, 0.00367F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Mushroom,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
childName = "Ankle_L",
localPos = new Vector3(-0.00161F, -0.00006F, 0.00008F),
localAngles = new Vector3(12.69558F, 20.85448F, 356.5715F),
localScale = new Vector3(0.00066F, 0.00066F, 0.00066F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.AttackSpeedOnCrit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
childName = "Head",
localPos = new Vector3(-0.00109F, 0.00117F, 0.00001F),
localAngles = new Vector3(280.6046F, 235.9428F, 211.7847F),
localScale = new Vector3(0.00655F, 0.00584F, 0.00655F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BleedOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
childName = "Spine_2",
localPos = new Vector3(-0.00363F, -0.00023F, 0.00225F),
localAngles = new Vector3(9.14773F, 306.9309F, 162.7443F),
localScale = new Vector3(-0.00532F, -0.00532F, -0.00532F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.WardOnLevel,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
childName = "Spine_2",
localPos = new Vector3(0.00032F, -0.0014F, 0.00154F),
localAngles = new Vector3(22.38836F, 355.2176F, 81.92944F),
localScale = new Vector3(0.00567F, 0.00567F, 0.00567F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.HealOnCrit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
childName = "Spine_2",
localPos = new Vector3(-0.00023F, -0.00151F, 0.00147F),
localAngles = new Vector3(15.07425F, 110.8939F, 351.6335F),
localScale = new Vector3(-0.00402F, -0.00377F, -0.00402F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.HealWhileSafe,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
childName = "Spine_1",
localPos = new Vector3(0.00209F, -0.00067F, -0.00001F),
localAngles = new Vector3(357.287F, 90.10506F, 359.7641F),
localScale = new Vector3(0.00138F, 0.00138F, 0.00138F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Clover,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
childName = "Spine_2",
localPos = new Vector3(-0.00088F, 0.00166F, -0.00061F),
localAngles = new Vector3(13.79218F, 179.8981F, 359.7485F),
localScale = new Vector3(0.0022F, 0.00228F, 0.0022F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BarrierOnOverHeal,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
childName = "Wrist_L",
localPos = new Vector3(-0.00034F, -0.00049F, -0.00097F),
localAngles = new Vector3(73.6835F, 357.1262F, 165.942F),
localScale = new Vector3(-0.00425F, -0.00425F, -0.0037F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.GoldOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
childName = "Head",
localPos = new Vector3(-0.00074F, -0.00001F, 0F),
localAngles = new Vector3(271.9399F, 234.162F, 215.6906F),
localScale = new Vector3(0.01096F, 0.01096F, 0.01096F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.WarCryOnMultiKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
childName = "Arm_1_L",
localPos = new Vector3(-0.00074F, -0.00014F, -0.00043F),
localAngles = new Vector3(16.82937F, 123.2503F, 265.3127F),
localScale = new Vector3(0.00776F, 0.00776F, 0.00776F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
childName = "Wrist_L",
localPos = new Vector3(-0.00052F, -0.0009F, -0.00057F),
localAngles = new Vector3(16.40958F, 195.9368F, 86.61452F),
localScale = new Vector3(0.00356F, 0.00356F, 0.00356F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IceRing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
childName = "Wrist_L",
localPos = new Vector3(-0.00118F, 0.00028F, 0.00007F),
localAngles = new Vector3(346.1126F, 300.9069F, 105.8654F),
localScale = new Vector3(-0.00155F, -0.00155F, -0.00162F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FireRing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
childName = "Wrist_R",
localPos = new Vector3(0.00125F, 0.00022F, -0.00009F),
localAngles = new Vector3(358.7645F, 106.3358F, 257.4453F),
localScale = new Vector3(0.00152F, 0.00152F, 0.00152F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.UtilitySkillMagazine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                            childName = "Arm_1_L",
localPos = new Vector3(-0.00084F, -0.00017F, -0.00041F),
localAngles = new Vector3(343.2083F, 6.70346F, 354.2627F),
localScale = new Vector3(0.00809F, 0.00809F, 0.00809F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                            childName = "Arm_1_R",
localPos = new Vector3(0.00075F, 0.00052F, 0.00014F),
localAngles = new Vector3(16.48954F, 192.4561F, 174.7431F),
localScale = new Vector3(0.0081F, 0.0081F, 0.0081F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.JumpBoost,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
childName = "Head",
localPos = new Vector3(0.0021F, -0.00111F, 0.00067F),
localAngles = new Vector3(77.48444F, 300.6191F, 217.4551F),
localScale = new Vector3(-0.00689F, -0.00689F, -0.00689F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ArmorReductionOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
childName = "Head",
localPos = new Vector3(-0.002F, -0.00008F, 0F),
localAngles = new Vector3(330.4154F, 90.10216F, 90.07761F),
localScale = new Vector3(0.0026F, 0.0026F, 0.0026F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.NearbyDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
childName = "Sword",
localPos = new Vector3(-0.002F, 0.1828F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1236F, 0.1236F, 0.1236F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ArmorPlate,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
childName = "Arm_1_R",
localPos = new Vector3(0.00138F, 0.00025F, -0.00016F),
localAngles = new Vector3(12.77693F, 88.40237F, 63.43773F),
localScale = new Vector3(-0.00348F, -0.00348F, -0.00348F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.CommandMissile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
childName = "Spine_2",
localPos = new Vector3(-0.00167F, -0.00123F, -0.00008F),
localAngles = new Vector3(30.94948F, 90.22654F, 178.8473F),
localScale = new Vector3(0.00452F, 0.00452F, 0.00452F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Feather,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
childName = "Ankle_L",
localPos = new Vector3(0.0009F, 0.00249F, 0.00052F),
localAngles = new Vector3(36.20286F, 336.7381F, 340.9268F),
localScale = new Vector3(0.00017F, 0.00017F, 0.00017F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Crowbar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
childName = "Spine_1",
localPos = new Vector3(0.0013F, 0.00006F, -0.00194F),
localAngles = new Vector3(314.1932F, 63.27642F, 20.01543F),
localScale = new Vector3(0.00369F, 0.00369F, 0.00369F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FallBoots,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
childName = "Ankle_L",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(355.0033F, 3.9363F, 338.8973F),
localScale = new Vector3(0.00203F, 0.00203F, 0.00203F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
childName = "Ankle_R",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(6.72753F, 13.72725F, 160.491F),
localScale = new Vector3(0.00203F, 0.00203F, 0.00203F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ExecuteLowHealthElite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
childName = "Head",
localPos = new Vector3(0.00031F, -0.00245F, 0.00004F),
localAngles = new Vector3(275.3861F, 92.96822F, 356.9129F),
localScale = new Vector3(-0.00454F, -0.00454F, -0.00454F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.EquipmentMagazine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
childName = "Spine_1",
localPos = new Vector3(0.00213F, 0.00107F, -0.0019F),
localAngles = new Vector3(6.18004F, 277.9501F, 292.5677F),
localScale = new Vector3(0.00109F, 0.00109F, 0.00109F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.NovaOnHeal,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
childName = "Head",
localPos = new Vector3(-0.00156F, 0.00123F, -0.00001F),
localAngles = new Vector3(15.95313F, 245.9711F, 152.1254F),
localScale = new Vector3(0.00402F, 0.00402F, 0.00402F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
childName = "Head",
localPos = new Vector3(-0.00164F, 0.00128F, 0F),
localAngles = new Vector3(4.00403F, 290.9153F, 199.9533F),
localScale = new Vector3(-0.00402F, 0.00402F, 0.00402F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Infusion,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
childName = "Spine_2",
localPos = new Vector3(0.00032F, 0.00168F, 0.00039F),
localAngles = new Vector3(76.35793F, 219.9026F, 269.0191F),
localScale = new Vector3(0.00356F, 0.00356F, 0.00356F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Medkit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
childName = "Spine_2",
localPos = new Vector3(0.00282F, 0.00028F, 0.00175F),
localAngles = new Vector3(341.3514F, 272.8997F, 119.8906F),
localScale = new Vector3(0.00492F, 0.00492F, 0.00492F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Bandolier,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
childName = "Spine_2",
localPos = new Vector3(0.00017F, -0.00018F, 0F),
localAngles = new Vector3(356.3298F, 224.0936F, 177.5675F),
localScale = new Vector3(0.00616F, 0.00046F, 0.00595F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BounceNearby,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
childName = "Head",
localPos = new Vector3(-0.00076F, 0.0004F, -0.00002F),
localAngles = new Vector3(290.3197F, 88.99999F, 0F),
localScale = new Vector3(0.00937F, 0.00502F, 0.00502F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IgniteOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
childName = "Spine_1",
localPos = new Vector3(0.00342F, 0.0004F, 0.00176F),
localAngles = new Vector3(12.09049F, 87.9631F, 159.0582F),
localScale = new Vector3(-0.00772F, -0.00772F, -0.00772F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.StunChanceOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
childName = "Spine_1",
localPos = new Vector3(0.00149F, 0.00139F, -0.0006F),
localAngles = new Vector3(3.56163F, 269.6879F, 269.9368F),
localScale = new Vector3(0.00701F, 0.00701F, 0.00701F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Firework,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
childName = "Spine_2",
localPos = new Vector3(-0.00312F, -0.00098F, -0.00144F),
localAngles = new Vector3(357.4473F, 245.3878F, 12.25656F),
localScale = new Vector3(0.005F, 0.005F, 0.005F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarDagger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
childName = "Spine_2",
localPos = new Vector3(-0.00277F, -0.00164F, -0.0016F),
localAngles = new Vector3(74.69921F, 172.9476F, 171.1663F),
localScale = new Vector3(0.00354F, 0.00354F, 0.00354F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Knurl,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
childName = "Spine_1",
localPos = new Vector3(-0.00321F, 0.00179F, -0.00001F),
localAngles = new Vector3(0.72608F, 291.3995F, 39.67429F),
localScale = new Vector3(0.00036F, 0.00036F, 0.00036F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BeetleGland,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetleGland"),
childName = "Spine_2",
localPos = new Vector3(0.0035F, 0.00039F, 0.00192F),
localAngles = new Vector3(80.76015F, 236.5915F, 290.6528F),
localScale = new Vector3(0.00071F, 0.00071F, 0.00071F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
childName = "Spine_1",
localPos = new Vector3(0.00179F, 0.0012F, -0.00155F),
localAngles = new Vector3(344.3633F, 269.6417F, 341.1981F),
localScale = new Vector3(0.00258F, 0.00258F, 0.00258F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SecondarySkillMagazine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
childName = "Spine_1",
localPos = new Vector3(0.00114F, 0.00099F, 0.00167F),
localAngles = new Vector3(9.23767F, 225.2803F, 48.84676F),
localScale = new Vector3(0.00059F, 0.00059F, 0.00059F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.StickyBomb,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
childName = "Spine_1",
localPos = new Vector3(0.00068F, -0.00047F, -0.00159F),
localAngles = new Vector3(357.4027F, 193.0614F, 265.9971F),
localScale = new Vector3(0.00276F, 0.00276F, 0.00276F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.TreasureCache,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
childName = "Spine_1",
localPos = new Vector3(0.00071F, 0.00107F, 0.00164F),
localAngles = new Vector3(57.8522F, 351.5668F, 179.0171F),
localScale = new Vector3(0.00532F, 0.00532F, 0.00532F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BossDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
childName = "Spine_2",
localPos = new Vector3(-0.00012F, 0.00185F, -0.00004F),
localAngles = new Vector3(1.20841F, 135.9041F, 169.2882F),
localScale = new Vector3(0.00395F, 0.00395F, 0.00395F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SlowOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
childName = "Spine_2",
localPos = new Vector3(0.00326F, 0.00496F, 0.00087F),
localAngles = new Vector3(6.95573F, 4.92211F, 95.55271F),
localScale = new Vector3(0.00608F, 0.00608F, 0.00608F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ExtraLife,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
childName = "Spine_1",
localPos = new Vector3(0.0009F, 0.00208F, 0.00102F),
localAngles = new Vector3(63.3436F, 111.7035F, 6.38693F),
localScale = new Vector3(-0.00177F, -0.00177F, -0.00177F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.KillEliteFrenzy,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
childName = "Head",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0.03596F, 359.8479F, 80.20514F),
localScale = new Vector3(-0.00327F, 0.00231F, -0.00342F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.RepeatHeal,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
childName = "Head",
localPos = new Vector3(0.00014F, 0F, 0F),
localAngles = new Vector3(270F, 90F, 0F),
localScale = new Vector3(0.00783F, 0.00783F, 0.00783F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.AutoCastEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
childName = "Leg_1_R",
localPos = new Vector3(0.00341F, -0.00013F, -0.00003F),
localAngles = new Vector3(7.38151F, 0.04676F, 17.87387F),
localScale = new Vector3(-0.00502F, -0.00502F, -0.00502F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IncreaseHealing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(-0.00106F, 0.00109F, -0.00033F),
localAngles = new Vector3(0.76415F, 212.3219F, 321.688F),
localScale = new Vector3(0.00222F, 0.00222F, 0.00222F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(-0.00106F, 0.00109F, 0.00067F),
localAngles = new Vector3(358.3875F, 140.8856F, 321.9834F),
localScale = new Vector3(0.00222F, 0.00222F, -0.00222F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.TitanGoldDuringTP,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
childName = "Spine_2",
localPos = new Vector3(-0.0033F, -0.00125F, 0.00361F),
localAngles = new Vector3(10.32203F, 331.5992F, 72.57765F),
localScale = new Vector3(0.00177F, 0.00177F, 0.00177F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintWisp,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
childName = "Arm_1_L",
localPos = new Vector3(-0.0009F, -0.00023F, -0.00065F),
localAngles = new Vector3(358.6624F, 12.30811F, 90.38126F),
localScale = new Vector3(-0.00206F, -0.00206F, -0.00206F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BarrierOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
childName = "Arm_1_L",
localPos = new Vector3(-0.00056F, -0.00022F, -0.00056F),
localAngles = new Vector3(2.89097F, 268.5808F, 108.1329F),
localScale = new Vector3(0.00786F, 0.00786F, 0.00786F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.TPHealingNova,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
childName = "Spine_2",
localPos = new Vector3(-0.00135F, 0.00162F, 0.00075F),
localAngles = new Vector3(80.82395F, 152.7265F, 60.47967F),
localScale = new Vector3(0.00248F, 0.00248F, 0.00025F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarUtilityReplacement,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
childName = "Spine_2",
localPos = new Vector3(-0.00338F, 0.00033F, 0.00209F),
localAngles = new Vector3(357.2255F, 191.6881F, 253.2039F),
localScale = new Vector3(0.00834F, 0.00834F, -0.00988F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Thorns,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
childName = "Spine_1",
localPos = new Vector3(0.00345F, -0.00003F, -0.00023F),
localAngles = new Vector3(2.26605F, 85.81224F, 1.85289F),
localScale = new Vector3(-0.01137F, -0.0115F, -0.00739F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarPrimaryReplacement,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
childName = "Head",
localPos = new Vector3(-0.00067F, 0.00129F, 0.00047F),
localAngles = new Vector3(0.46253F, 1.86414F, 1.57795F),
localScale = new Vector3(-0.00116F, -0.00125F, -0.00116F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.NovaOnLowHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
childName = "Head",
localPos = new Vector3(0.00133F, 0.00005F, -0.0007F),
localAngles = new Vector3(55.9383F, 104.6986F, 16.57966F),
localScale = new Vector3(-0.00147F, -0.0016F, -0.0016F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarTrinket,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
childName = "Head",
localPos = new Vector3(0.00252F, 0.00096F, -0.00007F),
localAngles = new Vector3(24.33764F, 29.64393F, 217.5904F),
localScale = new Vector3(0.01612F, 0.01391F, 0.01391F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Plant,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
childName = "Spine_2",
localPos = new Vector3(-0.00291F, -0.00006F, 0.00176F),
localAngles = new Vector3(354.0984F, 269.5106F, 54.03241F),
localScale = new Vector3(0.00073F, 0.00073F, 0.00073F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Bear,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
childName = "Spine_2",
localPos = new Vector3(-0.00002F, -0.00205F, -0.00004F),
localAngles = new Vector3(81.19859F, 273.4605F, 3.92822F),
localScale = new Vector3(0.00301F, 0.00301F, 0.00301F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.DeathMark,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
childName = "Head",
localPos = new Vector3(-0.00057F, 0.00058F, 0F),
localAngles = new Vector3(4.84932F, 266.6667F, 186.1772F),
localScale = new Vector3(0.0007F, 0.0007F, 0.0007F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ExplodeOnDeath,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
childName = "Spine_1",
localPos = new Vector3(0.00107F, 0.00061F, -0.00212F),
localAngles = new Vector3(359.868F, 0.54344F, 85.74318F),
localScale = new Vector3(0.00039F, 0.00039F, 0.00039F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Seed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
childName = "Spine_2",
localPos = new Vector3(0.00291F, 0.00029F, -0.00217F),
localAngles = new Vector3(64.38377F, 235.0652F, 327.1661F),
localScale = new Vector3(0.00033F, 0.00033F, 0.00033F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintOutOfCombat,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
childName = "Spine_1",
localPos = new Vector3(0.00215F, 0.00028F, 0.00196F),
localAngles = new Vector3(355.779F, 83.00674F, 344.3113F),
localScale = new Vector3(0.00486F, 0.00475F, 0.00486F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            /*
            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.CooldownOnCrit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkull"),
childName = "Root",
localPos = new Vector3(0F, 0.3997F, 0F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.2789F, 0.2789F, 0.2789F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            */

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Phasing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00145F, -0.00175F, -0.004F),
localAngles = new Vector3(12.97638F, 129.8512F, 67.72446F),
localScale = new Vector3(0.00351F, 0.00395F, 0.00386F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.PersonalShield,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
childName = "Spine_2",
localPos = new Vector3(-0.00425F, 0.00076F, 0F),
localAngles = new Vector3(7.43012F, 89.2873F, 182.1711F),
localScale = new Vector3(0.00274F, 0.00405F, 0.00274F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ShockNearby,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
childName = "Head",
localPos = new Vector3(-0.00169F, 0.00026F, 0F),
localAngles = new Vector3(273.4226F, 223.4632F, 226.2741F),
localScale = new Vector3(0.00484F, 0.00484F, 0.00484F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ShieldOnly,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
childName = "Head",
localPos = new Vector3(-0.00083F, -0.00003F, 0F),
localAngles = new Vector3(10.89128F, 158.5296F, 19.6292F),
localScale = new Vector3(0.00167F, 0.00167F, 0.00167F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
childName = "Head",
localPos = new Vector3(-0.0007F, -0.00002F, 0F),
localAngles = new Vector3(349.0289F, 200.5136F, 14.80645F),
localScale = new Vector3(0.00167F, 0.00167F, -0.00167F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.AlienHead,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
childName = "Spine_2",
localPos = new Vector3(0.00247F, 0.00098F, 0.00171F),
localAngles = new Vector3(35.39117F, 285.4119F, 64.83715F),
localScale = new Vector3(0.00944F, 0.00944F, 0.00944F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.HeadHunter,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
childName = "Head",
localPos = new Vector3(-0.00138F, 0.00042F, 0F),
localAngles = new Vector3(270.4178F, 89.10688F, 1.85081F),
localScale = new Vector3(0.00592F, 0.00226F, 0.0013F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.EnergizedOnEquipmentUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
childName = "Spine_2",
localPos = new Vector3(-0.0012F, 0.00156F, 0.00011F),
localAngles = new Vector3(69.43757F, 89.31647F, 188.0994F),
localScale = new Vector3(0.00301F, 0.00301F, 0.00301F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FlatHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
childName = "Head",
localPos = new Vector3(0.00014F, 0.00153F, 0F),
localAngles = new Vector3(13.87463F, 95.97186F, 3.12999F),
localScale = new Vector3(-0.00091F, -0.00085F, -0.00085F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Tooth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
childName = "Head",
localPos = new Vector3(-0.00004F, 0.00122F, 0.00001F),
localAngles = new Vector3(59.77426F, 97.10819F, 192.0412F),
localScale = new Vector3(0.01269F, 0.01269F, 0.01269F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Pearl,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
childName = "Head",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(2.15192F, 271.4372F, 357.7609F),
localScale = new Vector3(0.00171F, 0.00171F, 0.00171F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ShinyPearl,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
childName = "Head",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(2.15192F, 271.4372F, 357.7609F),
localScale = new Vector3(0.00171F, 0.00171F, 0.00171F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BonusGoldPackOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
childName = "Spine_1",
localPos = new Vector3(0.0011F, 0.00099F, 0.0014F),
localAngles = new Vector3(41.54558F, 174.1727F, 248.0228F),
localScale = new Vector3(-0.00074F, -0.00074F, -0.00074F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Squid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
childName = "Leg_1_R",
localPos = new Vector3(0.00389F, 0.00071F, -0.00003F),
localAngles = new Vector3(335.7159F, 99.92127F, 16.24356F),
localScale = new Vector3(0.00073F, 0.00073F, 0.00073F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Icicle,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
childName = "Spine_2",
localPos = new Vector3(-0.00467F, 0.00032F, 0.00381F),
localAngles = new Vector3(359.6196F, 359.4408F, 355.1894F),
localScale = new Vector3(1.22597F, 1.22597F, 1.22597F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Talisman,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
childName = "Head",
localPos = new Vector3(-0.00016F, 0F, 0.00439F),
localAngles = new Vector3(273.5862F, 219.2247F, 225.1412F),
localScale = new Vector3(0.51068F, 0.51068F, 0.51068F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LaserTurbine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
childName = "Leg_1_L",
localPos = new Vector3(-0.0026F, -0.00009F, 0F),
localAngles = new Vector3(6.72573F, 86.4082F, 186.5172F),
localScale = new Vector3(-0.00329F, -0.00329F, -0.00329F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FocusConvergence,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
childName = "Head",
localPos = new Vector3(-0.00017F, 0.00347F, -0.00258F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.07886F, 0.07886F, 0.07886F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            /*
            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Incubator,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
childName = "Root",
localPos = new Vector3(0F, 0.3453F, 0F),
localAngles = new Vector3(353.0521F, 317.2421F, 69.6292F),
localScale = new Vector3(0.0528F, 0.0528F, 0.0528F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            */

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FireballsOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
childName = "Ankle_L",
localPos = new Vector3(-0.00221F, 0.00072F, -0.00003F),
localAngles = new Vector3(307.4499F, 272.9737F, 3.96167F),
localScale = new Vector3(0.00037F, 0.00037F, 0.00037F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SiphonOnLowHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
childName = "Spine_2",
localPos = new Vector3(0.00308F, -0.00017F, -0.00152F),
localAngles = new Vector3(3.31278F, 344.9946F, 77.89365F),
localScale = new Vector3(0.00111F, 0.00111F, 0.00111F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BleedOnHitAndExplode,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
childName = "Ankle_L",
localPos = new Vector3(0.00024F, -0.00012F, -0.00044F),
localAngles = new Vector3(356.5298F, 2.36411F, 346.1515F),
localScale = new Vector3(-0.00085F, -0.00085F, -0.00085F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.MonstersOnShrineUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
childName = "Arm_1_L",
localPos = new Vector3(-0.00578F, 0.00109F, -0.00177F),
localAngles = new Vector3(335.9256F, 132.6315F, 329.6471F),
localScale = new Vector3(0.00175F, 0.00175F, 0.00175F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.RandomDamageZone,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
childName = "Spine_2",
localPos = new Vector3(-0.00032F, -0.00203F, 0F),
localAngles = new Vector3(281.7598F, 189.2777F, 261.7133F),
localScale = new Vector3(0.00133F, 0.00133F, 0.00133F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Fruit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
childName = "Head",
localPos = new Vector3(0.00385F, 0.00119F, -0.00106F),
localAngles = new Vector3(329.6442F, 19.59575F, 188.3629F),
localScale = new Vector3(0.00187F, 0.00187F, 0.00187F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
childName = "Head",
localPos = new Vector3(-0.00068F, -0.00003F, 0F),
localAngles = new Vector3(357.638F, 70.6295F, 358.3376F),
localScale = new Vector3(0.0007F, 0.0007F, 0.0007F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
childName = "Head",
localPos = new Vector3(-0.00031F, -0.00001F, 0.00041F),
localAngles = new Vector3(357.899F, 127.1866F, 356.7536F),
localScale = new Vector3(0.0007F, 0.0007F, 0.0007F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
childName = "Head",
localPos = new Vector3(-0.00157F, 0.00124F, 0F),
localAngles = new Vector3(42.44582F, 90.50886F, 2.27679F),
localScale = new Vector3(-0.00332F, -0.00332F, -0.00332F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
childName = "Head",
localPos = new Vector3(-0.00157F, 0.00124F, 0F),
localAngles = new Vector3(42.44582F, 90.50886F, 2.27679F),
localScale = new Vector3(-0.00332F, -0.00332F, -0.00332F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
childName = "Head",
localPos = new Vector3(-0.00132F, -0.00005F, 0F),
localAngles = new Vector3(4.55803F, 272.6534F, 178.6656F),
localScale = new Vector3(0.0002F, 0.0002F, 0.0002F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
childName = "Head",
localPos = new Vector3(-0.00132F, -0.00005F, 0F),
localAngles = new Vector3(4.55803F, 272.6534F, 178.6656F),
localScale = new Vector3(0.00041F, 0.00041F, 0.00041F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
childName = "Head",
localPos = new Vector3(-0.0011F, -0.00004F, 0F),
localAngles = new Vector3(2.30499F, 272.5617F, 180.7518F),
localScale = new Vector3(0.00044F, 0.00044F, 0.00044F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.CritOnUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
childName = "Head",
localPos = new Vector3(-0.00011F, 0.00273F, 0F),
localAngles = new Vector3(271.7174F, 86.0117F, 183.2281F),
localScale = new Vector3(0.00186F, 0.00186F, 0.00186F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.DroneBackup,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
childName = "Spine_1",
localPos = new Vector3(0.00033F, 0.00133F, -0.00111F),
localAngles = new Vector3(296.6234F, 171.1341F, 294.8875F),
localScale = new Vector3(0.00252F, 0.00252F, 0.00252F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Lightning,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
childName = "Arm_1_L",
localPos = new Vector3(0.00058F, 0.00126F, 0.00121F),
localAngles = new Vector3(309.0784F, 30.26222F, 293.19F),
localScale = new Vector3(0.0047F, 0.0047F, 0.0047F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.BurnNearby,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
childName = "Spine_1",
localPos = new Vector3(0.0014F, 0.00071F, -0.00203F),
localAngles = new Vector3(352.951F, 16.56111F, 67.83619F),
localScale = new Vector3(0.00036F, 0.00036F, 0.00036F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.CrippleWard,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
childName = "Head",
localPos = new Vector3(0.00711F, 0.00133F, 0.00197F),
localAngles = new Vector3(27.69918F, 173.5264F, 264.6112F),
localScale = new Vector3(0.00359F, 0.00359F, 0.00359F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
childName = "Spine_2",
localPos = new Vector3(-0.0016F, -0.00129F, -0.00001F),
localAngles = new Vector3(282.0249F, 44.724F, 322.4294F),
localScale = new Vector3(0.00227F, 0.00227F, 0.00192F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.GainArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
childName = "Spine_1",
localPos = new Vector3(0F, 0F, 0.00171F),
localAngles = new Vector3(289.257F, 219.673F, 245.2141F),
localScale = new Vector3(0.00414F, 0.00414F, 0.00438F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Recycle,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
childName = "Spine_1",
localPos = new Vector3(0.00199F, 0.00084F, 0.00195F),
localAngles = new Vector3(66.20851F, 351.6816F, 83.13299F),
localScale = new Vector3(0.0006F, 0.0006F, 0.0006F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.FireBallDash,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
childName = "Spine_1",
localPos = new Vector3(0.0005F, 0.00046F, 0.00178F),
localAngles = new Vector3(6.29985F, 258.69F, 89.97671F),
localScale = new Vector3(0.00281F, 0.00281F, 0.00281F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Cleanse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
childName = "Spine_2",
localPos = new Vector3(0.00104F, -0.0005F, -0.00009F),
localAngles = new Vector3(69.8809F, 259.7795F, 352.1404F),
localScale = new Vector3(0.00166F, 0.00166F, 0.00166F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Tonic,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
childName = "Spine_1",
localPos = new Vector3(0.00013F, -0.00002F, -0.00197F),
localAngles = new Vector3(17.77056F, 189.6838F, 95.5756F),
localScale = new Vector3(-0.00127F, -0.00127F, -0.00127F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Gateway,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
childName = "Arm_1_L",
localPos = new Vector3(0.00039F, -0.00027F, -0.00077F),
localAngles = new Vector3(339.6764F, 198.4896F, 125.4604F),
localScale = new Vector3(0.0028F, 0.0028F, 0.0028F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Meteor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
childName = "Head",
localPos = new Vector3(0F, 0F, 0.00489F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.91016F, 0.91016F, 0.91016F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Saw,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
childName = "Head",
localPos = new Vector3(0.00022F, -0.0048F, 0.00586F),
localAngles = new Vector3(358.4698F, 88.18073F, 290.429F),
localScale = new Vector3(-0.00148F, -0.00148F, -0.00148F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Blackhole,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
childName = "Head",
localPos = new Vector3(-0.0002F, -0.00001F, -0.00279F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Scanner,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00015F, -0.00016F, -0.00036F),
localAngles = new Vector3(331.5351F, 323.3058F, 4.78554F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.DeathProjectile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
childName = "Spine_1",
localPos = new Vector3(0.00051F, 0.00187F, 0.00069F),
localAngles = new Vector3(283.2257F, 326.9691F, 106.0962F),
localScale = new Vector3(0.00042F, 0.00042F, 0.00042F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.LifestealOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
childName = "Leg_1_L",
localPos = new Vector3(-0.00156F, 0.00054F, 0.00126F),
localAngles = new Vector3(331.7603F, 282.425F, 220.5234F),
localScale = new Vector3(-0.00031F, -0.00035F, -0.00031F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.TeamWarCry,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
childName = "Spine_2",
localPos = new Vector3(0.00099F, -0.00219F, -0.00002F),
localAngles = new Vector3(285.6679F, 58.09454F, 215.7021F),
localScale = new Vector3(-0.00191F, -0.00191F, -0.00148F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });



            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.AttackSpeedAndMoveSpeed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCoffee"),
childName = "Leg_1_L",
localPos = new Vector3(-0.00022F, -0.00005F, -0.00102F),
localAngles = new Vector3(358.614F, 176.1062F, 90.94106F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.BearVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBearVoid"),
childName = "Spine_2",
localPos = new Vector3(-0.00002F, -0.00205F, -0.00004F),
localAngles = new Vector3(81.19864F, 273.4605F, 3.92819F),
localScale = new Vector3(0.00301F, 0.00301F, 0.00301F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.BleedOnHitVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
childName = "Spine_2",
localPos = new Vector3(-0.00363F, -0.00023F, 0.00225F),
localAngles = new Vector3(9.14773F, 306.9309F, 162.7443F),
localScale = new Vector3(-0.00532F, -0.00532F, -0.00532F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ChainLightningVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
childName = "Spine_2",
localPos = new Vector3(-0.00032F, -0.00185F, 0.00002F),
localAngles = new Vector3(286.6041F, 88.5617F, 151.7254F),
localScale = new Vector3(-0.00569F, -0.00569F, -0.00569F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CloverVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCloverVoid"),
childName = "Spine_2",
localPos = new Vector3(-0.00088F, 0.00166F, -0.00061F),
localAngles = new Vector3(13.79218F, 179.8981F, 359.7485F),
localScale = new Vector3(0.0022F, 0.00228F, 0.0022F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CritDamage,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserSight"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00058F, 0.00004F, -0.00017F),
localAngles = new Vector3(333.3669F, 38.70263F, 67.23852F),
localScale = new Vector3(0.00048F, 0.00048F, 0.00048F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CritGlassesVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
childName = "Head",
localPos = new Vector3(-0.00058F, 0.00138F, 0F),
localAngles = new Vector3(274.0444F, 118.1071F, 333.7419F),
localScale = new Vector3(0.00197F, 0.00186F, 0.00186F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ElementalRingVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVoidRing"),
childName = "Wrist_R",
localPos = new Vector3(0.00125F, 0.00022F, -0.00009F),
localAngles = new Vector3(358.7645F, 106.3358F, 257.4453F),
localScale = new Vector3(0.00152F, 0.00152F, 0.00152F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.LunarSun,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySunHead"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00015F, -0.00016F, -0.00036F),
localAngles = new Vector3(331.5351F, 323.3058F, 4.78554F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.LunarSun,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00015F, -0.00016F, -0.00036F),
localAngles = new Vector3(331.5351F, 323.3058F, 4.78554F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.EquipmentMagazineVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
childName = "Spine_1",
localPos = new Vector3(0.00213F, 0.00107F, -0.0019F),
localAngles = new Vector3(6.18004F, 277.9501F, 292.5677F),
localScale = new Vector3(0.00109F, 0.00109F, 0.00109F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ExplodeOnDeathVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
childName = "Spine_1",
localPos = new Vector3(0.00107F, 0.00061F, -0.00212F),
localAngles = new Vector3(359.868F, 0.54344F, 85.74317F),
localScale = new Vector3(0.00039F, 0.00039F, 0.00039F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ExtraLifeVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippoVoid"),
childName = "Spine_1",
localPos = new Vector3(0.0009F, 0.00208F, 0.00102F),
localAngles = new Vector3(63.34361F, 111.7035F, 6.38692F),
localScale = new Vector3(-0.00177F, -0.00177F, -0.00177F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.FragileDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
childName = "Wrist_R",
localPos = new Vector3(0.00002F, 0.00007F, 0.00001F),
localAngles = new Vector3(0.39067F, 265.9609F, 278.2188F),
localScale = new Vector3(0.0023F, 0.00387F, 0.00317F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.FreeChest,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
childName = "Spine_1",
localPos = new Vector3(0.00121F, -0.00035F, -0.00211F),
localAngles = new Vector3(8.11174F, 113.7442F, 252.1412F),
localScale = new Vector3(0.0039F, 0.0039F, 0.0039F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HalfAttackSpeedHalfCooldowns,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00004F, 0.0003F, 0.00017F),
localAngles = new Vector3(296.9232F, 179.5322F, 311.6152F),
localScale = new Vector3(0.00404F, 0.00404F, 0.00404F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HalfSpeedDoubleHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
childName = "Arm_1_L",
localPos = new Vector3(-0.00011F, -0.00022F, -0.00019F),
localAngles = new Vector3(295.7096F, 176.8642F, 110.464F),
localScale = new Vector3(0.00517F, 0.00517F, 0.00517F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HealingPotion,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHealingPotion"),
childName = "Spine_1",
localPos = new Vector3(0.00099F, 0.00102F, 0.00175F),
localAngles = new Vector3(41.1794F, 341.6209F, 100.0668F),
localScale = new Vector3(0.00034F, 0.00034F, 0.00034F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ImmuneToDebuff,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
childName = "Spine_1",
localPos = new Vector3(0.00051F, 0.00028F, 0.00031F),
localAngles = new Vector3(275.3081F, 340.451F, 112.2108F),
localScale = new Vector3(0.01134F, 0.00909F, 0.00834F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MinorConstructOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
childName = "Spine_2",
localPos = new Vector3(-0.00414F, -0.00234F, -0.00459F),
localAngles = new Vector3(77.80402F, 217.4886F, 310.7868F),
localScale = new Vector3(0.34879F, 0.34879F, 0.34879F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MissileVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
childName = "Spine_2",
localPos = new Vector3(-0.00479F, 0.00007F, -0.00263F),
localAngles = new Vector3(280.3456F, 66.67075F, 6.73252F),
localScale = new Vector3(0.00075F, 0.00075F, 0.00075F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MoreMissile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayICBM"),
childName = "Spine_2",
localPos = new Vector3(-0.00079F, -0.00161F, -0.00037F),
localAngles = new Vector3(309.6678F, 307.1359F, 104.2177F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MoveSpeedOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGrappleHook"),
childName = "Spine_1",
localPos = new Vector3(0.001F, -0.00104F, -0.00123F),
localAngles = new Vector3(354.8669F, 354.6142F, 79.05666F),
localScale = new Vector3(0.00116F, 0.00116F, 0.00116F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });


            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MushroomVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
childName = "Ankle_R",
localPos = new Vector3(0.00171F, -0.00025F, 0.00019F),
localAngles = new Vector3(352.4758F, 192.9588F, 203.6024F),
localScale = new Vector3(0.00043F, 0.00043F, 0.00043F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.OutOfCombatArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
childName = "Spine_2",
localPos = new Vector3(-0.0015F, 0.00131F, 0.00003F),
localAngles = new Vector3(67.64236F, 90.15549F, 183.0891F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.PermanentDebuffOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScorpion"),
childName = "Spine_1",
localPos = new Vector3(0.00135F, -0.00103F, -0.00034F),
localAngles = new Vector3(278.3848F, 111.7765F, 344.8164F),
localScale = new Vector3(0.00899F, 0.00899F, 0.00899F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.PrimarySkillShuriken,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShuriken"),
childName = "Wrist_R",
localPos = new Vector3(0.0004F, 0.00015F, 0.00042F),
localAngles = new Vector3(346.3761F, 2.02263F, 345.6077F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RandomEquipmentTrigger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBottledChaos"),
childName = "Spine_1",
localPos = new Vector3(0.00231F, -0.00021F, 0.00242F),
localAngles = new Vector3(353.062F, 342.0303F, 89.63073F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RandomlyLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDomino"),
childName = "Spine_2",
localPos = new Vector3(-0.00644F, -0.00191F, -0.00234F),
localAngles = new Vector3(348.979F, 261.4951F, 10.25595F),
localScale = new Vector3(1.12851F, 1.12851F, 1.12851F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RegeneratingScrap,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
childName = "Spine_1",
localPos = new Vector3(0.00047F, -0.0007F, 0.00103F),
localAngles = new Vector3(297.6038F, 212.7115F, 236.4136F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.SlowOnHitVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
childName = "Spine_2",
localPos = new Vector3(0.00326F, 0.00496F, 0.00087F),
localAngles = new Vector3(6.95573F, 4.92211F, 95.55271F),
localScale = new Vector3(0.00608F, 0.00608F, 0.00608F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.StrengthenBurn,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasTank"),
childName = "Spine_1",
localPos = new Vector3(0.00125F, -0.00021F, -0.00185F),
localAngles = new Vector3(16.27512F, 15.98508F, 89.9665F),
localScale = new Vector3(0.00112F, 0.00112F, 0.00112F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.TreasureCacheVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKeyVoid"),
childName = "Spine_1",
localPos = new Vector3(0.00071F, 0.00107F, 0.00164F),
localAngles = new Vector3(57.85221F, 351.5668F, 179.0171F),
localScale = new Vector3(0.00532F, 0.00532F, 0.00532F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.VoidMegaCrabItem,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
childName = "Spine_1",
localPos = new Vector3(0.00122F, 0.00125F, 0F),
localAngles = new Vector3(277.7285F, 80.47313F, 8.49707F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunter,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTricornGhost"),
childName = "Arm_1_R",
localPos = new Vector3(-0.00015F, -0.00016F, -0.00036F),
localAngles = new Vector3(331.5351F, 323.3058F, 4.78554F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunter,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
childName = "Spine_2",
localPos = new Vector3(-0.0051F, -0.00315F, -0.00484F),
localAngles = new Vector3(19.65068F, 88.34724F, 5.44067F),
localScale = new Vector3(0.64005F, 0.64005F, 0.64005F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunterConsumed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTricornUsed"),
childName = "Head",
localPos = new Vector3(-0.00189F, 0.00033F, -0.00015F),
localAngles = new Vector3(286.9469F, 97.02748F, 351.0573F),
localScale = new Vector3(0.0076F, 0.00703F, 0.0074F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            // TODO: Find what keyAsset to use            
//            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
//            {
//                keyAsset = DLC1Content.Items.CritDamage,
//                displayRuleGroup = new DisplayRuleGroup
//                {
//                    rules = new ItemDisplayRule[]
//                    {
//                        new ItemDisplayRule
//                        {
//                            ruleType = ItemDisplayRuleType.ParentedPrefab,
//                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
//childName = "Arm_1_R",
//localPos = new Vector3(-0.00015F, -0.00016F, -0.00036F),
//localAngles = new Vector3(331.5351F, 323.3058F, 4.78554F),
//localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
//                            limbMask = LimbFlags.None
//                        }
//                    }
//                }
//            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.EliteVoidEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAffixVoid"),
childName = "Head",
localPos = new Vector3(-0.00043F, 0.00092F, 0F),
localAngles = new Vector3(351.1231F, 89.82127F, 2.34557F),
localScale = new Vector3(0.00143F, 0.00143F, 0.00143F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            // TODO: Find correct keyAsset
//            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
//            {
//                keyAsset = DLC1Content.Equipment.,
//                displayRuleGroup = new DisplayRuleGroup
//                {
//                    rules = new ItemDisplayRule[]
//                    {
//                        new ItemDisplayRule
//                        {
//                            ruleType = ItemDisplayRuleType.ParentedPrefab,
//                            followerPrefab = ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
//childName = "Arm_1_R",
//localPos = new Vector3(-0.00015F, -0.00016F, -0.00036F),
//localAngles = new Vector3(331.5351F, 323.3058F, 4.78554F),
//localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
//                            limbMask = LimbFlags.None
//                        }
//                    }
//                }
//            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.LunarPortalOnUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
childName = "Spine_2",
localPos = new Vector3(-0.00523F, -0.00147F, -0.00514F),
localAngles = new Vector3(280.157F, 37.81123F, 60.46198F),
localScale = new Vector3(0.5018F, 0.5018F, 0.5018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.Molotov,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMolotov"),
childName = "Spine_1",
localPos = new Vector3(0.00157F, 0.00139F, 0.00162F),
localAngles = new Vector3(40.78915F, 340.5419F, 98.40808F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.MultiShopCard,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
childName = "Spine_1",
localPos = new Vector3(0.00119F, 0.00036F, -0.00181F),
localAngles = new Vector3(350.0431F, 102.8576F, 291.0072F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.VendingMachine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVendingMachine"),
childName = "Spine_1",
localPos = new Vector3(0.00118F, 0.00158F, -0.00219F),
localAngles = new Vector3(66.80531F, 264.9326F, 355.8302F),
localScale = new Vector3(0.0018F, 0.0018F, 0.0018F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            #endregion

            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
            itemDisplayRuleSet.GenerateRuntimeValues();
        }

        public void Update()
        {

        }

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];
            newRendererInfos[1].defaultMaterial = materials[1];
            newRendererInfos[instance.mainRendererIndex].defaultMaterial = materials[2];

            return newRendererInfos;
        }
    }
}