using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class RevalisGale : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.revaliDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        public static float throwForce = 80f;       
       
        private float duration;
        private float fireTime;
        private float timer;
        private float movementSpeedCoefficient;
        private bool fired;
        private bool readySoundPlayed;
        private bool shouldLaunch;
        private Animator animator;

        public static AnimationCurve speedCoefficientCurve;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RevalisGale.baseDuration;
            this.fireTime = 0.10f;
            this.timer = 0f;
            readySoundPlayed = false;
            fired = false;
            shouldLaunch = false;
            // movementSpeedCoefficient = (this.moveSpeedStat / 100f);


            new BlastAttack
            {
                attacker = base.characterBody.gameObject,
                baseDamage = this.damageStat * RevalisGale.damageCoefficient,
                baseForce = 5000f,
                bonusForce = Vector3.zero,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                crit = base.characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = base.gameObject,
                position = characterBody.corePosition,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1f,
                radius = 20f,
                losType = BlastAttack.LoSType.NearestHit,
                teamIndex = base.characterBody.teamComponent.teamIndex
            }.Fire();

            CharacterMotor characterMotor = this.characterBody.characterMotor;
            characterMotor.Motor.ForceUnground();
            characterMotor.ApplyForce(Vector3.up * 6000f * (this.characterBody.rigidbody.mass / 100f), true, false);            
        }

        public override void OnExit()
        {
            base.OnExit();
            if (shouldLaunch)
            {
                SummonRevali();
            }
        }

        private void Fire()
        {
            if (!shouldLaunch)
                shouldLaunch = true;
            if (!readySoundPlayed)
            {
                Util.PlaySound("AbilityReady", base.gameObject);
                Util.PlaySound("Revali_TakeOff", base.gameObject);                
                readySoundPlayed = true;
            }

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

                              
            }            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();             
            this.Fire();
            fired = true;
            this.timer += Time.fixedDeltaTime;
            if (timer < duration)
            {
                
            }
            else
            {
                this.outer.SetNextStateToMain();
            }
            return;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}