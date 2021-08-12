using System;
using System.Collections.Generic;

namespace IsoEngine
{
    public class SceneManager
    {

        List<Scene> scenes = new List<Scene>();
        Scene activeScene;

        /// <summary>
        /// Initialize a new SceneManager.
        /// </summary>
        public SceneManager() { }
        /// <summary>
        /// Initialize a new SceneManager and add a Scene (which will be made active).
        /// </summary>
        /// <param name="initialScene">The initial Scene in the manager.</param>
        public SceneManager(Scene initialScene)
        {
            AddScene(initialScene);
        }
        /// <summary>
        /// Initialize a new SceneManager with a group of Scenes. The first Scene in the list will be made active.
        /// </summary>
        /// <param name="scenes">The list of Scenes to initialize with.</param>
        public SceneManager(List<Scene> scenes)
        {
            foreach (Scene s in scenes)
                AddScene(s);
        }
        /// <summary>
        /// Initialize a new SceneManager with a group of Scenes, and specify one Scene to become the current active Scene.
        /// </summary>
        /// <param name="scenes">The list of Scenes to initialize with.</param>
        /// <param name="activeScene">The Scene to be made active. It does not need to be included in the Scene list, but it should be.</param>
        public SceneManager(List<Scene> scenes, Scene activeScene)
        {
            foreach (Scene s in scenes)
                AddScene(s, automaticallyMakeActive: false);

            SetActiveScene(activeScene);
        }

        /// <summary>
        /// Add a Scene to the manager.
        /// </summary>
        /// <param name="s">The Scene to be added.</param>
        /// <param name="automaticallyMakeActive">Determines if the Scene will be made active if there is not another Scene currently active.</param>
        public void AddScene(Scene s, bool automaticallyMakeActive = true)
        {
            s.SetManager(this);
            scenes.Add(s);
            //set active scene if there isn't one
            if (activeScene == null || scenes.Count == 1)
                activeScene = s;
        }

        /// <summary>
        /// Get the current active Scene.
        /// </summary>
        /// <returns>The current active Scene.</returns>
        public Scene GetActiveScene()
        {
            return activeScene;
        }

        /// <summary>
        /// Set the currently active Scene. It does not need to be managed by this SceneManager, but it should be.
        /// </summary>
        /// <param name="activeScene">The scene to make active.</param>
        public void SetActiveScene(Scene activeScene)
        {
            this.activeScene = activeScene;
        }

    }
}
