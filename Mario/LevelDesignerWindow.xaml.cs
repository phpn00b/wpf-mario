using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mario.Model;
using Mario.ViewModel;
using Microsoft.Win32;
using ServiceStack;

namespace Mario
{
	/// <summary>
	/// Interaction logic for LevelDesignerWindow.xaml
	/// </summary>
	public partial class LevelDesignerWindow : Window
	{
		public LevelDesignerWindow()
		{
			InitializeComponent();
			LoadFile(@$"{AppDomain.CurrentDomain.BaseDirectory}level-1.json");
			VM._game.SetScrollViewer(Level.ScrollViewer);
		}

		private void LoadFile(string path)
		{
			if (File.Exists(path))
			{
				//ActiveFile = openFileDialog.FileName;
				//LastSaved = new FileInfo(openFileDialog.FileName).LastWriteTime.ToString("G");
				VM.Entities.Clear();
				VM._game.Stop();
				Thread.Sleep(100);
				VM._game.Entities.Clear();
				VM._game.Entities.Add(VM._game.Player);
				var entities = File.ReadAllText(path).FromJson<List<GameEntity.EntityDto>>();
				foreach (var entityDto in entities)
				{
					GameEntity newEntity = null;
					switch (entityDto.EntityType)
					{
						case GameEntityType.Mario:
							// handle player
							break;
						case GameEntityType.Gumba:

						case GameEntityType.Turtle:
							newEntity = new EnemyGameEntity(VM._game, entityDto);
							break;

						default:
							newEntity = new StaticGameEntity(VM._game, entityDto);
							break;
					}

					if (newEntity != null)
					{
						VM.Entities.Add(newEntity.Visual);
						//newEntity.Animate();
						newEntity.UpdatePosition();
					}
				}
				VM.Entities.Add(VM._game.Player.Visual);
				//entities.ForEach(o => VM.Entities.Add(o.Visual));
				Thread.Sleep(100);
				VM._game.Start();
			}

		}

		private LevelDesignerViewModel VM => DataContext as LevelDesignerViewModel;

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DefaultExt = ".json";

			var result = openFileDialog.ShowDialog();
			if (!result ?? false)
				return;
			LoadFile(openFileDialog.FileName);

		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = ".json";
			var result = saveFileDialog.ShowDialog();
			if (!result ?? false)
				return;
			//		if (File.Exists(saveFileDialog.FileName))
			//	{
			var data = VM.Entities.Select(o => (GameEntity.EntityDto)(o.DataContext as GameEntity)).ToArray();
			File.WriteAllText(saveFileDialog.FileName, data.ToJson());
		}

		private void LevelControl_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.IsRepeat)
				return;
			//InputLog.Text += e.Key.ToString() + "\n";
			InputCommand command = InputCommand.None;
			switch (e.Key)
			{
				case Key.A:
				case Key.Left:
					command = InputCommand.Left;
					break;
				case Key.D:
				case Key.Right:
					command = InputCommand.Right;
					break;
				case Key.Up:
				case Key.W:
					command = InputCommand.Up;
					break;
				case Key.S:
				case Key.Down:
					command = InputCommand.Down;
					break;
				case Key.Space:
					command = InputCommand.JumpA;
					break;
				case Key.Enter:
					command = InputCommand.ActionB;
					break;
				case Key.N:
					command = InputCommand.NewGame;
					break;
			}
			if (command != InputCommand.None)
				VM._game.HandleInputCommand(command, true);
		}

		private void LevelDesignerWindow_OnKeyUp(object sender, KeyEventArgs e)
		{
			InputCommand command = InputCommand.None;
			switch (e.Key)
			{
				case Key.A:
				case Key.Left:
					command = InputCommand.Left;
					break;
				case Key.D:
				case Key.Right:
					command = InputCommand.Right;
					break;
				case Key.Up:
				case Key.W:
					command = InputCommand.Up;
					break;
				case Key.S:
				case Key.Down:
					command = InputCommand.Down;
					break;
				case Key.Space:
					command = InputCommand.JumpA;
					break;
				case Key.Enter:
					command = InputCommand.ActionB;
					break;
			}
			if (command != InputCommand.None)
				VM._game.HandleInputCommand(command, false);
		}
	}
}
