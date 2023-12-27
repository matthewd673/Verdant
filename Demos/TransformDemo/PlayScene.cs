using Microsoft.Xna.Framework.Graphics;

using Verdant;

namespace TransformDemo;

public class PlayScene : Scene
{
    public PlayScene() : base("play")
    {
        // Empty
    }

    public override void Initialize()
    {
        base.Initialize();

        Crate crate = new(new Vec2(200, 200));
        EntityManager.AddEntity(crate);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        Verdant.Debugging.SimpleStats.Draw(this, spriteBatch, Resources.DebugFont);
    }
}
