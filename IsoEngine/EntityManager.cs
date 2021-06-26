using System;
using System.Collections.Generic;

namespace IsoEngine
{
    public class EntityManager
    {

        List<Entity> entities = new List<Entity>();
        List<Entity> addQueue = new List<Entity>();
        List<Entity> removeQueue = new List<Entity>();

        /// <summary>
        /// Add an Entity to the list, and mark this instance as its manager.
        /// </summary>
        /// <param name="e">The Entity to add.</param>
        public void AddEntity(Entity e)
        {
            addQueue.Add(e);
            e.SetManager(this);
        }

        /// <summary>
        /// Remove an Entity from the list, and mark its manager as null.
        /// </summary>
        /// <param name="e">The Entity to remove.</param>
        public void RemoveEntity(Entity e)
        {
            e.SetManager(null);
            removeQueue.Add(e);
        }

        /// <summary>
        /// Get a list of all Entities currently in the manager.
        /// </summary>
        /// <returns>A list of all managed Entities.</returns>
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

        /// <summary>
        /// Get a list of all Entities intersecting with a given rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle.</param>
        /// <param name="y">The y coordinate of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <param name="onlyTriggers">If true, only Colliders marked as triggers will be checked.</param>
        /// <param name="onlySolids">If true, only Colliders not marked as triggers will be checked.</param>
        /// <param name="onlyType">Exclude all Entities from list that aren't of a specified type.</param>
        /// <param name="ignoreEntity">Exclude a specified Entity from the list.</param>
        /// <returns>A list of all colliding Entities.</returns>
        public List<Entity> CheckRectCollisions(float x, float y, int w, int h, bool onlyTriggers = false, bool onlySolids = false, Type onlyType = null, Entity ignoreEntity = null) //mostly copy & paste from above...
        {
            List<Entity> colliding = new List<Entity>();

            foreach (Entity b in entities) //slow, but good enough for now
            {

                if (ignoreEntity != null)
                {
                    if (b == ignoreEntity)
                        continue;
                }

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

                    if (Math.CheckRectIntersection(x, y, w, h, c.pos.x, c.pos.y, c.w, c.h))
                    {
                        colliding.Add(b);
                        break;
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
