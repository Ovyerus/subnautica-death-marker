using System;
using UnityEngine;

namespace SubnauticaDeathMarker
{
    class DeathMarkerDisc : HandTarget, IHandTarget
    {
        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;
            main.SetInteractText(SubnauticaDeathMarker.InteractText);
            main.SetIcon(HandReticle.IconType.Hand);
        }

        public void OnHandClick(GUIHand hand)
        {
            // Kill self when clicked
            Destroy(gameObject);
        }
    }
}
