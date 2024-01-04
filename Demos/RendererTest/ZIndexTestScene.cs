using System.Collections.Generic;

using Verdant;

namespace RendererTest;

public class ZIndexTestScene : Scene
{
    public ZIndexTestScene()
        : base("zindex_test")
    {
        // Empty
    }

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < 150; i++)
        {
            Crate c = new(new Vec2(30 + GameMath.Random.Next(3), 475 - i * 3));
            EntityManager.AddEntity(c);
        }
    }
}

