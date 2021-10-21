using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.Model
{
	public enum GameEntityType : short
	{
		Mario = 999,
		Gumba = 0,
		Turtle = 1,
		BoxSolid = 2,
		BoxBrick = 3,
		BoxQuestion = 4,
		TubeEndVertical = 5,
		TubeEndHorizontal = 6,
		TubeSectionVertical = 7,
		TubeSectionHorizontal = 8,
		Cloud = 9,
		BoxGround =10
	}
}