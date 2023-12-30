using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Verdant;
using Verdant.UI;

namespace RendererTest;

public class MenuScene : Scene
{
    public MenuScene()
        : base("menu")
    {
        // Empty
    }

    public override void Initialize()
    {
        base.Initialize();

        UIStack optionsUIStack = new(new Vec2(12, 12))
        {
            Gap = 4,
        };

        UILabel label1 = new(new Vec2(), Resources.Font, "[1]: ZIndex test");
        optionsUIStack.AddElement(label1);

        UIManager.AddElement(optionsUIStack);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (InputHandler.IsKeyFirstPressed(Keys.D1))
        {
            Manager.ActiveID = "zindex_test";
        }
    }
}
