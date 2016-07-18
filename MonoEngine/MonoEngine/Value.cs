using System.Collections.Generic;

namespace MonoEngine
{
    class Value
    {
        public enum Type { INT, FLOAT, STRING, LIST, RANDOM };

        private object value;
        private Type type;

        public Value(int v)
        {
            value = v;
            type = Type.INT;
        }

        public Value(float v)
        {
            value = v;
            type = Type.FLOAT;
        }

        public Value(string v)
        {
            value = v;
            type = Type.STRING;
        }

        public Value(List<Value> v)
        {
            value = v;
            type = Type.LIST;
        }

        public Value(List<Value> v, bool isList)
        {
            value = v;

            if (isList)
                type = Type.LIST;
            else
                type = Type.RANDOM;
        }

        
    }
}
