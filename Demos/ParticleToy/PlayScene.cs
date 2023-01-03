using Verdant;
using Verdant.UI;
using System;
using Microsoft.Xna.Framework;

namespace ParticleToy
{
    internal class PlayScene : Scene
    {

        public PlayScene(string id) : base(id) { }

        ParticleSystem particleSystem;

        int[] WidthRange = new int[] { 8, 8 };
        int[] HeightRange = new int[] { 8, 8};

        float[] AngleRange = new float[] { 0, 0 };
        float[] VelocityMagnitudeRange = new float[] { 1, 1 };
        float[] AccelerationMagnitudeRange = new float[] { 0, 0 };
        float[] FrictionRange = new float[] { 0, 0 };
        int[] LifetimeRange = new int[] { 10, 20 };

        UISlider slider;
        UITextBox textBox;
        UIText text;

        public override void Initialize()
        {
            base.Initialize();

            particleSystem = new ParticleSystem(new Vec2(400, 300), 0);
            EntityManager.AddEntity(particleSystem);

            slider = new UISlider(new Vec2(50, 50), 0, 100, Sprites.SliderIndicator, Sprites.SliderBar, 250);
            UIManager.AddElement(slider);
            
            text = new UIText(new Vec2(320, 50), Sprites.DebugFont, "0");
            text.Position.Y -= text.Height / 2;
            UIManager.AddElement(text);

            textBox = new UITextBox(new Vec2(50, 100), Sprites.DebugFont, "");
            textBox.Padding = 4;
            textBox.MaxLength = 32;
            UIManager.AddElement(textBox);
        }

        public override void Update(GameTime gameTime)
        {
            text.Text = slider.Value.ToString();

            base.Update(gameTime);
        }

    }
}
