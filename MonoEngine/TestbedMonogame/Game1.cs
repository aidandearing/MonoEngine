using System.Xml;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using MonoEngine.Assets;
using MonoEngine.Audio;
using MonoEngine.Game;
using MonoEngine.Physics;
using MonoEngine.Physics.Physics2D;
using MonoEngine.Render;
using MonoEngine.Shapes;
using MonoEngine.UI;

namespace TestbedMonogame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // TODO: REMOVE THIS GROSSNESS
        BasicEffect effect;
        float h = 0;
        float s = 1;
        float v = 1;
        // TODO: GROSSNESS TO HERE

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            GraphicsHelper.graphics = graphics;
            Content.RootDirectory = "Content";
            ContentHelper.Content = Content;

            Window.AllowUserResizing = false;

            // Borderless Window functionality
            Window.IsBorderless = true;
            Window.Position = Point.Zero;

            Window.Title = "I am Poor and Hungry";

            // Scaleable resolution functionality
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 540;

            GraphicsHelper.screen = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            // Depth and Buffer format functionality
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth16;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Color;

            // V-Sync
            graphics.SynchronizeWithVerticalRetrace = true;

            // Anti-Aliasing functionality
            graphics.PreferMultiSampling = true;

            // Fullscreen functionality
            //graphics.ToggleFullScreen();

            this.Components.Add(Time.Instance(this));
            this.Components.Add(GameObjectManager.Instance(this));
            this.Components.Add(SoundManager.Instance(this));
            this.Components.Add(SongManager.Instance(this));
            this.Components.Add(PhysicsEngine.Instance(this, PhysicsEngine.EngineTypes.Physics2D));

            //PhysicsEngine.PhysicsSettings.WORLD_FORCE = new Vector3(0, -9.8f, 0);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Resources.Initialise();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsHelper.spriteBatch = spriteBatch;
            GraphicsHelper.graphicsDevice = GraphicsDevice;
            this.Components.Add(RenderManager.Instance(this));

            GameObject obj = new GameObject("wall");
            ModelRenderer renderer = ModelRenderer.MakeModelRenderer("BasicWall");
            
            // This is a hideous pipeline. How do I make it prettier?
            // Obviously I can default to supplying models with a BasicEffect
            // Do I want to hide Effect behind Materials?
            // There isn't much point to it, besides giving Effect a different name, as far as I can see it.
            // But then how do I best solve this?
            // Okay, back to the beginning
            // First, default to giving them a basic effect -> Monogame already does this
            // Second, give them an easier pipeline for getting textures to load. -> Monogame seems to know how to import their textures via fbx
            // Third, how do I give them an easy to use piece of xml that allows them to simply define a material for a model?
            effect = new BasicEffect(GraphicsHelper.graphicsDevice);
            effect.Texture = Resources.LoadAsset(new Sprite().GetType(), "metal_relief_fancy", SceneManager.activeScene) as Sprite;
            effect.TextureEnabled = true;
            effect.LightingEnabled = true;
            renderer.Model.SetEffect(effect);

            obj.AddComponent(renderer);

            PhysicsBody2D body = new PhysicsBody2D(obj, "wall", new AABB(obj.transform, 1, 1), new PhysicsMaterial(1, 0, 1), PhysicsEngine.BodyType.SIMPLE);
            obj.AddComponent(body);
            obj.AddComponent(new Camera("camera"));

            GameObjectManager.AddGameObject(obj);

            //obj = new GameObject("floor");
            //obj.transform.Translate(new Vector3(0f, 0, 0));
            //obj.AddComponent(ModelRenderer.MakeModelRenderer(obj, "FloorTile"));
            //body = new PhysicsBody2D(obj, "floorA", new Circle(obj.transform, 0.5f), new PhysicsMaterial(1,0,1f), PhysicsEngine.BodyType.STATIC);
            //body.Velocity = new Vector3(0.01f, 0, 0);
            //body.RegisterCollisionCallback(new Collision2D.OnCollision(OnCollision2DBody));
            //obj.AddComponent(body);

            //GameObjectManager.AddGameObject(obj);

            obj = new GameObject("floor2");
            obj.transform.Translate(new Vector3(2f, 0, 0));
            renderer = ModelRenderer.MakeModelRenderer("FloorTile");
            obj.AddComponent(renderer);
            body = new PhysicsBody2D(obj, "floorB", new Circle(obj.transform, 0.5f), new PhysicsMaterial(1, 0, 1f), PhysicsEngine.BodyType.SIMPLE);
            //body.Velocity = new Vector3(-0.01f, 0, 0);
            //body.RegisterCollisionCallback(new Collision2D.OnCollision(OnCollision2DBody));
            obj.AddComponent(body);

            GameObjectManager.AddGameObject(obj);

            UIText textObj = new UIText("test", new UIAlignment(UIAlignment.Alignment.BottomCenter), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "blood", "Hello Beautiful", 24);
            UIImage imgObj = new UIImage("logo", new Rectangle(0, 0, 100, 100), new UIAlignment(UIAlignment.Alignment.BottomRight), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "monogameLogo");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            SceneManager.UnloadScene();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // TODO: REMOVE THIS GROSSNESS
            h = (h + 180.0f * Time.DeltaTime) % 360.0f;

            float r = 0;
            float g = 0;
            float b = 0;

            int i;
            float f, p, q, t;
            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;
                return;
            }
            // sector 0 to 5
            float h_t = h / 60;             
            i = (int)System.Math.Floor(h_t);
            // factorial part of h
            f = h_t - i;                    
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
            base.Update(gameTime);

            r = g = b = 1;
            effect.AmbientLightColor = new Vector3(r, g, b);
            // TODO: GROSSNESS GOES TO HERE

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            // TODO: Add your drawing code here
            RenderManager.Instance(this).Draw(gameTime);
            ; base.Draw(gameTime);
        }
    }
}
