using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoEngine.UI
{
    public abstract class UIBehaviour
    {
        public UIObject obj;
        public float duration;

        public UIBehaviour(UIObject obj, float duration)
        {
            this.obj = obj;
            this.duration = duration;
        }

        public virtual void Update()
        {
            duration -= Time.DeltaTime;
        }
    }
}
