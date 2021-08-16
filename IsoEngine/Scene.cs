using System;
using IsoEngine.UI;

namespace IsoEngine
{
    public class Scene
    {

        public SceneManager Manager { get; set; }

        public EntityManager EntityManager { get; private set; }
        public UIManager UIManager { get; private set; }

        /// <summary>
        /// Create a new Scene. It will not be initialized immediately.
        /// </summary>
        public Scene() { }

        /// <summary>
        /// Perform basic initialization of the scene (create an EntityManager and UIManager). Can also be called to reset the scene.
        /// </summary>
        public virtual void Initialize()
        {
            EntityManager = new EntityManager();
            UIManager = new UIManager();
        }

        /// <summary>
        /// Perform the basic update loop. Update all managers in the scene, and the InputHandler.
        /// </summary>
        public virtual void Update()
        {
            InputHandler.Update(); //always update input

            EntityManager.Update();
            UIManager.Update();
        }

    }
}
