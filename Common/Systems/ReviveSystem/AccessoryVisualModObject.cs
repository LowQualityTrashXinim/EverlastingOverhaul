using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Systems.ObjectSystem;
using EverlastingOverhaul.Common.Systems.ObjectSystem.DataStructures;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace EverlastingOverhaul.Common.Systems.ReviveSystem;

public class AccessoryVisualModObject : ModObject {
	public int AccType = -1;
	public int alpha = 255;
	public override void SetDefaults() {
		timeLeft = 120;
	}
	public override void OnSpawn(IEntitySource source) {
		if (source is EntitySource_AccessoryVisual visual) {
			AccType = visual.AccType;
		}
	}
	public override void AI() {
		velocity = -Vector2.UnitY * 2;
		alpha = (int)MathHelper.Lerp(0, 255, timeLeft / 120f);
	}
	public override void Draw(SpriteBatch spritebatch) {
		if (AccType < 0) {
			return;
		}
		float opacity = alpha / 255f;
		Main.instance.LoadItem(AccType);
		Texture2D texture = TextureAssets.Item[AccType].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = position - Main.screenPosition + origin;
		Color color = new Color(255, 255, 255, 0) * opacity;
		color.A = (byte)alpha;
		spritebatch.Draw(texture, drawPos, null, color, 0, origin, 1f, SpriteEffects.None, 0);
	}
}
