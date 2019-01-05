using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using SMLHelper.V2.Handlers;

namespace SubnauticaDeathMarker
{
    public class SubnauticaDeathMarker
    {
        public static string InteractText = "RemoveDeathMarker";

        public static void Main()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("ovyerus.DeathMarker.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            LanguageHandler.SetLanguageLine(InteractText, "Remove Death Marker");
        }

        public static void NotifyOfPing(PingInstance ping)
        {
            PingManager.NotifyRename(ping);
            PingManager.NotifyColor(ping);
            PingManager.NotifyVisible(ping);
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("OnKill")]
    [HarmonyPatch(new Type[] { typeof(DamageType) })]
    public class PlayerPatcher {
        static void Prefix(DamageType damageType)
        {
            Console.WriteLine("[DeathMarker] yeah he died lmao im dying xd");

            Camera playerCam = Player.main.camRoot.mainCam;
            GameObject pingBase = new GameObject();
            GameObject pingModel = GameObject.Instantiate(Resources.Load<GameObject>("WorldEntities/Tools/DiveReelNode"));
            PingInstance ping = pingBase.AddComponent<PingInstance>();

            pingBase.AddComponent<DeathMarkerDisc>();
            //pingModel.transform.SetParent(pingBase.transform);

            // Set the ping to the player's position at death.
            pingBase.transform.position = playerCam.transform.position;
            pingModel.transform.position = pingBase.transform.position;

            // Set information about the ping.
            ping.enabled = false;
            ping.pingType = PingType.Signal;
            ping.origin = pingBase.transform;
            ping.colorIndex = 0;
            ping.visible = true;
            ping.SetLabel("Death");
            ping.enabled = true;

            SubnauticaDeathMarker.NotifyOfPing(ping);
        }
    }
}
