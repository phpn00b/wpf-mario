using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mario.Model;

namespace Mario.ViewModel
{
	public class GameEntityViewModel : BaseViewModel
	{
		public GameEntity Entity { get; init; }
		public GameEntityViewModel()
			: this(new StaticGameEntity(null)) { }

		internal GameEntityViewModel(GameEntity gameEntity)
		{
			Entity = gameEntity;
			if (gameEntity != null)
			{
				Visual = gameEntity.Visual;
			}

			if (gameEntity.GetType() == typeof(StaticGameEntity))
			{
				Types = new string[]
				{
					"BoxSolid",
					"BoxBrick",
					"BoxQuestion",
					"TubeEndVertical",
					"TubeEndHorizontal",
					"TubeSectionVertical",
					"TubeSectionHorizontal",
					"Cloud",
					"BoxGround"
				};
			}
			else
			{
				Types = new string[]
				{

					"Gumba",
					"Turtle"
				};
			}

			_selectedTypeIndex = Types.ToList().IndexOf(gameEntity.EntityType.ToString());//  (short)gameEntity.EntityType;
			_height = gameEntity.Height;
			_width = gameEntity.Width;
			_startX = gameEntity.StartX;
			_startY = gameEntity.StartY;
			_minX = gameEntity.MinX;
			_maxX = gameEntity.MaxX;
			Entity.Animate();
		}


		#region StartX Property

		private int _startX;

		public int StartX
		{
			get => _startX;
			set
			{
				if (_startX != value)
				{
					_startX = value;
					OnPropertyChanged("StartX");
				}
			}
		}

		#endregion

		#region StartY Property

		private int _startY;

		public int StartY
		{
			get => _startY;
			set
			{
				if (_startY != value)
				{
					_startY = value;
					OnPropertyChanged("StartY");
				}
			}
		}

		#endregion

		#region MaxX Property

		private int _maxX;

		public int MaxX
		{
			get => _maxX;
			set
			{
				if (_maxX != value)
				{
					_maxX = value;
					OnPropertyChanged("MaxX");
					Entity.MaxX = value;
				}
			}
		}

		#endregion

		#region MinX Property

		private int _minX;

		public int MinX
		{
			get => _minX;
			set
			{
				if (_minX != value)
				{
					_minX = value;
					OnPropertyChanged("MinX");
					Entity.MinX = value;
				}
			}
		}

		#endregion

		#region EntityType Property

		private GameEntityType _entityType;

		public GameEntityType EntityType
		{
			get => _entityType;
			set
			{
				if (_entityType != value)
				{
					_entityType = value;
					OnPropertyChanged("EntityType");
				}
			}
		}

		#endregion

		#region Height Property

		private int _height;

		public int Height
		{
			get => _height;
			set
			{
				if (_height != value)
				{
					_height = value;
					OnPropertyChanged("Height");
					Entity.Height = value;
					Entity.SetupSprite();
				}
			}
		}

		#endregion

		#region Width Property

		private int _width;

		public int Width
		{
			get => _width;
			set
			{
				if (_width != value)
				{
					_width = value;
					OnPropertyChanged("Width");
					Entity.Width = value;
					Entity.SetupSprite();
				}
			}
		}

		#endregion

		#region Visual Property

		private FrameworkElement _visual;

		public FrameworkElement Visual
		{
			get => _visual;
			set
			{
				if (_visual != value)
				{
					_visual = value;
					OnPropertyChanged("Visual");
				}
			}
		}

		#endregion

		#region SelectedTypeIndex Property

		private int _selectedTypeIndex = 0;

		public int SelectedTypeIndex
		{
			get => _selectedTypeIndex;
			set
			{
				if (_selectedTypeIndex != value)
				{
					_selectedTypeIndex = value;
					OnPropertyChanged("SelectedTypeIndex");
					if (value != -1)
					{
						try
						{
							Entity.EntityType = (GameEntityType)Enum.Parse(typeof(GameEntityType), Types[value]);
							Entity.SetupSprite();
						}
						catch (Exception e)
						{

						}
					}
				}
			}
		}

		#endregion

		public string[] Types { get; protected set; }


	}
}
