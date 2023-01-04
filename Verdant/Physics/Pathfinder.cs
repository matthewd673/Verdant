using System;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Physics
{
    /// <summary>
    /// Generates a path map from the Entities in a given Manager and finds valid paths between Entities that avoid obstacles.
    /// </summary>
    public class Pathfinder
    {

        private bool[,] pathMap;

        private readonly int cellWidth;
        private readonly int cellHeight;

        private Vec2Int origin; // top left corner of the pathmap

        // The maximum distance from walker to target that the walker will seek to traverse.
        public int MaxSeekDistance { get; set; }
        // The maximum number of cells that may be searched when pathfinding.
        public int MaxSearchCells { get; set; } = 100;
        // The last goal point that a walker attempted to reach.
        public Vec2 LastGoalPosition { get; private set; } = new Vec2(0, 0);

        /// <summary>
        /// Initialize a new Pathfinder.
        /// </summary>
        /// <param name="cellWidth">The width of each cell.</param>
        /// <param name="cellHeight">The height of each cell.</param>
        /// <param name="maxSeekDistance">The maximum distance between a walker and target before the walker stops trying to pathfind.</param>
        public Pathfinder(int cellWidth, int cellHeight, int maxSeekDistance)
        {
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            MaxSeekDistance = maxSeekDistance;
        }

        /// <summary>
        /// Generate a new path on the current map with the specified walker and target Entities.
        /// </summary>
        /// <param name="walker">The walker Entity.</param>
        /// <param name="target">The target Entity.</param>
        /// <param name="walkerCollider">The specific physics Component on the walker Entity to check against.</param>
        /// <param name="targetCollider">The specific physics Component on the target Entity to check against.</param>
        /// <param name="maxSeekDistance">The maximum distance from walker to target that the walker will seek to traverse. If omitted, the default MaxSeekDistance will be used.</param>
        /// <returns>A list of Vec2 points on the path (points are at the center of each PathCell, closest point to walker at index 0).</returns>
        public List<Vec2> FindPath(Entity walker, Entity target, Shape walkerCollider, Shape targetCollider, int maxSeekDistance = -1)
        {
            // ensure that the target is within the appropriate range before finding a path
            if (GameMath.GetDistance(walker.Position, target.Position) < (maxSeekDistance == -1 ? MaxSeekDistance : maxSeekDistance))
            {
                return GeneratePath(walkerCollider, targetCollider);
            }

            return new List<Vec2>(); // couldn't find path, return empty
        }

        /// <summary>
        /// Generate a new path on the current map with the specified walker and target Entities.
        /// </summary>
        /// <param name="walker">The walker Entity.</param>
        /// <param name="target">The target Entity.</param>
        /// <param name="maxSeekDistance">The maximum distance from walker to target that the walker will seek to traverse. If omitted, the default MaxSeekDistance will be used.</param>
        /// <returns>A list of Vec2 points on the path (points are at the center of each PathCell, closest point to walker at index 0).</returns>
        public List<Vec2> FindPath(PhysicsEntity walker, PhysicsEntity target, int maxSeekDistance = -1)
        {
            return FindPath(walker, target, walker.Components[0], target.Components[0], maxSeekDistance);
        }

        /// <summary>
        /// Find a path between the origin and the target goal.
        /// </summary>
        /// <returns>A list of all points in the path (where [0] is the target's current tile). Points are marked as the center of a PathCell.</returns>
        List<Vec2> GeneratePath(Shape walkerCollider, Shape targetCollider)
        {
            // define start & goal
            Shape walkerC = walkerCollider;
            Shape targetC = targetCollider;
            PathCell start = new PathCell((int)(walkerC.Position.X / cellWidth), (int)(walkerC.Position.Y / cellHeight));
            PathCell goal = new PathCell((int)(targetC.Position.X / cellWidth), (int)(targetC.Position.Y / cellHeight));

            LastGoalPosition = new Vec2(goal.X, goal.Y); // for stat tracking

            // prep algorithm
            PathCell current = null;

            List<PathCell> open = new List<PathCell>();
            List<PathCell> closed = new List<PathCell>();

            int g = 0;

            open.Add(start);

            while (open.Count > 0)
            {

                if (open.Count + closed.Count > MaxSearchCells)
                {
                    break;
                }

                // get square with lowest f
                var lowestF = open.Min(c => c.F);
                current = open.First(c => c.F == lowestF);

                // move current to closed
                closed.Add(current);
                open.Remove(current);

                // if goal is in closed list, we're done
                if (closed.FirstOrDefault(c => c.X == goal.X && c.Y == goal.Y) != null)
                    break;

                // find adjacent tiles
                List<PathCell> adjacent = GetAdjacentCells(current, open);
                g = current.G + 1;

                // loop through adjacent
                foreach (PathCell a in adjacent)
                {
                    // make sure it isn't already closed
                    if (closed.FirstOrDefault(c => c.X == a.X && c.Y == a.Y) != null)
                        continue;

                    // create and add to open if not open either
                    if (open.FirstOrDefault(c => c.X == a.X && c.Y == a.Y) == null)
                    {
                        // set metrics
                        a.G = g;
                        a.H = CalculateH(a, goal);
                        a.F = a.G = a.H;
                        a.Parent = current;

                        // add to top of open list
                        open.Insert(0, a);
                    }
                    else // is already open
                    {
                        // update g if this path is more efficient
                        if (g + a.H < a.F)
                        {
                            a.G = g;
                            a.F = a.G + a.H;
                            a.Parent = current;
                        }
                    }
                }
            }

            // generate a final list of points from start to end
            List<Vec2> finalPath = new List<Vec2>();

            while (current != null)
            {
                finalPath.Insert(0, new Vec2(current.X * cellWidth + cellWidth/2, current.Y * cellHeight + cellHeight/2)); // add center of cell as new point
                current = current.Parent;
            }

            return finalPath;
        }

        /// <summary>
        /// Calcualte the h score of a cell.
        /// </summary>
        /// <param name="cell">The cell to calculate for.</param>
        /// <param name="goal">The goal cell.</param>
        /// <returns>The h value.</returns>
        private static int CalculateH(PathCell cell, PathCell goal)
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
        /// Build the pathfinder's map of obstacles. Only Entities of a given type will be treated as obstacles.
        /// </summary>
        /// <typeparam name="T">The type of Entity to treat as an obstacle.</typeparam>
        /// <param name="entityManager">The EntityManager to search for obstacles within.</param>
        /// <param name="topLeft">The top left corner of the path map's bounds.</param>
        /// <param name="bottomRight">The bottom right corner of the path map's bounds.</param>
        public void BuildPathMap<T>(EntityManager entityManager, Vec2 topLeft = null, Vec2 bottomRight = null) where T : PhysicsEntity
        {
            // get a list of obstacles
            List<T> obstacles;
            if (topLeft == null || bottomRight == null)
            {
                obstacles = entityManager.GetAllEntities<T>();
            }
            else
            {
                obstacles = entityManager.CheckRectCollisions<T>(topLeft.X, topLeft.Y, (int)(bottomRight.X - topLeft.X), (int)(bottomRight.Y - topLeft.Y));
            }

            // create a list of colliders for all obstacles
            List<Shape> obsColliders = new List<Shape>();

            int minX = int.MaxValue;
            int maxX = 0;
            int minY = int.MaxValue;
            int maxY = 0;

            // find boundaries since they were not specified
            if (topLeft == null || bottomRight == null)
            {
                foreach (PhysicsEntity e in obstacles) // we need data that only PhysicsEntities have
                {
                    foreach (Shape s in e.Components)
                    {
                        obsColliders.Add(s);

                        float x = s.Position.X;
                        float y = s.Position.Y;
                        float width = 0;
                        float height = 0;

                        // TODO: add lines (so you can pathfind around walls)
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

                        if (x - (width / 2) < minX)
                            minX = (int)(x - (width / 2));
                        if (x + (width / 2) > maxX)
                            maxX = (int)(x + (width / 2));

                        if (y - (height / 2) < minY)
                            minY = (int)(y - (height / 2));
                        if (y + (height / 2) > maxY)
                            maxY = (int)(y + (height / 2));
                    }
                }
            }
            else
            {
                minX = (int) topLeft.X;
                maxX = (int) bottomRight.X;
                minY = (int) topLeft.Y;
                maxY = (int) bottomRight.Y;
            }

            origin = new Vec2Int(minX, minY);

            // create a pathmap that fits the size of the map (from origin to max)
            pathMap = new bool[(maxX - origin.X) / cellWidth + 1, (maxY - origin.Y) / cellHeight + 1];

            // check pathmap collisions
            for (int i = 0; i < pathMap.GetLength(0); i++)
            {
                for (int j = 0; j < pathMap.GetLength(1); j++)
                {
                    int cellX = i * cellWidth + origin.X + (cellWidth / 2);
                    int cellY = j * cellHeight + origin.Y + (cellHeight / 2);

                    if (entityManager.CheckRectCollisions<T>(cellX, cellY, cellWidth, cellHeight, includeTriggers: false).Count == 0)
                        pathMap[i, j] = true;
                    else
                        pathMap[i, j] = false;
                }
            }

        }

        /// <summary>
        /// Build the Pathfinder's map of obstacles.
        /// </summary>
        /// <param name="entityManager">The EntityManager to search for obstacles within.</param>
        /// <param name="topLeft">The top left corner of the path map's bounds.</param>
        /// <param name="bottomRight">The bottom right corner of the path map's bounds.</param>
        public void BuildPathMap(EntityManager entityManager, Vec2 topLeft = null, Vec2 bottomRight = null)
        {
            BuildPathMap<PhysicsEntity>(entityManager, topLeft, bottomRight);
        }

        /// <summary>
        /// Check if the cell at a given point is open (can be traversed).
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns></returns>
        public bool IsOpen(float x, float y)
        {
            if (x < origin.X || y < origin.Y) return false;

            return pathMap[(int)((x - origin.X) / cellWidth + 1), (int)((y - origin.Y) / cellHeight + 1)];
        }

        /// <summary>
        /// For debugging. Draw a grid visualizing all PathCells (white = walkable).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use when drawing.</param>
        public void Visualize(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Camera camera)
        {
            for (int i = 0; i < pathMap.GetLength(0); i++)
            {
                for (int j = 0; j < pathMap.GetLength(1); j++)
                {
                    Microsoft.Xna.Framework.Color drawColor = Microsoft.Xna.Framework.Color.Red;
                    if (pathMap[i, j])
                        drawColor = Microsoft.Xna.Framework.Color.White;

                    spriteBatch.Draw(Renderer.GetPixel(), camera.GetRenderBounds(i * cellWidth + origin.X, j * cellHeight + origin.Y, 1, cellHeight), drawColor);
                    spriteBatch.Draw(Renderer.GetPixel(), camera.GetRenderBounds(i * cellWidth + origin.X, j * cellHeight + origin.Y, cellWidth, 1), drawColor);
                    spriteBatch.Draw(Renderer.GetPixel(), camera.GetRenderBounds((i + 1) * cellWidth + origin.X, j * cellHeight + origin.Y, 1, cellHeight), drawColor);
                    spriteBatch.Draw(Renderer.GetPixel(), camera.GetRenderBounds(i * cellWidth + origin.X, (j * 1) * cellHeight + origin.Y, cellWidth, 1), drawColor);
                }
            }
        }
    }

    /// <summary>
    /// PathCell represents a single cell used by the A* algorithm as implemented in Pathfinder.
    /// </summary>
    class PathCell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }

        public PathCell Parent { get; set; }

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