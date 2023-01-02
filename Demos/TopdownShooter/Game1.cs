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
            PlayScene playScene = new PlayScene(0);
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

            if (sceneManager.ActiveScene.ID == 0)
            {
                //((PlayScene)sceneManager.ActiveScene).Pathfinder.Visualize(_spriteBatch, sceneManager.ActiveScene.Camera);
                //((PlayScene)sceneManager.ActiveScene).sanity.Draw(_spriteBatch, sceneManager.ActiveScene.Camera);
            }

            SimpleStats.Render(sceneManager.ActiveScene, _spriteBatch, Resources.DebugFont);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
