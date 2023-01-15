﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UIElement
    {

        // The UIManager managing the element.
        public UIManager Manager { get; set; }

        // The position of the UIElement in screen space.
        public Vec2 Position { get; set; }

        // The width of the UIElement in screen space.
        public float Width { get; set; }
        // The height of the UIElement in screen space.
        public float Height { get; set; }

        // Determines if the UIElement will be removed at the end of the update loop.
        public bool ForRemoval { get; set; }

        /// <summary>
        /// Initialize a new UIElement.
        /// </summary>
        /// <param name="position">The position of the UIElement.</param>
        /// <param name="width">The width of the UIElement.</param>
        /// <param name="height">The height of the UIElement.</param>
        public UIElement(Vec2 position, float width, float height)
        {
            Position = position;
            Width = width;
            Height = height;
        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void DrawBounds(SpriteBatch spriteBatch)
        {
            Renderer.DrawRectangle(spriteBatch,
                                   Position * Renderer.Scale,
                                   (Position + new Vec2(Width, Height)) * Renderer.Scale,
                                   Color.Pink
                                   );
        }
    }
}
