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

        List<int> offsets = new();
        for (int i = 0; i < 150; i++)
        {
            offsets.Add(i);
        }

        while (offsets.Count > 0)
        {
            int i = GameMath.Random.Next(offsets.Count);

            Crate c = new(new Vec2(30, 30 + i * 3));
            EntityManager.AddEntity(c);

            offsets.RemoveAt(i);
        }
    }
}

