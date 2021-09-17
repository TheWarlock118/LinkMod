using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HenryMod.SkillStates
{
    public class MiphasGrace : BaseSkillState
    {
        public static float damageCoefficient = 16f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.01f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = MiphasGrace.baseDuration;
            this.fireTime = 0.35f * this.duration;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            SkillLocator skillLocator = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<SkillLocator>();
            skillLocator.GetSkill(SkillSlot.Special).RestockSteplike();
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