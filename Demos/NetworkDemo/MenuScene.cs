using System;
using Verdant;
using Verdant.UI;

namespace NetworkDemo
{
    internal class MenuScene : Scene
    {

        public MenuScene(int id) : base(id) { }

        public override void Initialize()
        {
            base.Initialize();

            UIButton hostButton = new UIButton(Sprites.HostButton, new Vec2(32, 32));
            hostButton.Click += HostButton_Click;
            UIManager.AddElement(hostButton);

            UIButton joinButton = new UIButton(Sprites.JoinButton, new Vec2(32, 72));
            UIManager.AddElement(joinButton);
            joinButton.Click += JoinButton_Click;
        }

        private void HostButton_Click(object sender, EventArgs e)
        {
            Manager.AddScene(new PlayScene((int)Game1.SceneType.Play, true));
            Manager.ActiveID = (int)Game1.SceneType.Play;
        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            Manager.AddScene(new PlayScene((int)Game1.SceneType.Play, false));
            Manager.ActiveID = (int)Game1.SceneType.Play;
        }
        
    }
}
