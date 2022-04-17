using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Verdant;

namespace LoggingDemo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SceneManager sceneManager;
        enum SceneType
        {
            Play
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Verdant.Debugging.Log.WriteLine("Initialize()");

            sceneManager = new SceneManager();

            Scene playScene = new Scene((int)SceneType.Play);
            playScene.Initialize();

            playScene.EntityManager.AddEntity(new Player());

            sceneManager.AddScene(playScene);

            Renderer.Initialize(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 2);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Verdant.Debugging.Log.WriteLine("LoadContent()");

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

            //if (Sprites.Loaded)
            //{
            //    Verdant.Debugging.SimpleStats.Render(sceneManager.ActiveScene, _spriteBatch, Sprites.debugFont);
            //}

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
