using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mario.Controls;
using Mario.Model;

namespace Mario.ViewModel
{
	public class LevelDesignerViewModel : BaseViewModel
	{
		private static readonly Thickness SelectedThickness = new Thickness(2);
		private static readonly Thickness UnselectedThickness = new Thickness(0);

		internal readonly Game _game = new Game();
		public ObservableCollection<GameEntityControl> Entities { get; } = new ObservableCollection<GameEntityControl>();
		public GenericCommand NewLandscapeCommand { get; }

		public GenericCommand NewEnemyCommand { get; }

		public GenericCommand RemoveEntityCommand { get; }

		public LevelDesignerViewModel()
		{
			_game.IsInDesigner = true;
			_game.Player.SetupSprite();
			_game.Player.UpdatePosition();
			Entities.Add(_game.Player.Visual);
			NewLandscapeCommand = new GenericCommand(o => true, NewLandscape);
			NewEnemyCommand = new GenericCommand(o => true, NewEnemy);
			RemoveEntityCommand = new GenericCommand(o => SelectedEntity != null, RemoveSelectedEntity);
			_game.Start();
		}

		private void RemoveSelectedEntity(object obj)
		{
			_game.Entities.Remove(SelectedEntity.Entity);
			Entities.Remove(SelectedEntity.Visual as GameEntityControl);
			SelectedEntity = null;
		}

		private void NewEnemy(object obj)
		{
			var newItem = new EnemyGameEntity(_game);
			Entities.Add(newItem.Visual);
			SelectedEntity = new GameEntityViewModel(newItem);
			_game.Entities.Add(SelectedEntity.Entity);
		}

		private void NewLandscape(object obj)
		{
			var newItem = new StaticGameEntity(_game);
			Entities.Add(newItem.Visual);
			SelectedEntity = new GameEntityViewModel(newItem);
			_game.Entities.Add(SelectedEntity.Entity);
		}

		#region SelectedEntity Property

		private GameEntityViewModel _selectedEntity;

		public GameEntityViewModel SelectedEntity
		{
			get => _selectedEntity;
			set
			{
				if (_selectedEntity != value)
				{
					if (_selectedEntity != null)
					{
						_selectedEntity.Entity.Visual.Border.BorderThickness = UnselectedThickness;
						//_selectedEntity.Entity.Visual.Image.Height += 4;
						//_selectedEntity.Entity.Visual.Image.Width += 4;
					}
					_selectedEntity = value;
					if (value != null)
					{
						_selectedEntity.Entity.Visual.Border.BorderThickness = SelectedThickness;
					}
					OnPropertyChanged("SelectedEntity");
					RemoveEntityCommand.UpdateCanExecuteState();
				}

			}
		}

		#endregion
	}
}