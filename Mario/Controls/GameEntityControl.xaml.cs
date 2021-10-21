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
using Mario.ViewModel;

namespace Mario.Controls
{
	/// <summary>
	/// Interaction logic for GameEntity.xaml
	/// </summary>
	public partial class GameEntityControl : UserControl
	{
		public GameEntityControl()
		{
			InitializeComponent();
		}
		private Point _positionInBlock;

		private Model.GameEntity Model => DataContext as Model.GameEntity;

		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// when the mouse is down, get the position within the current control. (so the control top/left doesn't move to the mouse position)
			_positionInBlock = Mouse.GetPosition(this);
			var parentDataContext = (VisualParent as Canvas).DataContext as LevelDesignerViewModel;
			if (parentDataContext != null)
			{
				parentDataContext.SelectedEntity = new GameEntityViewModel(DataContext as Model.GameEntity);
			}
			// capture the mouse (so the mouse move events are still triggered (even when the mouse is not above the control)
			this.CaptureMouse();
		}

		private void UserControl_MouseMove(object sender, MouseEventArgs e)
		{
			// if the mouse is captured. you are moving it. (there is your 'real' boolean)
			if (this.IsMouseCaptured)
			{
				// get the parent container
				var container = VisualTreeHelper.GetParent(this) as UIElement;

				if (container == null)
					return;

				// get the position within the container
				var mousePosition = e.GetPosition(container);

				// move the usercontrol.
				var newX = (double)((int)(mousePosition.X - _positionInBlock.X) / 64) * 64;
				var newY = (double)((int)(mousePosition.Y - _positionInBlock.Y) / 64) * 64;
				if (newY > 768)
					newY = 768;
				Console.WriteLine($"position {newX},{newY}");
				this.RenderTransform = new TranslateTransform(newX, newY);
				Model.SetStartPosition(newX, newY);

			}
		}

		private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// release this control.
			this.ReleaseMouseCapture();
		}

	}
}
