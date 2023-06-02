using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class Magnesis : BaseSkillState
    {
        public static float duration = 6f;
        public static float radius = 30f;
        
        private float soundStopwatch;
        private float timer;
        private bool hasFired;
        private TeamFilter teamFilter;
        private RadialForce radialForce;
        private uint magnesisSoundLoopID;

        public override void OnEnter()
        {
            base.OnEnter();
            this.timer = 0f;                        
            if (base.characterBody.gameObject.GetComponent<TeamFilter>())
            {
                teamFilter = base.characterBody.gameObject.GetComponent<TeamFilter>();
            }
            else
            {
                teamFilter = base.characterBody.gameObject.AddComponent<TeamFilter>();
            }
            teamFilter.teamIndex = TeamIndex.Player;
            
            if (base.characterBody.gameObject.GetComponent<RadialForce>())
            {
                radialForce = base.characterBody.gameObject.GetComponent<RadialForce>();
            }
            else
            {
                radialForce = base.characterBody.gameObject.AddComponent<RadialForce>();
            }            
            radialForce.tetherVfxOrigin = null;

            radialForce.forceMagnitude = -5000f;
            radialForce.radius = Magnesis.radius;

            Log.LogDebug("Entering Magnesis");
            Log.LogDebug("Force Radius: " + radialForce.radius.ToString());
            Log.LogDebug("Force Magnitude: " + radialForce.forceMagnitude.ToString());
            
            this.soundStopwatch = 0f;
        }

        public override void OnExit()
        {
            base.OnExit();
            RadialForce radialForce = base.characterBody.gameObject.GetComponent<RadialForce>();
            radialForce.radius = 0f;
            radialForce.forceMagnitude = 0f;

            Log.LogDebug("Exiting Magnesis");
            Log.LogDebug("Force Radius: " + radialForce.radius.ToString());
            Log.LogDebug("Force Magnitude: " + radialForce.forceMagnitude.ToString());

            Util.PlaySound("Magnesis_End", base.characterBody.gameObject);
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

            if (base.characterBody.gameObject.GetComponent<TeamFilter>())
            {
                teamFilter = base.characterBody.gameObject.GetComponent<TeamFilter>();
            }
            else
            {
                teamFilter = base.characterBody.gameObject.AddComponent<TeamFilter>();
            }
            teamFilter.teamIndex = TeamIndex.Player;

            if (base.characterBody.gameObject.GetComponent<RadialForce>())
            {
                radialForce = base.characterBody.gameObject.GetComponent<RadialForce>();
            }
            else
            {
                radialForce = base.characterBody.gameObject.AddComponent<RadialForce>();
            }
            radialForce.tetherVfxOrigin = null;

            radialForce.forceMagnitude = -5000f;
            radialForce.radius = Magnesis.radius;

            if (base.inputBank.skill3.down && timer < Magnesis.duration)            
            {
                
                if (this.soundStopwatch <= 0f)
                {
                    magnesisSoundLoopID = Util.PlaySound("Magnesis_Start", base.characterBody.gameObject);
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
                    soundStopwatch = 2f;
                }
                soundStopwatch -= Time.fixedDeltaTime;                              
            }
            else
            {
                AkSoundEngine.StopPlayingID(magnesisSoundLoopID);
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