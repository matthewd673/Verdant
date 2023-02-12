using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant
{
    /// <summary>
    /// An object within the game world, to be extended for every type of object in the game.
    /// </summary>
    public class Entity
    {
        EntityManager _manager;

        // The EntityManager that manages this Entity.
        public EntityManager Manager
        {
            get { return _manager; }
            set
            {
                _manager = value;
                if (_manager != null)
                    Key = _manager.GetKeyFromPos(Position); //set initial key if manager isn't null
            }
        }

        // The key of the Entity within the manager's hash table.
        public string Key { get; private set; }
        // The key of the Entity at the end of the last update.
        public string PreviousKey { get; private set; } = "";

        // The RenderObject used to draw this Entity.
        public RenderObject Sprite { get; set; }

        // The position (center) of the Entity.
        public virtual Vec2 Position { get; set; }

        private int _width;
        // The draw width of the Entity.
        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                HalfWidth = value / 2;
            }
        }
        private int _height;
        // The draw height of the Entity.
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                HalfHeight = value / 2;
            }
        }
        protected int HalfWidth { get; private set; }
        protected int HalfHeight { get; private set; }

        // The method by which to update the ZIndex.
        public EntityManager.ZIndexMode ZIndexMode { get; set; } = EntityManager.ZIndexMode.ByIndex;

        // The z-index, used for sorting and depth-based rendering.
        public int ZIndex { get; set; } = 0;

        // A TransformState that is automatically applied to the Entity when rendering.
        public TransformState TransformState { get; set; }

        // Determines if the Entity should be removed at the end of the
        // next update loop.
        public bool ForRemoval { get; set; } = false;

        /// <summary>
        /// Initialize a new Entity.
        /// </summary>
        /// <param name="sprite">The Entity's sprite.</param>
        /// <param name="position">The position of the center of the Entity.</param>
        /// <param name="width">The width of the Entity. Defaults to the width of the RenderObject.</param>
        /// <param name="height">The height of the Entity. Defaults to the height of the RenderObject.</param>
        public Entity(RenderObject sprite, Vec2 position, int width = -1, int height = -1)
        {
            if (sprite != RenderObject.None)
            {
                // if the sprite is an animation, copy it automatically
                if (sprite.GetType() == typeof(Animation) || sprite.GetType().IsSubclassOf(typeof(Animation)))
                {
                    Sprite = ((Animation)sprite).Copy();
                }
                else
                {
                    Sprite = sprite;
                }
            }
            Position = position;

            Width = (width == -1 && sprite != RenderObject.None) ? sprite.Width : width;
            Height = (height == -1 && sprite != RenderObject.None) ? sprite.Height : height;
        }

        /// <summary>
        /// Perform the Entity's basic update actions - a good place to look for input events. Called in the EntityManager update loop.
        /// </summary>
        public virtual void Update()
        {
            // update key
            if (Manager != null) //only if a managed entity (not Particles, for example)
            {
                PreviousKey = Key;
                Key = Manager.GetKeyFromPos(Position);
            }

            // update z index
            if (ZIndexMode == EntityManager.ZIndexMode.Bottom)
                ZIndex = (int)(Position.Y + Height);
            else if (ZIndexMode == EntityManager.ZIndexMode.Top)
                ZIndex = (int)(Position.Y);
        }

        /// <summary>
        /// Draw the Entity.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite == RenderObject.None)
            {
                return;
            }

            if (TransformState == null)
            {
                Sprite.Draw(spriteBatch,
                    Manager.Scene.Camera.GetRenderBounds(
                        Position.X - HalfWidth,
                        Position.Y - HalfHeight,
                        Width,
                        Height
                        ));
            }
            else
            {
                if (TransformState.Multiply)
                {
                    Sprite.Draw(spriteBatch,
                        Manager.Scene.Camera.GetRenderBounds(
                            (Position.X - (Width * TransformState.Width / 2)) * TransformState.Position.X,
                            (Position.Y - (Height * TransformState.Height / 2)) * TransformState.Position.Y,
                            (int)(Width * TransformState.Width),
                            (int)(Height * TransformState.Height)
                            ),
                        0, // Entities do not have angles, so it'll always multiply by 0
                        new Vector2(
                            Sprite.Width * TransformState.Width / 2,
                            Sprite.Height * TransformState.Height / 2
                            ));
                }
                else
                {
                    Sprite.Draw(spriteBatch,
                        Manager.Scene.Camera.GetRenderBounds(
                            (TransformState.Width / 2f) + TransformState.Position.X,
                            (TransformState.Height / 2f) + TransformState.Position.Y,
                            (int)TransformState.Width,
                            (int)TransformState.Height
                            ),
                        TransformState.Angle, // even though Entities don't have angles, this is an absolute change so it gets an angle
                        new Vector2(
                            TransformState.Width / 2,
                            TransformState.Height / 2
                            ));
                }
            }
        }

        /// <summary>
        /// Check if this Entity is of a given type.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>True if the Entity is of the given type or is a subclass of the given type.</returns>
        public bool IsType(Type t)
        {
            return GetType() == t || GetType().IsSubclassOf(t);
        }

    }
}
