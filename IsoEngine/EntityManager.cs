using System;
using System.Collections.Generic;
using IsoEngine.Physics;

namespace IsoEngine
{
    public class EntityManager
    {

        public enum UpdateMode
        {
            All,
            NearCamera,
        }

        Dictionary<string, List<Entity>> entityTable = new Dictionary<string, List<Entity>>();
        public int CellSize { get; }

        List<Entity> addQueue = new List<Entity>();
        List<Entity> removeQueue = new List<Entity>();

        public int EntityCount { get; protected set; }
        public int EntityUpdateCount { get; protected set; }

        /// <summary>
        /// Initialize a new EntityManager.
        /// </summary>
        public EntityManager()
        {
            CellSize = 128;
        }
        /// <summary>
        /// Initialize a new EntityManager.
        /// </summary>
        /// <param name="cellSize">The size of each cell to use when organizing Entities in the hash table.</param>
        public EntityManager(int cellSize)
        {
            CellSize = cellSize;
        }

        /// <summary>
        /// Add an Entity to the manager, and mark this instance as its manager.
        /// </summary>
        /// <param name="e">The Entity to add.</param>
        public void AddEntity(Entity e)
        {
            addQueue.Add(e);
        }

        /// <summary>
        /// Add a list of Entities to the manager, and mark this instance as the manager of all of the Entities.
        /// </summary>
        /// <param name="l">The list of Entities to add.</param>
        public void AddEntityRange(List<Entity> l)
        {
            foreach (Entity e in l)
            {
                e.Manager = this;
                addQueue.Add(e);
            }
        }

        /// <summary>
        /// Remove an Entity from the list, and mark its manager as null.
        /// </summary>
        /// <param name="e">The Entity to remove.</param>
        public void RemoveEntity(Entity e)
        {
            removeQueue.Add(e);
        }

        /// <summary>
        /// Force the Entities in the add and remove queues to be added/removed to the manager. DO NOT USE WITHIN UPDATE LOOP.
        /// </summary>
        public void ApplyQueues()
        {
            //remove marked
            foreach (Entity e in removeQueue)
            {
                e.Manager = null;
                if (entityTable.ContainsKey(e.Key))
                {
                    entityTable[e.Key].Remove(e);
                    EntityCount--; //keep track
                }
            }
            //add marked
            foreach (Entity e in addQueue)
            {
                e.Manager = this;
                //add to appropriate list in table (create if necessary)
                if (entityTable.ContainsKey(e.Key))
                    entityTable[e.Key].Add(e);
                else
                    entityTable.Add(e.Key, new List<Entity>() { e });
                EntityCount++; //keep track
            }

            addQueue.Clear();
            removeQueue.Clear();
        }

        /// <summary>
        /// Get a list of all Entities currently in the manager.
        /// </summary>
        /// <returns>A list of all managed Entities.</returns>
        public List<Entity> GetAllEntities()
        {
            List<Entity> entityList = new List<Entity>();
            foreach (string key in entityTable.Keys)
                entityList.AddRange(entityTable[key]);
            return entityList;
        }

        /// <summary>
        /// Get all Entities of a current type currently in the manager.
        /// </summary>
        /// <typeparam name="T">The type of Entity to search for.</typeparam>
        /// <returns>A list of Entities of the given type in the manager.</returns>
        public List<T> GetAllEntitiesOfType<T>() where T : Entity
        {
            List<T> found = new List<T>();
            foreach (Entity e in GetAllEntities())
            {
                if (e.GetType() == typeof(T))
                    found.Add((T)e);
            }
            return found;
        }

        /// <summary>
        /// Get a list of all Entities near the given Entity. Check is performed in Entity's table cell and all neighboring cells.
        /// </summary>
        /// <param name="e">The Entity to search near.</param>
        /// <returns>A list of all near Entities.</returns>
        public List<Entity> GetNearEntities(Entity e)
        {
            List<Entity> nearEntities = new List<Entity>();
            Vec2Int cell = GetCellFromKey(e.Key);

            //loop through all neighboring cells and check
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    string checkKey = GetKeyFromCell(cell.X + i, cell.Y + j);
                    if (entityTable.ContainsKey(checkKey))
                        nearEntities.AddRange(entityTable[checkKey]);
                }
            }

            return nearEntities;
        }

        /// <summary>
        /// Get a list of all Entities of a given type near the given Entity. Check is performed in the Entity's table cell and all neighboring cells.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="e">The Entity to search near.</param>
        /// <returns>A list of all near Entities of the given type.</returns>
        public List<TEntity> GetNearEntitiesOfType<TEntity>(Entity e) where TEntity : Entity //largely copied from GetNearEntities
        {
            List<TEntity> nearEntities = new List<TEntity>();
            Vec2Int cell = GetCellFromKey(e.Key);

            //loop through all neighboring cells and check
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    string checkKey = GetKeyFromCell(cell.X + i, cell.Y + j);
                    if (entityTable.ContainsKey(checkKey))
                    {
                        foreach (Entity b in entityTable[checkKey])
                        {
                            if (b.GetType() == typeof(TEntity))
                                nearEntities.Add((TEntity)b);
                        }
                    }
                }
            }

            return nearEntities;
        }

        /// <summary>
        /// Get a list of all Entities bounded by a given rectangle. Entities are guaranteed to be within 1 cell of the bounds, but may not actually fall inside the bounds.
        /// </summary>
        /// <param name="x">The x position of the rectangle.</param>
        /// <param name="y">The y position of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<Entity> GetEntitiesInBounds(float x, float y, int w, int h)
        {
            List<Entity> boundedEntities = new List<Entity>();

            //calculate cell bounds
            int minCellX = (int)Math.Floor(x / (double)CellSize);
            int minCellY = (int)Math.Floor(y / (double)CellSize);
            int maxCellX = (int)Math.Floor((x + w) / (double)CellSize);
            int maxCellY = (int)Math.Floor((y + h) / (double)CellSize);

            //loop through all cells in bounds
            for (int i = minCellX; i <= maxCellX; i++)
            {
                for (int j = minCellY; j <= maxCellY; j++)
                {
                    string checkKey = GetKeyFromCell(i, j);
                    if (entityTable.ContainsKey(checkKey))
                        boundedEntities.AddRange(entityTable[checkKey]);
                }
            }

            return boundedEntities;
        }
        /// <summary>
        /// Get a list of all Entities bounded by a given rectangle. Entities are guaranteed to be within 1 cell of the bounds, but may not actually fall inside the bounds.
        /// </summary>
        /// <param name="pos">The position of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<Entity> GetEntitiesInBounds(Vec2 pos, int w, int h) { return GetEntitiesInBounds(pos.X, pos.Y, w, h); }

        /// <summary>
        /// Get a list of all Entities of a given type bounded by a given rectangle. Entities are guaranteed to be within 1 cell of the bounds, but may not actually fall inside the bounds.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="x">The x position of the rectangle.</param>
        /// <param name="y">The y position of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<TEntity> GetEntitiesInBoundsOfType<TEntity>(float x, float y, int w, int h) where TEntity : Entity
        {
            List<TEntity> boundedEntities = new List<TEntity>();

            //calculate cell bounds
            int minCellX = (int)Math.Floor(x / (double)CellSize);
            int minCellY = (int)Math.Floor(y / (double)CellSize);
            int maxCellX = (int)Math.Floor((x + w) / (double)CellSize);
            int maxCellY = (int)Math.Floor((y + h) / (double)CellSize);

            //loop through all cells in bounds
            for (int i = minCellX; i <= maxCellX; i++)
            {
                for (int j = minCellY; i <= maxCellY; j++)
                {
                    string checkKey = GetKeyFromCell(i, j);
                    if (entityTable.ContainsKey(checkKey))
                    {
                        foreach (Entity b in entityTable[checkKey])
                        {
                            if (b.GetType() == typeof(TEntity))
                                boundedEntities.Add((TEntity)b);
                        }
                    }
                }
            }

            return boundedEntities;
        }
        /// <summary>
        /// Get a list of all Entities of a given type bounded by a given rectangle. Entities are guaranteed to be within 1 cell of the bounds, but may not actually fall inside the bounds.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="pos">The position of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<TEntity> GetEntitiesInBoundsOfType<TEntity>(Vec2 pos, int w, int h) where TEntity : Entity { return GetEntitiesInBoundsOfType<TEntity>(pos.X, pos.Y, w, h); }

        /// <summary>
        /// Get a list of all Entities currently colliding with the specified Entity.
        /// </summary>
        /// <param name="e">The Entity used to check collisions. By default, all of the Entity's colliders will be checked.</param>
        /// <param name="specificCollider">Specify a single collider to check against. Useful if the Entity has more than one Collider attached to it.</param>
        /// <param name="onlyTriggers">If true, only Colliders marked as triggers will be checked.</param>
        /// <param name="onlySolids">If true, only Colliders not marked as triggers will be checked.</param>
        /// <returns>A list containing all colliding Entities.</returns>
        public List<Entity> GetAllColliding(Entity e, Collider specificCollider = null, bool onlyTriggers = false, bool onlySolids = false)
        {
            List<Entity> colliding = new List<Entity>();

            foreach (Entity b in GetNearEntities(e)) //slow, but good enough for now
            {

                if (b == e) //don't include self
                    continue;

                //foreach (Collider c in b.Colliders)
                //{
                //    //filter out triggers/solids if necessary
                //    if (onlySolids && c.Trigger)
                //        continue;
                //    if (onlyTriggers && !c.Trigger)
                //        continue;

                //    //check against only one collider, if specified
                //    if (specificCollider != null)
                //    {
                //        if (GameMath.CheckRectIntersection(specificCollider.Position, specificCollider.Width, specificCollider.Height, c.Position, c.Width, c.Height))
                //        {
                //            colliding.Add(b);
                //            break; //don't bother with any more of this entity's colliders
                //        }
                //    }
                //    else //no collider specified
                //    {
                //        foreach (Collider a in e.Colliders)
                //        {
                //            if (GameMath.CheckRectIntersection(a.Position, a.Width, a.Height, c.Position, c.Width, c.Height))
                //            {
                //                colliding.Add(b);
                //                break;
                //            }
                //        }
                //    }
                //}
            }

            return colliding;

        }

        /// <summary>
        /// Get a list of all Entities of a given type currently colliding with the specified Entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="e">The Entity used to check collisions. By default, all of the Entity's colliders will be checked.</param>
        /// <param name="specificCollider">Specify a single collider to check against. Useful if the Entity has more than one Collider attached to it.</param>
        /// <param name="onlyTriggers">If true, only Colliders marked as triggers will be checked.</param>
        /// <param name="onlySolids">If true, only Colliders not marked as triggers will be checked.</param>
        /// <returns>A list containing all colliding Entities.</returns>
        public List<TEntity> GetAllCollidingOfType<TEntity>(Entity e, Collider specificCollider = null, bool onlyTriggers = false, bool onlySolids = false) where TEntity : Entity //largely copied from GetAllColliding
        {
            List<TEntity> colliding = new List<TEntity>();

            foreach (TEntity b in GetNearEntitiesOfType<TEntity>(e))
            {
                if (b == (TEntity)e) //skip self
                    continue;

                foreach (Collider c in b.Colliders)
                {
                    //filter out triggers/solids if necessary
                    if (onlySolids && c.Trigger)
                        continue;
                    if (onlyTriggers && !c.Trigger)
                        continue;

                    //check against only one collider, if specified
                    if (specificCollider != null)
                    {
                        if (GameMath.CheckRectIntersection(specificCollider.Position, specificCollider.Width, specificCollider.Height, c.Position, c.Width, c.Height))
                        {
                            colliding.Add(b);
                            break; //don't bother with any more of this entity's colliders
                        }
                    }
                    else //no collider specified
                    {
                        foreach (Collider a in e.Colliders)
                        {
                            if (GameMath.CheckRectIntersection(a.Position, a.Width, a.Height, c.Position, c.Width, c.Height))
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
        /// <param name="ignoreList">A list of Entities to exclude from the check.</param>
        /// <returns>A list of all colliding Entities of the given type.</returns>
        public List<Entity> CheckRectCollisions(float x, float y, int w, int h, bool onlyTriggers = false, bool onlySolids = false, List<Entity> ignoreList = null) //mostly copy & paste from above...
        {
            List<Entity> colliding = new List<Entity>();

            foreach (Entity b in GetEntitiesInBounds(x, y, w, h)) //slow, but good enough for now
            {

                if (ignoreList != null)
                {
                    if (ignoreList.Contains(b))
                        continue;
                }

                foreach (Collider c in b.Colliders)
                {
                    //filter out triggers/solids if necessary
                    if (onlySolids && c.Trigger)
                        continue;
                    if (onlyTriggers && !c.Trigger)
                        continue;

                    if (GameMath.CheckRectIntersection(x, y, w, h, c.Position.X, c.Position.Y, c.Width, c.Height))
                    {
                        colliding.Add(b);
                        break;
                    }
                }
            }

            return colliding;
        }

        /// <summary>
        /// Get a list of all Entities of a given type intersecting with a given rectangle. 
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="x">The x coordinate of the rectangle.</param>
        /// <param name="y">The y coordinate of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        /// <param name="onlyTriggers">If true, only Colliders marked as triggers will be checked.</param>
        /// <param name="onlySolids">If true, only Colliders not marked as triggers will be checked.</param>
        /// <param name="ignoreList">A list of all Entities to exclude from the check.</param>
        /// <returns>A list of all colliding Entities of the given type.</returns>
        public List<TEntity> CheckRectCollisionsWithType<TEntity>(float x, float y, int w, int h, bool onlyTriggers = false, bool onlySolids = false, List<TEntity> ignoreList = null) where TEntity : Entity //copied from CheckRectCollisions
        {
            List<TEntity> colliding = new List<TEntity>();

            foreach (TEntity b in GetEntitiesInBoundsOfType<TEntity>(x, y, w, h))
            {

                if (ignoreList != null)
                {
                    if (ignoreList.Contains(b))
                        continue;
                }

                foreach (Collider c in b.Colliders)
                {
                    //filter out triggers/solids if necessary
                    if (onlySolids && c.Trigger)
                        continue;
                    if (onlyTriggers && !c.Trigger)
                        continue;

                    if (GameMath.CheckRectIntersection(x, y, w, h, c.Position.X, c.Position.Y, c.Width, c.Height))
                    {
                        colliding.Add(b);
                        break;
                    }
                }
            }

            return colliding;
        }

        /// <summary>
        /// Move an Entity from one cell in the table to another. The Entity will be moved from its PreviousKey to its Key. Called by Update if it detects that an Entity's key has changed.
        /// </summary>
        /// <param name="e">The Entity to move.</param>
        protected void MoveEntityCell(Entity e)
        {
            if (entityTable.ContainsKey(e.PreviousKey))
            {
                if (!entityTable[e.PreviousKey].Remove(e)) //attempt to remove
                    return; //if it couldn't be removed for some reason, assume it has already been moved and stop to avoid dupllicates

                if (entityTable.ContainsKey(e.Key))
                    entityTable[e.Key].Add(e);
                else
                    entityTable.Add(e.Key, new List<Entity> { e });
            }
        }

        protected void PhysicsLoop(List<Entity> updateList)
        {

            List<CollisionData> collisions = new List<CollisionData>();

            foreach (Entity e in updateList)
            {
                //TODO: accept input
                e.Move();
            }

            int i = 0;
            foreach (Entity e in updateList)
            {
                for (int j = i + 1; j < updateList.Count; j++)
                {
                    PhysicsMath.SATResult bestSAT = new PhysicsMath.SATResult(false, float.MinValue, null, null);

                    for (int k = 0; k < updateList[i].Components.Length; k++)
                    {
                        for (int l = 0; l < updateList[j].Components.Length; l++)
                        {
                            PhysicsMath.SATResult currentSAT = PhysicsMath.SAT(updateList[i].Components[k], updateList[j].Components[l]);
                            if (currentSAT.Penetration > bestSAT.Penetration)
                            {
                                bestSAT = currentSAT;
                            }
                        }
                    }

                    if (bestSAT.Penetration != float.MinValue)
                        collisions.Add(new CollisionData(updateList[i], updateList[j], bestSAT.Axis, bestSAT.Penetration, bestSAT.Vertex));

                }

                i++;
            }

            foreach (CollisionData c in collisions)
            {
                c.PenetrationResolution();
                c.CollisionResolution();
            }
        }

        /// <summary>
        /// Update all Entities in a given list. Called by the Update function (which provides an appropriate list).
        /// </summary>
        /// <param name="updateList">The list of Entities to update.</param>
        protected virtual void UpdateList(List<Entity> updateList)
        {
            EntityUpdateCount = 0;

            PhysicsLoop(updateList);

            //update all
            foreach (Entity e in updateList)
            {
                e.Update();
                EntityUpdateCount++; //keep track

                //remove marked entities
                if (e.ForRemoval)
                {
                    RemoveEntity(e);
                    continue; //don't bother with anything else if being removed
                }
                if (!e.Key.Equals(e.PreviousKey))
                    MoveEntityCell(e);
            }

            ApplyQueues();
        }

        /// <summary>
        /// Update the EntityManager.
        /// </summary>
        /// <param name="updateMode">The UpdateMode to use.</param>
        public void Update(UpdateMode updateMode = UpdateMode.All)
        {
            switch (updateMode)
            {
                case UpdateMode.All: //update all entities
                    UpdateList(GetAllEntities());
                    break;
                case UpdateMode.NearCamera: //update in rectangle near camera (with some padding to be safe)
                    UpdateList(GetEntitiesInBounds(
                        Renderer.Camera.Position.X - CellSize,
                        Renderer.Camera.Position.Y - CellSize,
                        Renderer.Camera.Width + (CellSize * 2),
                        Renderer.Camera.Height + (CellSize * 2)));
                    break;
            }
        }

        /// <summary>
        /// Given an Entity key value, get the coordinates of the corresponding cell.
        /// </summary>
        /// <param name="key">The key value.</param>
        /// <returns>A Vec2Int containing the coordinates of the cell corresponding to the key.</returns>
        public Vec2Int GetCellFromKey(string key)
        {
            string[] splitKey = key.Split(',');
            return new Vec2Int(Convert.ToInt32(splitKey[0]), Convert.ToInt32(splitKey[1]));
        }

        /// <summary>
        /// Given cell coordinates, build the proper Entity key.
        /// </summary>
        /// <param name="x">The cell x.</param>
        /// <param name="y">The cell y.</param>
        /// <returns>An Entity key.</returns>
        public string GetKeyFromCell(int x, int y)
        {
            return x.ToString() + "," + y.ToString();
        }
        /// <summary>
        /// Given a Vec2 representing cell coordinates, build the proper Entity key.
        /// </summary>
        /// <param name="cell">The cell coordinates.</param>
        /// <returns>An Entity key.</returns>
        public string GetKeyFromCell(Vec2Int cell) { return GetKeyFromCell(cell.X, cell.Y); }

        /// <summary>
        /// Given a position in the world, build an appropriate Entity key.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>An Entity key.</returns>
        public string GetKeyFromPos(float x, float y)
        {
            return GetKeyFromCell((int)(x / CellSize), (int)(y / CellSize));
        }
        /// <summary>
        /// Given a position in the world, build an appropriate Entity key.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>An Entity key.</returns>
        public string GetKeyFromPos(Vec2 pos) { return GetKeyFromPos(pos.X, pos.Y); }

    }
}
