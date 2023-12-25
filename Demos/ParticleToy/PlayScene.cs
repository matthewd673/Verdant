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

            particleSystem.DefaultTransformAnimation = new TransformAnimation(
                new TransformState(TransformStateBlendMode.Multiply)
                {
                    Height = 1f,
                    Width = 1f,
                },
                new TransformState(TransformStateBlendMode.Multiply)
                {
                    Height = 0f,
                    Width = 0f,
                },
                1000
                );
            particleSystem.DefaultTransformAnimation.FillForwards = true;

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
            UIStack controlStack = new UIStack(new Vec2(20, 20))
            {
                Gap = 4,
            };

            // SPAWN RATE
            UIStack rateStack = new UIStack(Vec2.Zero, vertical: false)
            {
                Gap = 6,
                Alignment = Alignment.Center,
            };
            rateStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Spawn Delay "));
            ControlTextBox rateText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider rateSlider = new(Vec2.Zero, 50, 3000, 200);
            rateSlider.OnChanged = () =>
            {
                Verdant.Debugging.Log.WriteLine(this.GetType());
                spawnTimer.Duration = rateSlider.Value;
                rateText.Text = rateSlider.Value.ToString("0.00");
            };

            rateText.OnSubmit = () =>
            {
                float value;
                float.TryParse(rateText.Text, out value);
                rateSlider.Value = value;
            };
            rateStack.AddElement(rateSlider);
            rateStack.AddElement(rateText);
            controlStack.AddElement(rateStack);

            // LIFETIME
            UIStack lifetimeStack = new UIStack(Vec2.Zero, vertical: false)
            {
                Gap = 6,
                Alignment = Alignment.Center,
            };
            lifetimeStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Particle Lifetime "));

            ControlTextBox lifetimeText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider lifetimeSlider = new(Vec2.Zero, 1, 3000, 200);
            lifetimeText.OnSubmit = () =>
            {
                float value;
                float.TryParse(lifetimeText.Text, out value);
                lifetimeSlider.Value = value;
            };
            lifetimeSlider.OnChanged = () =>
            {
                particleSystem.DefaultLifetime = lifetimeSlider.Value;
                lifetimeText.Text = lifetimeSlider.Value.ToString("0.00");
            };

            lifetimeStack.AddElement(lifetimeSlider);
            lifetimeStack.AddElement(lifetimeText);
            controlStack.AddElement(lifetimeStack);

            // RADIUS
            UIStack radiusStack = new UIStack(Vec2.Zero, vertical: false)
            {
                Gap = 6,
                Alignment = Alignment.Center,
            };
            radiusStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Radius "));

            ControlTextBox radiusText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider radiusSlider = new(Vec2.Zero, 0, 128, 200);

            radiusSlider.OnChanged = () =>
            {
                particleSystem.Radius = radiusSlider.Value;
                radiusText.Text = radiusSlider.Value.ToString("0.00");
            };
            radiusText.OnSubmit = () =>
            {
                float value;
                float.TryParse(radiusText.Text, out value);
                radiusSlider.Value = value;
            };

            radiusStack.AddElement(radiusSlider);
            radiusStack.AddElement(radiusText);
            controlStack.AddElement(radiusStack);

            // ACCELERATION
            UIStack accStack = new UIStack(Vec2.Zero, vertical: false)
            {
                Gap = 6,
                Alignment = Alignment.Center,
            };

            // X
            accStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Acc X "));
            ControlTextBox accXText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider accXSlider = new(Vec2.Zero, -2, 2, 75);

            accXSlider.OnChanged = () =>
            {
                particleSystem.DefaultAcceleration = new Vec2(accXSlider.Value, particleSystem.DefaultAcceleration.Y);
                accXText.Text = accXSlider.Value.ToString("0.00");
            };
            accXText.OnSubmit = () =>
            {
                float value;
                float.TryParse(accXText.Text, out value);
                accXSlider.Value = value;
            };

            accStack.AddElement(accXSlider);
            accStack.AddElement(accXText);

            // Y
            accStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Acc Y "));
            ControlTextBox accYText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider accYSlider = new(Vec2.Zero, -2, 2, 75);
            accYSlider.OnChanged = () =>
            {
                particleSystem.DefaultAcceleration = new Vec2(particleSystem.DefaultAcceleration.X, accYSlider.Value);
                accYText.Text = accYSlider.Value.ToString("0.00");
            };
            accYText.OnSubmit += () =>
            {
                float value;
                float.TryParse(accYText.Text, out value);
                accYSlider.Value = value;
            };
            accStack.AddElement(accYSlider);
            accStack.AddElement(accYText);
            controlStack.AddElement(accStack);

            // VELOCITY
            UIStack velStack = new UIStack(Vec2.Zero, vertical: false)
            {
                Gap = 6,
                Alignment = Alignment.Center,
            };

            // X
            velStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Vel X "));
            ControlTextBox velXText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider velXSlider = new(Vec2.Zero, -2, 2, 75);

            velXSlider.OnChanged = () =>
            {
                particleSystem.DefaultAcceleration = new Vec2(velXSlider.Value, particleSystem.DefaultAcceleration.Y);
                velXText.Text = velXSlider.Value.ToString("0.00");
            };
            velXText.OnSubmit = () =>
            {
                float value;
                float.TryParse(velXText.Text, out value);
                velXSlider.Value = value;
            };
            velStack.AddElement(velXSlider);
            velStack.AddElement(velXText);

            // Y
            velStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Vel Y "));
            ControlTextBox velYText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider velYSlider = new(Vec2.Zero, -2, 2, 75);

            velYSlider.OnChanged = () =>
            {
                particleSystem.DefaultAcceleration = new Vec2(particleSystem.DefaultAcceleration.X, velYSlider.Value);
                velYText.Text = velYSlider.Value.ToString("0.00");
            };
            velYText.OnSubmit = () =>
            {
                float value;
                float.TryParse(velYText.Text, out value);
                velYSlider.Value = value;
            };

            velStack.AddElement(velYSlider);
            velStack.AddElement(velYText);
            controlStack.AddElement(velStack);

            // FRICTION
            UIStack frictionStack = new UIStack(Vec2.Zero, vertical: false)
            {
                Gap = 6,
                Alignment = Alignment.Center,
            };
            frictionStack.AddElement(new UILabel(Vec2.Zero, Resources.Font, "Friction "));
            ControlTextBox frictionText = new(Vec2.Zero)
            {
                Numeric = true,
            };
            ControlSlider frictionSlider = new(Vec2.Zero, 0, 1, 200);

            frictionSlider.OnChanged = () =>
            {
                particleSystem.DefaultFriction = frictionSlider.Value;
                frictionText.Text = frictionSlider.Value.ToString("0.00");
            };
            frictionText.OnSubmit += () =>
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
            //base.Draw(spriteBatch);
            Renderer.VisualizeUIBounds = true;
            Renderer.Render(spriteBatch, this);

            Vector2 systemRenderPos = (Vector2)Camera.GetRenderPosition(particleSystem);
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
