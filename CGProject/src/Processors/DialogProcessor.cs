using System;
using System.Drawing;
using System.Collections.Generic;
using Draw.src.Model;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		
		#endregion
		
		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;
			

			ShapeList.Add(rect);
		}
		public void AddRandomEllipse()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
			ellipse.TransformationMatrix.RotateAt(45, new PointF(ellipse.Rectangle.X + ellipse.Width/2,
																 ellipse.Rectangle.Y + ellipse.Height/2));
			ellipse.FillColor = Color.FromArgb(126, 36, 252, 3);
			ellipse.StrokeColor = Color.Blue;

			ShapeList.Add(ellipse);

		}
		public void AddRandomLine()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			LineShape line = new LineShape(new Rectangle(x, y, 100, 2));
			line.FillColor = Color.Blue;
			ShapeList.Add(line);
		}
		public void AddRandomTriangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 100, 100));
			triangle.FillColor = Color.Blue;
			ShapeList.Add(triangle);
		}
		public void GroupShapes()
        {
			float minX = float.PositiveInfinity;
			float minY = float.PositiveInfinity;
			float maxX = float.NegativeInfinity;
			float maxY = float.NegativeInfinity;

			foreach(Shape shape in ShapeList)
            {
				if(minX>shape.Rectangle.X) minX = shape.Rectangle.X;
				if(minY>shape.Rectangle.Y) minY = shape.Rectangle.Y;
				if(maxX<shape.Rectangle.X+ shape.Width) maxX = shape.Rectangle.X+shape.Width;
				if(maxY<shape.Rectangle.Y+shape.Height) maxY = shape.Rectangle.Y+shape.Height;
            }
        }

		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--)
			{
				if (ShapeList[i].Contains(point)){
					ShapeList[i].FillColor = Color.FromArgb(150, 200, 0, 0);

					return ShapeList[i];
				}	
			}
			return null;
		}
		
		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			if (selection.Count>0) {
				foreach (Shape item in Selection)
						item.Location = new PointF(
						  item.Location.X + p.X - lastLocation.X,
						  item.Location.Y + p.Y - lastLocation.Y);
						lastLocation = p;			
			}
		}
        public override void DrawShape(Graphics grfx, Shape item)
        {
            base.DrawShape(grfx, item);
			if(Selection.Contains(item))
				grfx.DrawRectangle(new Pen(Color.Gray), item.Location.X-3, item.Location.Y-3, item.Width + 6, item.Height + 6);
		}
	}
}
