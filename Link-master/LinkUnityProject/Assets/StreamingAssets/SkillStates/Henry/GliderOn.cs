using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;

namespace LinkMod.SkillStates
{
    public class GliderOn : BaseState
    {        
        public override void OnEnter()
        {
            base.OnEnter();          
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                float num = base.characterMotor.velocity.y;
                num = Mathf.MoveTowards(num, 10f, 10f * Time.fixedDeltaTime);
                base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num, base.characterMotor.velocity.z);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }


    }
}
