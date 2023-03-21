using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    public class TriangleShape : Shape
    {
		#region Constructor

		public TriangleShape(RectangleF rect) : base(rect)
		{
		}

		public TriangleShape(TriangleShape triangle) : base(triangle)
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
			if (base.Contains(point))
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true
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
			Random rnd = new Random();
			int p1 = (int)Rectangle.X;
			int p2 = (int)Rectangle.Y;
			Point point1 = new Point(p1,p2);
			Point point2 = new Point(p1+100,p2);
			Point point3 = new Point(p1+50,p2-100);
			Point[] curvePoints =
					 {
				 point1,
				 point2,
				 point3
				
			 };
			grfx.FillPolygon(new SolidBrush(FillColor), curvePoints);
			grfx.DrawPolygon(Pens.Black, curvePoints);

		}
	}
}
