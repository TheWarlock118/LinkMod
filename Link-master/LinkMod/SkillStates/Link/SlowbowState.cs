using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class SlowbowState : BaseSkillState
    {
        Modules.UpdateValues updateValues;
        Animator animator;


        public override void OnEnter()
        {
            base.OnEnter();
            updateValues = characterBody.gameObject.GetComponent<Modules.UpdateValues>();
            animator = characterBody.modelLocator.modelTransform.GetComponent<Animator>();
        }

        public override void Update()
        {
            base.Update();
            if (skillLocator.GetSkill(SkillSlot.Secondary).cooldownRemaining == skillLocator.GetSkill(SkillSlot.Secondary).baseRechargeInterval) //If bow is not mid cooldown, allow for extreme slowfall
            {
                characterBody.characterMotor.velocity = Vector3.zero;
            }
            // Don't need to check recharge interval if using fast bow
            if (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME")
            {
                characterBody.characterMotor.velocity = Vector3.zero;
            }
            if (!updateValues.enteredSlowMo)
            {
                if (Modules.Config.SlowBowSound.Value)
                {
                    if (AkSoundEngine.GetGameObjectFromPlayingID(updateValues.slowMotionPlayID) != 0)
                        Util.PlaySound("SlowMotionEnter", characterBody.gameObject);
                }
                updateValues.enteredSlowMo = true;
            }
            if (updateValues.SlowMotionStopwatch <= 0f)
            {
                if (Modules.Config.SlowBowSound.Value)
                {
                    updateValues.slowMotionPlayID = Util.PlaySound("SlowMotionLoop", characterBody.gameObject);
                }
                updateValues.SlowMotionStopwatch = 2f;
            }
            else
            {
                updateValues.SlowMotionStopwatch -= Time.fixedDeltaTime;
            }

            if (!(characterBody.inputBank.skill2.down && characterBody.characterMotor.velocity.y < 0f && (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_3BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME")))
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            // Stop playing Slow-Motion loop
            if (!characterBody.inputBank.skill2.down || characterBody.characterMotor.isGrounded)
            {
                AkSoundEngine.StopPlayingID(updateValues.slowMotionPlayID);
                updateValues.SlowMotionStopwatch = 0f;
                updateValues.enteredSlowMo = false;
            }
        }
    }
}
