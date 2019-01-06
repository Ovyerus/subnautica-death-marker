using System;
using System.Reflection;
using System.IO;
using Harmony;
using UnityEngine;
using SMLHelper.V2.Handlers;
using Oculus.Newtonsoft.Json;

namespace SubnauticaDeathMarker
{
    using ConfigClasses;

    public class SubnauticaDeathMarker
    {
        public static string InteractText = "RemoveDeathMarker";
        public static ConfigJson Config;

        public static void Main()
        {
            string serialisedConfig = File.ReadAllText("./QMods/SubnauticaDeathMarker/mod.json");
            Config = JsonConvert.DeserializeObject<ModJson>(serialisedConfig).Config;

            HarmonyInstance harmony = HarmonyInstance.Create("ovyerus.DeathMarker.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            LanguageHandler.SetLanguageLine(InteractText, "Remove Death Marker");
            Console.WriteLine("[DeathMarker] Successfully loaded.");
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("OnKill")]
    [HarmonyPatch(new Type[] { typeof(DamageType) })]
    public class PlayerPatcher {
        static void Postfix(DamageType damageType)
        {
            Console.WriteLine("[DeathMarker] yeah he died lmao im dying xd");

            Camera playerCam = Player.main.camRoot.mainCam;
            GameObject pingBase = new GameObject();
            GameObject pingModel = GameObject.Instantiate(Resources.Load<GameObject>("WorldEntities/Tools/DiveReelNode"));
            PingInstance ping = pingBase.AddComponent<PingInstance>();
            Light pingLight = pingModel.AddComponent<Light>();
            SphereCollider pingCollider = pingModel.AddComponent<SphereCollider>();
            DeathMarkerInteractor pingInteractor = pingModel.AddComponent<DeathMarkerInteractor>();

            pingCollider.radius = 0.35f;

            // Set options for the light.
            pingLight.color = new Color(112 / 255, 255 / 255, 3 / 255, 0.25f);
            pingLight.type = LightType.Point;
            pingLight.range = 10f;

            pingModel.transform.SetParent(pingBase.transform);
            pingLight.transform.SetParent(pingModel.transform);
            pingCollider.transform.SetParent(pingModel.transform);

            // Set the ping to the player's position at death.
            Vector3 playerPos = playerCam.transform.position;
            pingBase.transform.position = playerPos;

            // Set information about the ping.
            ping.enabled = false;
            ping.pingType = PingType.Signal;
            ping.origin = pingBase.transform;
            ping.colorIndex = 0;
            ping.visible = true;

            if (SubnauticaDeathMarker.Config.AddCoords) ping.SetLabel($"Death ({playerPos.x:F1}, {playerPos.y:F1}, {playerPos.z:F1})");
            else ping.SetLabel("Death");

            ping.enabled = true;

            pingInteractor.Ping = ping;

            PingManager.Register(ping);
        }
    }
}
