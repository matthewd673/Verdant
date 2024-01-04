using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Verdant.UI;

namespace Verdant
{
    /// <summary>
    /// Contains managers and other game state information, independent from other Scenes.
    /// </summary>
    public class Scene
    {
        // The ID of the Scene within its manager.
        public string ID { get; private set; }
        // The SceneManager that manages the Scene.
        public SceneManager Manager { get; set; }
        // The Camera used to render the Scene.
        public Camera Camera { get; set; }
        // The EntityManager controlled by the Scene.
        public EntityManager EntityManager { get; protected set; }
        // The UIManager controlled by the Scene.
        public UIManager UIManager { get; protected set; }

        // The last Update call delta time (based on frame rate). This is not representative of the amount of time the Update call actually took to compute.
        public float DeltaTime { get; private set; } = 1f;

        /// <summary>
        /// Create a new Scene. It will not be initialized immediately.
        /// </summary>
        /// <param name="id">The ID of the Scene. Two Scenes in the same Manager cannot have the same ID.</param>
        public Scene(string id)
        {
            ID = id;
        }

        /// <summary>
        /// Perform basic initialization of the scene (create an EntityManager and UIManager). Can also be called to reset the scene.
        /// </summary>
        public virtual void Initialize()
        {
            EntityManager = new EntityManager();
            EntityManager.Scene = this;
            UIManager = new UIManager();

            Camera = new Camera(Renderer.ScreenWidth, Renderer.ScreenHeight);
            EntityManager.AddEntity(Camera);
        }

        /// <summary>
        /// Perform the basic update loop. Update all managers in the scene, and the InputHandler.
        /// <param name="gameTime">The MonoGame GameTime</param>
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            InputHandler.Update(); // always update input

            Timer.TickAll(DeltaTime);

            EntityManager.Update();
            UIManager.Update();

            //Camera.Update();
        }

        /// <summary>
        /// Render the Scene through its Camera.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Renderer.Render(spriteBatch, this);
        }
    }
}
