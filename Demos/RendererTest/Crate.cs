using Verdant;

namespace RendererTest;

public class Crate : Entity
{
    public Crate(Vec2 position)
        : base(position, Resources.Crate)
    {
        ZIndexMode = ZIndexMode.Manual;
        ZIndex = (int)position.Y;
    }
}

