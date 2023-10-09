using System;
using System.Collections.Generic;
using System.Diagnostics;

using Verdant.Physics;

namespace Verdant
{
    /// <summary>
    /// Manages the creation, deletion, and update logic of Entities.
    /// </summary>
    public class EntityManager
    {
        public enum ZIndexMode
        {
            // Assign a ZIndex based on the order it was added to the Manager.
            // This will only work if this is the Entity's ZIndexMode prior to being added to a Manager.
            ByIndex,
            // Manually assign ZIndex (default: 0).
            Manual,
            // Automatically assign ZIndex to the bottom of the Entity's Sprite after each update.
            Bottom,
            // Automatically assign ZIndex to the top of the Entity's Sprite after each update.
            Top,
        }

        // The Scene that this EntityManager belongs to.
        public Scene Scene { get; set; }

        private readonly EntityTable table = new();
        // The width and height of each cell in the internal Entity table.
        public int CellSize { get; }

        private List<Entity> addQueue = new List<Entity>();
        private List<Entity> removeQueue = new List<Entity>();

        // The number of Entities currently managed by the EntityManager.
        public int EntityCount { get; protected set; }
        // The number of Entities updated in the last Update call.
        public int EntityUpdateCount { get; protected set; }
        // The number of PhysicsEntities update in the last physics loop (within the last Update call).
        public int PhysicsEntityUpdateCount { get; protected set; }

        private Stopwatch updatePerformanceTimer = new Stopwatch();
        // The duration (in milliseconds) of the last Update call.
        public float UpdateDuration { get; protected set; }
        private Stopwatch physicsPerformanceTimer = new Stopwatch();
        // The duration (in milliseconds) of the last physics loop (within the last Update call).
        public float PhysicsDuration { get; protected set; }

        // Indicates if the EntityManager has entered the update loop.
        public bool Updating { get; protected set; } = false;

        private List<CollisionData> collisions = new List<CollisionData>();

        /// <summary>
        /// Initialize a new EntityManager.
        /// </summary>
        public EntityManager()
        {
            CellSize = 64;
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
            if (!Updating) ApplyQueues();
        }

        /// <summary>
        /// Add a list of Entities to the manager, and mark this instance as the manager of all of the Entities.
        /// </summary>
        /// <param name="l">The list of Entities to add.</param>
        public void AddEntityRange(List<Entity> l)
        {
            foreach (Entity e in l)
            {
                addQueue.Add(e);
            }
            if (!Updating) ApplyQueues();
        }

        /// <summary>
        /// Remove an Entity from the list, and mark its manager as null.
        /// NOTE: In most cases (such as if Entities are removing themselves or others
        /// in the middle of an update) set <c>Entity.ForRemoval = true</c> instead.
        /// </summary>
        /// <param name="e">The Entity to remove.</param>
        public void RemoveEntity(Entity e)
        {
            removeQueue.Add(e);
            if (!Updating) ApplyQueues();
        }

        private void ApplyQueues()
        {
            // add marked
            foreach (Entity e in addQueue)
            {
                e.Manager = this;
                if (e.ZIndexMode == ZIndexMode.ByIndex)
                    e.ZIndex = EntityCount;

                // add to table
                table.Insert(e.Key.X, e.Key.Y, e);

                EntityCount++; // keep track
                e.OnAdd(); // trigger event
            }

            // remove marked
            foreach (Entity e in removeQueue)
            {
                Vec2Int key = e.ForRemoval ? e.PreviousKey : e.Key;
                if (table.Remove(key.X, key.Y, e))
                {
                    e.Manager = null;
                    EntityCount--; // keep track
                    e.OnRemove(); // trigger event
                }
            }

            addQueue.Clear();
            removeQueue.Clear();
        }

        /// <summary>
        /// Get a list of all Entities of a given type near the given Entity. Check is performed in the Entity's table cell and all neighboring cells.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="e">The Entity to search near.</param>
        /// <returns>A list of all near Entities of the given type.</returns>
        public List<TEntity> GetNearEntities<TEntity>(Entity e) where TEntity : Entity
        {
            List<TEntity> nearEntities = new List<TEntity>();

            //loop through all neighboring cells and check
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    EntityList cell;
                    if (table.GetCell(e.Key.X + i, e.Key.Y + j, out cell))
                    {
                        foreach (Entity b in cell.GetEntities())
                        {
                            if (b.IsType(typeof(TEntity)))
                                nearEntities.Add((TEntity)b);
                        }
                    }
                }
            }

            return nearEntities;
        }

        /// <summary>
        /// Get a list of all Entities near the given Entity.
        /// Check is performed in Entity's table cell and all neighboring cells.
        /// </summary>
        /// <param name="e">The Entity to search near.</param>
        /// <returns>A list of all near Entities.</returns>
        public List<Entity> GetNearEntities(Entity e)
        {
            return GetNearEntities<Entity>(e);
        }

        /// <summary>
        /// Get a list of all Entities of a given type bounded by a given rectangle.
        /// Entities are guaranteed to be within 1 cell of the bounds,
        /// but may not actually fall inside the bounds.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="x">The x position of the rectangle.</param>
        /// <param name="y">The y position of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<TEntity> GetEntitiesInBounds<TEntity>(float x, float y, float width, float height) where TEntity : Entity
        {
            List<TEntity> boundedEntities = new List<TEntity>();

            // calculate cell bounds
            int minCellX = (int)Math.Floor(x / CellSize);
            int minCellY = (int)Math.Floor(y / CellSize);
            int maxCellX = (int)Math.Ceiling((x + width) / CellSize);
            int maxCellY = (int)Math.Ceiling((y + height) / CellSize);

            // loop through all cells in bounds
            for (int i = minCellX; i <= maxCellX; i++)
            {
                for (int j = minCellY; j <= maxCellY; j++)
                {
                    EntityList cell;
                    // skip if cell doesn't exist
                    if (!table.GetCell(i, j, out cell))
                    {
                        continue;
                    }

                    foreach (Entity b in cell.GetEntities())
                    {
                        if (b.IsType(typeof(TEntity)))
                            boundedEntities.Add((TEntity)b);
                    }
                }
            }

            return boundedEntities;
        }
        /// <summary>
        /// Get a list of all Entities of a given type bounded by a given rectangle.
        /// Entities are guaranteed to be within 1 cell of the bounds,
        /// but may not actually fall inside the bounds.
        /// </summary>
        /// <typeparam name="TEntity">The type of Entity to check for.</typeparam>
        /// <param name="pos">The position of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<TEntity> GetEntitiesInBounds<TEntity>(Vec2 pos, int width, int height) where TEntity : Entity
        {
            return GetEntitiesInBounds<TEntity>(pos.X, pos.Y, width, height);
        }

        /// <summary>
        /// Get a list of all Entities bounded by a given rectangle.
        /// Entities are guaranteed to be within 1 cell of the bounds,
        /// but may not actually fall inside the bounds.
        /// </summary>
        /// <param name="x">The x position of the rectangle.</param>
        /// <param name="y">The y position of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<Entity> GetEntitiesInBounds(float x, float y, int width, int height)
        {
            return GetEntitiesInBounds<Entity>(x, y, width, height);
        }

        /// <summary>
        /// Get a list of all Entities bounded by a given rectangle.
        /// Entities are guaranteed to be within 1 cell of the bounds,
        /// but may not actually fall inside the bounds.
        /// </summary>
        /// <param name="pos">The position of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns>A list of all bounded Entities.</returns>
        public List<Entity> GetEntitiesInBounds(Vec2 pos, int width, int height)
        {
            return GetEntitiesInBounds<Entity>(pos.X, pos.Y, width, height);
        }

        /// <summary>
        /// Get a list of all PhysicsEntities of a given type intersecting with a given rectangle. 
        /// </summary>
        /// <typeparam name="TPhysicsEntity">The type of Entity to check for.</typeparam>
        /// <param name="x">The x coordinate of the rectangle.</param>
        /// <param name="y">The y coordinate of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="includeTriggers">Check against PhysicsEntities that are triggers.</param>
        /// <param name="includeSolids">Check against PhysicsEntities that are not triggers.</param>
        /// <param name="ignoreList">A list of all Entities to exclude from the check.</param>
        /// <returns>A list of all colliding Entities of the given type.</returns>
        public List<TPhysicsEntity> CheckRectCollisions<TPhysicsEntity>(float x, float y, int width, int height, bool includeTriggers = false, bool includeSolids = true, List<TPhysicsEntity> ignoreList = null) where TPhysicsEntity : PhysicsEntity
        {
            List<TPhysicsEntity> colliding = new List<TPhysicsEntity>();

            List<TPhysicsEntity> searchList = GetEntitiesInBounds<TPhysicsEntity>(x, y, width, height);
            Rectangle bounds = new(x, y, x + height, y + 1, width); // why do these coordinates work?
            bounds.CalculateVertices();

            foreach (TPhysicsEntity e in searchList)
            {
                if (e.Trigger && !includeTriggers)
                    continue;
                if (!e.Trigger && !includeSolids)
                    continue;

                if (ignoreList != null && ignoreList.Contains(e))
                    continue;

                PhysicsMath.SATResult bestSAT = new PhysicsMath.SATResult(false, float.MinValue, null, null);

                for (int k = 0; k < e.Components.Length; k++)
                {
                    PhysicsMath.SATResult currentSAT = PhysicsMath.SAT(bounds, e.Components[k]);
                    if (currentSAT.Penetration > bestSAT.Penetration)
                    {
                        bestSAT = currentSAT;
                    }
                }

                if (bestSAT.Overlap)
                {
                    colliding.Add(e);
                }
            }

            return colliding;
        }

        /// <summary>
        /// Get a list of all PhysicsEntities intersecting with a given rectangle. 
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle.</param>
        /// <param name="y">The y coordinate of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="includeTriggers">Check against PhysicsEntities that are triggers.</param>
        /// <param name="includeSolids">Check against PhysicsEntities that are not triggers.</param>
        /// <param name="ignoreList">A list of all Entities to exclude from the check.</param>
        /// <returns>A list of all colliding Entities of the given type.</returns>
        public List<PhysicsEntity> CheckRectCollisions(float x, float y, int width, int height, bool includeTriggers = false, bool includeSolids = true, List<PhysicsEntity> ignoreList = null)
        {
            return CheckRectCollisions<PhysicsEntity>(x, y, width, height, includeTriggers, includeSolids, ignoreList);
        }

        /// <summary>
        /// Move an Entity from one cell in the table to another. The Entity will be moved from its PreviousKey to its Key. Called by Update if it detects that an Entity's key has changed.
        /// </summary>
        /// <param name="e">The Entity to move.</param>
        protected void MoveEntityCell(Entity e)
        {
            if (e.Key.X == e.PreviousKey.X &&
                e.Key.Y == e.PreviousKey.Y)
                return;

            EntityList currentCellList;
            if (table.GetCell(e.PreviousKey.X, e.PreviousKey.Y, out currentCellList))
            {
                if (!currentCellList.Remove(e)) // attempt to remove
                    return; // if it couldn't be removed for some reason, assume it has already been moved and stop to avoid dupllicates

                table.Insert(e.Key.X, e.Key.Y, e);
            }
        }

        private void PhysicsLoop(List<PhysicsEntity> updateList)
        {
            collisions.Clear();

            for (int i = 0; i < updateList.Count; i++)
            {
                PhysicsEntity a = updateList[i];
                a.Colliding.Clear();

                Vec2Int searchKey = a.Key;
                EntityList[] range = table.GetCellRange(searchKey.X - 1, searchKey.Y - 1, searchKey.X + 1, searchKey.Y + 1);
                for (int m = 0; m < range.Length; m++)
                {
                    List<PhysicsEntity> cellPhysicsEntities = range[m].GetPhysicsEntities();
                    for (int j = 0; j < cellPhysicsEntities.Count; j++)
                    {
                        // don't check non-physics entities
                        PhysicsEntity b = cellPhysicsEntities[j];

                        // don't check against your own self
                        if (b == a) continue;

                        PhysicsMath.SATResult bestSAT = new PhysicsMath.SATResult(false, float.MinValue, null, null);
                        for (int k = 0; k < a.Components.Length; k++)
                        {
                            for (int l = 0; l < b.Components.Length; l++)
                            {
                                PhysicsMath.SATResult currentSAT = PhysicsMath.SAT(a.Components[k], b.Components[l]);
                                if (currentSAT.Penetration > bestSAT.Penetration)
                                {
                                    bestSAT = currentSAT;
                                }
                            }
                        }

                        if (bestSAT.Overlap)
                        {
                            collisions.Add(new CollisionData(a, b, bestSAT.Axis, bestSAT.Penetration, bestSAT.Vertex));
                        }
                    }
                }
            }

            foreach (CollisionData c in collisions)
            {
                c.a.Colliding.Add(c.b);
                c.b.Colliding.Add(c.a);

                if (c.a.Trigger || c.b.Trigger) // skip triggers
                    continue;

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
            PhysicsEntityUpdateCount = 0;
            List<PhysicsEntity> physicsList = new();

            // update all
            foreach (Entity e in updateList)
            {
                e.Update();

                EntityUpdateCount++; // count entity updates

                // remove marked entities
                if (e.ForRemoval)
                {
                    RemoveEntity(e);
                    continue; // don't bother with anything else if being removed
                }

                // move physicsentities
                if (e.IsType(typeof(PhysicsEntity)))
                {
                    PhysicsEntity p = (PhysicsEntity) e;
                    p.Move();
                    physicsList.Add(p);
                    PhysicsEntityUpdateCount++; // count physics updates
                }

                MoveEntityCell(e);
            }

            physicsPerformanceTimer.Start();
            PhysicsLoop(physicsList);
            physicsPerformanceTimer.Stop();
            PhysicsDuration = physicsPerformanceTimer.ElapsedMilliseconds;
            physicsPerformanceTimer.Reset();

            ApplyQueues();
        }

        /// <summary>
        /// Update the EntityManager.
        /// </summary>
        /// <param name="updateMode">The UpdateMode to use.</param>
        public void Update()
        {
            updatePerformanceTimer.Start();
            Updating = true;
            //update in rectangle near camera (with some padding to be safe)
            UpdateList(GetEntitiesInBounds(
                Scene.Camera.Position.X - CellSize,
                Scene.Camera.Position.Y - CellSize,
                (int)(Scene.Camera.Width + (CellSize * 2)),
                (int)(Scene.Camera.Height + (CellSize * 2))));

            updatePerformanceTimer.Stop();
            UpdateDuration = updatePerformanceTimer.ElapsedMilliseconds;
            updatePerformanceTimer.Reset();
        }

        /// <summary>
        /// Given a position in the world, build an appropriate Entity key.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>An Entity key.</returns>
        public void SetEntityKey(Entity e)
        {
            e.Key = new((int)e.Position.X / CellSize,
                        (int)e.Position.Y / CellSize);
        }

    }
}
