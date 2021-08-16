using System;
using System.Collections.Generic;
using System.Linq;

namespace IsoEngine
{
    public class Pathfinder
    {

        public static bool[,] pathMap;

        int cellW;
        int cellH;

        int maxSeekDistance;

        int maxSearchCells = 100;

        public Vec2 lastGoalPos = new Vec2(0, 0);

        public Pathfinder(int cellW, int cellH, int maxSeekDistance)
        {
            this.cellW = cellW;
            this.cellH = cellH;
            this.maxSeekDistance = maxSeekDistance;
        }

        /// <summary>
        /// Generate a new path on the current path map with the specified walker and target Entities.
        /// </summary>
        /// <param name="walker">The walker Entity.</param>
        /// <param name="target">The target Entity.</param>
        /// <param name="walkerCollider">The specific Collider on the walker Entity to check against.</param>
        /// <param name="targetCollider">The specific Collider on the target Entity to check against.</param>
        /// <returns>A list of Vec2 points on the path (points are at the center of each PathCell, closest point to walker at index 0).</returns>
        public List<Vec2> UpdatePath(Entity walker, Entity target, Collider walkerCollider, Collider targetCollider)
        {
            //ensure that the target is within the appropriate range before finding a path
            //if (GetBasicDistance(walker, target) < maxSeekDistance) //&&
                //System.Math.Abs(lastTargetPos.x - targetCollider.pos.x) + System.Math.Abs(lastTargetPos.y - targetCollider.pos.y) > targetMoveThreshold)
            if (Math.GetDistance(walker.Position, target.Position) < maxSeekDistance)
            {
                List<PathCell> path = FindPath(walkerCollider, targetCollider);
                List<Vec2> pathPoints = new List<Vec2>();
                foreach (PathCell p in path)
                {
                    pathPoints.Add(new Vec2((p.x * cellW) + (cellW / 2), (p.y * cellH) + (cellH / 2)));
                }
                return pathPoints;
            }

            return new List<Vec2>(); //couldn't find path, return empty
        }

        /// <summary>
        /// Find a path between the origin and the target goal.
        /// </summary>
        /// <returns>A list of all PathCells in the path (closest point to target at index 0).</returns>
        List<PathCell> FindPath(Collider walkerCollider, Collider targetCollider)
        {
            //define start & goal
            Collider walkerC = walkerCollider;
            Collider targetC = targetCollider;
            PathCell start = new PathCell((int)(walkerC.pos.X / cellW), (int)(walkerC.pos.Y / cellH));
            PathCell goal = new PathCell((int)(targetC.pos.X / cellW), (int)(targetC.pos.Y / cellH));

            lastGoalPos = new Vec2(goal.x, goal.y); //for stat tracking

            //lastTargetPos = targetC.pos;

            //prep algorithm
            PathCell current = null;

            List<PathCell> open = new List<PathCell>();
            List<PathCell> closed = new List<PathCell>();

            int g = 0;

            open.Add(start);

            while (open.Count > 0)
            {

                if (open.Count + closed.Count > maxSearchCells)
                {
                    //pathCells = new List<PathCell>();
                    break;
                }

                //get square with lowest f
                var lowestF = open.Min(c => c.f);
                current = open.First(c => c.f == lowestF);

                //move current to closed
                closed.Add(current);
                open.Remove(current);

                //if goal is in closed list, we're done
                if (closed.FirstOrDefault(c => c.x == goal.x && c.y == goal.y) != null)
                    break;

                //find adjacent tiles
                List<PathCell> adjacent = GetAdjacentCells(current, open);
                g = current.g + 1;

                //loop through adjacent
                foreach (PathCell a in adjacent)
                {
                    //make sure it isn't already closed
                    if (closed.FirstOrDefault(c => c.x == a.x && c.y == a.y) != null)
                        continue;

                    //create and add to open if not open either
                    if (open.FirstOrDefault(c => c.x == a.x && c.y == a.y) == null)
                    {
                        //set metrics
                        a.g = g;
                        a.h = CalculateH(a, goal);
                        a.f = a.g = a.h;
                        a.parent = current;

                        //add to top of open list
                        open.Insert(0, a);
                    }
                    else //is already open
                    {
                        //update g if this path is more efficient
                        if (g + a.h < a.f)
                        {
                            a.g = g;
                            a.f = a.g + a.h;
                            a.parent = current;
                        }
                    }
                }
            }

            //generate a final list of path cells from start to end
            List<PathCell> finalPath = new List<PathCell>();

            while (current != null)
            {
                finalPath.Insert(0, current);
                current = current.parent;
            }

            //should already be in this tile, no need to move there
            if (finalPath.Count > 0)
                finalPath.RemoveAt(0);

            //pathCells = finalPath;

            return finalPath;
        }

        /// <summary>
        /// Calcualte the h score of a cell.
        /// </summary>
        /// <param name="cell">The cell to calculate for.</param>
        /// <param name="goal">The goal cell.</param>
        /// <returns>The h value.</returns>
        int CalculateH(PathCell cell, PathCell goal)
        {
            return System.Math.Abs(goal.x - cell.x) + System.Math.Abs(goal.y - cell.y);
        }

        /// <summary>
        /// Get all walkable cells adjacent to a specified cell.
        /// </summary>
        /// <param name="focus">The cell to search around.</param>
        /// <param name="open">The list of currently-open cells.</param>
        /// <returns>A list of valid adjacent cells.</returns>
        List<PathCell> GetAdjacentCells(PathCell focus, List<PathCell> open)
        {
            List<PathCell> adjacent = new List<PathCell>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    //make sure coord is safe
                    if (focus.x + i > 0 &&
                        focus.x + i < pathMap.GetLength(0) &&
                        focus.y + j > 0 &&
                        focus.y + j < pathMap.GetLength(1))
                    {
                        //make sure its walkable
                        if (pathMap[focus.x + i, focus.y + j])
                        {
                            //find node, create it if necessary, and add to returned list
                            PathCell node = open.Find(c => c.x == focus.x + i && c.y == focus.y + j);
                            if (node == null)
                                adjacent.Add(new PathCell(focus.x + i, focus.y + j));
                            else
                                adjacent.Add(node);
                        }
                    }
                }
            }

            return adjacent;
        }

        /// <summary>
        /// Build the pathfinder's map of obstacles. Automatically built on construction.
        /// </summary>
        /// <typeparam name="T">The type of Entity to treat as an obstacle.</typeparam>
        /// <param name="entityManager">The EntityManager to search for obstacles within.</param>
        public void BuildPathMap<T>(EntityManager entityManager) where T : Entity
        {
            //get a list of obstacles (currently only walls)
            List<T> obstacles = entityManager.GetAllEntitiesOfType<T>();

            //create a list of colliders for all obstacles
            List<Collider> obsColliders = new List<Collider>();

            int maxX = 0;
            int maxY = 0;

            foreach (Entity e in obstacles) //we need data that only Tiles have
            {
                foreach (Collider c in e.GetColliders())
                {
                    obsColliders.Add(c);

                    if (c.pos.X + c.w > maxX)
                        maxX = (int)c.pos.X + c.w;

                    if (c.pos.Y + c.h > maxY)
                        maxY = (int)c.pos.Y + c.h;
                }
            }

            //create a pathmap that fits the size of the map
            pathMap = new bool[(maxX / cellW) + 1, (maxY / cellH) + 1];

            //check pathmap collisions
            for (int i = 0; i < pathMap.GetLength(0); i++)
            {
                for (int j = 0; j < pathMap.GetLength(1); j++)
                {
                    int cellX = i * cellW;
                    int cellY = j * cellH;

                    //will get the job done but isn't excluding non-walls
                    if (entityManager.CheckRectCollisions(cellX, cellY, cellW, cellH, onlySolids: true).Count == 0)
                    //if (CollisionSolver.GetAllPotentialColliding(cellX, cellY, cellW, cellH, excludeTriggers: true).Count == 0)
                        pathMap[i, j] = true;
                    else
                        pathMap[i, j] = false;

                }
            }

        }

        public void Visualize(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            for(int i = 0; i < pathMap.GetLength(0); i++)
            {
                for (int j = 0; j < pathMap.GetLength(1); j++)
                {
                    Microsoft.Xna.Framework.Color drawColor = Microsoft.Xna.Framework.Color.Red;
                    if (pathMap[i, j])
                        drawColor = Microsoft.Xna.Framework.Color.White;
                    spriteBatch.Draw(Renderer.GetPixel(), Renderer.GetRenderBounds(i * cellW, j * cellH, 1, cellH), drawColor);
                    spriteBatch.Draw(Renderer.GetPixel(), Renderer.GetRenderBounds(i * cellW, j * cellH, cellW, 1), drawColor);
                    spriteBatch.Draw(Renderer.GetPixel(), Renderer.GetRenderBounds((i + 1) * cellW, j * cellH, 1, cellH), drawColor);
                    spriteBatch.Draw(Renderer.GetPixel(), Renderer.GetRenderBounds(i * cellW, (j * 1) * cellH, cellW, 1), drawColor);
                }
            }
        }

        /*
        float GetBasicDistance(Entity walker, Entity target)
        {
            return System.Math.Abs(target.pos.x - walker.pos.x) + System.Math.Abs(target.pos.y - walker.pos.y);
        }
        */

    }

}