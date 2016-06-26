using System;
using System.Collections.Generic;

namespace TestBed
{
    public class Food
    {
        public static Dictionary<string, Food> foods;

        private string name;
        public List<FoodModifier> nameModifiers = new List<FoodModifier>();
        public string Name(FoodState state)
        {
            string n = name;

            foreach (FoodModifier mod in nameModifiers)
            {
                if ((mod.cutstate == state.cutstate || mod.cutstate == FoodState.CutState.idc) && (mod.tempstate == state.tempstate || mod.tempstate == FoodState.TempState.idc))
                {
                    n = mod.GetValue(n);
                }
            }

            return n;
        }

        private int stack;
        public List<FoodModifier> stackModifiers = new List<FoodModifier>();
        public int Stack(FoodState state)
        {
            int s = stack;

            foreach (FoodModifier mod in stackModifiers)
            {
                if ((mod.cutstate == state.cutstate || mod.cutstate == FoodState.CutState.idc) && (mod.tempstate == state.tempstate || mod.tempstate == FoodState.TempState.idc))
                {
                    s = mod.GetValue(s);
                }
            }

            return s;
        }

        private float value;
        public List<FoodModifier> valueModifiers = new List<FoodModifier>();
        public float Value(FoodState state)
        {
            float v = value;

            foreach (FoodModifier mod in valueModifiers)
            {
                if ((mod.cutstate == state.cutstate || mod.cutstate == FoodState.CutState.idc) && (mod.tempstate == state.tempstate || mod.tempstate == FoodState.TempState.idc))
                {
                    v = mod.GetValue(v);
                }
            }

            return v;
        }

        public Food(string name, int stack, float value)
        {
            this.name = name;
            this.stack = stack;
            this.value = value;
        }

        public FoodState MakeFoodInstance(string name)
        {
            return new FoodState(name);
        }

        public void Write()
        {
            Console.WriteLine("\n" + name + "\n" + stack + "\n" + value);
        }
    }
}
