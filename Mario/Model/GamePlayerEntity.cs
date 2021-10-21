using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mario.Model
{
	public class GamePlayerEntity : GameEntity
	{
		private double _velocityX;
		private double _velocityY;
		private string _marioStand = "mario-stand";
		private string _marioTurn = "mario-turn";
		private string[] _runningAnimation = new[] { "mario-run-1", "mario-run-2", "mario-run-3" };
		private string _marioDead = "mario-dead";
		private string _marioJump = "mario-jump";
		private bool _isTurning;
		private bool _isJumping;
		private int _animationIndex;
		private string _activeSprite = "mario-stand";
		private string _activeDirection = "right";
		/// <inheritdoc />
		public GamePlayerEntity(Game game)
			: base(game)
		{
			EntityType = GameEntityType.Mario;
			SetupSprite();
			X = 100;
			Y = 704;
			UpdatePosition();

		}

		private void ProcessKeyCommands()
		{
			if (_isGone)
				return;
			switch (_activeCommand)
			{
				case InputCommand.Up:
					break;
				case InputCommand.Down:
					break;
				case InputCommand.Left:
					//todo:stopping
					_isTurning = _velocityX > 0;
					_velocityX -= 3f;
					if (_velocityX < -25)
						_velocityX = -25;
					break;
				case InputCommand.Right:
					_isTurning = _velocityX < 0;
					_velocityX += 3f;
					if (_velocityX > 25)
						_velocityX = 25;
					break;
				case InputCommand.ActionB:
					break;
			}
		}

		#region Overrides of GameEntity

		/// <inheritdoc />
		public override void Animate()
		{
			ProcessKeyCommands();
			string direction = "right";
			if (_isGone)
			{

				X += _velocityX;
				Y += _velocityY;
				if (_velocityX-- < 0)
					_velocityX = 0;

				if (_velocityY++ > 10)
					_velocityY = 10;
				UpdatePosition();
				return;
			}
			bool isRunning = false;
			string desiredSprite = "mario-stand";

			isRunning = _velocityX != 0;
			/*
			if (Math.Abs(_velocityX) < 0.75)
			{
				_velocityX = 0;
				isRunning = false;
			}*/
			if (isRunning)
				if (_velocityX > 0)
					direction = "right";
				else
					direction = "left";

			X += _velocityX;
			if (_isJumping)
				desiredSprite = "mario-jump";
			if (_isTurning && isRunning)
				desiredSprite = "mario-turn";
			if (desiredSprite == "mario-stand" && isRunning)
			{
				desiredSprite = "run";
			}
			if ((DateTime.Now - _lastAnimationUpdate).TotalMilliseconds > 150 && isRunning && !_isTurning)
			{
				_activeSprite = "run";
				_animationIndex = UpdateAnimation(_runningAnimation, _animationIndex + 1);
			}
			else
			{
				if (_activeSprite != desiredSprite)
				{
					_activeSprite = desiredSprite;
					var uriSource = new Uri(@$"/Mario;component/Assets/{desiredSprite}.png", UriKind.Relative);
					Visual.Image.Source = new BitmapImage(uriSource);
				}
			}

			if (_activeDirection != direction)
			{
				_activeDirection = direction;
				Visual.Image.RenderTransformOrigin = new Point(0.5, 0.5);
				ScaleTransform flipTrans = new ScaleTransform();
				flipTrans.ScaleX = direction == "right" ? 1 : -1;
				Visual.Image.RenderTransform = flipTrans;
				//flipTrans.ScaleY = -1;
			}
			ProcessVertical();
			if (X < 0)
				X = 0;
			//X += _velocityX;
			UpdatePosition();
			//Y += _velocityY;
		}

		private void ProcessVertical()
		{
			Y += _velocityY;
			if (Y > 800)
			{
				KillMario();
				//Visual.Visibility = Visibility.Collapsed;
				return;
			}
			Y += _velocityY;

			if (_velocityY < -20)
				_velocityY = 0;
			if (_velocityY < 10)
				_velocityY += 1;
			foreach (var collisionEntity in CollisionsEnumerable())
			{
				//todo: process this
				bool interceptProcessed = false;
				if (collisionEntity.GetType() == typeof(StaticGameEntity))
				{
					if (_velocityY >= 0)
					{
						
							// this means we are falling or standing on something and likely hit the top of something
							if (Math.Abs(Y - (collisionEntity.Y - collisionEntity.FullHeight)) < 22)
							{
								Y = collisionEntity.Y - collisionEntity.FullHeight;
								interceptProcessed = true;
							}

						_isJumping = false;
					}
					else
					{
						if (X > collisionEntity.X && X < collisionEntity.X + collisionEntity.FullWidth)
							// we are going up and likely bumped our head on the ceiling
							if (Math.Abs(Y - (collisionEntity.Y + FullHeight)) < 22)
						{
							Y = collisionEntity.Y + FullHeight;
							interceptProcessed = true;
						}
						_velocityY = 0;
					}
					if (interceptProcessed)
						_velocityY = 1;
					if (_velocityX != 0)
					{
						if (_velocityX < 0)
							_velocityX += 1;
						else
							_velocityX -= 1;
					}
					if (!interceptProcessed)
						if (_velocityX >= 0)
						{
							// moving right 
							if (Math.Abs(X - (collisionEntity.X - FullWidth)) < 26)
							{
								X = collisionEntity.X - FullWidth;
								_velocityX = 1;
								interceptProcessed = true;
							}
						}
						else
						{
							// moving left
							if (Math.Abs(X - (collisionEntity.X + collisionEntity.FullWidth)) < 26)
							{
								X = collisionEntity.X + collisionEntity.FullWidth;
								_velocityX = -1;
								interceptProcessed = true;
							}
						}


				}
				else if (collisionEntity.GetType() == typeof(EnemyGameEntity))
				{
					// todo: detect if we hit the person
					KillMario();
				}
			}
		}


		#endregion

		private InputCommand _activeCommand = InputCommand.None;
		public void SetKeyPressState(InputCommand command, bool isPressed)
		{
			if (_isGone)
			{
				if (isPressed && command == InputCommand.NewGame)
					Respawn();
				return;
			}
			switch (command)
			{
				case InputCommand.Up:
					break;
				case InputCommand.Down:
					break;
				case InputCommand.Left:
					_activeCommand = isPressed ? InputCommand.Left : InputCommand.None;

					break;
				case InputCommand.Right:
					_activeCommand = isPressed ? InputCommand.Right : InputCommand.None;
					break;
				case InputCommand.JumpA:
					if (_isJumping)
						return;
					_isJumping = true;
					_velocityY -= 15;
					break;
				case InputCommand.ActionB:
					break;
			}
		}


		private void KillMario()
		{
			var uriSource = new Uri(@$"/Mario;component/Assets/mario-dead.png", UriKind.Relative);
			Visual.Image.Source = new BitmapImage(uriSource);
			_velocityY = -30;
			_velocityX = 25;
			// mario fell off the screen
			if (Game.IsInDesigner)
			{
				//_respawnFrame = _frame + 50;
			}
			_isGone = true;
		}

		public void Respawn()
		{
			_isTurning = false;
			_isJumping = false;
			_isGone = false;
			_velocityX = 0;
			_velocityY = 0;
			X = 100;
			Y = 704;
			_activeCommand = InputCommand.None;
			UpdatePosition();
			Animate();
		}
	}
}
