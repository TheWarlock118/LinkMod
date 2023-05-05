using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace LinkMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;
        internal static BuffDef darukBuff;
        internal static BuffDef swordProjectileBuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {            
            darukBuff = AddNewBuff("DarukBuff", RoR2.LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, Color.red, false, false);
            swordProjectileBuff = AddNewBuff("SwordProjectileBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("SwordBuff"), Color.white, false, false);            
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}