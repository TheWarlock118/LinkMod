using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class Stasis : BaseSkillState
    {                
        public static float baseDuration = 5f;        
        public static float radius = 10f;

        private float duration;
        private float fireTime;
        private float timer;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Stasis.baseDuration;
            this.timer = 0f;            
            base.characterBody.SetAimTimer(2f);            
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
                        attackerFiltering = AttackerFiltering.AlwaysHitSelf,
                        crit = base.characterBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Freeze2s,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = characterBody.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 2.5f,
                        radius = 0.1f,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.characterBody.teamComponent.teamIndex
                    }.Fire();

                    this.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Immune.buffIndex, 5f);
                    this.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CrocoRegen.buffIndex, 5f);
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