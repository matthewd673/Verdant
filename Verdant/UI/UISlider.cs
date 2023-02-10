using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    public class UISlider : UIElement
    {

        // Indicates if the mouse is hovering on the slider's indicator.
        public bool Hovered { get; private set; } = false;
        // Indicates if the mouse is grabbing the slider's indicator.
        public bool Grabbed { get; private set; } = false;

        // The position of the indicator relative to the slider bar.
        public Vec2 IndicatorPosition { get; private set; }
        private int indicatorWidth;
        private int indicatorHeight;

        private int barWidth;
        private int barHeight;

        private float dragOffsetX = 0;

        // The minimum value represented by the slider.
        public float MinValue { get; }
        // The maximum value represented by the slider.
        public float MaxValue { get; }
        private float valueWidth;

        private float _value;

        // The current value of the slider. Setting this value will update the position of the indicator accordingly.
        public float Value
        {
            get { return _value; }
            set
            {
                _value = value;
                
                if (_value < MinValue)
                    _value = MinValue;
                if (_value > MaxValue)
                    _value = MaxValue;

                IndicatorPosition.X = ((_value-MinValue)/valueWidth) * (barWidth);
            }
        }

        // The visual offset of the indicator from its true position on the bar.
        public float IndicatorDrawOffsetX { get; set; }
        // The visual offset of the indicator from the center of the bar.
        public float IndicatorDrawOffsetY { get; set; }

        // The RenderObject to use when drawing the indicator.
        public RenderObject IndicatorSprite { get; set; }
        // The RenderObject to use when drawing the bar.
        public RenderObject BarSprite { get; set; }

        /// <summary>
        /// Initialize a new UISlider.
        /// </summary>
        /// <param name="indicatorSprite">The RenderObject to use when drawing the slider indicator.</param>
        /// <param name="barSprite">The RenderObject to use when drawing the slider bar. The RenderObject will be stretched horizontally to match the bar's width.</param>
        /// <param name="position">The position of the slider (top-left of slider bar).</param>
        /// <param name="minValue">The minimum value of the slider.</param>
        /// <param name="maxValue">The maximum value of the slider.</param>
        /// <param name="barWidth">The visual width of the slider bar.</param>
        public UISlider(Vec2 position, int minValue, int maxValue, RenderObject indicatorSprite, RenderObject barSprite, int barWidth)
            : base(position, Math.Max(barWidth, indicatorSprite.Width), Math.Max(barSprite.Height, indicatorSprite.Height))
        {
            IndicatorPosition = Vec2.Zero;
            indicatorWidth = indicatorSprite.Width;
            indicatorHeight = indicatorSprite.Height;

            IndicatorPosition.Y = -(indicatorHeight / 2);

            // visually this feels more right
            IndicatorDrawOffsetX = -indicatorWidth / 2;

            this.barWidth = barWidth;
            barHeight = barSprite.Height;

            IndicatorSprite = indicatorSprite;
            BarSprite = barSprite;

            MinValue = minValue;
            MaxValue = maxValue;
            valueWidth = (MaxValue - MinValue);
        }

        public override void Update()
        {
            // check for hover
            if (GameMath.CheckPointOnRectIntersection(
                (Vec2)InputHandler.MousePosition,
                (IndicatorPosition.X + AbsolutePosition.X + IndicatorDrawOffsetX) * Renderer.Scale,
                (IndicatorPosition.Y + AbsolutePosition.Y + IndicatorDrawOffsetY) * Renderer.Scale,
                indicatorWidth * Renderer.Scale,
                indicatorHeight * Renderer.Scale
                ))
            { // indicator is being hovered
                if (!Hovered)
                    OnHover();
                Hovered = true;
            }
            else // indicator is no longer hovered
            {
                if (Hovered)
                    OnHoverExit();
                Hovered = false;
            }

            // check for grab start
            if (Hovered && InputHandler.IsMouseFirstPressed())
            {
                OnGrab();
                Grabbed = true;

                // prepare for dragging
                dragOffsetX = IndicatorPosition.X - InputHandler.MousePosition.X;
            }
            // check for grab end
            if (Grabbed && InputHandler.IsMouseFirstReleased())
            {
                OnDrop();
                Grabbed = false;
            }

            // support drag
            if (Grabbed)
            {
                IndicatorPosition.X = InputHandler.MousePosition.X + dragOffsetX;
                if (IndicatorPosition.X < 0)
                    IndicatorPosition.X = 0;
                if (IndicatorPosition.X > barWidth)
                    IndicatorPosition.X = barWidth;

                _value = ((IndicatorPosition.X / barWidth) * (valueWidth)) + MinValue;
            }
        }

        public event EventHandler Hover;
        protected virtual void OnHover()
        {
            Hover?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler HoverExit;
        protected virtual void OnHoverExit()
        {
            HoverExit?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Grab;
        protected virtual void OnGrab()
        {
            Grab?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Drop;
        protected virtual void OnDrop()
        {
            Drop?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BarSprite.Draw(spriteBatch,
                           new Rectangle(
                               (int)(Position.X * Renderer.Scale),
                               (int)(Position.Y * Renderer.Scale),
                               barWidth * Renderer.Scale,
                               barHeight * Renderer.Scale)
                           );

            IndicatorSprite.Draw(spriteBatch,
                                 new Rectangle(
                                    (int)((IndicatorPosition.X + Position.X + IndicatorDrawOffsetX) * Renderer.Scale),
                                    (int)((IndicatorPosition.Y + Position.Y + IndicatorDrawOffsetY) * Renderer.Scale),
                                    indicatorWidth * Renderer.Scale,
                                    indicatorHeight * Renderer.Scale)
                                 );
        }

    }
}
