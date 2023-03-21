using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	[Serializable]
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked) {
				Shape temp = dialogProcessor.ContainsPoint(e.Location);
				if (temp != null)
				{
					if (dialogProcessor.Selection.Contains(temp))
                    {
						dialogProcessor.Selection.Remove(temp);
                    }
                    else 
					{ 
						dialogProcessor.Selection.Add(temp);
					}
					
				}
				if (dialogProcessor.Selection != null) {
					statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
					dialogProcessor.IsDragging = true;
					dialogProcessor.LastLocation = e.Location;
					viewPort.Invalidate();
				}
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}
		//Рисуване на елипса
		private void toolStripButton1_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomEllipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

			viewPort.Invalidate();
		}

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {

		}
		//Рисуване на линия
		private void toolStripButton2_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomLine();
			statusBar.Items[0].Text = "Последно действие: Рисуване на линия";

			viewPort.Invalidate();
		}
		//Рисуване на триъгълник
		private void toolStripButton3_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomTriangle();
			statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

			viewPort.Invalidate();
		}
		//Рисуване на правоъгълник
		private void newRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
			//MessageBox.Show("Menu Item Clicked");
			dialogProcessor.AddRandomRectangle();
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

			viewPort.Invalidate();
		}
		//Ctrl+c,Ctrl+v
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.Control && e.KeyCode == Keys.C)
			{
				dialogProcessor.CopyList = dialogProcessor.Selection;
			}else if (e.Control && e.KeyCode == Keys.V)
            {
				foreach (Shape shape in dialogProcessor.CopyList)
                {
					var type = shape.GetType().Name.ToString();
					switch (type)
                    {
						case "RectangleShape": dialogProcessor.AddRandomRectangle(); break;
						case "EllipseShape": dialogProcessor.AddRandomEllipse(); break;
						case "LineShape": dialogProcessor.AddRandomLine(); break;
						case "TriangleShape": dialogProcessor.AddRandomTriangle(); break;
						case "CircleShape": dialogProcessor.AddRandomCircle(); break;
					}
				}		
				viewPort.Invalidate();
			}
		}

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }
		//Завъртане на примитив
		private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
			ToolStripComboBox cmb = sender as ToolStripComboBox;
			int angle = int.Parse(cmb.Text);
		//	MessageBox.Show(cmb.Text);
                if (dialogProcessor.Selection.Count > 0)
                {
				foreach (Shape selectedItem in dialogProcessor.Selection)
				{
					var type = selectedItem.GetType().Name.ToString();
					if (type == "GroupShape")
						selectedItem.GroupRotate(angle);
				}
				dialogProcessor.RotateShape(angle);
				statusBar.Items[0].Text = "Последно действие: Завъртане на примитив.";
				viewPort.Invalidate();
			}
			viewPort.Invalidate();
		}
		//Изтриване на примитив
        private void deleteBtn_Click(object sender, EventArgs e)
        {
			foreach (Shape selectedItem in dialogProcessor.Selection)
			{
				for (int i = dialogProcessor.ShapeList.Count - 1; i >= 0; i--)
				{
					if (dialogProcessor.ShapeList.ElementAt(i) == selectedItem)
					{
						dialogProcessor.ShapeList.Remove(selectedItem);
					}
				}
			}
			dialogProcessor.Selection.Clear();
			statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните елементи";
			viewPort.Invalidate();
		}
		//Оцветяване на примитив
        private void colorBtn_Click(object sender, EventArgs e)
        {
			colorDialog1.ShowDialog();
			foreach (Shape selectedItem in dialogProcessor.Selection)
			{
				var type = selectedItem.GetType().Name.ToString();
				if (type == "GroupShape")
					selectedItem.GroupFillColor(Color.FromArgb(150, colorDialog1.Color));
				selectedItem.FillColor = Color.FromArgb(150, colorDialog1.Color);
				//selectedItem.FillColor = colorDialog1.Color;
			}
			//dialogProcessor.ColoringSelectedShapes();
			statusBar.Items[0].Text = "Последно действие: Запълване на цвета на селектираните елементи";

			viewPort.Invalidate();

		}
		//Запазване на файл като jpeg
		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
			saveFileDialog1.Title = "Save an Image File";
			saveFileDialog1.ShowDialog();
			if (saveFileDialog1.FileName != "")
			{
				// Saves the Image via a FileStream created by the OpenFile method.
				System.IO.FileStream fs =
					(System.IO.FileStream)saveFileDialog1.OpenFile();
				// Saves the Image in the appropriate ImageFormat based upon the
				// File type selected in the dialog box.
				// NOTE that the FilterIndex property is one-based.
				switch (saveFileDialog1.FilterIndex)
				{
					case 1:
						this.saveAsToolStripMenuItem.Image.Save(fs,
						  System.Drawing.Imaging.ImageFormat.Jpeg);
						break;

					case 2:
						this.saveAsToolStripMenuItem.Image.Save(fs,
						  System.Drawing.Imaging.ImageFormat.Bmp);
						break;

					case 3:
						this.saveAsToolStripMenuItem.Image.Save(fs,
						  System.Drawing.Imaging.ImageFormat.Gif);
						break;
				}

				fs.Close();
			}
		}
		//Запазване на файл
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dialogProcessor.WriteShapeListToFile((List<Shape>)dialogProcessor.ShapeList, saveFileDialog1.FileName);
			}
			statusBar.Items[0].Text = "Последно действие: Записване на файл.";
		}
		//Отваряне на файл
        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dialogProcessor.ShapeList = (List<Shape>)dialogProcessor.LoadShapeListFromFile(openFileDialog1.FileName);
				viewPort.Invalidate();
			}
			statusBar.Items[0].Text = "Последно действие: Отваряне на файл.";
		}
		//Рисуване на кръг
		private void toolStripButton4_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomCircle();
			statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";

			viewPort.Invalidate();
		}
		//Селекция на всички примитиви
		private void toolStripLabel1_Click(object sender, EventArgs e)
        {
			dialogProcessor.SelectAllShapes();
			statusBar.Items[0].Text = "Последно действие: Селекция на всички примитиви.";

			viewPort.Invalidate();
		}
		//Групиране на избраните примитиви
		private void toolStripLabel2_Click(object sender, EventArgs e)
        {
			dialogProcessor.GroupShapes();
			statusBar.Items[0].Text = "Последно действие: Групиране на избраните примитиви";
			
			viewPort.Invalidate();
		}
		//Разгрупиране на избраните примитиви
		private void toolStripLabel3_Click(object sender, EventArgs e)
        {
			dialogProcessor.UnGroupShapes();
			statusBar.Items[0].Text = "Последно действие: Разгрупиране на избраните примитиви";
			viewPort.Invalidate();
		}
		//Премахване на селекция от всички примитиви
		private void toolStripLabel4_Click(object sender, EventArgs e)
        {
			dialogProcessor.DisselectAllShapes();
			statusBar.Items[0].Text = "Последно действие: Премахване на селекция от всички примитиви.";

			viewPort.Invalidate();
		}
		//Бутон за изпит
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomTestShape();
			statusBar.Items[0].Text = "Последно действие: Рисуване на примитив за изпит";

			viewPort.Invalidate();
		}
    }
}
