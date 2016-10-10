using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Physics.Physics2D
{
    public interface IPhysicsListener2D
    {
        void OnCollision2DStay(Collision2D collision);
        void OnCollision2DStart(Collision2D collision);
        void OnCollision2DStop(Collision2D collision);
    }
}
