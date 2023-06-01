using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using LinkMod.Modules;

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

        private ChildLocator childLocator;
        private HurtBoxGroup hurtBoxGroup;

        public override void OnEnter()
        {
            base.OnEnter();            
            this.duration = Shield.baseDuration;
            this.fireTime = 20f * this.duration;
            base.characterBody.SetAimTimer(1000000f);
            this.timer = 0f;
            this.animationTimer = 1f;
            this.childLocator = base.GetModelChildLocator();
            if (characterMotor.isGrounded)
            {
                base.PlayAnimation("Gesture, Override", "ShieldGuard");
                Util.PlaySound("Weapon_Shield_Metal_Equip0" + UnityEngine.Random.Range(0, 2), base.gameObject);
                Util.PlaySound("ShieldGuardUp", base.gameObject);                
                //this.childLocator.FindChild("ShieldHitbox").gameObject.SetActive(true);                
            }
        }

        public void MyOnExit()
        {
            this.outer.SetNextStateToMain();
            base.OnExit();
            if (!this.hasFired)
            {
                this.hasFired = true;
            }
            base.characterBody.SetAimTimer(1f);
            base.PlayAnimation("Gesture, Override", "BufferEmpty");
            Util.PlaySound("Weapon_Shield_Metal_UnEquip0" + UnityEngine.Random.Range(0, 2), base.gameObject);            

            //this.childLocator.FindChild("ShieldHitbox").gameObject.SetActive(false);
            // base.characterBody.GetComponent<UpdateValues>().isBlocking = false;
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
                base.OnEnter();
            }


            this.timer += Time.fixedDeltaTime;
            if (!base.inputBank.jump.down)
            {
                if(base.inputBank.skill3.down || base.inputBank.skill4.down)
                {
                    MyOnExit();
                    return;
                }
                if (base.inputBank.skill2.down)
                {                    
                    base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Slow80.buffIndex, 0.1f);
                    base.characterBody.AddTimedBuffAuthority(Buffs.shieldBuff.buffIndex, 0.1f);                    
                }
                else
                {
                    MyOnExit();
                    return;
                }
            }
            else
            {
                MyOnExit();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}