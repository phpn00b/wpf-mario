using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mario.Model
{

	public abstract class GameEntity
	{
		public const int AnimationFrequency = 250;
		protected DateTime _lastAnimationUpdate = DateTime.Now;
		private static int _nextIndex;
		private readonly int _entityId = _nextIndex++;
		protected bool _isGone;
		public class EntityDto
		{
			public GameEntityType EntityType { get; set; }



			public int Height { get; set; }

			public int Width { get; set; }

			public int MinX { get; set; }

			public int MaxX { get; set; }

			public int StartX { get; set; }

			public int StartY { get; set; }
		}


		[IgnoreDataMember]
		protected Game Game { get; init; }

		public bool BlockY { get; protected set; }

		public bool BlockX { get; protected set; }

		public GameEntityType EntityType { get; set; }

		protected GameEntity(Game game)
		{
			Game = game;
			Visual = new Controls.GameEntityControl
			{
				DataContext = this
			};

		}

		protected GameEntity(Game game, EntityDto dto)
		{
			Game = game;
			Visual = new Controls.GameEntityControl
			{
				DataContext = this
			};
			EntityType = dto.EntityType;
			Height = dto.Height;
			Width = dto.Width;
			MinX = dto.MinX;
			MaxX = dto.MaxX;
			StartY = dto.StartY;
			StartX = dto.StartX;
			X = dto.StartX;
			Y = dto.StartY;
		}


		public Controls.GameEntityControl Visual { get; protected set; }


		public bool IsVisible { get; set; }

		public double X { get; protected set; }

		public double Y { get; protected set; }

		public int Height { get; set; } = 1;

		public int FullHeight { get; set; }

		public int FullWidth { get; set; }
		public int Width { get; set; } = 1;

		public IEnumerable<GameEntity> CollisionsEnumerable()
		{
			for (int i = 0; i < Game.Entities.Count; i++)
			{
				if (_entityId != Game.Entities[i]._entityId && !_isGone)
					if (Game.Entities[i].CheckCollision(this))
						yield return Game.Entities[i];
			}
		}

		public bool CheckCollision(GameEntity otherEntity)
		{
			if (_isGone)
				return false;
			return
				X < otherEntity.X + otherEntity.FullWidth
				&& X + FullWidth > otherEntity.X
				&& Y < otherEntity.Y + otherEntity.FullHeight
				&& Y + FullHeight > otherEntity.Y;
		}


		public abstract void Animate();


		public int MinX { get; set; } = -1;

		public int MaxX { get; set; }

		public int StartX { get; set; }

		public int StartY { get; set; }

		internal void UpdatePosition()
		{
			Visual.RenderTransform = new TranslateTransform(X, Y);

		}

		internal string[] _animationAssets;
		public void SetupSprite()
		{
			string name = "";
			int spriteHeight = 64;
			int spriteWidth = 64;
			switch (EntityType)
			{
				case GameEntityType.Mario:
					spriteHeight = 64;
					name = "mario-stand";
					break;
				case GameEntityType.Gumba:
					name = "gumba-live";
					_animationAssets = new[] { "gumba-live", "gumba-live-1" };
					break;
				case GameEntityType.Turtle:
					name = "turtle-live";
					break;
				case GameEntityType.BoxSolid:
					name = "box-fixed";
					break;
				case GameEntityType.BoxBrick:
					name = "box-brick";
					break;
				case GameEntityType.BoxQuestion:
					name = "box-question";
					break;
				case GameEntityType.BoxGround:
					name = "box-ground";
					break;
				case GameEntityType.TubeEndVertical:
					break;
				case GameEntityType.TubeEndHorizontal:
					break;
				case GameEntityType.TubeSectionVertical:
					break;
				case GameEntityType.TubeSectionHorizontal:
					break;
				case GameEntityType.Cloud:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			int totalHeight = spriteHeight * Height;
			int totalWidth = spriteWidth * Width;
			Visual.Height = totalHeight;
			Visual.Width = totalWidth;
			var uriSource = new Uri(@$"/Mario;component/Assets/{name}.png", UriKind.Relative);
			Visual.Image.Source = new BitmapImage(uriSource);
			Visual.Image.Height = spriteHeight;
			Visual.Image.Width = spriteWidth;
			FullWidth = totalWidth;
			FullHeight = totalHeight;
			if (Height != 1 || Width != 1)
			{
				var ib = new ImageBrush();
				ib.ImageSource = new BitmapImage(new Uri(@$"{AppDomain.CurrentDomain.BaseDirectory}/Assets/{name}.png", UriKind.Relative));
				ib.TileMode = TileMode.Tile;
				ib.ViewportUnits = BrushMappingMode.Absolute;
				ib.Viewport = new Rect(0, 0, spriteWidth, spriteHeight);
				//ib.Viewport = new Rect(new Size(0.5, 0.5));

				Visual.Border.Background = ib;
				Visual.Image.Visibility = Visibility.Collapsed;
			}
			else
			{
				Visual.Image.Visibility = Visibility.Visible;
			}
			//= new BitmapImage(new Uri($"/Assets/{name}.png", UriKind.Relative));
		}

		public void SetStartPosition(double newX, double newY)
		{
			X = (int)newX;
			StartX = (int)newX;
			Y = (int)newY;
			StartY = (int)newY;
		}

		public static implicit operator EntityDto(GameEntity model)
		{
			if (model == null)
				return null;
			return new EntityDto
			{
				MinX = model.MinX,
				MaxX = model.MaxX,
				StartX = model.StartX,
				StartY = model.StartY,
				EntityType = model.EntityType,
				Height = model.Height,
				Width = model.Width
			};
		}

		protected int UpdateAnimation(string[] assets, int indexToActivate)
		{
			if (indexToActivate > assets.Length - 1)
				indexToActivate = 0;
			string name = assets[indexToActivate];
			var uriSource = new Uri(@$"/Mario;component/Assets/{name}.png", UriKind.Relative);
			Visual.Image.Source = new BitmapImage(uriSource);
			_lastAnimationUpdate = DateTime.Now;
			return indexToActivate;
		}

	}
}