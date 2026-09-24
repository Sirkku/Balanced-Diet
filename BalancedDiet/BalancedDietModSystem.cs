using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace BalancedDiet
{
    public class BalancedDietModSystem : ModSystem
    {
        private static bool patched = false;

        public override void Start(ICoreAPI api)
        {
            if(!patched) { 
                var harmony = new Harmony(Mod.Info.ModID);
                harmony.PatchAll();
                patched = true;
            }

        }

        // public override void StartServerSide(ICoreServerAPI api) { }

        // public override void StartClientSide(ICoreClientAPI api) { }

    }
}
