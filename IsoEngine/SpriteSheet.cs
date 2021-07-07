using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class SpriteSheet
    {

        Texture2D[,] sprites;
        int width;
        int height;

        public SpriteSheet(Texture2D sheet, int width, GraphicsDevice graphicsDevice)
        {
            BuildSpriteSheet(sheet, width, 1, graphicsDevice);
        }
        public SpriteSheet(Texture2D sheet, int width, int height, GraphicsDevice graphicsDevice)
        {
            BuildSpriteSheet(sheet, width, height, graphicsDevice);
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

        public Texture2D Get(int x)
        {
            return sprites[x, 0];
        }

        public Texture2D Get(int x, int y)
        {
            return sprites[x, y];
        }

    }
}
