using System.Xml;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        UIText uiObj;
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
            obj.AddComponent(ModelRenderer.MakeModelRenderer("BasicWall"));
            PhysicsBody2D body = new PhysicsBody2D(obj, "wall", new AABB(obj.transform, 1, 1), new PhysicsMaterial(1,0,1), PhysicsEngine.BodyType.SIMPLE);
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
            ModelRenderer renderer = ModelRenderer.MakeModelRenderer("FloorTile");
            obj.AddComponent(renderer);
            body = new PhysicsBody2D(obj, "floorB", new Circle(obj.transform, 0.5f), new PhysicsMaterial(1,0,1f), PhysicsEngine.BodyType.SIMPLE);
            body.Velocity = new Vector3(-0.01f, 0, 0);
            //body.RegisterCollisionCallback(new Collision2D.OnCollision(OnCollision2DBody));
            obj.AddComponent(body);

            GameObjectManager.AddGameObject(obj);

            uiObj = new UIText("test", GraphicsHelper.screen, new UIAlignment(), new UIAlignment(), UIObject.flags.None, "blood", "Hello Beautiful", 24);
            UIImage img = new UIImage("test", new Rectangle(0, 0, 100, 100), new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "monogameLogo");
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
            base.Update(gameTime);
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
;           base.Draw(gameTime);
        }
    }
}
