using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verdant;
using Verdant.UI;

namespace TopdownShooter
{
    internal class TitleScene : Scene
    {

        public TitleScene() : base("title") { }

        public override void Initialize()
        {
            base.Initialize();

            UIStack stack = new UIStack(new Vec2(50, 50));
            stack.Gap = 12;
            stack.AddElement(new UISprite(Resources.Logo, Vec2.Zero));

            UIButton playButton = new UIButton(Resources.PlayButton, Vec2.Zero);
            playButton.Click += PlayButton_Click;
            stack.AddElement(playButton);

            UIManager.AddElement(stack);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Manager.ActiveID = "play";
        }
    }
}
