using System;

namespace SubnauticaDeathMarker
{
    class DeathMarkerInteractor : HandTarget, IHandTarget
    {
        public PingInstance Ping;

        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;
            main.SetInteractText(SubnauticaDeathMarker.InteractText);
            main.SetIcon(HandReticle.IconType.Hand);
        }

        public void OnHandClick(GUIHand hand)
        {
            // Kill self when clicked
            if (Ping != null)
            {
                Ping.visible = false;
                Ping.enabled = false;
            }

            PingManager.Unregister(Ping);
            Destroy(Ping);
            Destroy(gameObject);
        }
    }
}
