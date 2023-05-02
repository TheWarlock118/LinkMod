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
        }

        public override void Update()
        {
            base.Update();            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        //private EntityStateMachine gliderStateMachine;
    }
}
