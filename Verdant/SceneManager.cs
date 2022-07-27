using System;
using System.Collections.Generic;

namespace Verdant
{
    public class SceneManager
    {

        Dictionary<int, Scene> scenes = new Dictionary<int, Scene>();
        public Scene ActiveScene { get { return scenes[ActiveID]; } }
        public bool HasActive { get; private set; } = false;
        private int _activeId;
        public int ActiveID {
            get { return _activeId; }
            set
            {
                HasActive = true;
                _activeId = value;
                scenes[_activeId].Initialize();
            }
        }

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
        /// <param name="activeId">The ID of the Scene to be made active.</param>
        public SceneManager(List<Scene> scenes, int activeId)
        {
            foreach (Scene s in scenes)
                AddScene(s, automaticallyMakeActive: false);

            ActiveID = activeId;
        }

        /// <summary>
        /// Add a Scene to the manager.
        /// </summary>
        /// <param name="s">The Scene to be added.</param>
        /// <param name="automaticallyMakeActive">Determines if the Scene will be made active if there is not another Scene currently active.</param>
        public void AddScene(Scene s, bool automaticallyMakeActive = true)
        {
            s.Manager = this;
            scenes.Add(s.ID, s);
            //set active scene if there isn't one
            if (automaticallyMakeActive && (!HasActive || scenes.Count == 1))
                ActiveID = s.ID;
        }
    }
}
