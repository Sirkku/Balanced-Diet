using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BalancedDiet
{
    public class ServerConfig
    {
        public float oneFoodCategorySatietyMultiplier = 1.0f;
        public float twoFoodCategorySatietyMultiplier = 1.1f;
        public float threeFoodCategorySatietyMultiplier = 1.2f;

        public bool allowCommandMakeMeHungry = true;
    }
}
