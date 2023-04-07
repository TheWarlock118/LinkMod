using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LinkMod.Modules.Survivors
{
    internal abstract class SurvivorBase
    {
        internal static SurvivorBase instance;

        internal abstract string bodyName { get; set; }

        internal abstract GameObject bodyPrefab { get; set; }
        internal abstract GameObject displayPrefab { get; set; }

        internal abstract float sortPosition { get; set; }

        internal string fullBodyName => bodyName + "Body";

        internal abstract ConfigEntry<bool> characterEnabled { get; set; }

        internal abstract UnlockableDef characterUnlockableDef { get; set; }

        internal abstract BodyInfo bodyInfo { get; set; }

        internal abstract int mainRendererIndex { get; set; }
        internal abstract CustomRendererInfo[] customRendererInfos { get; set; }

        internal abstract Type characterMainState { get; set; }

        internal abstract ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal abstract List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal virtual void Initialize()
        {
            instance = this;
            InitializeCharacter();
        }

        internal virtual void InitializeCharacter()
        {
            // this creates a config option to enable the character- feel free to remove if the character is the only thing in your mod
            characterEnabled = Modules.Config.CharacterEnableConfig(bodyName);

            if (characterEnabled.Value)
            {
                InitializeUnlockables();

                bodyPrefab = Modules.Prefabs.CreatePrefab("LinkBody", "mdlLinkHylian", bodyInfo);
                bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);
                bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BaseStates.DeathState));

                CustomRendererInfo[] customRendererInfosManual = new CustomRendererInfo[] {
                    new CustomRendererInfo
                    {
                        childName = "Boots_001__Mt_Lower_001",
                        material = Modules.Assets.CreateMaterial("Mt_armor_lower"),
                    },
                    new CustomRendererInfo
                    {
                        childName = "Metal_001__Mt_Upper_001",
                        material = Modules.Assets.CreateMaterial("Mt_armor_upper"),
                    },
                    new CustomRendererInfo
                    {
                        childName = "Face__Mt_Face",
                        material = Modules.Assets.CreateMaterial("Mt_Face"),
                    },
                    new CustomRendererInfo
                    {
                        childName = "Boots_001__Mt_Lower_001",
                        material = Modules.Assets.CreateMaterial("Mt_armor_lower"),
                    },
                    new CustomRendererInfo
                    {
                        childName = "Metal_001__Mt_Upper_001",
                        material = Modules.Assets.CreateMaterial("Mt_armor_upper"),
                    },
                };
                Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfosManual, mainRendererIndex);

                displayPrefab = Modules.Prefabs.CreateDisplayPrefab("mdlLinkHylianDisplay", bodyPrefab, bodyInfo);

                Modules.Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, new Color(0.12f, 0.39f, 0.25f, 1), bodyName.ToUpper(), characterUnlockableDef, sortPosition);                
                InitializeSkills();
                InitializeSkins();
                InitializeItemDisplays();
                InitializeDoppelganger();
                InitializeHitboxes();
                Modules.Survivors.MyCharacter.instance.SetItemDisplays();
            }
        }        

        internal virtual void InitializeUnlockables()
        {
        }

        internal virtual void InitializeSkills()
        {            
        }

        internal virtual void InitializeHitboxes()
        {
        }

        internal virtual void InitializeSkins()
        {
        }

        internal virtual void InitializeDoppelganger()
        {
            Modules.Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, bodyName + "MonsterMaster", "Merc");
        }

        internal virtual void InitializeItemDisplays()
        {
            CharacterModel characterModel = bodyPrefab.GetComponentInChildren<CharacterModel>();

            itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        internal virtual void SetItemDisplays()
        {

        }
    }
}
