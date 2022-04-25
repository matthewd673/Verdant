using Verdant;
using Verdant.UI;
using System;

namespace ParticleToy
{
    internal class PlayScene : Scene
    {

        public PlayScene(int id) : base(id) { }

        UISlider slider;

        public override void Initialize()
        {
            base.Initialize();

            slider = new UISlider(Sprites.SliderIndicator, Sprites.SliderBar, new Vec2(50, 50), 0, 100, 250);
            UIManager.AddElement(slider);
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
