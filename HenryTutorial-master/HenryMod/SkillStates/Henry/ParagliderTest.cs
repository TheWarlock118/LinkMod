using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class ParagliderTest : BaseSkillState
    {
        public static float damageCoefficient = 16f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.01f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private Animator animator;
        private EntityStateMachine gliderStateMachine;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = MiphasGrace.baseDuration;
            this.fireTime = 0.35f * this.duration;
            this.animator = base.GetModelAnimator();
            this.gliderStateMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Jet");
            base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (inputBank.skill3.down)
            {
                if (base.isAuthority)
                {
                    this.gliderStateMachine.SetNextState(new GliderOn());
                    object obj = base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded;
                    bool flag = this.gliderStateMachine.state.GetType() == typeof(GliderOn);
                    object obj2 = obj;
                    if (obj2 != null && !flag)
                    {
                        this.gliderStateMachine.SetNextState(new GliderOn());
                    }
                    if (obj2 == null && flag)
                    {
                        this.gliderStateMachine.SetNextState(new Idle());
                    }
                }
            }
            else
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