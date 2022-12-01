using System;
using Microsoft.Xna.Framework;
using Verdant.UI;

namespace Verdant
{
    public class Scene
    {

        public int ID { get; private set; }

        public SceneManager Manager { get; set; }

        public EntityManager EntityManager { get; protected set; }
        public UIManager UIManager { get; protected set; }

        public float DeltaTime { get; private set; } = 1f;

        /// <summary>
        /// Create a new Scene. It will not be initialized immediately.
        /// </summary>
        /// <param name="id">The ID of the Scene. Two Scenes in the same Manager cannot have the same ID.</param>
        public Scene(int id)
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
        }

        /// <summary>
        /// Perform the basic update loop. Update all managers in the scene, and the InputHandler.
        /// <param name="gameTime">The MonoGame GameTime</param>
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            DeltaTime = (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            InputHandler.Update(); // always update input

            EntityManager.Update();
            UIManager.Update();
        }

    }
}
