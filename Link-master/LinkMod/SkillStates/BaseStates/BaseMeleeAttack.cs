﻿using KinematicCharacterController;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Audio;
using R2API;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace LinkMod.SkillStates.BaseStates
{
    public class BaseMeleeAttack : BaseSkillState
    {
        public int swingIndex;

        protected string hitboxName = "Sword";

        protected DamageType damageType = DamageType.Generic;
        protected float damageCoefficient = Modules.StaticValues.swordDamageCoefficient;
        protected float procCoefficient = 1f;
        protected float pushForce = 0f;
        protected Vector3 bonusForce = Vector3.zero;
        protected float baseDuration = 1f;
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected float baseEarlyExitTime = 0.4f;
        protected float hitStopDuration = 0.012f;
        protected float attackRecoil = 0.75f;
        protected float hitHopVelocity = 4f;
        protected bool cancelled = false;

        private float projectileForce = 60f;
        private float projectileDamageCoef = Modules.StaticValues.swordDamageCoefficient / 2;

        protected string hitSoundString = "SwordOnHit";
        protected string muzzleString = "SwingCenter";
        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;

        private float earlyExitTime;
        public float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        protected bool inHitPause;
        private bool hasHopped;
        protected float stopwatch;
        protected Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExitTime = this.baseEarlyExitTime / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            base.StartAimMode(0.5f + this.duration, false);
            // base.characterBody.outOfCombat = 0f;
            this.animator.SetBool("attacking", true);

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
            }

            this.PlayAttackAnimation();

            this.attack = new OverlapAttack();
            this.attack.damageType = this.damageType;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = this.damageCoefficient * this.damageStat;
            this.attack.procCoefficient = this.procCoefficient;
            this.attack.hitEffectPrefab = this.hitEffectPrefab;
            this.attack.forceVector = this.bonusForce;
            this.attack.pushAwayForce = this.pushForce;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.impactSound = this.impactSound;            
        }

        protected virtual void PlayAttackAnimation()
        {
            base.PlayCrossfade("Gesture, Override", "Sword" + (1 + swingIndex), "Slash.playbackRate", this.duration, 0.05f);
        }

        public override void OnExit()
        {
            if (!this.hasFired && !this.cancelled) this.FireAttack();
            
            base.OnExit();

            this.animator.SetBool("attacking", false);
        }

        protected virtual void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.muzzleString, true);
        }

        protected virtual void OnHitEnemyAuthority()
        {
            Util.PlaySound(this.hitSoundString, base.gameObject);            
            if (!this.hasHopped)
            {
                if (base.characterMotor && !base.characterMotor.isGrounded && this.hitHopVelocity > 0f)
                {
                    base.SmallHop(base.characterMotor, this.hitHopVelocity);
                }

                this.hasHopped = true;
            }

            if (!this.inHitPause && this.hitStopDuration > 0f)
            {
                this.storedVelocity = base.characterMotor.velocity;
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                this.hitPauseTimer = this.hitStopDuration / this.attackSpeedStat;
                this.inHitPause = true;
            }
        }

        private void FireAttack()
        {
            if (!(base.inputBank.jump.down && base.characterMotor.velocity.y < 0f))
            {
                if (!this.hasFired)
                {
                    this.hasFired = true;
                    
                    this.PlayAttackSound();                    
                    if (this.characterBody.healthComponent.combinedHealth / this.characterBody.healthComponent.fullCombinedHealth >= .9f)
                    {
                        if (base.isAuthority)
                        {
                            
                            Ray aimRay = base.GetAimRay();
                            ProjectileManager.instance.FireProjectile(EntityStates.Vulture.Weapon.FireWindblade.projectilePrefab, //Placeholder projectile
                            aimRay.origin,
                            Util.QuaternionSafeLookRotation(aimRay.direction),
                            base.gameObject,
                            this.projectileDamageCoef * this.damageStat,
                            40f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            this.projectileForce);
                        }
                    }

                    if (base.isAuthority)
                    {
                        this.PlaySwingEffect();
                        base.AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
                    }

                    if (base.isAuthority)
                    {
                        if (this.attack.Fire())
                        {
                            this.OnHitEnemyAuthority();
                        }
                    }
                }                
            }
        }

        private void PlayAttackSound() 
        {
            // 33% chance for Link to yell when attacking
            if (UnityEngine.Random.Range(1, 3) == 1)
            {
                Util.PlayAttackSpeedSound("SwordAttack" + (1 + swingIndex) + "_" + UnityEngine.Random.Range(1, 3), base.gameObject, this.attackSpeedStat);
            }
            Util.PlayAttackSpeedSound("Sword_Swing" + (1 + swingIndex), base.gameObject, this.attackSpeedStat);
            if (swingIndex == 3)
                Util.PlayAttackSpeedSound("Sword_Swing2", base.gameObject, this.attackSpeedStat);

        }
        protected virtual void SetNextState()
        {
            int index = this.swingIndex;
            if (index < 3)
            {
                index++;
            }
            else
                index = 0;

            this.outer.SetNextState(new BaseMeleeAttack
            {
                swingIndex = index
            });
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                base.characterMotor.velocity = this.storedVelocity;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Swing.playbackRate", 0f);
            }

            if (this.stopwatch >= (this.duration * this.attackStartTime) && this.stopwatch <= (this.duration * this.attackEndTime))
            {
                this.FireAttack();
            }

            if (this.stopwatch >= (this.duration - this.earlyExitTime) && base.isAuthority)
            {
                if (base.inputBank.skill1.down)
                {
                    if (!this.hasFired) this.FireAttack();
                    this.SetNextState();
                    return;
                }
            }

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}