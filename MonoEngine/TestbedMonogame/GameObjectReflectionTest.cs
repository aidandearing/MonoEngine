using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestbedMonogame
{
    public class GameObjectReflectionTest : MonoEngine.Game.GameObject
    {
        public GameObjectReflectionTest(string name) : base(name)
        {

        }

        public void OnCollision2DStart(MonoEngine.Physics.Physics2D.Collision2D collision)
        {
            Console.WriteLine("Hi, " + Name + " collide start");
        }

        public void OnCollision2DStay(MonoEngine.Physics.Physics2D.Collision2D collision)
        {
            Console.WriteLine("Hi, " + Name + " collide still");
        }

        public void OnCollision2DStop(MonoEngine.Physics.Physics2D.Collision2D collision)
        {
            Console.WriteLine("Hi, " + Name + " collide stop");
        }
    }
}
