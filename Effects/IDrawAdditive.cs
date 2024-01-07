using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FM
{
	public interface IDrawAdditive
	{
		void AdditiveCall(SpriteBatch sb, Vector2 screenPos);
	}
}
