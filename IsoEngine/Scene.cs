using System;
using IsoEngine.UI;

namespace IsoEngine
{
    public class Scene
    {

        SceneManager manager;

        public EntityManager entityManager;
        public UIManager uiManager;

        /// <summary>
        /// Create a new Scene. It will not be initialized immediately.
        /// </summary>
        public Scene() { }

        /// <summary>
        /// Manually set the scene's manager.
        /// </summary>
        /// <param name="manager">The new parent SceneManager.</param>
        public void SetManager(SceneManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Get the Scene's current manager;
        /// </summary>
        /// <returns>The current parent SceneManager.</returns>
        public SceneManager GetManager()
        {
            return manager;
        }

        /// <summary>
        /// Perform basic initialization of the scene (create an EntityManager and UIManager). Can also be called to reset the scene.
        /// </summary>
        public virtual void Initialize()
        {
            entityManager = new EntityManager();
            uiManager = new UIManager();
        }

        /// <summary>
        /// Perform the basic update loop. Update all managers in the scene, and the InputHandler.
        /// </summary>
        public virtual void Update()
        {
            InputHandler.Update(); //always update input

            entityManager.Update();
            uiManager.Update();
        }

    }
}
