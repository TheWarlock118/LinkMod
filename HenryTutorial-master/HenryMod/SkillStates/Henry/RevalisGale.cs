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
        private bool revaliSummoned;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RevalisGale.baseDuration;
            this.fireTime = 0f;
            revaliSummoned = false;
            
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
            base.PlayAnimation("Gesture, Override", "Glide");
        }

        private void Fire()
        {
            base.characterMotor.velocity = new Vector3(0f, 0f, 0f);
        }

        private void SummonRevali()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                ProjectileManager.instance.FireProjectile(Modules.Projectiles.revaliPrefab,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(aimRay.direction),
                    base.gameObject,
                    0f,
                    0f,
                    false,
                    DamageColorIndex.Default,
                    null,
                    0f);
                base.PlayAnimation("Gesture, Override", "Crouch");
            }
            revaliSummoned = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            fired = false;
            if (characterBody.characterMotor.isGrounded)
            {
                if (!revaliSummoned)
                    SummonRevali();
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