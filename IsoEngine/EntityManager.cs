using System;
using System.Collections.Generic;

namespace IsoEngine
{
    public class EntityManager
    {

        List<Entity> entities = new List<Entity>();
        List<Entity> addQueue = new List<Entity>();
        List<Entity> removeQueue = new List<Entity>();

        public void AddEntity(Entity e)
        {
            addQueue.Add(e);
        }

        public void RemoveEntity(Entity e)
        {
            removeQueue.Add(e);
        }

        public List<Entity> GetEntities()
        {
            return entities;
        }

        public void Update()
        {
            //update all
            foreach (Entity e in entities)
            {
                e.Update();
            }

            //remove marked
            foreach (Entity e in removeQueue)
            {
                entities.Remove(e);
            }
            //add marked
            entities.AddRange(addQueue);

            addQueue.Clear();
            removeQueue.Clear();
        }

    }
}
