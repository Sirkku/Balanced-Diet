using HarmonyLib;
using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

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
                balancedDietConfig.SetFloat("oneFoodCategorySatietyMultiplier", serverConfig.oneFoodCategorySatietyMultiplier);
                balancedDietConfig.SetFloat("twoFoodCategorySatietyMultiplier", serverConfig.twoFoodCategorySatietyMultiplier);
                balancedDietConfig.SetFloat("threeFoodCategorySatietyMultiplier", serverConfig.threeFoodCategorySatietyMultiplier);
                balancedDietConfig.SetBool("allowCommandMakeMeHungry", serverConfig.allowCommandMakeMeHungry);
            }
            catch (Exception e)
            {
                api.Logger.Error("Failed to store BalancedDiet config to world config");
                api.Logger.Error(e.Message);
            }

            if(serverConfig.allowCommandMakeMeHungry)
            {
                api.ChatCommands.Create("makemehungry")
                .WithDescription("Sets player satiety to 0")
                .RequiresPrivilege(Privilege.chat)
                .RequiresPlayer()
                .HandleWith((args) =>
                {
                    var player = args.Caller.Entity.GetBehavior<EntityBehaviorHunger>();
                    player?.DairyLevel = 0;
                    player?.FruitLevel = 0;
                    player?.GrainLevel = 0;
                    player?.ProteinLevel = 0;
                    player?.VegetableLevel = 0;
                    player?.Saturation = 0;
                    return TextCommandResult.Success();
                });
            }
        }

        
        public override void StartClientSide(ICoreClientAPI api) {
            try
            {
                // load even unneeded ones so it's simpler for me to update later
                ITreeAttribute balancedDietConfig = api.World.Config.GetOrAddTreeAttribute("balancedDietConfig");
                serverConfig.oneFoodCategorySatietyMultiplier =
                    balancedDietConfig.GetFloat("oneFoodCategorySatietyMultiplier", serverConfig.oneFoodCategorySatietyMultiplier);
                serverConfig.twoFoodCategorySatietyMultiplier =
                    balancedDietConfig.GetFloat("twoFoodCategorySatietyMultiplier", serverConfig.twoFoodCategorySatietyMultiplier);
                serverConfig.threeFoodCategorySatietyMultiplier =
                    balancedDietConfig.GetFloat("threeFoodCategorySatietyMultiplier", serverConfig.threeFoodCategorySatietyMultiplier);
                serverConfig.allowCommandMakeMeHungry =
                    balancedDietConfig.GetBool("allowCommandMakeMeHungry", serverConfig.allowCommandMakeMeHungry);
            } 
            catch (Exception e)
            {
                api.Logger.Error("Failed to load BalancedDiet config from world config, using default values");
                api.Logger.Error(e.Message);
            }

            var handbook = api.ModLoader.GetModSystem<ModSystemSurvivalHandbook>();
            handbook.OnInitCustomPages += (List<GuiHandbookPage> pages) =>
            {
                BalancedDietHandbookPage new_page = new BalancedDietHandbookPage();
                new_page.Init(api);
                pages.Add(new_page);
            };
        }

    }
}
