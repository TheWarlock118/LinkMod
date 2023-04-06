using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class DaruksProtection : BaseSkillState
    {
        public static float damageCoefficient = 16f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 5f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private bool hasExploded;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = DaruksProtection.baseDuration;
            this.fireTime = 0.35f;
            this.animator = base.GetModelAnimator();            
            base.PlayAnimation("Gesture, Override", "ShieldGuard", "ThrowBomb.playbackRate", this.fireTime);
            base.PlayCrossfade("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.fireTime, this.fireTime);
            Util.PlaySound("ShieldGuardUp", base.gameObject);
            Util.PlaySound("AbilityReady", base.gameObject);
            characterBody.AddBuff(LinkMod.Modules.Buffs.darukBuff);
            characterBody.healthComponent.AddBarrier(1f);
        }

        public override void OnExit()
        {           
            base.OnExit();            
        }

        public override void FixedUpdate()
        {
            this.outer.SetNextStateToMain();
        }

        public void SummonDaruk()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                ProjectileManager.instance.FireProjectile(Modules.Projectiles.darukPrefab,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(aimRay.direction),
                    base.gameObject,
                    0f,
                    0f,
                    false,
                    DamageColorIndex.Default,
                    null,
                    0f);                
            }            
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}