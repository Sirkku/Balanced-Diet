using HarmonyLib;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace BalancedDiet
{

    public class BalancedDietModSystem : ModSystem
    {
        public static ServerConfig serverConfig = new ServerConfig();
        private static bool patched = false;

        public override void Start(ICoreAPI api)
        {
            if(!patched) { 
                var harmony = new Harmony(Mod.Info.ModID);
                harmony.PatchAll();
                patched = true;
            }
        }

        public override void StartServerSide(ICoreServerAPI api) {
            try
            {
                serverConfig = api.LoadModConfig<ServerConfig>("balanceddiet.json") ?? new ServerConfig();
                api.StoreModConfig<ServerConfig>(serverConfig, "balanceddiet.json");
            }
            catch (Exception e)
            {
                api.Logger.Error("Failed to load or store BalancedDiet config, using default values");
                api.Logger.Error(e.Message);
                serverConfig = new ServerConfig();
            }

            api.World.Config.RemoveAttribute("balancedDietConfig");
            try
            {
                ITreeAttribute balancedDietConfig = api.World.Config.GetOrAddTreeAttribute("balancedDietConfig");
                balancedDietConfig.SetFloat("twoFoodCategorySatietyBonus", serverConfig.twoFoodCategorySatietyBonus);
                balancedDietConfig.SetFloat("threeFoodCategorySatietyBonus", serverConfig.threeFoodCategorySatietyBonus);
            }
            catch (Exception e)
            {
                api.Logger.Error("Failed to store BalancedDiet config to world config");
                api.Logger.Error(e.Message);
            }
        }

        
        public override void StartClientSide(ICoreClientAPI api) {
            try
            {
                ITreeAttribute balancedDietConfig = api.World.Config.GetOrAddTreeAttribute("balancedDietConfig");
                serverConfig.twoFoodCategorySatietyBonus =
                    balancedDietConfig.GetFloat("twoFoodCategorySatietyBonus", serverConfig.twoFoodCategorySatietyBonus);
                serverConfig.threeFoodCategorySatietyBonus =
                    balancedDietConfig.GetFloat("threeFoodCategorySatietyBonus", serverConfig.threeFoodCategorySatietyBonus);
            } 
            catch (Exception e)
            {
                api.Logger.Error("Failed to load BalancedDiet config from world config, using default values");
                api.Logger.Error(e.Message);
            }
        }

    }
}
