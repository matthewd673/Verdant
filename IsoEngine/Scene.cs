using System;

namespace IsoEngine
{
    public class Scene
    {

        SceneManager manager;

        public EntityManager entityManager;
        public UIManager uiManager;

        public Scene()
        {

        }

        public void SetManager(SceneManager manager)
        {
            this.manager = manager;
        }

        public SceneManager GetManager()
        {
            return manager;
        }

        public virtual void Initialize()
        {
            entityManager = new EntityManager();
            uiManager = new UIManager();
        }

        public virtual void Update()
        {
            InputHandler.Update(); //always update input

            entityManager.Update();
            uiManager.Update();
        }

    }
}
