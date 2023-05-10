using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;

namespace LinkMod.SkillStates
{
    public class ShootFast : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.bowDamageCoefficient / 2;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.5f;
        public static float force = 30f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject projectilePrefab;

        private float duration;
        private float fireTime;
        private float timer;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shoot.baseDuration / 2;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(1000000f);
            this.muzzleString = "Muzzle";
            this.timer = 0f;
            ShootFast.damageCoefficient = Modules.StaticValues.bowDamageCoefficient / 2;
            ShootFast.force = 30f;
            string[] sounds = { "Bow_Draw0", "Bow_Draw1", "Bow_Draw2", "Bow_Draw3", "Bow_Draw4", "Bow_Draw5" };
            Util.PlaySound(sounds[Random.Range(0, 5)], base.gameObject);
            Util.PlaySound("FireArrow_Charge", base.gameObject);
            base.PlayAnimation("Gesture, Override", "BowDraw", "ShootGun.playbackRate", 0.9f);
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.SetAimTimer(1f);
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                string[] sounds = { "Bow_Release1", "Bow_Release2", "Bow_Release3", "Bow_Release5" };
                Util.PlaySound(sounds[Random.Range(0,3)], base.gameObject);
                base.PlayAnimation("Gesture, Override", "BowShoot", "ShootGun.playbackRate", 1.8f);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    ProjectileDamage projectileDamage = Modules.Projectiles.fireArrowPrefab.GetComponent<ProjectileDamage>();
                    projectileDamage.damageType = DamageType.IgniteOnHit;                    
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.fireArrowPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        ShootFast.damageCoefficient * this.damageStat,
                        39f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        ShootFast.force);
                }

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.timer += Time.fixedDeltaTime;
            
            if (base.inputBank.skill2.down) 
            {
                if (base.fixedAge <= this.duration && base.isAuthority) //The longer the skill is held down, the more damage and force the arrow has
                {
                    ShootFast.damageCoefficient += (this.timer * 0.1f);
                    ShootFast.force += (this.timer * 10f);
                }
                if(this.timer >= 1f && (this.timer % 1f == 0))
                {
                    Util.PlaySound("FireArrow_Charge_Complete", base.gameObject);
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