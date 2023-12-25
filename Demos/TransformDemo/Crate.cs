using Verdant;

namespace TransformDemo;

public class Crate : Entity
{
    public Crate(Vec2 position)
        : base(Resources.Crate, position, 32, 32)
    {
        // Empty
    }
}
