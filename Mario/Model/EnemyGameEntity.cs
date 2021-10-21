using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mario.Model
{
	public class EnemyGameEntity : GameEntity
	{
		private long _frame;
		private long _respawnFrame = long.MaxValue;
		private const int PerFrameX = 5;
		private int _velocityX = -5;
		private int _velocityY = 1;
		private int _animationIndex = 0;


		private void Reset()
		{
			X = StartX;
			Y = StartY;
			_velocityY = 1;
			_velocityX = -5;
			_isGone = false;
			Visual.Visibility = Visibility.Visible;
			UpdatePosition();

		}

		/// <inheritdoc />
		public EnemyGameEntity(Game game) : base(game)
		{
			StartX = 400;
			StartY = 768;
			X = StartX;
			Y = StartY;
			SetupSprite();
		}

		public EnemyGameEntity(Game game, EntityDto dto) :
			base(game, dto)
		{
			SetupSprite();
			game.Entities.Add(this);
		}

		#region Overrides of GameEntity

		/// <inheritdoc />
		public override void Animate()
		{
			_frame++;
			if (_isGone)
			{
				if (_frame > _respawnFrame)
				{
					_respawnFrame = long.MaxValue;
					Reset();
					return;
				}
				else
					return;
			}
			X += _velocityX;
			if (X < MinX)
				_velocityX = PerFrameX;
			else if (X > MaxX)
				_velocityX = -PerFrameX;
			Y += _velocityY;
			if (Y > 800 || X < 0)
			{
				if (Game.IsInDesigner)
				{
					_respawnFrame = _frame + 50;
				}
				_isGone = true;
				Visual.Visibility = Visibility.Collapsed;
				return;
			}
			if (_velocityY < 10)
				_velocityY += 1;
			foreach (var collisionEntity in CollisionsEnumerable())
			{
				//todo: process this
				Y = collisionEntity.Y - collisionEntity.FullHeight;
				_velocityY = 1;

			}
			UpdatePosition();
			if ((DateTime.Now - _lastAnimationUpdate).TotalMilliseconds > AnimationFrequency)
				_animationIndex = UpdateAnimation(_animationAssets, _animationIndex + 1);
		}




		#endregion
	}
}