using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mario
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		private void UserControl_MouseMove(object sender, MouseEventArgs e)
		{
			FrameworkElement ellipse = sender as FrameworkElement;
			if (ellipse != null && e.LeftButton == MouseButtonState.Pressed)
			{
				DragDrop.DoDragDrop(ellipse, "test", DragDropEffects.Move);
			}
		}

		private void UserControl_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{

		}

		private void UserControl_DragEnter(object sender, DragEventArgs e)
		{

		}

		private void UserControl_DragOver(object sender, DragEventArgs e)
		{
			//if (!e.Data.GetDataPresent(typeof(MyDataType)))
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
			}
		}

		private void UserControl_Drop(object sender, DragEventArgs e)
		{
			//if (e.Data.GetDataPresent(typeof(MyDataType)))
			{
				// do whatever you want do with the dropped element
				//MyDataType droppedThingie = e.Data.GetData(typeof(MyDataType)) as MyDataType;
			}
		}

		private void UserControl_DragLeave(object sender, DragEventArgs e)
		{

		}
	}
}
