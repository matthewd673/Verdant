using System;
using System.Collections.Generic;

namespace IsoEngine
{
    public class SceneManager
    {

        List<Scene> scenes = new List<Scene>();
        Scene activeScene;

        public SceneManager() { }
        public SceneManager(Scene initialScene)
        {
            scenes.Add(initialScene);
            activeScene = initialScene;
        }
        public SceneManager(List<Scene> scenes)
        {
            this.scenes = scenes;

            foreach (Scene s in this.scenes)
                s.SetManager(this);

            activeScene = scenes[0];
        }
        public SceneManager(List<Scene> scenes, Scene activeScene)
        {
            this.scenes = scenes;

            foreach (Scene s in this.scenes)
                s.SetManager(this);

            SetActiveScene(scenes[0]);
        }

        public void AddScene(Scene s)
        {
            s.SetManager(this);
            scenes.Add(s);
            //set active scene if there isn't one
            if (activeScene == null || scenes.Count == 1)
                activeScene = s;
        }

        public Scene GetActiveScene()
        {
            return activeScene;
        }

        public void SetActiveScene(Scene activeScene)
        {
            this.activeScene = activeScene;
        }

    }
}
