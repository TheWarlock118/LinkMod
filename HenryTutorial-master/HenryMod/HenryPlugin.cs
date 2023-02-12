using BepInEx;
using BepInEx.Configuration;
using HenryMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
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

        private bool resed = false;
        private bool miphaOnCooldown = false;
        private bool urbosaOnCooldown = false;
        private bool revaliOnCooldown = false;
        private bool darukOnCooldown = false;

        // Maybe use this instead of resed?
        private float stopwatch = -1f;

        


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
            SkillLocator skillLocator = self.GetComponent<SkillLocator>();
            //Mipha's Grace Functionality - Remove Consumed Dio's, Start Cooldown on Res

            try
            {
                if(stopwatch > -1f)
                    Log.LogDebug("Stopwatch = " + stopwatch.ToString());
            } catch { }
            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special) != null)
                {
                    if (stopwatch > 0f)
                        stopwatch -= Time.fixedDeltaTime;
                    else if (stopwatch != -1f && stopwatch < 0f)
                    {
                        if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                        {
                            if (self.healthComponent.alive)
                            {
                                Log.LogDebug("Removing Stock & Extra Item");
                                skillLocator.GetSkill(SkillSlot.Special).DeductStock(1);
                                self.inventory.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                                stopwatch = -1f;
                            }
                        }
                    }

                }
            }


            #region ChampionReady

            if (skillLocator && skillLocator.GetSkill(SkillSlot.Special) != null)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        miphaOnCooldown = true;
                    }
                    else if (miphaOnCooldown && stopwatch == -1f)
                    {
                        miphaOnCooldown = false;
                        if (Modules.Config.MiphaReadySound.Value)
                            Util.PlaySound("MiphasGraceReady", self.gameObject);
                    }
                }

                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_DARUK_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        darukOnCooldown = true;
                    }
                    else if (darukOnCooldown)
                    {
                        darukOnCooldown = false;
                        if (Modules.Config.DarukReadySound.Value)
                            Util.PlaySound("DaruksProtectionReady", self.gameObject);
                    }
                }

                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_URBOSA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        urbosaOnCooldown = true;
                    }
                    if (urbosaOnCooldown)
                    {
                        urbosaOnCooldown = false;
                        if (Modules.Config.UrbosaReadySound.Value)
                            Util.PlaySound("UrbosasFuryReady", self.gameObject);
                    }

                }
           
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_REVALI_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        revaliOnCooldown = true;
                    }
                    else if (revaliOnCooldown)
                    {
                        revaliOnCooldown = false;
                        if (Modules.Config.RevaliReadySound.Value)
                            Util.PlaySound("RevalisGaleReady", self.gameObject);
                    }

                }
            }

            #endregion
            

            //Paraglider & Slow-Bow Functionality
            
            if (self.inputBank)
            {
                if (skillLocator && skillLocator.GetSkill(SkillSlot.Primary) != null)
                {
                    if (skillLocator.GetSkill(SkillSlot.Primary).skillDef.skillName == "ROB_HENRY_BODY_PRIMARY_SWORD_NAME")// Check to make sure only Link is affected
                    {
                        if (self.inputBank.skill2.down && self.characterMotor.velocity.y < 0f)
                        {
                            if (skillLocator.GetSkill(SkillSlot.Secondary).cooldownRemaining == skillLocator.GetSkill(SkillSlot.Secondary).baseRechargeInterval) //If bow is not mid cooldown, allow for extreme slowfall
                            {
                                self.characterMotor.velocity = new Vector3(0f, 0f, 0f);
                            }

                        }
                        else if (self.inputBank.jump.down && self.characterMotor.velocity.y < 0f && !self.characterMotor.isGrounded)
                        {
                            self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, -3f, self.characterMotor.velocity.z);
                            if (self.inputBank.skill2.down)
                            {
                                self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, 0f, self.characterMotor.velocity.z);
                            }
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
                        
            if (self)
            {
                CharacterBody characterBody = self.GetComponent<CharacterBody>();                
                SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();                
                if (skillLocator && characterBody && skillLocator.GetSkill(SkillSlot.Special) != null)
                {                    
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                    {
                        if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f)) {
                            if (!characterBody.healthComponent.alive && !characterBody.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("ExtraLife")))
                            {                                
                                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
                                Util.PlaySound("MiphasGraceUse", characterBody.gameObject);
                                stopwatch = 0.25f;
//                                resed = true;                                
                            }
                        }                        
                    }
                    
                }
                
            }    
            
        }
        
        // Deprecated - Left for Animator getting 
        private void PlayDeathAnimation(CharacterBody body)
        {
            Animator bodyAnimator = body.modelLocator.modelTransform.GetComponent<Animator>();
            bodyAnimator.speed = 1f;
            bodyAnimator.Update(0f);
            int layerIndex = bodyAnimator.GetLayerIndex("Gesture, Override");
            bodyAnimator.PlayInFixedTime("Die", layerIndex, 0f);
            Log.LogDebug("Playing Death Animation, or Trying to anyway");
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