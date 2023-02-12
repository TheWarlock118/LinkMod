using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace HenryMod.SkillStates
{
    public class HenryMain : GenericCharacterMain
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.gliderStateMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Jet");
        }

        public override void Update()
        {
            base.Update();            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        private EntityStateMachine gliderStateMachine;
    }
}
