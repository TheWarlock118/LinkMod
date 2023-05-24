using BepInEx;
using BepInEx.Configuration;
using LinkMod.Modules.Survivors;
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

namespace LinkMod
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

    public class LinkPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.TheWarlock117.LinkMod";
        public const string MODNAME = "LinkMod";
        public const string MODVERSION = "1.1.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "CASEY";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static LinkPlugin instance;        
        private void Awake()
        {
            Log.Init(Logger);
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

            // TODO: For Multiplayer Testing - comment this out before Uploading
            // On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };            
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
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.Update += CharacterBody_Update;                     
        }

        private void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            
            orig(self);
            Modules.UpdateValues updateValues = self.gameObject.GetComponent<Modules.UpdateValues>();
                        
            if (updateValues)
            {                
                SkillLocator skillLocator = self.GetComponent<SkillLocator>();

                #region MiphaGraceRemoveDioAndSetCooldown
                if (skillLocator)
                {
                    if (skillLocator.GetSkill(SkillSlot.Special) != null)
                    {
                        if (self.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("UseAmbientLevel")))
                        {                            
                            if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_MIPHA_NAME")
                            {                                
                                if (!(skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                                {                                    
                                    skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                                    self.inventory.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                                    self.inventory.RemoveItem(ItemCatalog.FindItemIndex("UseAmbientLevel")); // UpdateValues reset on respawn, so this is used in place of UpdateValues.resed                                    
                                }
                            }
                        }
                    }
                }
                #endregion

                #region ChampionReady

                if (skillLocator && skillLocator.GetSkill(SkillSlot.Special) != null)
                {
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_MIPHA_NAME")
                    {
                        if ((skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                        {
                            updateValues.miphaOnCooldown = true;
                        }
                        else if (updateValues.miphaOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                        {
                            updateValues.miphaOnCooldown = false;
                            if (Modules.Config.MiphaReadySound.Value)
                                Util.PlaySound("MiphasGraceReady", self.gameObject);
                        }
                    }

                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_DARUK_NAME")
                    {
                        if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            updateValues.darukOnCooldown = true;
                        }
                        else if (updateValues.darukOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            updateValues.darukOnCooldown = false;
                            if (Modules.Config.DarukReadySound.Value)
                                Util.PlaySound("DaruksProtectionReady", self.gameObject);
                        }
                    }

                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_URBOSA_NAME")
                    {
                        if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            updateValues.urbosaOnCooldown = true;
                        }
                        if (updateValues.urbosaOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            updateValues.urbosaOnCooldown = false;
                            if (Modules.Config.UrbosaReadySound.Value)
                                Util.PlaySound("UrbosasFuryReady", self.gameObject);
                        }

                    }

                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_REVALI_NAME")
                    {
                        if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            updateValues.revaliOnCooldown = true;
                        }
                        else if (updateValues.revaliOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                        {
                            updateValues.revaliOnCooldown = false;
                            if (Modules.Config.RevaliReadySound.Value)
                                Util.PlaySound("RevalisGaleReady", self.gameObject);
                        }

                    }
                }

                #endregion

                #region ParagliderSlow-Bow

                string[] paraEquipSounds = { "Pl_Parashawl_Equip00", "Pl_Parashawl_Equip02", "Pl_Parashawl_Equip04" };
                string[] paraGlideSounds = { "Pl_Parashawl_FlapFast00", "Pl_Parashawl_FlapFast01" };
                string[] paraUnEquipSounds = { "Pl_Parashawl_UnEquip00", "Pl_Parashawl_UnEquip03", "Pl_Parashawl_UnEquip04" };
  
                // Reset playedLowHealth sound
                if (updateValues.playedLowHealth && (self.healthComponent.combinedHealth / self.healthComponent.fullCombinedHealth >= .2f))
                {
                    updateValues.playedLowHealth = false;
                }

                Animator animator = self.modelLocator.modelTransform.GetComponent<Animator>();

                // Stop playing Slow-Motion loop
                if (!self.inputBank.skill2.down || self.characterMotor.isGrounded)
                {
                    AkSoundEngine.StopPlayingID(updateValues.slowMotionPlayID);
                    updateValues.SlowMotionStopwatch = 0f;
                    updateValues.enteredSlowMo = false;
                }

                // Play low HP sound
                if (!updateValues.playedLowHealth && (self.healthComponent.combinedHealth / self.healthComponent.fullCombinedHealth < .2f))
                {
                    Util.PlaySound("LowHP", self.gameObject);
                    updateValues.playedLowHealth = true;
                }

                // Unequip paraglider - play sound, fall animation
                if (updateValues.enteredParaglider && !self.inputBank.skill2.down && (!self.inputBank.jump.down || self.characterMotor.isGrounded))
                {
                    if (!updateValues.playedFall)
                    {

                        animator.Play("Fall", 2);
                        updateValues.playedFall = true;
                        updateValues.playedParaEquipSound = false;

                        if (!updateValues.playedParaUnEquipSound)
                        {
                            Util.PlaySound(paraUnEquipSounds[Random.Range(0, 2)], self.gameObject);
                            updateValues.playedParaUnEquipSound = true;
                        }
                    }
                }

                // Handle bow slow-mo
                if (self.inputBank.skill2.down && self.characterMotor.velocity.y < 0f && (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_3BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME"))
                {                    
                    if (skillLocator.GetSkill(SkillSlot.Secondary).cooldownRemaining == skillLocator.GetSkill(SkillSlot.Secondary).baseRechargeInterval) //If bow is not mid cooldown, allow for extreme slowfall
                    {
                        self.characterMotor.velocity = new Vector3(0f, 0f, 0f);
                    }
                    // Don't need to check recharge interval if using fast bow
                    if (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME")
                    {
                        self.characterMotor.velocity = new Vector3(0f, 0f, 0f);
                    }
                    if (!updateValues.enteredSlowMo)
                    {
                        if (Modules.Config.SlowBowSound.Value)
                        {
                            Util.PlaySound("SlowMotionEnter", self.gameObject);
                        }
                        updateValues.enteredSlowMo = true;
                    }
                    if (updateValues.SlowMotionStopwatch <= 0f)
                    {
                        if (Modules.Config.SlowBowSound.Value)
                        {
                            updateValues.slowMotionPlayID = Util.PlaySound("SlowMotionLoop", self.gameObject);
                        }
                        updateValues.SlowMotionStopwatch = 2f;
                    }
                    else
                    {
                        updateValues.SlowMotionStopwatch -= Time.fixedDeltaTime;
                    }

                } // Handle paraglider gliding and equipping
                else if (self.inputBank.jump.down && self.characterMotor.velocity.y < 0f && !self.characterMotor.isGrounded)
                {
                    updateValues.enteredParaglider = true;
                    updateValues.playedFall = false;

                    self.AddTimedBuffAuthority(Modules.Buffs.paragliderBuff.buffIndex, 0.1f);

                    //animator.CrossFadeInFixedTime("Glide", 0.01f, 2);
                    animator.Play("Glide", 2);

                    // Util.PlaySound(paraGlideSounds[Random.Range(0, 1)], base.gameObject);

                    updateValues.playedParaUnEquipSound = false;

                    if (!updateValues.playedParaEquipSound)
                    {
                        Util.PlaySound(paraEquipSounds[Random.Range(0, 2)], self.gameObject);
                        updateValues.playedParaEquipSound = true;
                    }

                    self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, -1f, self.characterMotor.velocity.z);
                    if (self.inputBank.skill2.down)
                    {
                        self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, 0f, self.characterMotor.velocity.z);
                    }
                }

                #endregion

                #region DarukShield
                if (self.HasBuff(LinkMod.Modules.Buffs.darukBuff))
                {
                    self.healthComponent.AddBarrier(1f);
                    if (updateValues.DarukSoundStopwatch <= 0f)
                    {
                        if (Modules.Config.DarukShieldSound.Value)
                        {
                            updateValues.darukShieldPlayID = Util.PlaySound("Daruk_Shield_Loop", self.gameObject);
                        }
                        updateValues.DarukSoundStopwatch = 3f;
                    }
                    else
                    {
                        updateValues.DarukSoundStopwatch -= Time.fixedDeltaTime;
                    }
                }
                #endregion                
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {            
            Log.Init(Logger);            
            Modules.UpdateValues updateValues = self.gameObject.GetComponent<CharacterBody>().GetComponent<Modules.UpdateValues>();
            
            if (self && updateValues)
            {
                CharacterBody characterBody = self.gameObject.GetComponent<CharacterBody>();
                SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();                
                // Shield Guarding
                if (damageInfo.attacker)
                {
                    CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();                                                            
                    if (characterBody.HasBuff(Modules.Buffs.shieldBuff) && updateValues.ShouldBlock(attackerBody.corePosition, 40f))
                    {                        
                        damageInfo.damage = 0f;
                        damageInfo.rejected = true;
                        updateValues.blockedAttacks += 1;
                        Util.PlaySound("Guard_" + UnityEngine.Random.Range(0, 3), characterBody.gameObject);
                    }
                }
            }

            // Orig needs to be called here to work both with shielding and mipha's, and allow normal damage to function properly            
            orig(self, damageInfo);

            if (self && updateValues) 
            {
                CharacterBody characterBody = self.GetComponent<CharacterBody>();
                SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();
                if (skillLocator && characterBody && skillLocator.GetSkill(SkillSlot.Special) != null)
                {

                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_MIPHA_NAME")
                    {
                        if (!(skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                        {
                            if (!characterBody.healthComponent.alive)
                            {
                                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
                                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("UseAmbientLevel"), 1);
                                Util.PlaySound("MiphasGraceUse", characterBody.gameObject);
                                skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                            }
                        }
                    }
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_DARUK_NAME")
                    {
                        if (characterBody.HasBuff(LinkMod.Modules.Buffs.darukBuff))
                        {
                            updateValues.blockDaruk = true;                            
                        }                        
                    }
                }   
                
                // Daruk shield break
                if (updateValues.blockDaruk)
                {
                    updateValues.blockDaruk = false;
                    updateValues.darukBlockedAttacks++;
                    characterBody.RemoveBuff(LinkMod.Modules.Buffs.darukBuff);
                    characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Immune.buffIndex, 3f);
                    damageInfo.damage = 0f;
                    damageInfo.rejected = true;
                    characterBody.healthComponent.barrier = 0f;
                    skillLocator.GetSkill(SkillSlot.Special).AddOneStock();
                    skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                    SummonDaruk(characterBody);
                    Util.PlaySound("Daruk_Shield_Break", self.gameObject);
                    Util.PlaySound("Daruk_Yell", self.gameObject);
                    AkSoundEngine.StopPlayingID(updateValues.darukShieldPlayID);
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
        }

    }
}