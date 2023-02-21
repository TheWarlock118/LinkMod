using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.VagrantMonster;

namespace HenryMod.SkillStates
{
    public class UrbosasFury : BaseSkillState
    {
        public static float damageCoefficient = 16f;
        public static float procCoefficient = 1f;
        public static float baseDuration = .51f;
        public static float throwForce = 80f;
        public float blastAttackRadius = 50f;
        public float blastAttackProcCoefficient = 1f;
        public float blastAttackDamageCoefficient = 8f;
        public float blastAttackForce = 50f;
        
        private float duration;
        private float fireTime;
        private bool playedReadySound;
        private bool shouldFire;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = UrbosasFury.baseDuration;
            this.fireTime = 1f;
            this.shouldFire = false;
            playedReadySound = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (shouldFire)
            {
                SummonUrbosa();
                new BlastAttack
                {
                    attacker = base.characterBody.gameObject,
                    baseDamage = base.characterBody.damage * 16f,
                    baseForce = 50f,
                    bonusForce = Vector3.zero,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    position = characterBody.corePosition,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 200f,
                    losType = BlastAttack.LoSType.NearestHit,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                }.Fire();
                EffectData effectData = new EffectData();
                effectData.origin = base.characterBody.corePosition;
                effectData.SetHurtBoxReference(base.characterBody.mainHurtBox);
                EffectManager.SpawnEffect(FireMegaNova.novaEffectPrefab, effectData, true);

                Util.PlaySound("Urbosa_Lightning", base.gameObject);
                Util.PlaySound("Urbosa_Yell", base.gameObject);
                Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            }
            else
            {
                SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();
                skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                skillLocator.GetSkill(SkillSlot.Special).AddOneStock();
                skillLocator.GetSkill(SkillSlot.Special).Reset();
            }
        }

        private void Fire()
        {
            if (!shouldFire)
                shouldFire = true;
            if (!playedReadySound)
            {
                Util.PlaySound("Urbosa_Ready", base.gameObject);
                Util.PlaySound("AbilityReady", base.gameObject);
                playedReadySound = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();            
            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }            
            if (!base.inputBank.skill4.down)
            {
                this.outer.SetNextStateToMain();
                return;
            }            
        }

        private void SummonUrbosa()
        {
            /*
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                EffectManager.SpawnEffect(Modules.Assets.urbosaEffect, new EffectData
                {
                    origin = base.GetAimRay().origin,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                }, true);
            }
            */            
                       
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
                ProjectileManager.instance.FireProjectile(urbosaInfo);                    
            }          
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}