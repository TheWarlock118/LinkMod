using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace LinkMod.SkillStates
{
    public class GliderState : SkillStates.BaseStates.LinkMain
    {
        Modules.UpdateValues updateValues;
        SkillLocator skillLocator;
        Animator animator;

        string[] paraEquipSounds = { "Pl_Parashawl_Equip00", "Pl_Parashawl_Equip02", "Pl_Parashawl_Equip04" };
        string[] paraGlideSounds = { "Pl_Parashawl_FlapFast00", "Pl_Parashawl_FlapFast01" };
        string[] paraUnEquipSounds = { "Pl_Parashawl_UnEquip00", "Pl_Parashawl_UnEquip03", "Pl_Parashawl_UnEquip04" };

        public override void OnEnter()
        {
            base.OnEnter();
            updateValues = characterBody.gameObject.GetComponent<Modules.UpdateValues>();
            skillLocator = characterBody.GetComponent<SkillLocator>();
            animator = characterBody.modelLocator.modelTransform.GetComponent<Animator>();

            updateValues.enteredParaglider = true;
            updateValues.playedFall = false;                                        
        }

        public override void Update()
        {
            base.Update();
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();

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
        }
    }
}
