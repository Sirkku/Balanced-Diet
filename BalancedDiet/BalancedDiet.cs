using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;

namespace BalancedDiet
{
    public class BalancedDiet
    {
        public static float CalculateSatietyBonus(FoodNutritionProperties[]? foodNutritionProperties)
        {
            if (foodNutritionProperties == null) return 1.0f;

            int hasFruit = 0;
            int hasVegetable = 0;
            int hasProtein = 0;
            int hasGrain = 0;
            int hasDairy = 0;

            for (int i = 0; i < foodNutritionProperties.Length; i++)
            {
                switch (foodNutritionProperties[i].FoodCategory)
                {
                    case EnumFoodCategory.Fruit:
                        hasFruit = 1;
                        break;
                    case EnumFoodCategory.Vegetable:
                        hasVegetable = 1;
                        break;
                    case EnumFoodCategory.Protein:
                        hasProtein = 1;
                        break;
                    case EnumFoodCategory.Grain:
                        hasGrain = 1;
                        break;
                    case EnumFoodCategory.Dairy:
                        hasDairy = 1;
                        break;
                }
            }
            int components = hasFruit + hasVegetable + hasProtein + hasGrain + hasDairy;

            if (components == 2)
            {
                return BalancedDietModSystem.serverConfig.twoFoodCategorySatietyMultiplier;
            }
            else if (components > 2)
            {
                return BalancedDietModSystem.serverConfig.threeFoodCategorySatietyMultiplier;
            }

            return BalancedDietModSystem.serverConfig.oneFoodCategorySatietyMultiplier;
        }

        /**
         * 1.1f -> +10%
         * 0.9f -> -10%
         */
        public static string asPercentDiff(float multiplier)
        {
            if(multiplier >= 1.0f)
            {
                return String.Format("+{0}%", Math.Round((multiplier - 1.0f) * 100.0f));
            }
            else
            {
                return String.Format("{0}%", Math.Round((multiplier - 1.0f) * 100.0f));
            }
        }
    }
}
