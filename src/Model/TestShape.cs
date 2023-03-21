using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    [Serializable]
    public class TestShape : Shape
    {
        public TestShape(RectangleF rect) : base(rect)
        {

        }
        public TestShape(RectangleShape rectangle) : base(rectangle)
        {

        }
        public override bool Contains(PointF point)
        {
            if ((base.Contains(point) && (((Math.Pow(point.X - (Rectangle.X + Rectangle.Width / 2), 2) / Math.Pow((Rectangle.Width / 2), 2)) + (Math.Pow(point.Y - (Rectangle.Y + Rectangle.Height / 2), 2) / Math.Pow((Rectangle.Height / 2), 2))) <= 1)))
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                return true;
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            base.RotateShape(grfx);
            //grfx.DrawPolygon(new Pen(StrokeColor), Rectangle.X, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height, Rectangle.X - Rectangle.Width, Rectangle.Y - Rectangle.Height);

            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(new Pen(StrokeColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            float x1, y1, x2, y2, x3, y3, x4, y4, r, x0, y0, x5, y5, x6, y6;

            r = Rectangle.Width / 2;
            x0 = Rectangle.X + Rectangle.Width / 2;
            y0 = Rectangle.Y + Rectangle.Height / 2;
            
            //right Line 
            x1 = (float)(x0 + r * Math.Cos(0 * (Math.PI / 180)));
            y1 = (float)(y0 + r * Math.Sin(-90 * (Math.PI / 180)));

            x2 = (float)(x0 + r * Math.Cos(0 * (Math.PI / 180)));
            y2 = (float)(y0 + r * Math.Sin(90 * (Math.PI / 180)));

            //left Line
            x3 = (float)(x0 + r * Math.Cos(180 * (Math.PI / 180)));
            y3 = (float)(y0 + r * Math.Sin(-90 * (Math.PI / 180)));

            x4 = (float)(x0 + r * Math.Cos(180 * (Math.PI / 180)));
            y4 = (float)(y0 + r * Math.Sin(90 * (Math.PI / 180)));

            //middle line
            x5 = (float)(x0 + r * Math.Cos(0 * (Math.PI / 180)));
            y5 = (float)(y0 + r * Math.Sin(0 * (Math.PI / 180)));

            x6 = (float)(x0 + r * Math.Cos(180 * (Math.PI / 180)));
            y6 = (float)(y0 + r * Math.Sin(180 * (Math.PI / 180)));

            grfx.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
            grfx.DrawLine(new Pen(Color.Black), x3, y3, x4, y4);
            grfx.DrawLine(new Pen(Color.Black), x5, y5, x6, y6);
            
            grfx.ResetTransform();
        }
    }
}
