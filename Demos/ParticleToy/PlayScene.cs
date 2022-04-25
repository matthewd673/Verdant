using Verdant;
using Verdant.UI;
using System;

namespace ParticleToy
{
    internal class PlayScene : Scene
    {

        public PlayScene(int id) : base(id) { }

        ParticleSystem particleSystem;

        int[] WidthRange = new int[] { 8, 8 };
        int[] HeightRange = new int[] { 8, 8};

        float[] AngleRange = new float[] { 0, 0 };
        float[] VelocityMagnitudeRange = new float[] { 1, 1 };
        float[] AccelerationMagnitudeRange = new float[] { 0, 0 };
        float[] FrictionRange = new float[] { 0, 0 };
        int[] LifetimeRange = new int[] { 10, 20 };

        UISlider slider;
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
        }

        public override void Update()
        {
            text.Text = slider.Value.ToString();

            base.Update();
        }

    }
}
