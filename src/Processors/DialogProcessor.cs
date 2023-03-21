using System;
using System.Drawing;
using System.Collections.Generic;
using Draw.src.Model;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	[Serializable]
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
			ellipse.FillColor = Color.FromArgb(126, 36, 252, 3);
			ellipse.StrokeColor = Color.Blue;

			ShapeList.Add(ellipse);

		}
		public void AddRandomCircle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			CircleShape circle = new CircleShape(new Rectangle(x, y, 100, 100));
			circle.FillColor = Color.FromArgb(126, 36, 252, 3);
			circle.StrokeColor = Color.Blue;

			ShapeList.Add(circle);

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
				if(minX>shape.Location.X) minX = shape.Location.X;
				if(minY>shape.Location.Y) minY = shape.Location.Y;
				if(maxX<shape.Location.X+ shape.Width) maxX = shape.Location.X+shape.Width;
				if(maxY<shape.Location.Y+shape.Height) maxY = shape.Location.Y+shape.Height;
            }
            GroupShape group = new GroupShape(new RectangleF(minX, minY, maxX-minX, maxY-minY))
            {
                SubShapes = Selection
            };
			Selection = new List<Shape>();
			Selection.Add(group);
			foreach(var item in group.SubShapes)
				ShapeList.Remove(item);
			ShapeList.Add(group);
        }
		public void UnGroupShapes()
		{
			List<Shape> shapesGroup = new List<Shape>();
			foreach (GroupShape groupShape in Selection.ToList())
			{
				foreach (var shape in groupShape.SubShapes)
				{
					shapesGroup.Add(shape);
				}
				groupShape.SubShapes.Clear();
				ShapeList.Remove(groupShape);
				Selection.Remove(groupShape);
				foreach (var shape in shapesGroup)
				{
					Selection.Remove(shape);
					ShapeList.Add(shape);
				}
			}
		}
		public void SelectAllShapes()
		{
			foreach (Shape shape in ShapeList)
			{
				Selection.Add(shape);
			}
		}
		public void DisselectAllShapes()
		{
			foreach (Shape shape in ShapeList)
			{
				Selection.Remove(shape);
			}
		}		
		public void GroupFillColor(Color color)
		{
			foreach (GroupShape shape in Selection)
			{
				shape.GroupFillColor(color);
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
					//ShapeList[i].FillColor = Color.FromArgb(150, 200, 0, 0);

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
				foreach (Shape shape in Selection)
				{
					var type = shape.GetType().Name.ToString();
					if (type.Equals("GroupShape"))
					{
						shape.GroupeMove(p.X - lastLocation.X, p.Y - lastLocation.Y);
					}
					shape.Location = new PointF(shape.Location.X + p.X - lastLocation.X, shape.Location.Y + p.Y - lastLocation.Y);
				}
				lastLocation = p;
			}
		}
        public override void DrawShape(Graphics grfx, Shape item)
        {
            base.DrawShape(grfx, item);
			if (Selection.Contains(item))
				item.StrokeColor = Color.Red;
			else item.StrokeColor = Color.Black;
		}
		public void RotateShape(float rotateAngle)
		{
			if (Selection.Count != 0)
			{
				foreach (var shape in Selection)
				{
						shape.ShapeAngle = rotateAngle;
				}
			}
		}
		public void DeleteSelected()
        {
			foreach (Shape item in Selection)
				ShapeList.Remove(item);
			Selection  = new List<Shape>();
        }
		public void WriteShapeListToFile(object obj, string path = null)
        {
			Stream stream;
			IFormatter formatter = new BinaryFormatter();
			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Create, FileAccess.Write, FileShare.None);
			}
			else
			{
				string preparePath = path + ".asd";
				stream = new FileStream(preparePath, FileMode.Create);

			}
			formatter.Serialize(stream, obj);
			stream.Close();
		}
		public object LoadShapeListFromFile(string path = null)
        {
			object obj;

			Stream stream;
			IFormatter binaryFormatter = new BinaryFormatter();
			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Open);
			}
			else
			{
				stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			}
			obj = binaryFormatter.Deserialize(stream);
			stream.Close();
			return obj;
		}

		public void AddRandomTestShape()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			TestShape testShape = new TestShape(new Rectangle(x, y, 100, 100));
			testShape.FillColor = Color.White;
			testShape.StrokeColor = Color.Black;

			ShapeList.Add(testShape);

		}
	}
}
