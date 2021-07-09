using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class SpriteSheet
    {

        protected Texture2D[,] sprites;
        int width;
        int height;

        /// <summary>
        /// Initialize a new SpriteSheet.
        /// </summary>
        /// <param name="sheet">The Texture2D sheet of sprites. Sprites should be in one row, ordered left to right.</param>
        /// <param name="width">The number of sprites in the sheet.</param>
        /// <param name="graphicsDevice">The GraphicsDevice to use when cropping sprites.</param>
        public SpriteSheet(Texture2D sheet, int width, GraphicsDevice graphicsDevice)
        {
            BuildSpriteSheet(sheet, width, 1, graphicsDevice);
        }
        /// <summary>
        /// Initialize a new 2D SpriteSheet.
        /// </summary>
        /// <param name="sheet">The Texture2D sheet of sprites. Sprites should be in a 2D grid of uniform rows and columns.</param>
        /// <param name="width">The number of sprites in each row.</param>
        /// <param name="height">The number of sprites in each column.</param>
        /// <param name="graphicsDevice">The GraphicsDevice to use when cropping.</param>
        public SpriteSheet(Texture2D sheet, int width, int height, GraphicsDevice graphicsDevice)
        {
            BuildSpriteSheet(sheet, width, height, graphicsDevice);
        }
        /// <summary>
        /// Initialize a new SpriteSheet from an existing array of Texture2Ds.
        /// </summary>
        /// <param name="sprites">The array of Texture2Ds to use.</param>
        public SpriteSheet(Texture2D[,] sprites)
        {
            this.sprites = sprites;
            width = sprites.GetLength(0);
            height = sprites.GetLength(1);
        }

        void BuildSpriteSheet(Texture2D sheet, int width, int height, GraphicsDevice graphicsDevice)
        {
            int spriteW = sheet.Width / width;
            int spriteH = sheet.Height / height;

            sprites = new Texture2D[width, height];

            int ct = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //crop frame from sheet
                    Texture2D slice = new Texture2D(graphicsDevice, spriteW, spriteH);
                    Color[] colorData = new Color[spriteW * spriteH];
                    sheet.GetData(0, new Rectangle(i * spriteW, j * spriteH, spriteW, spriteH), colorData, 0, colorData.Length);
                    slice.SetData(colorData);
                    //add to array
                    sprites[i, j] = slice;
                }
            }
        }

        /// <summary>
        /// Get the sprite at the x position in the first row.
        /// </summary>
        /// <param name="x">The x position of the sprite.</param>
        /// <returns>The Texture2D sprite located at the given coordinates.</returns>
        public Texture2D Get(int x)
        {
            return sprites[x, 0];
        }

        /// <summary>
        /// Get the sprite at the x and y coordinates on the sheet.
        /// </summary>
        /// <param name="x">The x position of the sprite.</param>
        /// <param name="y">The y position of the sprite.</param>
        /// <returns>The Texture2D sprite located at the given coordinates.</returns>
        public Texture2D Get(int x, int y)
        {
            return sprites[x, y];
        }

        /// <summary>
        /// Get a 2D array containing all sprites in the sheet.
        /// </summary>
        /// <returns>A 2D array of Texture2D sprites.</returns>
        public Texture2D[,] GetFullSheet()
        {
            return sprites;
        }

    }
}
