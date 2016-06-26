using System;
using System.Collections.Generic;

namespace TestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            Food.foods = new Dictionary<string, Food>();
            Food.foods.Add("apple", new Food("apple", 1, 1.00f));
            Food.foods["apple"].nameModifiers.Add(new FoodModifier(FoodModifier.Modifier.prepend, FoodState.CutState.sliced, FoodState.TempState.idc, "sliced "));
            Food.foods["apple"].stackModifiers.Add(new FoodModifier(FoodModifier.Modifier.multiply, FoodState.CutState.sliced, FoodState.TempState.idc, "12"));
            Food.foods["apple"].nameModifiers.Add(new FoodModifier(FoodModifier.Modifier.prepend, FoodState.CutState.idc, FoodState.TempState.frozen, "frozen "));
            Food.foods["apple"].nameModifiers.Add(new FoodModifier(FoodModifier.Modifier.prepend, FoodState.CutState.idc, FoodState.TempState.cooled, "chilled "));
            Food.foods["apple"].nameModifiers.Add(new FoodModifier(FoodModifier.Modifier.prepend, FoodState.CutState.idc, FoodState.TempState.warm, "warmed "));
            FoodState apple = new FoodState("apple");
            apple.Write();
            apple.tempstate = FoodState.TempState.frozen;
            apple.Write();
            apple.tempstate = FoodState.TempState.cooled;
            apple.Write();
            apple.tempstate = FoodState.TempState.warm;
            apple.Write();
            apple.tempstate = FoodState.TempState.room;
            apple.Write();
            apple.cutstate = FoodState.CutState.sliced;
            apple.Write();
            apple.tempstate = FoodState.TempState.frozen;
            apple.Write();
            apple.tempstate = FoodState.TempState.cooled;
            apple.Write();
            apple.tempstate = FoodState.TempState.warm;
            apple.Write();
            
            Console.Read();
        }
    }
}
