using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class Cryonis : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.cryonisDamageCoefficient;
        public static float baseDuration = 0.65f;
        public static float radius = 10f;

        private float duration;
        private float fireTime;
        private float timer;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Cryonis.baseDuration / this.attackSpeedStat;
            this.timer = 0f;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                if (base.isAuthority)
                {
                    this.hasFired = true;
                    Ray aimRay = base.GetAimRay();
                    Util.PlaySound("Cryonis_Make", base.characterBody.gameObject);
                    new BlastAttack
                    {
                        attacker = base.characterBody.gameObject,
                        baseDamage = Cryonis.damageCoefficient * this.damageStat,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        crit = base.characterBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Freeze2s,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = base.gameObject,
                        position = characterBody.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 1f,
                        radius = 10f,
                        losType = BlastAttack.LoSType.NearestHit,
                        teamIndex = base.characterBody.teamComponent.teamIndex
                    }.Fire();

                    Quaternion aimUp = Quaternion.Euler(-90, 0, 0);
                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (aimRay.direction*10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(36, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(72, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(108, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(144, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(180, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(216, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(252, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(288, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);

                    ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageIcewallpillarProjectile"),
                        aimRay.origin + (Quaternion.AngleAxis(324, Vector3.up) * aimRay.direction * 10f),
                        aimUp,
                        base.gameObject,
                        Cryonis.damageCoefficient * this.damageStat,
                        0f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        0f);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.Fire();
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