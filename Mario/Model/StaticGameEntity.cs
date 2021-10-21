using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.Model
{
	public class StaticGameEntity : GameEntity
	{
		/// <inheritdoc />
		public StaticGameEntity(Game game) : base(game)
		{
			EntityType = GameEntityType.BoxGround;
			BlockX = true;
			BlockY = true;
			SetupSprite();
		}

		public StaticGameEntity(Game game, EntityDto dto) :
			base(game, dto)
		{
			BlockX = true;
			BlockY = true;
			SetupSprite();
			game.Entities.Add(this);
		}

		#region Overrides of GameEntity

		/// <inheritdoc />
		public override void Animate()
		{

		}

		#endregion
	}
}
