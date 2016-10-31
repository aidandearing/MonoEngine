using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Physics
{
    [Serializable]
    public class PhysicsMaterial
    {
        public float d;
        public float Density
        {
            get
            {
                return d;
            }

            set
            {
                d = value;
            }
        }

        public float f;
        public float Friction
        {
            get
            {
                return f;
            }

            set
            {
                f = value;
            }
        }

        public float r;
        public float Restitution
        {
            get
            {
                return r;
            }

            set
            {
                r = value;
            }
        }

        public PhysicsMaterial()
        {
            d = PhysicsEngine.PhysicsSettings.DEFAULT_MATERIAL_DENSITY;
            f = PhysicsEngine.PhysicsSettings.DEFAULT_MATERIAL_FRICTION;
            r = PhysicsEngine.PhysicsSettings.DEFAULT_MATERIAL_RESTITUTION;
        }

        public PhysicsMaterial(float density, float friction, float restitution)
        {
            d = density;
            f = friction;
            r = restitution;
        }
    }
}
