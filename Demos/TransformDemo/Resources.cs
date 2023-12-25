using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Verdant;

namespace TransformDemo;

public static class Resources
{
    public static Sprite Crate { get; private set; }

    public static void LoadResources(ContentManager content)
    {
        Crate = content.Load<Texture2D>("crate");
    }
}
