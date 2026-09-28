using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
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

    [HarmonyPatch(typeof(BlockMeal), "GetContentNutritionFacts", new Type[] { typeof(IWorldAccessor), typeof(ItemSlot), typeof(ItemStack[]), typeof(EntityAgent), typeof(bool), typeof(float), typeof(float) })]
    public class ModifyGetContentNutritionFacts
    {
        public static bool Prefix(ref String __result, IWorldAccessor world, ItemSlot inSlotorFirstSlot, ItemStack[] contentStacks, EntityAgent? forEntity, bool mulWithStacksize = false, float nutritionMul = 1f, float healthMul = 1f)
        {
            FoodNutritionProperties[] contentNutritionProperties = BlockMeal.GetContentNutritionProperties(world, inSlotorFirstSlot, contentStacks, forEntity, mulWithStacksize, nutritionMul, healthMul);
            Dictionary<EnumFoodCategory, float> dictionary = new Dictionary<EnumFoodCategory, float>();
            float num = 0f;
            foreach (FoodNutritionProperties foodNutritionProperties in contentNutritionProperties)
            {
                if (foodNutritionProperties != null)
                {
                    dictionary.TryGetValue(foodNutritionProperties.FoodCategory, out var value);
                    num += foodNutritionProperties.Health;
                    dictionary[foodNutritionProperties.FoodCategory] = value + foodNutritionProperties.Satiety;
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Lang.Get("Nutrition Facts"));
            float satietyBonus = BalancedDiet.CalculateSatietyBonus(contentNutritionProperties);
            if (satietyBonus < 1.01f && satietyBonus > 0.99f)
            {
                foreach (KeyValuePair<EnumFoodCategory, float> item in dictionary)
                {
                    stringBuilder.AppendLine(Lang.Get("nutrition-facts-line-satiety", Lang.Get("foodcategory-" + item.Key.ToString().ToLowerInvariant()), Math.Round(item.Value)));
                }
            }
            else 
            {
                foreach (KeyValuePair<EnumFoodCategory, float> item in dictionary)
                {
                    stringBuilder.Append(Lang.Get("nutrition-facts-line-satiety", Lang.Get("foodcategory-" + item.Key.ToString().ToLowerInvariant()), Math.Round(item.Value)));
                    
                    stringBuilder.AppendFormat(" ({0})", BalancedDiet.asPercentDiff(satietyBonus));
                   
                    stringBuilder.Append(Environment.NewLine);
                }
            }

            if (num != 0f)
            {
                stringBuilder.AppendLine("- " + Lang.Get("Health: {0}{1} hp", (num > 0f) ? "+" : "", num));
            }

            __result = stringBuilder.ToString();
            return false;
        }
    }
}
