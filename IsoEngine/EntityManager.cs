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

        //overly simplified, for now
        /// <summary>
        /// Get a list of all Entities currently colliding with the specified Entity.
        /// </summary>
        /// <param name="e">The Entity used to check collisions. By default, all of the Entity's colliders will be checked.</param>
        /// <param name="specificCollider">Specify a single collider to check against. Useful if the Entity has more than one Collider attached to it.</param>
        /// <param name="onlyTriggers">If true, only Colliders marked as triggers will be checked.</param>
        /// <param name="onlySolids">If true, only Colliders not marked as triggers will be checked.</param>
        /// <param name="onlyType">Exclude all Entities from list that aren't of a specified type.</param>
        /// <returns>A list containing all colliding Entities.</returns>
        public List<Entity> GetAllColliding(Entity e, Collider specificCollider = null, bool onlyTriggers = false, bool onlySolids = false, Type onlyType = null)
        {
            List<Entity> colliding = new List<Entity>();

            foreach (Entity b in entities) //slow, but good enough for now
            {

                if (b == e) //don't include self
                    continue;

                if (onlyType != null)
                {
                    if (b.GetType() != onlyType)
                        continue;
                }

                foreach (Collider c in b.GetColliders())
                {
                    //filter out triggers/solids if necessary
                    if (onlySolids && c.trigger)
                        continue;
                    if (onlyTriggers && !c.trigger)
                        continue;

                    //check against only one collider, if specified
                    if (specificCollider != null)
                    {
                        if (Math.CheckRectIntersection(specificCollider.pos, specificCollider.w, specificCollider.h, c.pos, c.w, c.h))
                        {
                            colliding.Add(b);
                            break; //don't bother with any more of this entity's colliders
                        }
                    }
                    else //no collider specified
                    {
                        foreach (Collider a in e.GetColliders())
                        {
                            if (Math.CheckRectIntersection(a.pos, a.w, a.h, c.pos, c.w, c.h))
                            {
                                colliding.Add(b);
                                break;
                            }
                        }
                    }
                }
            }

            return colliding;

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
