using BepInEx.Configuration;
using UnityEngine;

namespace LinkMod.Modules
{
    public static class Config
    {
        public static void ReadConfig()
        {
            Config.MiphaReadySound = LinkPlugin.instance.Config.Bind<bool>("Champion Ready Sounds", "Miphas Grace", true, "Enables the sound 'Mipha's Grace is Ready' when the cooldown is over.");
            Config.RevaliReadySound = LinkPlugin.instance.Config.Bind<bool>("Champion Ready Sounds", "Revalis Gale", true, "Enables the sound 'Revali's Gale is Ready' when the cooldown is over.");
            Config.UrbosaReadySound = LinkPlugin.instance.Config.Bind<bool>("Champion Ready Sounds", "Urbosas Fury", true, "Enables the sound 'Urbosa's Fury is Ready' when the cooldown is over.");
            Config.DarukReadySound = LinkPlugin.instance.Config.Bind<bool>("Champion Ready Sounds", "Daruks Protection", true, "Enables the sound 'Daruk's Protection is Ready to Roll' when the cooldown is over.");

            Config.SlowBowSound = LinkPlugin.instance.Config.Bind<bool>("Character Sounds", "Bow Slow-Mo", true, "Enables the looping slow-mo sound when using a bow while falling.");
            Config.DarukShieldSound = LinkPlugin.instance.Config.Bind<bool>("Character Sounds", "Daruk Shield", true, "Enables the looping sound that plays when the Daruk's protection shield is up.");
            Config.StasisTimerSound = LinkPlugin.instance.Config.Bind<bool>("Character Sounds", "Stasis Timer", true, "Enables the timer count-down sound when using Stasis.");            

            Config.SwordDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Sword Damage %", 300, "Default: 300% Sword Beam does 1/2 this damage.");
            Config.BowDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Bow Damage %", 600, "Default: 600% Falcon Bow fire arrows deal 1/2 this damage, Great Eagle Bow frost arrows deal 1/3.");
            Config.BombDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Bomb Damage %", 500, "Default: 500%");
            Config.BombArrowDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Bomb Arrow Damage %", 400, "Default: 400%");
            Config.UrbosaDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Urbosa Damage %", 600, "Default: 600%");
            Config.RevaliDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Revali Damage %", 50, "Default: 50%");
            Config.CryonisDamageCoeffConfig = LinkPlugin.instance.Config.Bind<float>("Balance", "Cryonis Damage %", 250, "Default: 250%");
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return LinkPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }
        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return LinkPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }


        public static ConfigEntry<bool> CharacterEnabledConfig;

        public static ConfigEntry<bool> MiphaReadySound;
        public static ConfigEntry<bool> RevaliReadySound;
        public static ConfigEntry<bool> UrbosaReadySound;
        public static ConfigEntry<bool> DarukReadySound;

        public static ConfigEntry<bool> SlowBowSound;
        public static ConfigEntry<bool> StasisTimerSound;
        public static ConfigEntry<bool> DarukShieldSound;

        public static ConfigEntry<float> SwordDamageCoeffConfig;
        public static ConfigEntry<float> BowDamageCoeffConfig;
        public static ConfigEntry<float> BombDamageCoeffConfig;
        public static ConfigEntry<float> BombArrowDamageCoeffConfig;
        public static ConfigEntry<float> UrbosaDamageCoeffConfig;
        public static ConfigEntry<float> RevaliDamageCoeffConfig;
        public static ConfigEntry<float> CryonisDamageCoeffConfig;
    }


}