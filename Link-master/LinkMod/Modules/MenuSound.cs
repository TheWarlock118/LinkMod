using System;
using RoR2;
using UnityEngine;

namespace LinkMod.Modules
{
    public class MenuSound : MonoBehaviour
    {
        private uint playID;
        private void OnEnable()
        {
            base.Invoke("PlayEffect", 0.05f);
        }

        private void PlayEffect()
        {
            this.playID = Util.PlaySound("CharSelect", base.gameObject);
        }

        private void OnDestroy()
        {
            bool flag = this.playID > 0U;
            if (flag)
            {
                AkSoundEngine.StopPlayingID(this.playID);
            }
        }
    }
}
