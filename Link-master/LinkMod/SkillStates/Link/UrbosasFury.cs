﻿using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.VagrantMonster;
using R2API;

namespace LinkMod.SkillStates
{
    public class UrbosasFury : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.urbosaDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = .51f;
        public static float throwForce = 80f;
        public float blastAttackRadius = 50f;
        public float blastAttackProcCoefficient = 0.5f;
        public float blastAttackDamageCoefficient = 8f;
        public float blastAttackForce = 50f;
        
        private float duration;
        private float fireTime;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = UrbosasFury.baseDuration;
            this.fireTime = 0.5f;
            SummonUrbosa();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            new BlastAttack
            {
                attacker = base.characterBody.gameObject,
                baseDamage = this.damageStat * UrbosasFury.damageCoefficient,
                baseForce = blastAttackForce,
                bonusForce = Vector3.zero,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                crit = base.characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.Shock5s,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = base.gameObject,
                position = characterBody.corePosition,
                procChainMask = default(ProcChainMask),
                procCoefficient = this.blastAttackProcCoefficient,
                radius = 150f,
                losType = BlastAttack.LoSType.NearestHit,
                teamIndex = base.characterBody.teamComponent.teamIndex
            }.Fire();
            EffectData effectData = new EffectData();
            effectData.origin = base.characterBody.corePosition;
            effectData.SetHurtBoxReference(base.characterBody.mainHurtBox);

            GameObject urbosaExplosion = PrefabAPI.InstantiateClone(FireMegaNova.novaEffectPrefab, "urbosaExplosion");
            ShakeEmitter urbosaEmitter = urbosaExplosion.AddComponent<ShakeEmitter>();
            urbosaEmitter.duration = 0f;


            EffectManager.SpawnEffect(urbosaExplosion, effectData, true);

            Util.PlaySound("Urbosa_Lightning", base.gameObject);
            Util.PlaySound("Urbosa_Yell", base.gameObject);
            Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void SummonUrbosa()
        {                                   
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                FireProjectileInfo urbosaInfo = new FireProjectileInfo
                {
                    projectilePrefab = Modules.Projectiles.urbosaPrefab,
                    position = aimRay.origin,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                    owner = base.gameObject,
                    damage = 0f,
                    force = 0f,
                    crit = false,
                    damageColorIndex = DamageColorIndex.Default,
                    target = null,
                    speedOverride = 0f,
                    fuseOverride = 0.00001f,
                };
                urbosaInfo.useFuseOverride = true;
                urbosaInfo.useSpeedOverride = true;
                ProjectileManager.instance.FireProjectile(urbosaInfo);                    
            }          
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}