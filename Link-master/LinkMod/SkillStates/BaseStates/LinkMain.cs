using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace LinkMod.SkillStates.BaseStates
{
    public class LinkMain : GenericCharacterMain
    {
        public override void OnEnter()
        {
            base.OnEnter();            
            this.characterBody.gameObject.AddComponent<Modules.UpdateValues>();
            ChildLocator childLocator = base.GetModelChildLocator();
            childLocator.FindChild("ShieldHitbox").gameObject.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            #region SwordBuff
            if ((this.healthComponent.combinedHealth / this.healthComponent.fullCombinedHealth) >= 0.9f && !this.HasBuff(Modules.Buffs.swordProjectileBuff))
            {
                this.characterBody.AddBuff(Modules.Buffs.swordProjectileBuff);
            }
            else if(this.HasBuff(Modules.Buffs.swordProjectileBuff) && (this.healthComponent.combinedHealth / this.healthComponent.fullCombinedHealth) < 0.9f)
            {
                this.characterBody.RemoveBuff(Modules.Buffs.swordProjectileBuff);
            }
            #endregion

            Modules.UpdateValues updateValues = characterBody.gameObject.GetComponent<Modules.UpdateValues>();
            SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();

            #region MiphaGraceRemoveDioAndSetCooldown
            if (skillLocator)
            {
                if (skillLocator.GetSkill(SkillSlot.Special) != null)
                {
                    if (characterBody.inventory.itemAcquisitionOrder.Contains(ItemCatalog.FindItemIndex("UseAmbientLevel")))
                    {
                        if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_MIPHA_NAME")
                        {
                            if (!(skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                            {
                                skillLocator.GetSkill(SkillSlot.Special).RemoveAllStocks();
                                characterBody.inventory.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                                characterBody.inventory.RemoveItem(ItemCatalog.FindItemIndex("UseAmbientLevel")); // UpdateValues reset on respawn, so this is used in place of UpdateValues.resed                                    
                            }
                        }
                    }
                }
            }
            #endregion

            #region ChampionReady
            if (skillLocator && skillLocator.GetSkill(SkillSlot.Special) != null)
            {
                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_MIPHA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                    {
                        updateValues.miphaOnCooldown = true;
                    }
                    else if (updateValues.miphaOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).stock < 1))
                    {
                        updateValues.miphaOnCooldown = false;
                        if (Modules.Config.MiphaReadySound.Value)
                            Util.PlaySound("MiphasGraceReady", characterBody.gameObject);
                    }
                }

                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_DARUK_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        updateValues.darukOnCooldown = true;
                    }
                    else if (updateValues.darukOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        updateValues.darukOnCooldown = false;
                        if (Modules.Config.DarukReadySound.Value)
                            Util.PlaySound("DaruksProtectionReady", characterBody.gameObject);
                    }
                }

                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_URBOSA_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        updateValues.urbosaOnCooldown = true;
                    }
                    if (updateValues.urbosaOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        updateValues.urbosaOnCooldown = false;
                        if (Modules.Config.UrbosaReadySound.Value)
                            Util.PlaySound("UrbosasFuryReady", characterBody.gameObject);
                    }

                }

                if (skillLocator.GetSkill(SkillSlot.Special).skillDef.skillName == "CASEY_LINK_BODY_SPECIAL_REVALI_NAME")
                {
                    if ((skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        updateValues.revaliOnCooldown = true;
                    }
                    else if (updateValues.revaliOnCooldown && !(skillLocator.GetSkill(SkillSlot.Special).cooldownRemaining > 0f))
                    {
                        updateValues.revaliOnCooldown = false;
                        if (Modules.Config.RevaliReadySound.Value)
                            Util.PlaySound("RevalisGaleReady", characterBody.gameObject);
                    }

                }
            }

            #endregion

            #region ParagliderSlow-Bow

            string[] paraEquipSounds = { "Pl_Parashawl_Equip00", "Pl_Parashawl_Equip02", "Pl_Parashawl_Equip04" };
            string[] paraGlideSounds = { "Pl_Parashawl_FlapFast00", "Pl_Parashawl_FlapFast01" };
            string[] paraUnEquipSounds = { "Pl_Parashawl_UnEquip00", "Pl_Parashawl_UnEquip03", "Pl_Parashawl_UnEquip04" };

            // Reset playedLowHealth sound
            if (updateValues.playedLowHealth && (characterBody.healthComponent.combinedHealth / characterBody.healthComponent.fullCombinedHealth >= .2f))
            {
                updateValues.playedLowHealth = false;
            }

            Animator animator = characterBody.modelLocator.modelTransform.GetComponent<Animator>();

            // Stop playing Slow-Motion loop
            if (!characterBody.inputBank.skill2.down || characterBody.characterMotor.isGrounded)
            {
                AkSoundEngine.StopPlayingID(updateValues.slowMotionPlayID);
                updateValues.SlowMotionStopwatch = 0f;
                updateValues.enteredSlowMo = false;
            }

            // Play low HP sound
            if (!updateValues.playedLowHealth && (characterBody.healthComponent.combinedHealth / characterBody.healthComponent.fullCombinedHealth < .2f))
            {
                Util.PlaySound("LowHP", characterBody.gameObject);
                updateValues.playedLowHealth = true;
            }

            // Unequip paraglider - play sound, fall animation
            if (updateValues.enteredParaglider && !characterBody.inputBank.skill2.down && (!characterBody.inputBank.jump.down || characterBody.characterMotor.isGrounded))
            {
                if (!updateValues.playedFall)
                {

                    animator.Play("Fall", 2);
                    updateValues.playedFall = true;
                    updateValues.playedParaEquipSound = false;

                    if (!updateValues.playedParaUnEquipSound)
                    {
                        Util.PlaySound(paraUnEquipSounds[UnityEngine.Random.Range(0, 2)], characterBody.gameObject);
                        updateValues.playedParaUnEquipSound = true;
                    }
                }
            }

            // Handle bow slow-mo
            if (characterBody.inputBank.skill2.down && characterBody.characterMotor.velocity.y < 0f && (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_3BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME"))
            {
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

            } // Handle paraglider gliding and equipping
            else if (characterBody.inputBank.jump.down && characterBody.characterMotor.velocity.y < 0f && !characterBody.characterMotor.isGrounded)
            {
                updateValues.enteredParaglider = true;
                updateValues.playedFall = false;

                this.characterBody.AddTimedBuffAuthority(Modules.Buffs.paragliderBuff.buffIndex, 0.1f);
                

                //animator.CrossFadeInFixedTime("Glide", 0.01f, 2);
                animator.Play("Glide", 2);

                // Util.PlaySound(paraGlideSounds[Random.Range(0, 1)], base.gameObject);

                updateValues.playedParaUnEquipSound = false;

                if (!updateValues.playedParaEquipSound)
                {
                    Util.PlaySound(paraEquipSounds[UnityEngine.Random.Range(0, 2)], characterBody.gameObject);
                    updateValues.playedParaEquipSound = true;
                }

                characterBody.characterMotor.velocity = new Vector3(characterBody.characterMotor.velocity.x, -1f, characterBody.characterMotor.velocity.z);
                if (characterBody.inputBank.skill2.down)
                {
                    characterBody.characterMotor.velocity = new Vector3(characterBody.characterMotor.velocity.x, 0f, characterBody.characterMotor.velocity.z);
                }
            }

            #endregion

            #region DarukShield
            if (characterBody.HasBuff(LinkMod.Modules.Buffs.darukBuff))
            {
                characterBody.healthComponent.AddBarrier(1f);
                if (updateValues.DarukSoundStopwatch <= 0f)
                {
                    if (Modules.Config.DarukShieldSound.Value)
                    {
                        updateValues.darukShieldPlayID = Util.PlaySound("Daruk_Shield_Loop", characterBody.gameObject);
                    }
                    updateValues.DarukSoundStopwatch = 3f;
                }
                else
                {
                    updateValues.DarukSoundStopwatch -= Time.fixedDeltaTime;
                }
            }
            #endregion                            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }        
    }
}
