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
            //this.gliderStateMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Jet");
            this.characterBody.gameObject.AddComponent<Modules.UpdateValues>();
            ChildLocator childLocator = base.GetModelChildLocator();
            childLocator.FindChild("ShieldHitbox").gameObject.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            if ((this.healthComponent.combinedHealth / this.healthComponent.fullCombinedHealth) >= 0.9f && !this.HasBuff(Modules.Buffs.swordProjectileBuff))
            {
                this.characterBody.AddBuff(Modules.Buffs.swordProjectileBuff);
            }
            else if(this.HasBuff(Modules.Buffs.swordProjectileBuff) && (this.healthComponent.combinedHealth / this.healthComponent.fullCombinedHealth) < 0.9f)
            {
                this.characterBody.RemoveBuff(Modules.Buffs.swordProjectileBuff);
            }            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        //private EntityStateMachine gliderStateMachine;
    }
}
