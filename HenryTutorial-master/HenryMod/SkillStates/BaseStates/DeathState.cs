using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace LinkMod.SkillStates.BaseStates
{
	public class DeathState : GenericCharacterDeath
	{
		private Vector3 previousPosition;
		private float upSpeedVelocity;
		private float upSpeed;
		private Animator modelAnimator;

		public override void OnEnter()
		{
			base.OnEnter();
			Log.LogDebug("Entering Death State");
			Vector3 vector = Vector3.up * 3f;
			if (base.characterMotor)
			{
				vector += base.characterMotor.velocity;
				base.characterMotor.enabled = false;
			}

			// RagDoll code - implement ragdoll controller?
			PlayDeathAnimation();
			if (base.cachedModelTransform)
			{
				RagdollController component = base.cachedModelTransform.GetComponent<RagdollController>();
				if (component)
				{					
					component.BeginRagdoll(vector);
				}
			}
		


			
			if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_MIPHA_NAME" && !(skillLocator.GetSkill(SkillSlot.Special).stock < 1))
			{
				SummonMipha();
			}
			else
			{
				Util.PlaySound("GameOver", base.gameObject);
			}
			
		}

        public override void OnExit()
        {
            base.OnExit();				
		}

		private void SummonMipha()
		{
			if (base.isAuthority)
			{
				Ray aimRay = base.GetAimRay();

				FireProjectileInfo miphaInfo = new FireProjectileInfo
				{
					projectilePrefab = Modules.Projectiles.miphaPrefab,
					position = aimRay.origin,
					rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
					owner = base.gameObject,
					damage = 0f,
					force = 0f,
					crit = false,
					damageColorIndex = DamageColorIndex.Default,
					target = null,
					speedOverride = 0f,
					fuseOverride = -1f,
				};
				miphaInfo.useFuseOverride = true;
				ProjectileManager.instance.FireProjectile(miphaInfo);
			}
		}

		public override void PlayDeathAnimation(float crossfadeDuration = 0.1f)
		{
			Log.LogDebug("Playing Link Death Animation");
			base.PlayAnimation("Gesture, Ovveride", "Die");
		}
		public override bool shouldAutoDestroy
		{
			get
			{
				return false;
			}
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (NetworkServer.active && base.fixedAge > 4f)
			{
				EntityState.Destroy(base.gameObject);
			}
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}

	}
}