using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
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
        private bool readySoundPlayed;
        private bool shouldLaunch;
        private Animator animator;

        public static AnimationCurve speedCoefficientCurve;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RevalisGale.baseDuration;
            this.fireTime = 0.10f;
            readySoundPlayed = false;
            fired = false;
            shouldLaunch = false;
            // base.PlayAnimation("Gesture, Override", "Crouch");
        }

        public override void OnExit()
        {
            base.OnExit();
            if (shouldLaunch)
            {
                if (fired)
                {
                    CharacterMotor characterMotor = this.characterBody.characterMotor;
                    characterMotor.Motor.ForceUnground();
                    characterMotor.ApplyForce(Vector3.up * 2500f * (this.moveSpeedStat / 5f) * (this.characterBody.rigidbody.mass / 100f), false, false);                    
                }
                // base.PlayAnimation("Gesture, Override", "Glide");
                // Util.PlaySound("Revali_Wind2", base.gameObject);
                SummonRevali();
            }
            else
            {
                SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();
                skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                skillLocator.GetSkill(SkillSlot.Special).AddOneStock();
                skillLocator.GetSkill(SkillSlot.Special).Reset();
            }
        }

        private void Fire()
        {
            if (!shouldLaunch)
                shouldLaunch = true;
            if (!readySoundPlayed)
            {
                Util.PlaySound("AbilityReady", base.gameObject);
                // Util.PlaySound("Revali_Wind", base.gameObject);
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

                new BlastAttack
                {
                    attacker = base.characterBody.gameObject,
                    baseDamage = base.characterBody.damage * 1f,
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
                    radius = 10f,
                    losType = BlastAttack.LoSType.NearestHit,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                }.Fire();                
            }            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();             
            this.Fire();
            fired = true;

            this.outer.SetNextStateToMain();
            return;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}