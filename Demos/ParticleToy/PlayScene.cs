using Verdant;
using Verdant.UI;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ParticleToy
{
    internal class PlayScene : Scene
    {

        public PlayScene(string id) : base(id) { }

        ParticleSystem particleSystem;
        private Timer spawnTimer;

        public override void Initialize()
        {
            base.Initialize();

            InitializeParticleSystem();
            InitializeControls();
        }

        private void InitializeParticleSystem()
        {
            particleSystem = new ParticleSystem(new Vec2(600, 300));
            particleSystem.SelfRemove = false;
            EntityManager.AddEntity(particleSystem);

            spawnTimer = new Timer(500, (Timer t) =>
            {
                particleSystem.SpawnParticle(new Particle(Resources.Particle));
                spawnTimer.Reset();
                spawnTimer.Start();
            });
            spawnTimer.Start();
        }

        private void InitializeControls()
        {
            UIStack controlStack = new UIStack(new Vec2(20, 20));
            controlStack.Gap = 12;

            // RADIUS
            UIStack radiusStack = new UIStack(Vec2.Zero, vertical: false);
            radiusStack.Gap = 12;
            radiusStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Radius "));
            UITextBox radiusText = new UITextBox(Vec2.Zero, Resources.Font);
            UISlider radiusSlider = new UISlider(Vec2.Zero, 0, 128, Resources.SliderIndicator, Resources.SliderBar, 250);
            radiusSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.Radius = radiusSlider.Value;
                radiusText.Text = radiusSlider.Value.ToString("0.00");
            };
            radiusText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(radiusText.Text, out value);
                radiusSlider.Value = value;
            };
            radiusStack.AddElement(radiusSlider);
            radiusStack.AddElement(radiusText);
            controlStack.AddElement(radiusStack);

            UIManager.AddElement(controlStack);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 systemRenderPos = (Vector2)Camera.GetRenderPos(particleSystem);
            spriteBatch.Draw(Renderer.Pixel,
                             new Rectangle(
                                 (int)systemRenderPos.X - 1,
                                 (int)systemRenderPos.Y - 1,
                                 2,
                                 2),
                             Color.Black
                             );
        }

    }
}
