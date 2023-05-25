using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class Magnesis : BaseSkillState
    {
        public static float baseDuration = 0.65f;
        public static float radius = 30f;

        private float duration;        
        private float timer;
        private bool hasFired;        

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Magnesis.baseDuration / this.attackSpeedStat;
            this.timer = 0f;
            base.characterBody.SetAimTimer(2f);
            TeamFilter teamFilter = base.characterBody.gameObject.AddComponent<TeamFilter>();
            teamFilter.teamIndex = TeamIndex.Player;
            RadialForce radialForce = new RadialForce();
            if (base.characterBody.gameObject.GetComponent<RadialForce>())
            {
                radialForce = base.characterBody.gameObject.GetComponent<RadialForce>();
            }
            else
            {
                radialForce = base.characterBody.gameObject.AddComponent<RadialForce>();
            }

            radialForce.radius = Magnesis.radius;
            radialForce.forceMagnitude = -5000f;
            radialForce.tetherVfxOrigin = null;

            
        }

        public override void OnExit()
        {
            base.OnExit();
            RadialForce radialForce = base.characterBody.gameObject.GetComponent<RadialForce>();
            radialForce.radius = 0f;
            radialForce.forceMagnitude = 0f;
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (base.isAuthority)
                {                    

                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.Fire();
            this.timer += Time.fixedDeltaTime;

            if (base.inputBank.skill3.down && timer < 3f)
            {
                new BlastAttack
                {
                    attacker = base.characterBody.gameObject,
                    baseDamage = 0f,
                    baseForce = 0f,
                    bonusForce = Vector3.zero,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.CrippleOnHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    position = characterBody.corePosition,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0.0001f,
                    radius = Magnesis.radius,
                    losType = BlastAttack.LoSType.NearestHit,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                }.Fire();                
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }
            return;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}