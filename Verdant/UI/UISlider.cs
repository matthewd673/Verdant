using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Verdant.UI
{
    /// <summary>
    /// A UIElement that accepts numeric input within a given range.
    /// </summary>
    public class UISlider : UIElement
    {

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

                OnChanged();
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
        public UISlider(Vec2 position, float minValue, float maxValue, RenderObject indicatorSprite, RenderObject barSprite, int barWidth)
            : base(position, Math.Max(barWidth, indicatorSprite.Width), Math.Max(barSprite.Height, indicatorSprite.Height))
        {
            IndicatorPosition = new Vec2(0, 0);
            indicatorWidth = indicatorSprite.Width;
            indicatorHeight = indicatorSprite.Height;

            IndicatorDrawOffsetY = -(indicatorHeight / 2);

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
            // check for grab start
            if (Hovered && InputHandler.IsMouseFirstPressed())
            {
                OnGrabBegin();
                Grabbed = true;

                // prepare for dragging
                dragOffsetX = IndicatorPosition.X - InputHandler.MousePosition.X;
            }
            // check for grab end
            if (Grabbed && InputHandler.IsMouseFirstReleased())
            {
                OnGrabEnd();
                Grabbed = false;
            }

            // support drag
            if (Grabbed)
            {
                IndicatorPosition.X = InputHandler.MousePosition.X + dragOffsetX;
                Value = ((IndicatorPosition.X / barWidth) * (valueWidth)) + MinValue;
            }
        }

        protected virtual void OnGrabBegin() { }

        protected virtual void OnGrabEnd() { }

        protected virtual void OnChanged() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BarSprite.Draw(spriteBatch,
                           new Rectangle(
                               (int)(AbsoluteElementPosition.X * Renderer.UIScale),
                               (int)(AbsoluteElementPosition.Y - IndicatorDrawOffsetY * Renderer.UIScale),
                               barWidth * Renderer.UIScale,
                               barHeight * Renderer.UIScale)
                           );

            IndicatorSprite.Draw(spriteBatch,
                                 new Rectangle(
                                    (int)((IndicatorPosition.X + AbsoluteElementPosition.X + IndicatorDrawOffsetX) * Renderer.UIScale),
                                    (int)((IndicatorPosition.Y + AbsoluteElementPosition.Y) * Renderer.UIScale),
                                    indicatorWidth * Renderer.UIScale,
                                    indicatorHeight * Renderer.UIScale)
                                 );
        }

    }
}
