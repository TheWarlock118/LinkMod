using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LinkMod.Modules
{
    class UpdateValues : MonoBehaviour
    {
        public string testString = "YEAH BABY";

        public bool resed = false;
        public bool miphaOnCooldown = false;
        public bool urbosaOnCooldown = false;
        public bool revaliOnCooldown = false;
        public bool darukOnCooldown = false;
        public bool enteredParaglider = false;
        public bool playedFall = false;
        public bool blockDaruk = false;
        public bool playedParaEquipSound = false;
        public bool playedParaUnEquipSound = false;
        public bool enteredSlowMo = false;
        public bool playedLowHealth = false;

        public float SlowMotionStopwatch = 0f;
        public float DarukSoundStopwatch = 0f;

        public uint slowMotionPlayID;
        public uint darukShiedlPlayID;
        
    }
}
