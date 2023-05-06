using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace LinkMod.Modules
{
    class UpdateValues : MonoBehaviour
    {                
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
        public uint darukShieldPlayID;

        public int blockedAttacks = 0;
        public int darukBlockedAttacks = 0;


        // I have to give credit to the Enforcer mod for some heavy inspiration. https://github.com/TheTimeSweeper/EnforcerMod/blob/61376500e7edc007e3998bc9e72eaeffeb173ab6/EnforcerMod_VS/Components/Enforcer/EnforcerComponent.cs
        public bool ShouldBlock(Vector3 attackPos, float blockAngle)
        {
            bool shouldBlock = false;

            Vector3 aimDirection = base.GetComponent<CharacterBody>().inputBank.aimDirection;
            Vector3 enemyDirection = attackPos - base.GetComponent<CharacterBody>().corePosition;

            float enemyAngle = Vector3.Angle(aimDirection, enemyDirection);
            if (enemyAngle < blockAngle)
                shouldBlock = true;

            return shouldBlock;
        }
        
    }
}
