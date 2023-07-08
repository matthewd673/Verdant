using System;
namespace Verdant.UI
{
	public class BoxModel
	{
		// The width of the UIElements's content area.
		public virtual float Width { get; set; }
		// The height of the UIElements's content area.
		public virtual float Height { get; set; }

		// The UIElement's padding.
		public BoxDimensions Padding { get; set; }
		// The UIElement's margin.
		public BoxDimensions Margin { get; set; }

		// The visible width of the UIElement (its content area + padding).
		public float ElementWidth
		{
			get { return Width + Padding.Horizontal; }
		}

        // The visible height of the UIElement (its content area + padding).
        public float ElementHeight
		{
			get { return Height + Padding.Vertical; }
		}

		// The total width of the UIElement (its content area + padding + margin).
		public float TotalWidth
		{
			get { return Width + Padding.Horizontal + Margin.Horizontal; }
		}

		// The total height of the UIElement (its content area + padding + margin).
		public float TotalHeight
		{
			get { return Height + Padding.Vertical + Margin.Vertical; }
		}
	}
}

