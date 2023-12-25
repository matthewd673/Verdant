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

        Crate crate = new(new Vec2(0, 0));
        EntityManager.AddEntity(crate);
    }
}
