using Microsoft.Xna.Framework.Graphics;
using Verdant;

namespace TransformDemo;

public class Crate : Entity
{
    public Crate(Vec2 position)
        : base(Resources.Crate, position, 32, 32)
    {
        // Empty
    }

    public void AddTransform(TransformState transform)
    {
        TransformStates.Add(transform);
    }

    public bool RemoveTransform(TransformState transform)
    {
        return TransformStates.Remove(transform);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        ((PlayScene)Manager.Scene).SetTransformInfo(BaseTransform.Position.X,
                                                    BaseTransform.Position.Y,
                                                    BaseTransform.Width,
                                                    BaseTransform.Height,
                                                    BaseTransform.Angle
            );
    }
}
