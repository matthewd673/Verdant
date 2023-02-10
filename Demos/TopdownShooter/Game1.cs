using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Verdant;
using Verdant.Debugging;

namespace TopdownShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SceneManager sceneManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Renderer.Initialize(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 1);

            sceneManager = new SceneManager();
            PlayScene playScene = new PlayScene("play");
            playScene.Initialize();
            sceneManager.AddScene(playScene);

            SimpleStats.TextColor = Color.White;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Resources.LoadResources(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            sceneManager.ActiveScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Renderer.Render(_spriteBatch, sceneManager.ActiveScene, visualizeBodies: false);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
