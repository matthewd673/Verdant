using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant;

/// <summary>
/// An object within the game world, to be extended for every unique object in the game.
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
            UpdateKey();
        }
    }

    // The key of the Entity within the manager's hash table.
    public Vec2Int Key { get; internal set; } = new();
    // The key of the Entity at the end of the last update.
    public Vec2Int PreviousKey { get; private set; } = new();

    // The RenderObject used to draw this Entity.
    public RenderObject Sprite { get; set; }

    // The position (center) of the Entity.
    public virtual Vec2 Position { get; set; }

    private float _width;
    // The draw width of the Entity.
    public float Width
    {
        get { return _width; }
        set
        {
            _width = value;
            HalfWidth = value / 2;
        }
    }
    private float _height;
    // The draw height of the Entity.
    public float Height
    {
        get { return _height; }
        set
        {
            _height = value;
            HalfHeight = value / 2;
        }
    }
    protected float HalfWidth { get; private set; }
    protected float HalfHeight { get; private set; }

    // The method by which to update the ZIndex.
    public ZIndexMode ZIndexMode { get; set; } = ZIndexMode.ByIndex;

    // The z-index, used for sorting and depth-based rendering.
    public int ZIndex { get; set; } = 0;

    protected Transform BaseTransform { get; private set; } = new(TransformBlendMode.Override);
    // A sequence of TransformStates that will be applied when rendering.
    // TransformStates are applied from first to last in the array with
    // no other order of operations.
    protected List<Transform> TransformStates { get; private set; } = new();

    // Determines if the Entity should be removed at the end of the
    // next update loop.
    public bool ForRemoval { get; set; } = false;

    public Entity(Vec2 position)
    {
        Position = position;
        Sprite = RenderObject.None;
        Width = 0;
        Height = 0;
    }

    /// <summary>
    /// Initialize a new Entity.
    /// </summary>
    /// <param name="sprite">The Entity's sprite.</param>
    /// <param name="position">The Entity's position.</param>
    public Entity(Vec2 position, RenderObject sprite)
    {
        if (sprite != RenderObject.None)
        {
            // if the sprite is an animation, copy it automatically
            if (sprite.GetType() == typeof(Animation) ||
                sprite.GetType().IsSubclassOf(typeof(Animation)))
            {
                Sprite = ((Animation)sprite).Copy();
            }
            else
            {
                Sprite = sprite;
            }
        }

        Position = position;
        Width = sprite != RenderObject.None ? sprite.Width : 0;
        Height = sprite != RenderObject.None ? sprite.Height : 0;
    }

    /// <summary>
    /// Initialize a new Entity with a custom width and height.
    /// </summary>
    /// <param name="sprite">The Entity's sprite.</param>
    /// <param name="position">The Entity's position.</param>
    /// <param name="width">The width of the Entity.</param>
    /// <param name="height">The height of the Entity.</param>
    public Entity(Vec2 position, RenderObject sprite, float width, float height)
    {
        if (sprite != RenderObject.None)
        {
            // if the sprite is an animation, copy it automatically
            if (sprite.GetType() == typeof(Animation) ||
                sprite.GetType().IsSubclassOf(typeof(Animation)))
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
    /// Called when the Entity has been added to an EntityManager and is ready to use.
    /// NOTE: This occurs immediately after the Entity has been processed through the add queue.
    /// </summary>
    public virtual void OnAdd()
    {
        // Empty
    }

    /// <summary>
    /// Called when the Entity has been removed from an EntityManager.
    /// NOTE: This occurs immediately after the Entity has been processed through the remove queue.
    /// This will not be called if the Entity is removed from an EntityManager that it wasn't managed by.
    /// </summary>
    public virtual void OnRemove()
    {
        // Empty
    }

    private void UpdateKey()
    {
        if (Manager == null)
        {
            return;
        }

        PreviousKey.X = Key.X;
        PreviousKey.Y = Key.Y;

        Key.X = (int)(Position.X / Manager.CellSize);
        Key.Y = (int)(Position.Y / Manager.CellSize);
    }

    /// <summary>
    /// Perform the Entity's basic update actions - a good place to look for input events. Called in the EntityManager update loop.
    /// </summary>
    public virtual void Update()
    {
        // update key
        UpdateKey();

        // update z index
        if (ZIndexMode == ZIndexMode.Bottom)
        {
            ZIndex = (int)(Position.Y + HalfHeight);
        }
        else if (ZIndexMode == ZIndexMode.Top)
        {
            ZIndex = (int)(Position.Y - HalfHeight);
        }
    }

    protected internal virtual void UpdateBaseTransform()
    {
        BaseTransform.Position.X = Position.X;
        BaseTransform.Position.Y = Position.Y;
        BaseTransform.Width = Width;
        BaseTransform.Height = Height;
        BaseTransform.Angle = 0; // Entities don't have rotation
    }

    /// <summary>
    /// Draw the Entity.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // skip the draw step if there is no Sprite
        if (Sprite == RenderObject.None)
        {
            return;
        }

        // update base TransformState with the Entity's properties
        UpdateBaseTransform();

        // apply all TransformStates onto the base state
        foreach (Transform t in TransformStates)
        {
            BaseTransform.Blend(t);
        }

        // apply camera transform
        Manager.Scene.Camera.BlendOnto(BaseTransform);

        // draw sprite using the final transform
        Sprite.Draw(spriteBatch, BaseTransform);
    }

    /// <summary>
    /// Check if this Entity is a given type or descended from that type.
    /// </summary>
    /// <returns>True if the Entity is of the given type or is a subclass of the given type.</returns>
    public bool IsType<T>()
    {
        return GetType() == typeof(T) || GetType().IsSubclassOf(typeof(T));
    }
}

