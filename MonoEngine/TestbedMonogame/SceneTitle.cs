using MonoEngine;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Render;
using MonoEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestbedMonogame
{
    class SceneTitle : Scene
    {
        UIImage sky_bottom;
        Gradient sky_bottom_gradient;

        UIImage sky_stars;
        Gradient sky_stars_gradient;

        UIImage sky_top;
        Gradient sky_top_gradient;

        UIImage sky_sun_glow;
        Gradient sky_sun_glow_gradient;

        UIImage sky_sun;
        Gradient sky_sun_gradient;

        UIImage sky_moon;
        Gradient sky_moon_gradient;

        UIImage sky_glow;
        Gradient sky_glow_gradient;

        UIText title;
        Gradient title_gradient;

        UIImage[] skyline;
        Gradient skyline_gradient;

        UIImage[] skyline_windows;
        Gradient skyline_windows_gradient;

        UIImage[] skyline_glow;
        Gradient skyline_glow_gradient;

        UIImage logo;

        Material[] materials;

        public bool isInit = false;

        public SceneTitle() : base()
        {

        }

        public void Initialise()
        {
            if (isInit)
                return;

            isInit = true;

            GameObjectManager.AddGameObject(Camera.Orthographic("mainCamera", new Vector3(0, 0, 1f), Vector3.Zero));

            materials = new Material[1];

            RenderTargetSettings settings_target = new RenderTargetSettings()
            {
                blend = BlendState.AlphaBlend,
                depth = DepthStencilState.DepthRead,
                effect = null,
                mode = SpriteSortMode.Deferred,
                sampler = SamplerState.LinearClamp
            };
            Resources.LoadRenderTarget2D("UI_dithered", this, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.Depth16, 0, RenderTargetUsage.DiscardContents, settings_target);
            RenderTargetRenderer dithered = RenderTargetRenderer.MakeRenderTargetRenderer("UI_dithered", new RenderTargetSettings(), 1);
            materials[0] = Resources.LoadAsset(new Material().GetType(), "shader_noise", this) as Material;
            dithered.settings.effect = materials[0];
            dithered.settings.effect.CurrentTechnique = dithered.settings.effect.Techniques["RCDCLSB"];
            dithered.settings.effect.Parameters["S"].SetValue(Random.Range());
            dithered.settings.effect.Parameters["N"].SetValue(0.001f);

            settings_target = new RenderTargetSettings()
            {
                blend = BlendState.AlphaBlend,
                depth = DepthStencilState.DepthRead,
                effect = null,
                mode = SpriteSortMode.Deferred,
                sampler = SamplerState.PointClamp
            };
            Resources.LoadRenderTarget2D("UI_nearest", this, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.Depth16, 0, RenderTargetUsage.DiscardContents, settings_target);
            RenderTargetRenderer nearest = RenderTargetRenderer.MakeRenderTargetRenderer("UI_nearest", new RenderTargetSettings(), 2);
            //materials[1] = Resources.LoadAsset(new Material().GetType(), "shader_noise", this) as Material;
            //nearest.settings.effect = materials[1];
            //nearest.settings.effect.CurrentTechnique = nearest.settings.effect.Techniques["RCDCLSB"];
            //nearest.settings.effect.Parameters["S"].SetValue(Random.Range());
            //nearest.settings.effect.Parameters["N"].SetValue(0.0005f);

            sky_bottom_gradient = new Gradient(
            new Color[]
            {
                new Color(112,181,255),
                new Color(181,112,255),
                new Color(255,71,71),
                new Color(25,25,94),
                new Color(0,0,25),
                new Color(0,0,25),
                new Color(25,25,94),
                new Color(255,71,71),
                new Color(181,112,255),
                new Color(112,181,255)
            },
            new float[]
            {
                0.0f,
                0.11f,
                0.25f,
                0.30f,
                0.35f,
                0.65f,
                0.70f,
                0.75f,
                0.89f,
                1.0f
            });

            sky_sun_gradient = new Gradient(
            new Color[]
            {
                new Color(255,255,196),
                new Color(255,128,128),
                new Color(255,128,64),
                new Color(255,128,64),
                new Color(255,128,128),
                new Color(255,255,196)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.35f,
                0.65f,
                0.75f,
                1.0f
            });

            sky_sun_glow_gradient = new Gradient(
            new Color[]
            {
                new Color(255,255,196,64),
                new Color(255,0,0,128),
                new Color(255,196,196,255),
                new Color(255,196,196,255),
                new Color(255,0,0,128),
                new Color(255,255,196,64)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.35f,
                0.65f,
                0.75f,
                1.0f
            });

            sky_moon_gradient = new Gradient(
            new Color[]
            {
                new Color(0.75f,0.75f,1.0f,1.0f),
                new Color(0.75f,0.75f,1.0f,1.0f),
                new Color(1.0f,1.0f,0.75f,1.0f),
                new Color(0.75f,0.75f,1.0f,1.0f),
                new Color(0.75f,0.75f,1.0f,1.0f)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            }
            );

            sky_stars_gradient = new Gradient(
            new Color[]
            {
                new Color(255,255,255,0),
                new Color(255,255,255,32),
                new Color(255,255,255,255),
                new Color(255,255,255,32),
                new Color(255,255,255,0)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            });

            sky_top_gradient = new Gradient(
            new Color[]
            {
                new Color(25,25,128),
                new Color(50,50,128),
                new Color(110,25,110),
                new Color(0,0,25),
                new Color(0,0,0),
                new Color(0,0,0),
                new Color(0,0,25),
                new Color(110,25,110),
                new Color(50,50,128),
                new Color(25,25,128)
            },
            new float[]
            {
                0.0f,
                0.11f,
                0.25f,
                0.30f,
                0.35f,
                0.65f,
                0.70f,
                0.75f,
                0.89f,
                1.0f
            });

            title_gradient = new Gradient(
            new Color[]
            {
                new Color(255,255,255),
                new Color(255,255,255),
                new Color(255,255,128),
                new Color(255,255,255),
                new Color(255,255,255)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            }
            );

            skyline_gradient = new Gradient(
            new Color[]
            {
                new Color(0.5f,0.5f,0.5f,1.0f),
                new Color(0.5f,0.5f,0.5f,1.0f),
                new Color(0.0f,0.0f,0.0f,1.0f),
                new Color(0.1f,0.1f,0.1f,1.0f),
                new Color(0.25f,0.25f,0.25f,1.0f),
                new Color(0.5f,0.5f,0.5f,1.0f)
            },
            new float[]
            {
                0.0f,
                0.20f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            }
            );

            skyline_windows_gradient = new Gradient(
            new Color[]
            {
                new Color(125,175,195,255),
                new Color(175,125,195,128),
                new Color(16,16,16,255),
                new Color(175,125,195,128),
                new Color(125,175,195,255)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            }
            );

            skyline_glow_gradient = new Gradient(
            new Color[]
            {
                new Color(125,175,195,0),
                new Color(175,125,195,196),
                new Color(255,255,127,255),
                new Color(175,125,195,196),
                new Color(125,175,195,0)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            }
            );

            sky_glow_gradient = new Gradient(
            new Color[]
            {
                new Color(125,175,195,0),
                new Color(175,125,195,32),
                new Color(255,255,127,128),
                new Color(175,125,195,32),
                new Color(125,175,195,0)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.5f,
                0.75f,
                1.0f
            }
            );

            sky_top = new UIImage("Backdrop", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.TopLeft), new UIAlignment(UIAlignment.Alignment.TopLeft), UIObject.flags.None, "blank", "UI_dithered");
            sky_stars = new UIImage("Stars", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "stars", "UI_dithered");
            sky_moon = new UIImage("Moon", new Rectangle(0, 0, 128, 128), new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "moon", "UI_dithered");
            sky_bottom = new UIImage("Backdrop_gradient", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "gradient_white_to_transparent", "UI_dithered");
            sky_sun_glow = new UIImage("Sun_glow", new Rectangle(0,0,2000,2000), new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "glow_center_circle", "UI_dithered");
            sky_sun = new UIImage("Sun", new Rectangle(0, 0, 128, 128), new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "sun", "UI_dithered");

            sky_glow = new UIImage("Glow", new Rectangle(0, 0, GraphicsHelper.screen.Width * 2, GraphicsHelper.screen.Height * 2), new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "glow_bottom_circle", "UI_dithered");

            skyline = new UIImage[5];
            skyline_windows = new UIImage[5];
            skyline_glow = new UIImage[5];

            skyline[0] = new UIImage("Skyline0", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_0", "UI_nearest");
            skyline_windows[0] = new UIImage("Skyline0_window", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_0_window", "UI_nearest");
            skyline_glow[0] = new UIImage("Skyline0_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_0_glow", "UI_nearest");
            skyline[1] = new UIImage("Skyline1", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_1", "UI_nearest");
            skyline_windows[1] = new UIImage("Skyline0_window", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_1_window", "UI_nearest");
            skyline_glow[1] = new UIImage("Skyline1_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_1_glow", "UI_nearest");
            skyline[2] = new UIImage("Skyline2", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_2", "UI_nearest");
            skyline_windows[2] = new UIImage("Skyline0_window", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_2_window", "UI_nearest");
            skyline_glow[2] = new UIImage("Skyline2_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_2_glow", "UI_nearest");
            skyline[3] = new UIImage("Skyline3", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_3", "UI_nearest");
            skyline_windows[3] = new UIImage("Skyline0_window", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_3_window", "UI_nearest");
            skyline_glow[3] = new UIImage("Skyline3_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_3_glow", "UI_nearest");
            skyline[4] = new UIImage("Skyline4", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_4", "UI_nearest");
            skyline_windows[4] = new UIImage("Skyline0_window", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_4_window", "UI_nearest");
            skyline_glow[4] = new UIImage("Skyline4_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_4_glow", "UI_nearest");

            title = new UIText("Title", new UIAlignment(UIAlignment.Alignment.TopLeft), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "microtype", "I am poor and hungry", 128);

            logo = new UIImage("Logo", new Rectangle(0, 0, 100, 100), new UIAlignment(UIAlignment.Alignment.BottomRight), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "monogameLogo");
        }

        public override void Update()
        {
            Initialise();

            ((Effect)materials[0]).Parameters["S"].SetValue(Random.Range());

            float elapsed = (Time.ElapsedTime % 300.0F) / 300.0F;
            sky_top.spriteRenderer.sprite.Colour = sky_top_gradient.Evaluate(elapsed);
            sky_stars.spriteRenderer.sprite.Colour = sky_stars_gradient.Evaluate(elapsed);
            sky_bottom.spriteRenderer.sprite.Colour = sky_bottom_gradient.Evaluate(elapsed);
            sky_glow.spriteRenderer.sprite.Colour = sky_glow_gradient.Evaluate(elapsed);
            sky_sun.spriteRenderer.sprite.Colour = sky_sun_gradient.Evaluate(elapsed);
            sky_sun_glow.spriteRenderer.sprite.Colour = sky_sun_glow_gradient.Evaluate(elapsed);
            sky_moon.spriteRenderer.sprite.Colour = sky_moon_gradient.Evaluate(elapsed);

            float x_s = GraphicsHelper.screen.Width / 2.0f + (float)System.Math.Cos(elapsed * 3.14159 * 2 - MathHelper.PiOver2) * (float)GraphicsHelper.screen.Width / 2;
            float y_s = GraphicsHelper.screen.Height + (float)System.Math.Sin(elapsed * 3.14159 * 2 - MathHelper.PiOver2) * (float)GraphicsHelper.screen.Width / 2;

            float x_m = GraphicsHelper.screen.Width / 2.0f + (float)System.Math.Cos(elapsed * 3.14159 * 2 + MathHelper.PiOver2) * (float)GraphicsHelper.screen.Width / 2;
            float y_m = GraphicsHelper.screen.Height + (float)System.Math.Sin(elapsed * 3.14159 * 2 + MathHelper.PiOver2) * (float)GraphicsHelper.screen.Width / 2;

            sky_sun.spriteRenderer.sprite.DestinationRect = new Rectangle(new Point((int)x_s, (int)y_s), new Point(128, 128));
            sky_sun_glow.spriteRenderer.sprite.DestinationRect = new Rectangle(new Point((int)x_s, (int)y_s), new Point(2000, 2000));
            sky_moon.spriteRenderer.sprite.DestinationRect = new Rectangle(new Point((int)x_m, (int)y_m), new Point(128, 128));

            float i = skyline.Length;
            float t = skyline.Length * 2;
            foreach (UIImage line in skyline)
            {
                line.spriteRenderer.sprite.Colour = Color.Lerp(skyline_gradient.Evaluate(elapsed), sky_bottom_gradient.Evaluate(elapsed), i / t);
                i--;
            }

            i = skyline_windows.Length;
            t = skyline_windows.Length * 5;
            foreach (UIImage window in skyline_windows)
            {
                Color colour = skyline_windows_gradient.Evaluate(elapsed);
                Color assigned = Color.Lerp(colour, sky_bottom_gradient.Evaluate(elapsed), i / t);
                assigned.A = colour.A;
                window.spriteRenderer.sprite.Colour = assigned;
                i--;
            }

            i = skyline_glow.Length;
            t = skyline_glow.Length * 5;
            foreach (UIImage glow in skyline_glow)
            {
                Color colour = skyline_glow_gradient.Evaluate(elapsed);
                Color assigned = Color.Lerp(colour, sky_bottom_gradient.Evaluate(elapsed), i / t);
                assigned.A = colour.A;
                glow.spriteRenderer.sprite.Colour = assigned;
                i--;
            }

            title.textRenderer.Colour = title_gradient.Evaluate(elapsed);
        }
    }
}
