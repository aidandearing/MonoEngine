using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine.Input
{
    class InputHandler
    {
        public List<InputProfile> inputProfiles;

        public InputHandler()
        {
            inputProfiles = new List<InputProfile>();
        }

    }
}
