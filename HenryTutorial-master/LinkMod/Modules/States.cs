﻿using LinkMod.SkillStates;
using LinkMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace LinkMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(LinkMain));
            entityStates.Add(typeof(DeathState));
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(SlashCombo));

            entityStates.Add(typeof(Shoot));
            entityStates.Add(typeof(ShootTri));
            entityStates.Add(typeof(ShootFast));


            entityStates.Add(typeof(Roll));

            entityStates.Add(typeof(ThrowBomb));
            entityStates.Add(typeof(Shield));

            entityStates.Add(typeof(MiphasGrace));
            entityStates.Add(typeof(DaruksProtection));
            entityStates.Add(typeof(RevalisGale));
            entityStates.Add(typeof(UrbosasFury));

            entityStates.Add(typeof(GliderOn));
            
            entityStates.Add(typeof(ParagliderTest));
        }
    }
}