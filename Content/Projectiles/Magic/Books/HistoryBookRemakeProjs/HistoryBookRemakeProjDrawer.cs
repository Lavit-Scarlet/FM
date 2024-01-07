using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace FM.Content.Projectiles.Magic.Books.HistoryBookRemakeProjs;

public struct HistoryBookRemakeProjDrawer
{
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Projectile proj)
	{
		MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
		miscShaderData.UseSaturation(-2.8f);
		miscShaderData.UseOpacity(4f);
		miscShaderData.Apply();
		HistoryBookRemakeProjDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
		HistoryBookRemakeProjDrawer._vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip)
	{
		Color color1 = Color.Red;
		Color color2 = Color.Red;
		Color result = Color.Lerp(color1, color2, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
		result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip)
	{
		float num = 1f;
		float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
		num *= 1f - (1f - lerpValue) * (1f - lerpValue);
		return MathHelper.Lerp(0f, 32f, num);
	}
}