using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class Cryonis : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.cryonisDamageCoefficient;
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
                    Ray aimRay = base.GetAimRay();
                    Quaternion aimUp = Quaternion.Euler(-90, 0, 0);
                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (aimRay.direction*10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);
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