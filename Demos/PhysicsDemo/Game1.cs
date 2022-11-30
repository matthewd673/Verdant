using Verdant;
using Verdant.Debugging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PhysicsDemo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        enum SceneType
        {
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
            base.Initialize();

            Renderer.Initialize(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 1);

            sceneManager = new SceneManager();

            PlayScene playScene = new PlayScene((int)SceneType.Play);
            playScene.Initialize();

            sceneManager.AddScene(playScene);

            Log.WriteLine("Finished initializing");
            Log.WriteLine(Renderer.Camera.Position.ToString());
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

            sceneManager.ActiveScene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Renderer.Render(_spriteBatch, sceneManager.ActiveScene, visualizeBodies: true);
            SimpleStats.Render(sceneManager.ActiveScene, _spriteBatch, Sprites.DebugFont);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
