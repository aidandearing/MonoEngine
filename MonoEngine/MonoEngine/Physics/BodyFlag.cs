using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Physics
{
    public class BodyFlag
    {
        public PhysicsEngine.BodyType flagBodyType = 0;

        public BodyFlag(int type)
        {
            flagBodyType = (PhysicsEngine.BodyType)type;
        }

        public BodyFlag(PhysicsEngine.BodyType type)
        {
            flagBodyType = type;
        }

        public override string ToString()
        {
            return "" + (int)flagBodyType;
        }

        public static implicit operator PhysicsEngine.BodyType(BodyFlag flag)
        {
            return flag.flagBodyType;
        }

        public bool HasFlag(PhysicsEngine.BodyType type)
        {
            return flagBodyType.HasFlag(type);
        }
    }
}
