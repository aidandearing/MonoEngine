using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoEngine.Game
{
    public class EventManager : GameComponent
    {
        public delegate void OnRenderEvent();
        public delegate void OnClickEvent();

        public event OnRenderEvent OnRender;
        public event OnClickEvent OnClick;

        private static EventManager instance;
        public static EventManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new EventManager(game) : instance;

            return instance;
        }

        public EventManager(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

    }
}
