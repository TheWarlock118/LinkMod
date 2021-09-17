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
            if (!characterBody.healthComponent.alive)
            {
                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (inputBank.jump.down)
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                {
                    baseToken = "YOU HAVE JUMPED"
                });
            }
            if (!characterBody.healthComponent.alive)
            {
                characterBody.inventory.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"), 1);
            }
        }


        private EntityStateMachine gliderStateMachine;
    }
}
