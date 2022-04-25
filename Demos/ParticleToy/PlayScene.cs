using Verdant;
using Verdant.UI;
using System;

namespace ParticleToy
{
    internal class PlayScene : Scene
    {

        public PlayScene(int id) : base(id) { }

        UISlider slider;
        UIText text;

        public override void Initialize()
        {
            base.Initialize();

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
