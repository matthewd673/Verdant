using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Verdant;
using Verdant.UI;

namespace TransformDemo;

public class PlayScene : Scene
{
    private Crate crate;

    private TransformState addPosition = new(TransformStateBlendMode.Add)
    {
        Position = new(10, 10),
    };
    private TransformState addScale = new(TransformStateBlendMode.Add)
    {
        Width = 10,
        Height = 10,
    };
    private TransformState addAngle = new(TransformStateBlendMode.Add)
    {
        Angle = 1f,
    };
    private TransformState mulPosition = new(TransformStateBlendMode.Multiply)
    {
        Position = new(2, 2),
    };
    private TransformState mulScale = new(TransformStateBlendMode.Multiply)
    {
        Width = 2,
        Height = 2,
    };
    private TransformState mulAngle = new(TransformStateBlendMode.Multiply)
    {
        Angle = 0.5f,
    };

    private UILabel positionLabel;
    private UILabel scaleLabel;
    private UILabel transformPositionLabel;
    private UILabel transformScaleLabel;
    private UILabel transformAngleLabel;
    private UILabel cameraLabel;

    public PlayScene() : base("play")
    {
        // Empty
    }

    private void InitializeUI()
    {
        UIStack stack = new(new Vec2())
        {
            Gap = 1,
        };

        positionLabel = new(new Vec2(), Resources.DebugFont, "Position");
        stack.AddElement(positionLabel);
        scaleLabel = new(new Vec2(), Resources.DebugFont, "Scale");
        stack.AddElement(scaleLabel);
        transformPositionLabel = new(new Vec2(), Resources.DebugFont, "Transform Position");
        stack.AddElement(transformPositionLabel);
        transformScaleLabel = new(new Vec2(), Resources.DebugFont, "Transform Scale");
        stack.AddElement(transformScaleLabel);
        transformAngleLabel = new(new Vec2(), Resources.DebugFont, "Transform Angle");
        stack.AddElement(transformAngleLabel);
        cameraLabel = new(new Vec2(), Resources.DebugFont, "Camera Position");
        stack.AddElement(cameraLabel);

        UIManager.AddElement(stack);
    }

    public override void Initialize()
    {
        base.Initialize();

        crate = new(new Vec2(200, 200));
        EntityManager.AddEntity(crate);

        InitializeUI();
    }

    private void ToggleTransform(TransformState transform)
    {
        if (!crate.RemoveTransform(transform))
        {
            crate.AddTransform(transform);
        }
    }

    private void UpdateTransformToggles()
    {
        if (InputHandler.IsKeyFirstPressed(Keys.D1))
        {
            ToggleTransform(addPosition);
        }
        if (InputHandler.IsKeyFirstPressed(Keys.D2))
        {
            ToggleTransform(addScale);
        }
        if (InputHandler.IsKeyFirstPressed(Keys.D3))
        {
            ToggleTransform(addAngle);
        }
        if (InputHandler.IsKeyFirstPressed(Keys.D4))
        {
            ToggleTransform(mulPosition);
        }
        if (InputHandler.IsKeyFirstPressed(Keys.D5))
        {
            ToggleTransform(mulScale);
        }
        if (InputHandler.IsKeyFirstPressed(Keys.D6))
        {
            ToggleTransform(mulAngle);
        }
    }

    private void UpdateUILabels()
    {
        positionLabel.Text = $"Position: {crate.Position}";
        scaleLabel.Text = $"Scale: ({crate.Width}, {crate.Height})";

        cameraLabel.Text = $"Camera Position: ({Camera.Position.X}, {Camera.Position.Y})";
    }

    public void SetTransformInfo(float x, float y,
                                 float width, float height,
                                 float angle
            )
    {
        transformPositionLabel.Text = $"Transform Position: ({x}, {y})";
        transformScaleLabel.Text = $"Transform Scale: ({width}, {height})";
        transformAngleLabel.Text = $"Transform Angle: {angle}";
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // allow user to interactively toggle transforms
        UpdateTransformToggles();

        // handle camera movement
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

        // update ui
        UpdateUILabels();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // draw debug lines
        Renderer.DrawLine(spriteBatch,
                          new Vec2(0, crate.Position.Y),
                          new Vec2(crate.Position.X, crate.Position.Y),
                          Color.Magenta
                );
        Renderer.DrawLine(spriteBatch,
                          new Vec2(crate.Position.X, 0),
                          new Vec2(crate.Position.Y, crate.Position.Y),
                          Color.Magenta
                );
    }
}
