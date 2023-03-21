﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
	/// </summary>
	[Serializable]
	public abstract class Shape
	{
		#region Constructors
		
		public Shape()
		{
		}

		public Shape(RectangleF rect)
		{
			rectangle = rect;
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.Location = shape.Location;
			this.rectangle = shape.rectangle;
			
			this.strokeColor = shape.strokeColor;	
			this.FillColor =  shape.FillColor;
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Обхващащ правоъгълник на елемента.
		/// </summary>
		private RectangleF rectangle;		
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}
		
		/// <summary>
		/// Широчина на елемента.
		/// </summary>
		public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		private float shapeAngle;

		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location {
			get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}
		public virtual float ShapeAngle
		{
			get { return shapeAngle; }
			set { shapeAngle = value; }
		}
        public virtual void RotateShape(Graphics grfx)
        {
			grfx.TranslateTransform(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);
			grfx.RotateTransform(ShapeAngle);

			grfx.TranslateTransform(-(Rectangle.X + Rectangle.Width / 2), -(Rectangle.Y + Rectangle.Height / 2));
		}

        /// <summary>
        /// Цвят на елемента.
        /// </summary>
        private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}
		private Color strokeColor;
		public virtual Color StrokeColor
		{
			get { return strokeColor; }
			set { strokeColor = value; }
		}
		/*private Matrix transformationMatrix = new Matrix();
		public virtual Matrix TransformationMatrix
		{
			get { return transformationMatrix; }
			set { transformationMatrix = value; }
		}*/

		private Color transColor;
		public virtual Color TransColor
		{
			get { return transColor; }
			set { transColor = value; }
		}
		#endregion


		/// <summary>
		/// Проверка дали точка point принадлежи на елемента.
		/// </summary>
		/// <param name="point">Точка</param>
		/// <returns>Връща true, ако точката принадлежи на елемента и
		/// false, ако не пренадлежи</returns>
		/// 

		public string WhatIsMyShape(string myShape)
		{
			return myShape;
		}
		public virtual bool Contains(PointF point)
		{
			return Rectangle.Contains(point.X, point.Y);
		}
		public virtual void GroupeMove(float dx, float dy)
		{
			Location = new PointF(Location.X + dx, Location.Y + dy);
		}
		public virtual void GroupRotate(float angle)
		{
			ShapeAngle = angle;
		}
		public virtual void GroupFillColor(Color color)
		{
			fillColor = color;
		}
		/// <summary>
		/// Визуализира елемента.
		/// </summary>
		/// <param name="grfx">Къде да бъде визуализиран елемента.</param>
		public virtual void DrawSelf(Graphics grfx)
		{
			// shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
		}		
	}
}
