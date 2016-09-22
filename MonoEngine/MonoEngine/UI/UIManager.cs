﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Render;

namespace MonoEngine.UI
{
    public class UIManager : DrawableGameComponent
    {
        public List<UIBehaviour> uiBehaviours;
        public List<UIBehaviour> deadBehaviours;
        private static UIManager instance;
        public static UIManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new UIManager(game) : instance;

            return instance;
        }

        private UIManager(Microsoft.Xna.Framework.Game game) : base(game)
        {
            uiBehaviours = new List<UIBehaviour>();
            deadBehaviours = new List<UIBehaviour>();
            //creates a render target the size of the screen for rendering UI with standard format and no depth buffer
            Resources.LoadRenderTarget2D("UI", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color, Microsoft.Xna.Framework.Graphics.DepthFormat.None, 0, Microsoft.Xna.Framework.Graphics.RenderTargetUsage.DiscardContents);
        }

        public void AddUIBehaviour(UIBehaviour behaviour)
        {
            uiBehaviours.Add(behaviour);
        }
        public void RemoveUIBehaviour(UIBehaviour behaviour)
        {
            deadBehaviours.Add(behaviour);
        }

        public void Update()
        {
            foreach (UIBehaviour behaviour in uiBehaviours)
            {
                behaviour.Update();
            }

            foreach (UIBehaviour behaviour in deadBehaviours)
            {
                uiBehaviours.Remove(behaviour);
            }
            deadBehaviours = new List<UIBehaviour>();
        }

    }
}
