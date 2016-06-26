using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    public class FoodModifier
    {
        public enum Modifier { add, multiply, prepend, append };

        public Modifier modifier;
        public FoodState.CutState cutstate;
        public FoodState.TempState tempstate;
        public string value;

        public FoodModifier(Modifier mod, FoodState.CutState cs, FoodState.TempState ts, string val)
        {
            modifier = mod;
            value = val;

            cutstate = cs;
            tempstate = ts;
        }

        public int GetValue(int val)
        {
            switch(modifier)
            {
                case Modifier.add:
                    return int.Parse(value) + val;
                case Modifier.multiply:
                    return int.Parse(value) * val;
                case Modifier.prepend:
                    return int.Parse(value + val.ToString());
                case Modifier.append:
                    return int.Parse(val.ToString() + value);
            }

            return val;
        }

        public float GetValue(float val)
        {
            switch (modifier)
            {
                case Modifier.add:
                    return float.Parse(value) + val;
                case Modifier.multiply:
                    return float.Parse(value) * val;
                case Modifier.prepend:
                    return float.Parse(value + val.ToString());
                case Modifier.append:
                    return float.Parse(val.ToString() + value);
            }

            return val;
        }

        public string GetValue(string val)
        {
            switch (modifier)
            {
                case Modifier.add:
                    return value + val;
                case Modifier.multiply:
                    return value + val;
                case Modifier.prepend:
                    return value + val;
                case Modifier.append:
                    return val + value;
            }

            return val;
        }
    }
}
