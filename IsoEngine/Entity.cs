using System;
using IsoEngine.Physics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace IsoEngine
{

    [Serializable]
    public class Entity : Body
    {
        
        [JsonIgnore]
        EntityManager _manager;
        [JsonIgnore]
        public EntityManager Manager
        {   get { return _manager; }
            set
            {
                _manager = value;
                if (_manager != null)
                    Key = _manager.GetKeyFromPos(Position); //set initial key if manager isn't null
            }
        }
        public string Key { get; private set; }
        public string PreviousKey { get; private set; } = "";
        
        [JsonIgnore]
        public RenderObject Sprite { get; set; }

        private int _width;
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

        public float Rotation { get; set; } = 0f;
        protected Vec2Int RotationOrigin { get; set; } = Vec2Int.Zero;

        protected bool SetZIndexToBase { get; set; }
        public int ZIndex { get; protected set; } = 0;

        public bool ForRemoval { get; set; } = false;

        private float _bodyX;
        private float _bodyY;
        private float _bodyW;
        private float _bodyH;
        private float _bodyM;

        /// <summary>
        /// Initialize a new Entity.
        /// </summary>
        /// <param name="sprite">The Entity's sprite.</param>
        /// <param name="position">The position of the Entity.</param>
        /// <param name="width">The width of the Entity. Defaults to the width of the RenderObject.</param>
        /// <param name="height">The height of the Entity. Defaults to the height of the RenderObject.</param>
        public Entity(RenderObject sprite, Vec2 position, int width, int height, float mass = 1f) :
            base()
        {
            //apply default properties
            SetZIndexToBase = true;

            Sprite = sprite;
            Position = position;
            Width = width;
            Height = height;

            BodyParent = this;

            _bodyX = position.X;
            _bodyY = position.Y;
            _bodyW = width;
            _bodyH = height;
            _bodyM = mass;

            //set automatic rotation origin
            //TODO: when working with textures stretched to different aspect ratios, this will result in an off-center origin
            RotationOrigin = new Vec2Int(Width / 2, Height / 2);

            InitializeBody();
        }

        //by default, entities are boxes (that don't rotate)
        protected virtual void InitializeBody()
        {
            float x1 = _bodyX;
            float y1 = _bodyY;
            float x2 = x1;
            float y2 = y1 + _bodyH;
            float r = _bodyW / 2;

            Vec2 top = new Vec2(x1, y1);
            Vec2 bottom = new Vec2(x2, y2);

            Vec2 recVec1 = bottom + (bottom - top).Unit().Normal() * r;
            Vec2 recVec2 = top + (bottom - top).Unit().Normal() * r;

            Physics.Rectangle rectangle1 = new Physics.Rectangle(recVec1.X, recVec1.Y, recVec2.X, recVec2.Y, 2 * r);
            rectangle1.CalculateVertices();

            Components = new Shape[] { rectangle1 };
            Mass = _bodyM;

            Inertia = Mass * (
                (float)Math.Pow(2 * rectangle1.Width, 2) +
                (float)Math.Pow(Height + 2 * rectangle1.Width, 2)
                ) / 12;
        }

        public override void Move()
        {
            base.Move();

            if (Components[0].GetType() == typeof(Physics.Rectangle))
                ((Physics.Rectangle)Components[0]).CalculateVertices();
        }

        /// <summary>
        /// Perform the Entity's basic update loop: performing physics movement, updating child Colliders, and updating the z-index if appropriate.
        /// </summary>
        public virtual void Update()
        {
            //update key
            if (Manager != null) //only if a managed entity (not Particles, for example)
            {
                PreviousKey = Key;
                Key = Manager.GetKeyFromPos(Position);
            }

            //update z index
            if (SetZIndexToBase)
                ZIndex = (int)(Position.Y + Height);

        }

        /// <summary>
        /// Set the Entity's bounds and rotation to be equal to those of a given TransformState.
        /// </summary>
        /// <param name="state">The TransformState to mirror.</param>
        public void ApplyTransformState(TransformState state)
        {
            Position.X = state.X;
            Position.Y = state.Y;
            Width = (int)state.Width;
            Height = (int)state.Height;
            Rotation = state.Rotation;
        }

        /// <summary>
        /// Perfom a basic render of the Entity.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite == null)
                return;

            //if (Rotation == 0f) //no rotation, simple draw
            //    spriteBatch.Draw(Sprite.Get(), Renderer.Camera.GetRenderBounds(this), Color.White);
            //else
            //{
            //    Microsoft.Xna.Framework.Rectangle renderRect = Renderer.Camera.GetRenderBounds(this);
            //    renderRect.X += RotationOrigin.X * Renderer.Scale;
            //    renderRect.Y += RotationOrigin.Y * Renderer.Scale;
            //    spriteBatch.Draw(Sprite.Get(), renderRect, null, Color.White, Rotation, (Vector2)RotationOrigin * Renderer.Scale, SpriteEffects.None, 0);
            //}

            spriteBatch.Draw(Sprite.Get(),
                Renderer.Camera.GetRenderBounds(Position.X - HalfWidth, Position.Y - HalfHeight, Width, Height), Color.White);

        }

    }
}
