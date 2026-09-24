using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace BalancedDiet.Patches
{

    [HarmonyPatch(typeof(BlockMeal), "GetContentNutritionProperties", new Type[] { typeof(IWorldAccessor), typeof(ItemSlot), typeof(ItemStack[]), typeof(EntityAgent), typeof(bool), typeof(float), typeof(float) })]
    public class ModifyGetContentNutritionProperties
    {
        public static void Postfix(ref FoodNutritionProperties[] __result)
        {
            float satietyBonus = BalancedDiet.CalculateSatietyBonus(__result);

            for (int i = 0; i < __result.Length; i++)
            {
                __result[i]?.Satiety *= satietyBonus;
            }
        }
    }
}
