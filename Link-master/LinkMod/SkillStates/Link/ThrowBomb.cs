﻿using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using RoR2.Projectile;
using R2API;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class ThrowBomb : SkillStates.BaseStates.LinkMain
    {
        public static float damageCoefficient = 1f; // Bomb itself should have no damage. All damage comes from explosion
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.65f;
        public static float throwForce = 50f;

        private float duration;
        private float fireTime;
        private bool hasFired;        
        
        private Animator animator;


        public override void OnEnter()
        {

            base.OnEnter();
            this.duration = ThrowBomb.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(1000000f);
            this.animator = base.GetModelAnimator();

            Util.PlaySound("BombDraw", base.gameObject);
            base.PlayAnimation("Gesture, Override", "DrawBomb", "ThrowBomb.playbackRate", this.duration);
        }

        public override void OnExit()
        {            
            base.OnExit();
            base.characterBody.SetAimTimer(1f);

            
            if (!this.hasFired)
            {
                this.hasFired = true;                
                //Util.PlaySound("LinkBombThrow", base.gameObject);
              
                if (base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded)
                {
                    // Drop bomb straight down
                    if (base.isAuthority)
                    {
                        Ray aimRay = base.GetAimRay();
                        Quaternion aimDown = new Quaternion(0, 0, 0, 0);
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.bombPrefab,
                            aimRay.origin,
                            aimDown,
                            base.gameObject,
                            ThrowBomb.damageCoefficient * this.damageStat,
                            1500f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            0f);
                    }
                }
                else if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();                   
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.bombPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        ThrowBomb.damageCoefficient * this.damageStat,
                        1500f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        ThrowBomb.throwForce);
                }
                base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);                
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();        
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}