using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;

namespace LinkMod.SkillStates
{
    public class ShootTri : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.bowDamageCoefficient / 3;
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
            this.duration = ShootTri.baseDuration;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(1000000f);
            this.muzzleString = "Muzzle";
            this.timer = 0f;
            ShootTri.damageCoefficient = Modules.StaticValues.bowDamageCoefficient;
            ShootTri.force = 30f;
            string[] sounds = { "Bow_Draw0", "Bow_Draw1", "Bow_Draw2", "Bow_Draw3", "Bow_Draw4", "Bow_Draw5" };
            Util.PlayAttackSpeedSound(sounds[Random.Range(0, 5)], base.gameObject, this.timer);
            base.PlayAnimation("Gesture, Override", "BowEquip", "ShootGun.playbackRate", 1.8f);
            base.PlayAnimation("Gesture, Override", "BowDraw", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                string[] sounds = { "Bow_Release1", "Bow_Release2", "Bow_Release3", "Bow_Release5" };
                base.PlayAnimation("Gesture, Override", "BowShoot", "ShootGun.playbackRate", 1.8f);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.arrowPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        ShootTri.damageCoefficient * this.damageStat,
                        40f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        ShootTri.force);
                    Util.PlaySound(sounds[Random.Range(0, 3)], base.gameObject);

                    /*
                    //debugging
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                    {
                        baseToken = "aimRay: x" + aimRay.direction.x + " y: " + aimRay.direction.y + " z: " + aimRay.direction.z
                    });
                    */

                    Vector3 arrow2Direction = new Vector3(aimRay.direction.x - 0.1f, aimRay.direction.y, aimRay.direction.z); //Adjusts direction for 2nd arrow to slight right
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.arrowPrefab, 
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(arrow2Direction),
                        base.gameObject,
                        ShootTri.damageCoefficient * this.damageStat,
                        4000f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        ShootTri.force);
                    Util.PlaySound(sounds[Random.Range(0, 3)], base.gameObject);

                    /*
                    //Debugging
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                    {
                        baseToken = "2nd aimRay: x" + arrow2Direction.x + " y: " + arrow2Direction.y + " z: " + arrow2Direction.z
                    });
                    */

                    Vector3 arrow3Direction = new Vector3(aimRay.direction.x + 0.1f, arrow2Direction.y, aimRay.direction.z); //Adjusts direction for 3rd to slight left
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.arrowPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(arrow3Direction),
                        base.gameObject,
                        ShootTri.damageCoefficient * this.damageStat,
                        4000f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        ShootTri.force);
                    Util.PlaySound(sounds[Random.Range(0, 3)], base.gameObject);
                    /*
                    //Debugging
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                    {
                        baseToken = "3rd aimRay: x" + arrow3Direction.x + " y: " + arrow3Direction.y + " z: " + arrow3Direction.z
                    });
                    */
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
                    ShootTri.damageCoefficient += (this.timer * 0.1f);
                    ShootTri.force += (this.timer * 5f);
                }
                this.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Slow80.buffIndex, 0.1f);
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