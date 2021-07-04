using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoEngine
{
    public class SpriteSheet
    {

        Texture2D[] sprites;

        public SpriteSheet(Texture2D sheet, int count, GraphicsDevice graphicsDevice)
        {
            int spriteW = sheet.Width / count;

            sprites = new Texture2D[count];

            for (int i = 0; i < count; i++)
            {
                //crop frame from sheet
                Texture2D slice = new Texture2D(graphicsDevice, spriteW, sheet.Height);
                Color[] colorData = new Color[spriteW * sheet.Height];
                sheet.GetData(0, new Rectangle(i * spriteW, 0, spriteW, sheet.Height), colorData, 0, colorData.Length);
                slice.SetData(colorData);
                //add to array
                sprites[i] = slice;
            }

        }

        public Texture2D Get(int index)
        {
            return sprites[index];
        }

    }
}
