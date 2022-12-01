using System;
using System.Collections.Generic;
using System.Linq;
using Verdant.Physics;

namespace Verdant
{
    public class Pathfinder
    {

        bool[,] pathMap;

        int cellW;
        int cellH;

        public int MaxSeekDistance { get; set; }
        public int MaxSearchCells { get; set; } = 100;

        public Vec2 LastGoalPosition { get; private set; } = new Vec2(0, 0);

        /// <summary>
        /// Initialize a new Pathfinder.
        /// </summary>
        /// <param name="cellW">The width of each cell.</param>
        /// <param name="cellH">The height of each cell.</param>
        /// <param name="maxSeekDistance">The maximum distance between a walker and target before the walker stops trying to pathfind. Checked in UpdatePath.</param>
        public Pathfinder(int cellW, int cellH, int maxSeekDistance)
        {
            this.cellW = cellW;
            this.cellH = cellH;
            MaxSeekDistance = maxSeekDistance;
        }

        /// <summary>
        /// Generate a new path on the current path map with the specified walker and target Entities.
        /// </summary>
        /// <param name="walker">The walker Entity.</param>
        /// <param name="target">The target Entity.</param>
        /// <param name="walkerCollider">The specific Collider on the walker Entity to check against.</param>
        /// <param name="targetCollider">The specific Collider on the target Entity to check against.</param>
        /// <returns>A list of Vec2 points on the path (points are at the center of each PathCell, closest point to walker at index 0).</returns>
        public List<Vec2> UpdatePath(Entity walker, Entity target, Shape walkerCollider, Shape targetCollider)
        {
            //ensure that the target is within the appropriate range before finding a path
            if (GameMath.GetDistance(walker.Position, target.Position) < MaxSeekDistance)
            {
                List<PathCell> path = FindPath(walkerCollider, targetCollider);
                List<Vec2> pathPoints = new List<Vec2>();
                foreach (PathCell p in path)
                {
                    pathPoints.Add(new Vec2((p.X * cellW) + (cellW / 2), (p.Y * cellH) + (cellH / 2)));
                }
                return pathPoints;
            }

            return new List<Vec2>(); //couldn't find path, return empty
        }

        /// <summary>
        /// Find a path between the origin and the target goal.
        /// </summary>
        /// <returns>A list of all PathCells in the path (closest point to target at index 0).</returns>
        List<PathCell> FindPath(Shape walkerCollider, Shape targetCollider)
        {
            //define start & goal
            Shape walkerC = walkerCollider;
            Shape targetC = targetCollider;
            PathCell start = new PathCell((int)(walkerC.Position.X / cellW), (int)(walkerC.Position.Y / cellH));
            PathCell goal = new PathCell((int)(targetC.Position.X / cellW), (int)(targetC.Position.Y / cellH));

            LastGoalPosition = new Vec2(goal.X, goal.Y); //for stat tracking

            //lastTargetPos = targetC.pos;

            //prep algorithm
            PathCell current = null;

            List<PathCell> open = new List<PathCell>();
            List<PathCell> closed = new List<PathCell>();

            int g = 0;

            open.Add(start);

            while (open.Count > 0)
            {

                if (open.Count + closed.Count > MaxSearchCells)
                {
                    //pathCells = new List<PathCell>();
                    break;
                }

                //get square with lowest f
                var lowestF = open.Min(c => c.F);
                current = open.First(c => c.F == lowestF);

                //move current to closed
                closed.Add(current);
                open.Remove(current);

                //if goal is in closed list, we're done
                if (closed.FirstOrDefault(c => c.X == goal.X && c.Y == goal.Y) != null)
                    break;

                //find adjacent tiles
                List<PathCell> adjacent = GetAdjacentCells(current, open);
                g = current.G + 1;

                //loop through adjacent
                foreach (PathCell a in adjacent)
                {
                    //make sure it isn't already closed
                    if (closed.FirstOrDefault(c => c.X == a.X && c.Y == a.Y) != null)
                        continue;

                    //create and add to open if not open either
                    if (open.FirstOrDefault(c => c.X == a.X && c.Y == a.Y) == null)
                    {
                        //set metrics
                        a.G = g;
                        a.H = CalculateH(a, goal);
                        a.F = a.G = a.H;
                        a.parent = current;

                        //add to top of open list
                        open.Insert(0, a);
                    }
                    else //is already open
                    {
                        //update g if this path is more efficient
                        if (g + a.H < a.F)
                        {
                            a.G = g;
                            a.F = a.G + a.H;
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
            return Math.Abs(goal.X - cell.X) + Math.Abs(goal.Y - cell.Y);
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
                    if (focus.X + i > 0 &&
                        focus.X + i < pathMap.GetLength(0) &&
                        focus.Y + j > 0 &&
                        focus.Y + j < pathMap.GetLength(1))
                    {
                        //make sure its walkable
                        if (pathMap[focus.X + i, focus.Y + j])
                        {
                            //find node, create it if necessary, and add to returned list
                            PathCell node = open.Find(c => c.X == focus.X + i && c.Y == focus.Y + j);
                            if (node == null)
                                adjacent.Add(new PathCell(focus.X + i, focus.Y + j));
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
        public void BuildPathMap<T>(EntityManager entityManager) where T : PhysicsEntity
        {
            //get a list of obstacles (currently only walls)
            List<T> obstacles = entityManager.GetAllEntitiesOfType<T>();

            //create a list of colliders for all obstacles
            List<Shape> obsColliders = new List<Shape>();

            int maxX = 0;
            int maxY = 0;

            foreach (PhysicsEntity e in obstacles) // we need data that only PhysicsEntities have
            {
                foreach (Shape s in e.Components)
                {
                    obsColliders.Add(s);

                    float x = s.Position.X;
                    float y = s.Position.Y;
                    float width = 0;
                    float height = 0;
                    
                    // lines won't work (because pathfinding off of lines makes no sense)
                    if (s.GetType() == typeof(Rectangle))
                    {
                        width = ((Rectangle)s).Width;
                        height = ((Rectangle)s).Length;
                    }
                    if (s.GetType() == typeof(Circle))
                    {
                        float r = ((Circle)s).Radius;

                        x -= r; // because circle's origin is at the center
                        y -= r;
                        width = ((Circle)s).Radius * 2;
                        height = ((Circle)s).Radius * 2;
                    }


                    if (x + width > maxX)
                        maxX = (int)(x + width);

                    if (y + height > maxY)
                        maxY = (int)(y + height);
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

        /// <summary>
        /// For debugging. Draw a grid visualizing all PathCells (white = walkable).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use when drawing.</param>
        public void Visualize(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            for(int i = 0; i < pathMap.GetLength(0); i++)
            {
                for (int j = 0; j < pathMap.GetLength(1); j++)
                {
                    Microsoft.Xna.Framework.Color drawColor = Microsoft.Xna.Framework.Color.Red;
                    if (pathMap[i, j])
                        drawColor = Microsoft.Xna.Framework.Color.White;
                    // TODO: fix this when Pathfinder is reworked
                    //spriteBatch.Draw(Renderer.GetPixel(), Renderer.Camera.GetRenderBounds(i * cellW, j * cellH, 1, cellH), drawColor);
                    //spriteBatch.Draw(Renderer.GetPixel(), Renderer.Camera.GetRenderBounds(i * cellW, j * cellH, cellW, 1), drawColor);
                    //spriteBatch.Draw(Renderer.GetPixel(), Renderer.Camera.GetRenderBounds((i + 1) * cellW, j * cellH, 1, cellH), drawColor);
                    //spriteBatch.Draw(Renderer.GetPixel(), Renderer.Camera.GetRenderBounds(i * cellW, (j * 1) * cellH, cellW, 1), drawColor);
                }
            }
        }
    }

    /// <summary>
    /// PathCell represents a single cell used by the A* algorithm as implemented in Pathfinder.
    /// </summary>
    class PathCell
    {
        public int X;
        public int Y;

        public int F;
        public int G;
        public int H;

        public PathCell parent;

        /// <summary>
        /// Initialize a new PathCell.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public PathCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

}