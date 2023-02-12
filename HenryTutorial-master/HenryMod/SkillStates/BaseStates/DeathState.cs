using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace HenryMod.SkillStates.BaseStates
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
			/*
			if (base.cachedModelTransform)
			{
				RagdollController component = base.cachedModelTransform.GetComponent<RagdollController>();
				if (component)
				{
					component.BeginRagdoll(vector);
				}
			}
			*/


			PlayDeathAnimation();
			if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")			
				SummonMipha();
			/*
			if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "ROB_HENRY_BODY_SPECIAL_MIPHA_NAME")
			{				
				if (!(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
				{
					if (!characterBody.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("ExtraLife")))
					{
						characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
						Util.PlaySound("MiphasGraceUse", characterBody.gameObject);

						

						// Both of these implementations cause death screen to play. Above may not be clean but it does seem necessary

						/*
						CharacterMaster characterMaster = characterBody.master;
						characterMaster.RespawnExtraLife();
						*/


						/*
						Vector3 vectorDeathFoot = characterMaster.deathFootPosition;
						if (characterMaster.killedByUnsafeArea)
                        {
							vectorDeathFoot = (TeleportHelper.FindSafeTeleportDestination(characterMaster.deathFootPosition, characterMaster.bodyPrefab.GetComponent<CharacterBody>(), RoR2Application.rng) ?? characterMaster.deathFootPosition);
                        }
						characterMaster.Respawn(vectorDeathFoot, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
						characterMaster.GetBody().AddTimedBuff(RoR2Content.Buffs.Immune, 3f);

						if (characterMaster.bodyInstanceObject)
                        {
							foreach (EntityStateMachine entityStateMachine in characterMaster.bodyInstanceObject.GetComponents<EntityStateMachine>())
                            {
								entityStateMachine.initialStateType = entityStateMachine.mainStateType;
                            }
							EffectManager.SpawnEffect(Modules.Projectiles.miphaPrefab, new EffectData
							{
								origin = vectorDeathFoot,
								rotation = characterMaster.bodyInstanceObject.transform.rotation

							}, true);

						}
											
					}
				}
			}
			*/
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