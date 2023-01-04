using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.Physics
{
    /// <summary>
    /// The foundation of PhysicsEntity colliders.
    /// </summary>
    public class Shape
    {
        internal Vec2[] Vertices { get; set; }
        internal Vec2 Position { get; set; }
        // The direction of the Shape.
        public Vec2 Dir { get; set; }

        /// <summary>
        /// Draw the Shape (such as when visualizing a PhysicsEntity's body).
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with.</param>
        /// <param name="camera">The Scene Camera to render through.</param>
        /// <param name="color">The color to draw the Shape with.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Camera camera, Color color) { }

    }
}
