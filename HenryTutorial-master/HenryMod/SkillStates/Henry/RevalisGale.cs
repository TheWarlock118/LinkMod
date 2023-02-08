using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HenryMod.SkillStates
{
    public class RevalisGale : BaseSkillState
    {
        public static float damageCoefficient = 16f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 2f;
        public static float throwForce = 80f;       
       
        private float duration;
        private float fireTime;
        private bool fired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RevalisGale.baseDuration;
            this.fireTime = 0f;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (fired)
            {
                CharacterMotor characterMotor = this.characterBody.characterMotor;
                characterMotor.Motor.ForceUnground();
                characterMotor.ApplyForce(Vector3.up * 5000f * (this.characterBody.rigidbody.mass / 100f), false, false);
            }
        }

        private void Fire()
        {
            characterBody.AddTimedBuff(RoR2Content.Buffs.Slow80, 0.1f);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            fired = false;
            if (characterBody.characterMotor.isGrounded)
            {
                if (base.fixedAge >= this.fireTime)
                {
                    this.Fire();
                    fired = true;
                }

                if (base.fixedAge >= this.duration && base.isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
            else
            {
                SkillLocator skillLocator = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<SkillLocator>();
                //skillLocator.GetSkill(SkillSlot.Special).RestockSteplike();
                skillLocator.GetSkill(SkillSlot.Special).AddOneStock();
                fired = false;
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