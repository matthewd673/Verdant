using Verdant;
using Verdant.UI;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Verdant.Debugging;

namespace ParticleToy
{
    internal class PlayScene : Scene
    {

        public PlayScene(string id) : base(id) { }

        static ParticleSystem particleSystem;
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

            // SPAWN RATE
            UIStack rateStack = new UIStack(Vec2.Zero, vertical: false);
            rateStack.Gap = 12;
            rateStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Spawn Delay "));
            UITextBox rateText = new UITextBox(Vec2.Zero, Resources.Font);
            rateText.Numeric = true;
            UISlider rateSlider = new UISlider(Vec2.Zero, 50, 3000, Resources.SliderIndicator, Resources.SliderBar, 200);
            rateSlider.Change += (object sender, EventArgs args) =>
            {
                spawnTimer.Duration = rateSlider.Value;
                rateText.Text = rateSlider.Value.ToString("0.00");
            };
            rateText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(rateText.Text, out value);
                rateSlider.Value = value;
            };
            rateStack.AddElement(rateSlider);
            rateStack.AddElement(rateText);
            controlStack.AddElement(rateStack);

            // LIFETIME
            UIStack lifetimeStack = new UIStack(Vec2.Zero, vertical: false);
            lifetimeStack.Gap = 12;
            lifetimeStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Particle Lifetime "));
            UITextBox lifetimeText = new UITextBox(Vec2.Zero, Resources.Font);
            lifetimeText.Numeric = true;
            UISlider lifetimeSlider = new UISlider(Vec2.Zero, 1, 3000, Resources.SliderIndicator, Resources.SliderBar, 200);
            lifetimeSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.DefaultLifetime = lifetimeSlider.Value;
                lifetimeText.Text = lifetimeSlider.Value.ToString("0.00");
            };
            lifetimeText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(lifetimeText.Text, out value);
                lifetimeSlider.Value = value;
            };
            lifetimeStack.AddElement(lifetimeSlider);
            lifetimeStack.AddElement(lifetimeText);
            controlStack.AddElement(lifetimeStack);

            // RADIUS
            UIStack radiusStack = new UIStack(Vec2.Zero, vertical: false);
            radiusStack.Gap = 12;
            radiusStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Radius "));
            UITextBox radiusText = new UITextBox(Vec2.Zero, Resources.Font);
            radiusText.Numeric = true;
            UISlider radiusSlider = new UISlider(Vec2.Zero, 0, 128, Resources.SliderIndicator, Resources.SliderBar, 200);
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

            // ACCELERATION
            UIStack accStack = new UIStack(Vec2.Zero, vertical: false);
            accStack.Gap = 12;
            // X
            accStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Acc X "));
            UITextBox accXText = new UITextBox(Vec2.Zero, Resources.Font);
            accXText.Numeric = true;
            UISlider accXSlider = new UISlider(Vec2.Zero, -2, 2, Resources.SliderIndicator, Resources.SliderBar, 75);
            accXSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.DefaultAcceleration = new Vec2(accXSlider.Value, particleSystem.DefaultAcceleration.Y);
                accXText.Text = accXSlider.Value.ToString("0.00");
            };
            accXText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(accXText.Text, out value);
                accXSlider.Value = value;
            };
            accStack.AddElement(accXSlider);
            accStack.AddElement(accXText);
            // Y
            accStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Acc Y "));
            UITextBox accYText = new UITextBox(Vec2.Zero, Resources.Font);
            accYText.Numeric = true;
            UISlider accYSlider = new UISlider(Vec2.Zero, -2, 2, Resources.SliderIndicator, Resources.SliderBar, 75);
            accYSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.DefaultAcceleration = new Vec2(particleSystem.DefaultAcceleration.X, accYSlider.Value);
                accYText.Text = accYSlider.Value.ToString("0.00");
            };
            accYText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(accYText.Text, out value);
                accYSlider.Value = value;
            };
            accStack.AddElement(accYSlider);
            accStack.AddElement(accYText);
            controlStack.AddElement(accStack);

            // VELOCITY
            UIStack velStack = new UIStack(Vec2.Zero, vertical: false);
            velStack.Gap = 12;
            // X
            velStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Vel X "));
            UITextBox velXText = new UITextBox(Vec2.Zero, Resources.Font);
            velXText.Numeric = true;
            UISlider velXSlider = new UISlider(Vec2.Zero, -2, 2, Resources.SliderIndicator, Resources.SliderBar, 75);
            velXSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.DefaultAcceleration = new Vec2(velXSlider.Value, particleSystem.DefaultAcceleration.Y);
                velXText.Text = velXSlider.Value.ToString("0.00");
            };
            velXText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(velXText.Text, out value);
                velXSlider.Value = value;
            };
            velStack.AddElement(velXSlider);
            velStack.AddElement(velXText);
            // Y
            velStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Vel Y "));
            UITextBox velYText = new UITextBox(Vec2.Zero, Resources.Font);
            velYText.Numeric = true;
            UISlider velYSlider = new UISlider(Vec2.Zero, -2, 2, Resources.SliderIndicator, Resources.SliderBar, 75);
            velYSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.DefaultAcceleration = new Vec2(particleSystem.DefaultAcceleration.X, velYSlider.Value);
                velYText.Text = velYSlider.Value.ToString("0.00");
            };
            velYText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(velYText.Text, out value);
                velYSlider.Value = value;
            };
            velStack.AddElement(velYSlider);
            velStack.AddElement(velYText);
            controlStack.AddElement(velStack);

            // FRICTION
            UIStack frictionStack = new UIStack(Vec2.Zero, vertical: false);
            frictionStack.Gap = 12;
            frictionStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Friction "));
            UITextBox frictionText = new UITextBox(Vec2.Zero, Resources.Font);
            frictionText.Numeric = true;
            UISlider frictionSlider = new UISlider(Vec2.Zero, 0, 1, Resources.SliderIndicator, Resources.SliderBar, 200);
            frictionSlider.Change += (object sender, EventArgs args) =>
            {
                particleSystem.DefaultFriction = frictionSlider.Value;
                frictionText.Text = frictionSlider.Value.ToString("0.00");
            };
            frictionText.Submit += (object sender, EventArgs args) =>
            {
                float value;
                float.TryParse(velXText.Text, out value);
                velXSlider.Value = value;
            };
            frictionStack.AddElement(frictionSlider);
            frictionStack.AddElement(frictionText);
            controlStack.AddElement(frictionStack);

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

            PerformanceMonitor.Draw(this, spriteBatch);
        }       
    }
}
