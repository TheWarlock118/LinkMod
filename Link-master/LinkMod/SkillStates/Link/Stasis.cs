using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class Stasis : BaseSkillState
    {                
        public static float baseDuration = 0.65f;        
        public static float radius = 10f;

        private float duration;
        private float fireTime;
        private float timer;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Stasis.baseDuration / this.attackSpeedStat;
            this.timer = 0f;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();            
        }

        public override void OnExit()
        {
            base.OnExit();            
            
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                if (base.isAuthority)
                {
                    this.hasFired = true;
                    new BlastAttack
                    {
                        attacker = base.characterBody.gameObject,
                        baseDamage = 0f,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        crit = base.characterBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Nullify,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = characterBody.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 50f,
                        radius = Stasis.radius,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.characterBody.teamComponent.teamIndex
                    }.Fire();

                    new BlastAttack
                    {
                        attacker = base.characterBody.gameObject,
                        baseDamage = 0f,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        crit = base.characterBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Nullify,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = characterBody.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 50f,
                        radius = Stasis.radius,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.characterBody.teamComponent.teamIndex
                    }.Fire();

                    new BlastAttack
                    {
                        attacker = base.characterBody.gameObject,
                        baseDamage = 0f,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        crit = base.characterBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Nullify,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = characterBody.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 50f,
                        radius = Stasis.radius,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.characterBody.teamComponent.teamIndex
                    }.Fire();

                    new BlastAttack
                    {
                        attacker = base.characterBody.gameObject,
                        baseDamage = 0f,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        crit = base.characterBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Freeze2s,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = characterBody.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 1.5f,
                        radius = Stasis.radius,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.characterBody.teamComponent.teamIndex
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();            
            this.Fire();            
            this.timer += Time.fixedDeltaTime;
            if (timer < duration)
            {

            }
            else
            {
                this.outer.SetNextStateToMain();
            }
            return;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}