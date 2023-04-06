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

        

        public override ItemDisplaysBase itemDisplays => new Modules.LinkItemDisplays();
        public virtual CharacterModel prefabCharacterModel { get; set; }
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

        internal static Material LinkMat = Modules.Assets.CreateMaterial("matLink");
        internal override int mainRendererIndex { get; set; } = 2;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {
                new CustomRendererInfo
                {
                    childName = "mdlLinkHylian",
                },
        };


        internal override Type characterMainState { get; set; } = typeof(SkillStates.BaseStates.LinkMain);

        // item display stuffs
        internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal override UnlockableDef characterUnlockableDef { get; set; }
        private static UnlockableDef masterySkinUnlockableDef;

        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        internal override void InitializeUnlockables()
        {
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
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
            Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)), "Weapon", prefix + "_LINK_BODY_PRIMARY_SWORD_NAME", prefix + "_LINK_BODY_PRIMARY_SWORD_DESCRIPTION", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("MasterSword"), true));
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
                baseRechargeInterval = 1f,
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
                baseRechargeInterval = 2f,
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
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = true,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_SECONDARY_ROLL_NAME",
                skillNameToken = prefix + "_LINK_BODY_SECONDARY_ROLL_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_SECONDARY_ROLL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("GreatEagleBow"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Roll)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });
            SkillDef[] secondaries = { shootSkillDef, shootTriSkillDef, shootFastSkillDef, shieldSkillDef};

            Modules.Skills.AddSecondarySkills(bodyPrefab, secondaries);
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
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,                
            });

            SkillDef passSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_LINK_BODY_PASSIVE_NAME",
                skillNameToken = prefix + "_LINK_BODY_PASSIVE_NAME",
                skillDescriptionToken = prefix + "_LINK_BODY_PASSIVE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Paraglider"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ParagliderTest)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef[] utilities = { bombSkillDef};
            Modules.Skills.AddUtilitySkills(bodyPrefab, utilities);
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
                baseRechargeInterval = 300f,
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

            SkillDef[] specials = { miphaSkillDef, darukSkillDef, revaliSkillDef, urbosaSkillDef };
            Modules.Skills.AddSpecialSkills(bodyPrefab, specials);
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
                mainRenderer,
                model);

            skins.Add(defaultSkin);
            #endregion

            #region RitoSkin            
            SkinDef ritoSkin = Modules.Skins.CreateSkinDef("Snowquill Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("RitoSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            ritoSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers, "mdlLinkRito");

            skins.Add(ritoSkin);
            #endregion

            #region GerudoSkin
            SkinDef gerudoSkin = Modules.Skins.CreateSkinDef("Gerudo Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("GerudoSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            gerudoSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers, "mdlLinkGerudo");

            skins.Add(gerudoSkin);
            #endregion

            #region WildSkin
            SkinDef wildSkin = Modules.Skins.CreateSkinDef("Wild Outfit",
                Assets.mainAssetBundle.LoadAsset<Sprite>("WildSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            wildSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers, "mdlLinkWild");

            skins.Add(wildSkin);
            #endregion

            #region DarkSkin            
            SkinDef darkSkin = Modules.Skins.CreateSkinDef("Dark Link",
                Assets.mainAssetBundle.LoadAsset<Sprite>("DarkSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            darkSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers, "mdlLinkDark");

            skins.Add(darkSkin);
            #endregion

            #region MasterySkin            
            SkinDef masterySkin = Modules.Skins.CreateSkinDef("Champion's Tunic",
                Assets.mainAssetBundle.LoadAsset<Sprite>("MasterySkin"),
                defaultRenderers,
                mainRenderer,
                model);

            //masterySkin.meshReplacements = new SkinDef.MeshReplacement[] {
            //    new SkinDef.MeshReplacement
            //    {
            //        mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("Boots_160__Mt_Lower_160"),
            //        renderer = mainRenderer,
            //    },
            //};
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers, "mdlLinkChampion");
            skins.Add(masterySkin);
            #endregion

            

            skinController.skins = skins.ToArray();
        }

        internal override void InitializeItemDisplays()
        {
            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;            
            prefabCharacterModel = Modules.Prefabs.SetupCharacterModelNew(bodyPrefab, customRendererInfos);
            prefabCharacterModel.itemDisplayRuleSet = itemDisplayRuleSet;

            if (itemDisplays != null)
            {
                RoR2.ContentManagement.ContentManager.onContentPacksAssigned += SetItemDisplays;
            }
        }
        public void SetItemDisplays(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            itemDisplays.SetItemDisplays(prefabCharacterModel.itemDisplayRuleSet);
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