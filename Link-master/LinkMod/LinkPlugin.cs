using BepInEx;
using BepInEx.Configuration;
using LinkMod.Modules.Survivors;
using R2API;
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
        public const string MODVERSION = "1.2.3";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "CASEY";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static LinkPlugin instance;        
        private void Awake()
        {
            Log.Init(Logger);
            instance = this;
            
            // load assets and read config
            Modules.ModAssets.Initialize();
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
            Modules.Survivors.MyCharacter.instance.SetItemDisplays();
        }

        private void Hook()
        {            
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {                                  
            Modules.UpdateValues updateValues = self.gameObject.GetComponent<CharacterBody>().GetComponent<Modules.UpdateValues>();

            #region ShieldBlocking
            if (self && updateValues)
            {
                CharacterBody characterBody = self.gameObject.GetComponent<CharacterBody>();                
                // Shield Guarding
                if (damageInfo.attacker)
                {
                    CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();

                    // Thanks TheTimesweeper for this!!
                    // Check if the damage source is the attacker                    
                    GameObject blockEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CaptainBodyArmorBlockEffect");
                    
                    EffectData blockEffectData = new EffectData
                    {
                        origin = damageInfo.position,
                        rotation = Util.QuaternionSafeLookRotation(characterBody.inputBank.aimDirection)                        
                    };
                    if (damageInfo.attacker == damageInfo.inflictor)
                    {
                        if (characterBody.HasBuff(Modules.Buffs.shieldBuff) && updateValues.ShouldBlock(attackerBody.corePosition, 40f))
                        {
                            damageInfo.damage = 0f;
                            damageInfo.rejected = true;
                            updateValues.blockedAttacks += 1;

                            if (Modules.Config.ShieldBlockEffect.Value)
                                EffectManager.SpawnEffect(blockEffect, blockEffectData, true);                            

                            Util.PlaySound("Guard_" + UnityEngine.Random.Range(0, 3), characterBody.gameObject);
                        }
                    }
                    else if(damageInfo.inflictor != null) // Otherwise block based on inflictor (i.e. projectile)
                    {
                        if (characterBody.HasBuff(Modules.Buffs.shieldBuff) && updateValues.ShouldBlock(damageInfo.inflictor.transform.position, 40f))
                        {
                            damageInfo.damage = 0f;
                            damageInfo.rejected = true;
                            updateValues.blockedAttacks += 1;
                            if (Modules.Config.ShieldBlockEffect.Value)                            
                                EffectManager.SpawnEffect(blockEffect, blockEffectData, true);                            

                            Util.PlaySound("Guard_" + UnityEngine.Random.Range(0, 3), characterBody.gameObject);
                        }
                    }
                }
            }
            #endregion

            // Orig needs to be called here to work both with shielding and mipha's, and allow normal damage to function properly            
            orig(self, damageInfo);
            
            if (self && updateValues) 
            {                
                CharacterBody characterBody = self.GetComponent<CharacterBody>();
                SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();
                if (skillLocator && characterBody && skillLocator.GetSkill(SkillSlot.Special) != null)
                {

                    #region MiphasRes
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
                    #endregion

                    #region DarukCheckBlock
                    if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_DARUK_NAME")
                    {
                        if (characterBody.HasBuff(LinkMod.Modules.Buffs.darukBuff))
                        {
                            updateValues.blockDaruk = true;                            
                        }                        
                    }
                    #endregion
                }

                #region DarukBlock
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
                #endregion
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
    }
}