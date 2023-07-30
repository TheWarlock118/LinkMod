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

        public override void ProcessJump()
        {
            base.ProcessJump();
            if (this.hasCharacterMotor && this.hasInputBank && base.isAuthority)
            {
                if(this.outer.state.GetType() != typeof(GliderState) && (base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded))
                {
                    this.outer.SetNextState(new GliderState());
                }
                else if (this.outer.state.GetType() == typeof(GliderState) && !(base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded))
                {                    
                    this.outer.SetNextStateToMain();
                }
            }
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

            #region ExitInputBankStates
            if (this.hasInputBank && base.isAuthority)
            {
                // Exit Shield
                if (this.outer.state.GetType() == typeof(Shield) && (!base.inputBank.skill2.down || base.inputBank.skill3.down || base.inputBank.skill4.down || base.inputBank.jump.down))
                {
                    this.outer.SetNextStateToMain();
                }

                // Exit Bow
                if ((this.outer.state.GetType() == typeof(Shoot) || this.outer.state.GetType() == typeof(ShootTri) || this.outer.state.GetType() == typeof(ShootFast)) && !base.inputBank.skill2.down)
                {
                    this.outer.SetNextStateToMain();
                }

                // Exit Bomb
                if ((this.outer.state.GetType() == typeof(ThrowBomb)) && !base.inputBank.skill3.down)
                {
                    this.outer.SetNextStateToMain();
                }
            }
            #endregion

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

            #region LowHPSound
            // Reset playedLowHealth sound
            if (updateValues.playedLowHealth && (characterBody.healthComponent.combinedHealth / characterBody.healthComponent.fullCombinedHealth >= .2f))
            {
                updateValues.playedLowHealth = false;
            }

            // Play low HP sound
            if (!updateValues.playedLowHealth && (characterBody.healthComponent.combinedHealth / characterBody.healthComponent.fullCombinedHealth < .2f))
            {
                Util.PlaySound("LowHP", characterBody.gameObject);
                updateValues.playedLowHealth = true;
            }
            #endregion



            #region ParagliderSlow-Bow

            // Handle bow slow-mo
            if (this.outer.state.GetType() != typeof(SlowbowState) && (characterBody.inputBank.skill2.down && characterBody.characterMotor.velocity.y < 0f && (skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_3BOW_NAME" || skillLocator.GetSkill(SkillSlot.Secondary).skillDef.skillName == "CASEY_LINK_BODY_SECONDARY_FASTBOW_NAME")))
            {
                this.outer.SetNextState(new SlowbowState());

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
