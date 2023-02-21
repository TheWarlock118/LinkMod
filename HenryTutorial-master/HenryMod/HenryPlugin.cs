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
        private bool enteredParaglider = false;
        private bool playedFall = false;
        private bool blockDaruk = false;
        private bool playedParaEquipSound = false;
        private bool playedParaUnEquipSound = false;
        private bool enteredSlowMo = false;
        private float SlowMotionStopwatch = 0f;
        private float DarukSoundStopwatch = 0f;
                

        


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
            Log.Init(Logger);
            orig(self);
            SkillLocator skillLocator = self.GetComponent<SkillLocator>();

            #region MiphaGraceRemoveDioCooldown
            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special) != null)
                {
                    if (resed)
                    {
                        if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                        {
                            if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                            {
                                Log.LogDebug("Removing Stock & Extra Item");
                                skillLocator.GetSkill(SkillSlot.Special).DeductStock(1);
                                self.inventory.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                                resed = false;
                            }
                        }                        
                    }
                }
            }
            #endregion

            #region ChampionReady

            if (skillLocator && skillLocator.GetSkill(SkillSlot.Special) != null)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        miphaOnCooldown = true;
                    }
                    else if (miphaOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
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
                    else if (darukOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
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
                    if (urbosaOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
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
                    else if (revaliOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        revaliOnCooldown = false;
                        if (Modules.Config.RevaliReadySound.Value)
                            Util.PlaySound("RevalisGaleReady", self.gameObject);
                    }

                }
            }

            #endregion


            #region ParagliderSlow-Bow

            string[] paraEquipSounds = { "Pl_Parashawl_Equip00", "Pl_Parashawl_Equip02", "Pl_Parashawl_Equip04" };
            string[] paraGlideSounds = { "Pl_Parashawl_FlapFast00", "Pl_Parashawl_FlapFast01"};
            string[] paraUnEquipSounds = { "Pl_Parashawl_UnEquip00", "Pl_Parashawl_UnEquip03", "Pl_Parashawl_UnEquip04" };

            if (self.inputBank)
            {
                if (skillLocator && skillLocator.GetSkill(SkillSlot.Special) != null)
                {
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_URBOSA_NAME" || skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_DARUK_NAME" || skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_REVALI_NAME" || skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME" || skillLocator.GetSkill(SkillSlot.Primary).skillDef.skillName == "ROB_HENRY_BODY_PRIMARY_SWORD_NAME")// Check to make sure only Link is affected
                    {
                        Animator animator = self.modelLocator.modelTransform.GetComponent<Animator>();
                        if (enteredParaglider && !self.inputBank.skill2.down && (!self.inputBank.jump.down || self.characterMotor.isGrounded))
                        {
                            if (!playedFall)
                            {
                                
                                animator.Play("Fall", 2);
                                playedFall = true;
                                playedParaEquipSound = false;

                                if (!playedParaUnEquipSound)
                                {
                                    Util.PlaySound(paraUnEquipSounds[Random.Range(0, 2)], base.gameObject);
                                    playedParaUnEquipSound = true;
                                }
                            }
                        }
                        if(enteredSlowMo && (!self.inputBank.skill2.down || self.characterMotor.velocity.y >= 0f))
                        {
                            enteredSlowMo = false;
                        }
                        if (self.inputBank.skill2.down && self.characterMotor.velocity.y < 0f && (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "ROB_HENRY_BODY_SECONDARY_BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "ROB_HENRY_BODY_SECONDARY_3BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "ROB_HENRY_BODY_SECONDARY_FASTBOW_NAME"))
                        {
                            if (skillLocator.GetSkill(SkillSlot.Secondary).cooldownRemaining == skillLocator.GetSkill(SkillSlot.Secondary).baseRechargeInterval) //If bow is not mid cooldown, allow for extreme slowfall
                            {
                                self.characterMotor.velocity = new Vector3(0f, 0f, 0f);
                            }
                            if (!enteredSlowMo)
                            {
                                Util.PlaySound("SlowMotionEnter", self.gameObject);
                                enteredSlowMo = true;
                            }
                            if(SlowMotionStopwatch <= 0f)
                            {
                                Util.PlaySound("SlowMotionLoop", self.gameObject);
                                SlowMotionStopwatch = 2f;
                            }
                            else
                            {
                                SlowMotionStopwatch -= Time.fixedDeltaTime;
                            }
                            
                        }
                        else if (self.inputBank.jump.down && self.characterMotor.velocity.y < 0f && !self.characterMotor.isGrounded)
                        {
                            enteredParaglider = true;
                            playedFall = false;


                            animator.CrossFadeInFixedTime("Glide", 0.01f, 2);

                            // Util.PlaySound(paraGlideSounds[Random.Range(0, 1)], base.gameObject);

                            playedParaUnEquipSound = false;
                            
                            if (!playedParaEquipSound)
                            {
                                Util.PlaySound(paraEquipSounds[Random.Range(0, 2)], base.gameObject);
                                playedParaEquipSound = true;
                            }

                            self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, -1f, self.characterMotor.velocity.z);
                            if (self.inputBank.skill2.down)
                            {
                                self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, 0f, self.characterMotor.velocity.z);
                            }
                        }
                    }
                }
            }
            #endregion

            if (self.HasBuff(HenryMod.Modules.Buffs.darukBuff))
            {
                self.healthComponent.AddBarrier(1f);
                if(DarukSoundStopwatch <= 0f)
                {
                    Util.PlaySound("Daruk_Shield_Loop", base.gameObject);
                    DarukSoundStopwatch = 3f;
                }
                else
                {
                    DarukSoundStopwatch -= Time.fixedDeltaTime;
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
                        if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            if (!characterBody.healthComponent.alive && !characterBody.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("ExtraLife")))
                            {
                                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
                                Util.PlaySound("MiphasGraceUse", characterBody.gameObject);
                                skillLocator.GetSkill(SkillSlot.Special).DeductStock(1);
                                resed = true;
                            }
                        }
                    }
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_DARUK_NAME")
                    {
                        if (characterBody.HasBuff(HenryMod.Modules.Buffs.darukBuff))
                        {
                            blockDaruk = true;
                            
                        }                        
                    }
                }                
                if (blockDaruk)
                {
                    blockDaruk = false;
                    characterBody.RemoveBuff(HenryMod.Modules.Buffs.darukBuff);
                    characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
                    characterBody.healthComponent.barrier = 0f;
                    skillLocator.GetSkill(SkillSlot.Special).AddOneStock();
                    skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                    SummonDaruk(characterBody);
                    Util.PlaySound("Daruk_Shield_Break", self.gameObject);
                    Util.PlaySound("Daruk_Yell", self.gameObject);
                }

            }    
            
        }

        public void SummonDaruk(CharacterBody body)
        {
            Ray aimRay = new Ray
            {
                direction = body.inputBank.aimDirection,
                origin = body.inputBank.aimOrigin
            };    
            ProjectileManager.instance.FireProjectile(Modules.Projectiles.darukPrefab,
                aimRay.origin,
                Util.QuaternionSafeLookRotation(aimRay.direction),
                body.gameObject,
                0f,
                0f,
                false,
                DamageColorIndex.Default,
                null,
                0f);
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