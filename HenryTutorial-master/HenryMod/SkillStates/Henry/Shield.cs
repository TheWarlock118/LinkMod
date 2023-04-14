using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace LinkMod.SkillStates
{
    public class Shield : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 10f;

        private float duration;
        private float fireTime;
        private float timer;
        private float animationTimer;
        private bool hasFired;
        private string muzzleString;        

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shield.baseDuration;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.timer = 0f;
            this.animationTimer = 1f;
            base.PlayAnimation("Gesture, Override", "ShieldGuard");
            Util.PlaySound("Weapon_Shield_Metal_Equip0" + UnityEngine.Random.Range(0, 2), base.gameObject);
            Util.PlaySound("ShieldGuardUp", base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!this.hasFired)
            {
                this.hasFired = true;
            }
            base.PlayAnimation("Gesture, Override", "BufferEmpty");
            Util.PlaySound("Weapon_Shield_Metal_UnEquip0" + UnityEngine.Random.Range(0, 2), base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(animationTimer > 0f)
            {
                animationTimer -= Time.fixedDeltaTime;
            }
            else
            {
                animationTimer = 2f;
                base.PlayAnimation("Gesture, Override", "ShieldGuard");                
            }


            this.timer += Time.fixedDeltaTime;
            if (!(base.inputBank.jump.down && base.characterMotor.velocity.y < 0f))
            {
                if (base.inputBank.skill2.down && timer<this.duration)
                {
                    base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Immune.buffIndex, 0.1f);
                    base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Slow80.buffIndex, 0.1f);
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