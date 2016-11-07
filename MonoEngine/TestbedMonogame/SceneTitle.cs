using MonoEngine;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Render;
using MonoEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestbedMonogame
{
    class SceneTitle : Scene
    {
        UIImage sky_bottom;
        Gradient sky_bottom_gradient;

        UIImage sky_top;
        Gradient sky_top_gradient;

        UIText title;
        Gradient title_gradient;

        UIImage[] skyline;
        Gradient skyline_gradient;

        UIImage[] skyline_glow;
        Gradient skyline_glow_gradient;

        UIImage logo;

        Material material;

        public bool isInit = false;

        public SceneTitle() : base()
        {

        }

        public void Initialise()
        {
            if (isInit)
                return;

            isInit = true;
            Resources.LoadRenderTarget2D("UI_dithered", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            RenderTargetRenderer.Settings settings = new RenderTargetRenderer.Settings();
            settings.sampler = SamplerState.LinearClamp;
            RenderTargetRenderer dithered = RenderTargetRenderer.MakeRenderTargetRenderer("UI_dithered", settings, 1);
            material = Resources.LoadAsset(new Material().GetType(), "shader_noise", this) as Material;
            dithered.settings.effect = material;
            dithered.settings.effect.CurrentTechnique = dithered.settings.effect.Techniques["RCDCSSB"];
            dithered.settings.effect.Parameters["S"].SetValue(Random.Range());
            dithered.settings.effect.Parameters["N"].SetValue(0.4f);

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
                new Color(255,255,50),
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

            skyline_glow_gradient = new Gradient(
            new Color[]
            {
                new Color(1.0f,1.0f,1.0f,0.0f),
                new Color(1.0f,1.0f,1.0f,0.0f),
                new Color(1.0f,1.0f,0.2f,1.0f),
                new Color(1.0f,1.0f,0.2f,1.0f),
                new Color(1.0f,1.0f,0.2f,1.0f),
                new Color(1.0f,1.0f,1.0f,0.0f),
                new Color(1.0f,1.0f,1.0f,0.0f)
            },
            new float[]
            {
                0.0f,
                0.25f,
                0.2501f,
                0.5f,
                0.7499f,
                0.75f,
                1.0f
            }
            );

            sky_top = new UIImage("Backdrop", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.TopLeft), new UIAlignment(UIAlignment.Alignment.TopLeft), UIObject.flags.None, "blank", "UI_dithered");

            skyline = new UIImage[5];
            skyline_glow = new UIImage[3];

            sky_bottom = new UIImage("Backdrop_gradient0", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "gradient_white_to_transparent", "UI_dithered");

            skyline[0] = new UIImage("Skyline0", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_0");
            skyline_glow[0] = new UIImage("Skyline0_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_0_glow");
            skyline[1] = new UIImage("Skyline1", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_1");
            skyline_glow[1] = new UIImage("Skyline0_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_1_glow");
            skyline[2] = new UIImage("Skyline2", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_2");
            skyline_glow[2] = new UIImage("Skyline0_glow", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_2_glow");
            skyline[3] = new UIImage("Skyline3", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_3");
            skyline[4] = new UIImage("Skyline4", GraphicsHelper.screen, new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "skyline_4");

            title = new UIText("Title", new UIAlignment(UIAlignment.Alignment.Center), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "crumbled", "I am poor and hungry", 96);

            logo = new UIImage("Logo", new Rectangle(0, 0, 100, 100), new UIAlignment(UIAlignment.Alignment.BottomRight), new UIAlignment(UIAlignment.Alignment.Center), UIObject.flags.None, "monogameLogo");
        }

        public override void Update()
        {
            Initialise();

            //((Effect)material).Parameters["S"].SetValue(Random.Range());

            float elapsed = (Time.ElapsedTime % 60) / 60.0f;
            sky_top.spriteRenderer.sprite.Colour = sky_top_gradient.Evaluate(elapsed);
            sky_bottom.spriteRenderer.sprite.Colour = sky_bottom_gradient.Evaluate(elapsed);

            float i = skyline.Length;
            float t = skyline.Length * 5;
            foreach (UIImage line in skyline)
            {
                line.spriteRenderer.sprite.Colour = Color.Lerp(skyline_gradient.Evaluate(elapsed), sky_bottom_gradient.Evaluate(elapsed), i/t);
                i--;
            }

            i = skyline_glow.Length;
            t = skyline_glow.Length * 1;
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
