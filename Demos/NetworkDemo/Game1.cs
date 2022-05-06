using Verdant;
using Verdant.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetworkDemo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public enum SceneType
        {
            Menu,
            Play,
        }

        SceneManager sceneManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            Renderer.Initialize(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 2);

            sceneManager = new SceneManager();

            MenuScene menuScene = new MenuScene((int)SceneType.Menu);
            sceneManager.AddScene(menuScene);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Sprites.LoadSprites(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            sceneManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Renderer.Render(_spriteBatch, sceneManager.ActiveScene);
            Verdant.Debugging.SimpleStats.Render(sceneManager.ActiveScene, _spriteBatch, Sprites.DebugFont);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
