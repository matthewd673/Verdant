using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    /// <summary>
    /// A SpriteSheet is a RenderObject that stores an entire texture sheet, indexed by X,Y coordinates.
    /// </summary>
    public class SpriteSheet : RenderObject
    {
        protected int spriteWidth;
        protected int spriteHeight;

        protected int horizontalSprites;
        protected int verticalSprites;

        /// <summary>
        /// Initialize a new SpriteSheet, indexed by X coordinates.
        /// </summary>
        /// <param name="sheetTexture">The Texture2D sheet of sprites. Sprites should be in one row, ordered left to right.</param>
        /// <param name="spriteWidth">The width of each sprite.</param>
        public SpriteSheet(Texture2D sheetTexture, int spriteWidth)
        {
            texture = sheetTexture;
            this.spriteWidth = spriteWidth;
            spriteHeight = sheetTexture.Height;
            width = spriteWidth;
            height = spriteHeight;
            horizontalSprites = sheetTexture.Width / width;
            verticalSprites = 1;
        }

        /// <summary>
        /// Initialize a new 2D SpriteSheet, indexed by X and Y coordinates.
        /// </summary>
        /// <param name="sheetTexture">The Texture2D sheet of sprites. Sprites should be in a 2D grid of uniform rows and columns.</param>
        /// <param name="spriteWidth">The width of each sprite.</param>
        /// <param name="spriteHeight">The height of each sprite.</param>
        public SpriteSheet(Texture2D sheetTexture, int spriteWidth, int spriteHeight)
        {
            texture = sheetTexture;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            width = spriteWidth;
            height = spriteHeight;
            horizontalSprites = sheetTexture.Width / width;
            verticalSprites = sheetTexture.Height / height;
        }

        /// <summary>
        /// Get a Sprite of the full sheet cropped at the given x (and optionally y position).
        /// </summary>
        /// <param name="x">The x position of the sprite.</param>
        /// <param name="y">The y position of the sprite.</param>
        /// <returns>A Sprite of the sheet cropped at the given coordinates.</returns>
        public Sprite GetSprite(int x, int y = 0)
        {
            return new Sprite(texture, new Rectangle(x * spriteWidth, y * spriteHeight, spriteWidth, spriteHeight));
        }

        /// <summary>
        /// Draw the first sprite on the sheet.
        /// Use DrawIndex to draw a specific sprite.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen-space bounds to draw to.</param>
        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds)
        {
            DrawIndex(spriteBatch, bounds, 0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin)
        {
            DrawIndex(spriteBatch, bounds, angle, origin, 0);
        }

        /// <summary>
        /// Draw the sprite at the given index on the sheet.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="bounds">The screen-space bounds to draw to.</param>
        /// <param name="x">The x coordinate of the sprite on the sheet.</param>
        /// <param name="y">The y coordinate of the sprite on the sheet. Some sheets are not indexed with y coordinates.</param>
        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, int x, int y = 0)
        {
            spriteBatch.Draw(texture,
                             bounds,
                             new Rectangle(x * spriteWidth, y * spriteHeight, spriteWidth, spriteHeight),
                             Color.White
                             );
        }

        public override void DrawIndex(SpriteBatch spriteBatch, Rectangle bounds, float angle, Vector2 origin, int x, int y = 0)
        {
            spriteBatch.Draw(texture,
                             bounds,
                             new Rectangle(x * spriteWidth, y * spriteHeight, spriteWidth, spriteHeight),
                             Color.White,
                             angle,
                             origin,
                             SpriteEffects.None,
                             0
                             );
        }
    }
}
