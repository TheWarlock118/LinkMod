using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HenryMod.SkillStates
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
            base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.fireTime);
            
        }

        public override void OnExit()
        {           
            base.OnExit();
            characterBody.healthComponent.barrier = 0f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();            
            if (base.inputBank.skill4.down)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 0.1f);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Slow80, 0.1f);
                characterBody.healthComponent.AddBarrier(100f);
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }
            if ((base.inputBank.skill1.down || base.inputBank.skill2.down || base.inputBank.skill3.down))
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