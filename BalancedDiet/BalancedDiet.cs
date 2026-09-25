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
            float satietyBonus = 1.0f;
            int components = hasFruit + hasVegetable + hasProtein + hasGrain + hasDairy;

            if (components == 2)
            {
                satietyBonus = BalancedDietModSystem.serverConfig.twoFoodCategorySatietyBonus;
            }
            else if (components > 2)
            {
                satietyBonus = BalancedDietModSystem.serverConfig.threeFoodCategorySatietyBonus;
            }

            return satietyBonus;
        }
    }
}
