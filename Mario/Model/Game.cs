using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Mario.Model
{
	public class Game
	{
		private bool _isRunning;
		private readonly object _mutex = new object();
		private readonly Timer _timer = new Timer(50);
		public GamePlayerEntity Player { get; init; }

		public List<GameEntity> Entities { get; } = new List<GameEntity>();
		public bool IsInDesigner { get; set; }

		public Game()
		{
			Player = new GamePlayerEntity(this);
			Player.StartX = 200;
			Player.StartY = 768;
			Entities.Add(Player);
			_timer.Elapsed += OnDrawFrame;
			_timer.Start();
		}

		private void OnDrawFrame(object sender, ElapsedEventArgs e)
		{
			if (Application.Current?.Dispatcher != null)
				Application.Current.Dispatcher.Invoke(() =>
			{
				lock (_mutex)
				{
					if (_isRunning)
						ProcessAnimation();
				}
			});
		}

		private void ProcessAnimation()
		{
			Entities.ForEach(o => o.Animate());
			// todo scroll window when needed
			//if (Player.X > 600)
			//{
			int offset = (int)(Player.X - (1680 / 2));
			_levelScrollViewer.ScrollToHorizontalOffset(offset);
			//}
		}

		public void Start()
		{
			lock (_mutex)
			{
				_isRunning = true;
			}
		}

		public void Stop()
		{
			lock (_mutex)
			{
				_isRunning = true;
			}
		}

		public void HandleInputCommand(InputCommand command, bool ispressed)
		{
			Player.SetKeyPressState(command, ispressed);

		}

		private ScrollViewer _levelScrollViewer;
		public void SetScrollViewer(ScrollViewer levelScrollViewer)
		{
			_levelScrollViewer = levelScrollViewer;
		}
	}
}