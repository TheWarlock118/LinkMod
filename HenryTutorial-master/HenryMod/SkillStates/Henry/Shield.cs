using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;

namespace HenryMod.SkillStates
{
    public class Shield : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 10f;

        private float duration;
        private float fireTime;
        private float timer;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shield.baseDuration;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.timer = 0f;
            base.PlayAnimation("Gesture, Override", "ShieldGuard");
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!this.hasFired)
            {
                this.hasFired = true;
            }
            base.PlayAnimation("Gesture, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.timer += Time.fixedDeltaTime;
            if (!(base.inputBank.jump.down && base.characterMotor.velocity.y < 0f))
            {
                if (base.inputBank.skill2.down && timer<this.duration)
                {
                    this.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 0.1f);
                    this.characterBody.AddTimedBuff(RoR2Content.Buffs.Slow80, 0.1f);
                }
                else
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}