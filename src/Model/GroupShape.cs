using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
	[Serializable]
	public class GroupShape :Shape
    {
		#region Constructor

		public GroupShape(RectangleF rect) : base(rect)
		{
		}

		public GroupShape(RectangleShape rectangle) : base(rectangle)
		{
		}
		public List<Shape> SubShapes = new List<Shape>();
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

			foreach (Shape shape in SubShapes)
			{
				if (shape.Contains(point))
					//foreach item in SubShape check if item Contains returns true
					return true;
				else
					// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
					return false;
			}
			return false;
		}
		public override void GroupeMove(float dx, float dy)
		{
			base.GroupeMove(dx, dy);
			foreach (var shape in SubShapes)
			{
				shape.GroupeMove(dx * 2, dy * 2);
			}
		}
		public override void GroupFillColor(Color color)
		{
			base.GroupFillColor(color);
			foreach (var shape in SubShapes)
			{
				shape.FillColor = color;
			}
		}
		public override void GroupRotate(float angle)
		{
			base.GroupRotate(angle);
			foreach (var shape in SubShapes)
			{
				shape.ShapeAngle = angle;
			}
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);

			foreach(Shape shape in SubShapes)
            {
				shape.DrawSelf(grfx);
				grfx.ResetTransform();
            }
			
			
		}
        public override PointF Location { get => base.Location; set => base.Location = value; }
    }
}
