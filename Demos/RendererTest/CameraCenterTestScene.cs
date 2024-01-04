using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Verdant;
using Verdant.UI;

namespace RendererTest;

public class CameraCenterTestScene : Scene
{
    private UILabel positionLabel;
    private UILabel cellLabel;

    public CameraCenterTestScene()
        : base("camera_center_test")
    {
        // Empty
    }

    private void InitializeUI()
    {
        UIStack stack = new(new Vec2(4, 4))
        {
            Gap = 4,
        };

        positionLabel = new(
            new Vec2(), Resources.Font, "Camera Position");
        stack.AddElement(positionLabel);

        cellLabel = new(
            new Vec2(), Resources.Font, "Camera Cell");
        stack.AddElement(cellLabel);

        UIManager.AddElement(stack);
    }

    public override void Initialize()
    {
        base.Initialize();

        InitializeUI();

        // add crate at (0, 0)
        EntityManager.AddEntity(new Crate(new Vec2()));
    }

    private void UpdateLabels()
    {
        positionLabel.Text = $"Camera Position: {Camera.Position}";
        cellLabel.Text = $"Camera Cell: {Camera.Key}";
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (InputHandler.KeyboardState.IsKeyDown(Keys.W))
        {
            Camera.Position.Y -= 1;
        }
        if (InputHandler.KeyboardState.IsKeyDown(Keys.S))
        {
            Camera.Position.Y += 1;
        }
        if (InputHandler.KeyboardState.IsKeyDown(Keys.A))
        {
            Camera.Position.X -= 1;
        }
        if (InputHandler.KeyboardState.IsKeyDown(Keys.D))
        {
            Camera.Position.X += 1;
        }

        UpdateLabels();
    }
}
