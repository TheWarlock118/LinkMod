using BepInEx;
using BepInEx.Configuration;
using HenryMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using BepInEx.Logging;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace HenryMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    public class HenryPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.DeveloperName.MyCharacterMod";
        public const string MODNAME = "MyCharacterMod";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "ROB";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static HenryPlugin instance;

        private bool resed;
        private bool miphaOnCooldown;
        private bool urbosaOnCooldown;
        private bool revaliOnCooldown;
        private bool darukOnCooldown;
        


        private void Awake()
        {
            instance = this;

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;

            Hook();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            Modules.Survivors.MyCharacter.instance.SetItemDisplays();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.Update += CharacterBody_Update;            
        }



        private void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            orig(self);

            SkillLocator skillLocator = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<SkillLocator>();

            //Mipha's Grace Functionality - Remove Consumed Dio's and Start Cooldown on Res
            if (resed)
            {
                
                if (skillLocator)
                {
                    if (self.healthComponent.alive)
                    {
                        skillLocator.GetSkill(SkillSlot.Special).DeductStock(1);
                        self.inventory.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                        
                    }
                }
            }

            #region ChampionReady

            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        miphaOnCooldown = true;
                    }

                }
            }
            if (miphaOnCooldown && skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
            {
                if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                {
                    miphaOnCooldown = false;
                    
                    if (Modules.Config.MiphaReadySound.Value)
                        Util.PlaySound("MiphasGraceReady", self.gameObject);
                }

            }

            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_DARUK_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        darukOnCooldown = true;
                    }

                }
            }
            if (darukOnCooldown && skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_DARUK_NAME")
            {
                if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                {
                    darukOnCooldown = false;
                    if(Modules.Config.DarukReadySound.Value)
                        Util.PlaySound("DaruksProtectionReady", self.gameObject);
                }

            }

            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_URBOSA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        urbosaOnCooldown = true;
                    }

                }
            }
            if (urbosaOnCooldown && skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_URBOSA_NAME")
            {
                if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                {
                    urbosaOnCooldown = false;
                    if(Modules.Config.UrbosaReadySound.Value)
                        Util.PlaySound("UrbosasFuryReady", self.gameObject);
                }

            }

            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_REVALI_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        revaliOnCooldown = true;
                    }

                }
            }
            if (revaliOnCooldown && skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_REVALI_NAME")
            {
                if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                {
                    revaliOnCooldown = false;
                    if(Modules.Config.RevaliReadySound.Value)
                        Util.PlaySound("RevalisGaleReady", self.gameObject);
                }

            }
            #endregion

            //Paraglider & Slow-Bow Functionality
            if (self.inputBank)
            {
                if (skillLocator.GetSkill(SkillSlot.Primary).skillDef.skillName == "ROB_HENRY_BODY_PRIMARY_SWORD_NAME")//Check to make sure only Link is affected
                {
                    if (self.inputBank.jump.down && self.characterMotor.velocity.y < 0f && !self.characterMotor.isGrounded)
                    {
                        self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, -3f, self.characterMotor.velocity.z);
                        if (self.inputBank.skill2.down)
                        {
                            self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, 0f, self.characterMotor.velocity.z);
                        }
                    }

                    if(self.inputBank.skill2.down && self.characterMotor.velocity.y < 0f)
                    {
                            if (((skillLocator.GetSkill(SkillSlot.Secondary).cooldownRemaining == 2f))) //If bow is not mid cooldown, allow for extreme slowfall
                            {
                                self.characterMotor.velocity = new Vector3(0f, 0f, 0f);
                            }                      
                                              
                    }
                }
            }


        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            //Mipha's Grace Functionality - On death, if using Mipha's Grace and no dio's, add dio to inventory and set res to true
            Log.Init(Logger);
            orig(self, damageInfo);
            CharacterBody characterBody = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<CharacterBody>();
            resed = false;
            if (self)
            {
                SkillLocator skillLocator = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<SkillLocator>();
                if (skillLocator)
                {
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                    {
                        if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f)) {
                            if (!characterBody.healthComponent.alive && !characterBody.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("ExtraLife")))
                            {                                
                                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
                                Util.PlaySound("MiphasGraceUse", characterBody.gameObject);
                                resed = true;
                            }
                        }
                        
                    }
                }
            }            
        }


        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.armorBuff))
                {
                    self.armor += 300f;
                }
            }
        }
    }
}