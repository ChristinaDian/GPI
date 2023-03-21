using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
	/// <summary>
	/// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
	/// </summary>
	public class EllipseShape : Shape
	{
		#region Constructor
		
		public EllipseShape(RectangleF rect) : base(rect)
		{
		}
		
		public EllipseShape(EllipseShape ellipse) : base(ellipse)
		{
		}
		
		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			double x = Rectangle.X + 50;
			double y = Rectangle.Y + 100;
			double rx = 50;
			double ry = 100;
			double px = point.X;
			double py = point.Y;

			if (Math.Pow(px - x, 2) / Math.Pow(rx, 2) + Math.Pow(py - y, 2) / Math.Pow(ry, 2) <= 1)
				
				// Проверка дали е в обекта само, ако точката е в обхващащата елипса
				return true;
			else
				// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
				return false;
		}
		
		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);
			GraphicsState state = grfx.Save();
			Matrix m = grfx.Transform.Clone();
			m.Multiply(TransformationMatrix);

			grfx.Transform = m;
			
			
			grfx.FillEllipse(new SolidBrush(FillColor),Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(new Pen(StrokeColor),Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

			grfx.Restore(state);
		}
	}
}
