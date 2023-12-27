using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{

    /// <summary>
    /// Sprite is a wrapper class for Texture2D which can be used anywhere a RenderObject is required. Texture2Ds can be implicitly converted to Sprites.
    /// </summary>
    public class Sprite : RenderObject
    {
        protected Rectangle? crop = null;

        /// <summary>
        /// Initialize a new Sprite.
        /// </summary>
        /// <param name="sprite">The Sprite's texture.</param>
        public Sprite(Texture2D sprite)
        {
            texture = sprite;
            width = texture.Width;
            height = texture.Height;
        }

        /// <summary>
        /// Initialize a new Sprite.
        /// </summary>
        /// <param name="sprite">The Sprite's texture.</param>
        /// <param name="crop">The section of the texture to draw.</param>
        public Sprite(Texture2D sprite, Rectangle crop)
        {
            texture = sprite;
            this.crop = crop;
            width = crop.Width;
            height = crop.Height;
        }

        public override void Draw(SpriteBatch spriteBatch, TransformState transform)
        {
            spriteBatch.Draw(
                    texture,
                    transform.GetRenderRectangle(),
                    crop,
                    Color.White,
                    transform.Angle,
                    new Vector2(transform.HalfWidth, transform.HalfHeight),
                    SpriteEffects.None,
                    0
                    );
        }

        /// <summary>
        /// Draw the Sprite's texture at the given screen bounds.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen-space bounds to draw to.</param>
        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.Draw(texture, bounds, crop, Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin)
        {
            spriteBatch.Draw(
                texture,
                bounds,
                crop,
                Color.White,
                angle,
                origin,
                SpriteEffects.None,
                0
                );
        }

        public override void DrawIndex(SpriteBatch spriteBatch, TransformState transform, int x, int y = 0)
        {
            Draw(spriteBatch, transform);
        }

        /// <summary>
        /// Sprites do not support DrawIndex, it will be a normal Draw call.
        /// </summary>
        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, int x, int y = 0)
        {
            Draw(spriteBatch, bounds);
        }

        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin, int x, int y = 0)
        {
            Draw(spriteBatch, bounds, angle, origin);
        }

        public static implicit operator Sprite(Texture2D texture2d) => new Sprite(texture2d);

    }
}
