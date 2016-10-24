using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.UI;
using MonoEngine.Render;
using MonoEngine.Assets;
using MonoEngine.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.UI
{
    public class UIImage : UIObject
    {
        public SpriteRenderer spriteRenderer;

        public UIImage(string name, Rectangle bounds, UIAlignment boundsAlign, UIAlignment alignment, flags flags, string imageName) : base(name, bounds, boundsAlign, alignment, flags)
        {
            spriteRenderer = SpriteRenderer.MakeSpriteRenderer(name, imageName);
            spriteRenderer.Name = name;
            spriteRenderer.sprite = Resources.LoadAsset(new Sprite().GetType(), imageName, SceneManager.activeScene) as Sprite;

            spriteRenderer.sprite.DestinationRect = new Rectangle((int)origin.X, (int)origin.Y, this.bounds.Width, this.bounds.Height);
        }
    }
}
