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


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = UrbosasFury.baseDuration;
            this.fireTime = 0.5f;            
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
                baseDamage = base.characterBody.damage * 16f,
                baseForce = 50f,
                bonusForce = Vector3.zero,
                attackerFiltering = AttackerFiltering.NeverHit,
                crit = base.characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = base.gameObject,
                position = characterBody.corePosition,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1f,
                radius = 50f,
                losType = BlastAttack.LoSType.NearestHit,
                teamIndex = base.characterBody.teamComponent.teamIndex
            }.Fire();
            EffectData effectData = new EffectData();
            effectData.origin = base.characterBody.corePosition;
            effectData.SetHurtBoxReference(base.characterBody.mainHurtBox);
            EffectManager.SpawnEffect(FireMegaNova.novaEffectPrefab, effectData, true);

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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}