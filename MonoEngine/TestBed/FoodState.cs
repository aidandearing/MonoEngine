using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    public class FoodState
    {
        public enum CutState { idc, none, sliced, pasted, blended };
        public enum TempState { idc, frozen, cooled, room, warm };

        public CutState cutstate;
        public TempState tempstate;
        public string name;

        public FoodState(string name)
        {
            this.name = name;
        }

        public string Name()
        {
            return Food.foods[name].Name(cutstate, tempstate);
        }

        public int Stack()
        {
            return Food.foods[name].Stack(cutstate, tempstate);
        }

        public float Value()
        {
            return Food.foods[name].Value(cutstate, tempstate);
        }

        public void Write()
        {
            Console.WriteLine("\n" + Name() + "\n" + Stack() + "\n" + Value());
        }
    }
}
