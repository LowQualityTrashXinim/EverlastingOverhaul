using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_CreamSoda : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.CreamSoda;
	public override int LifeAmount() => 90;
	public override int ManaAmount() => 140;
	public override int CoolDownBetweenUse() => 240;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_CreamSoda_Buff>(), ModUtils.ToMinute(14));
	}
}
public class Roguelike_CreamSoda_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.CreamSoda;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_CreamSoda_ModPlayer>().CreamSoda = true;
	}
}
public class Roguelike_CreamSoda_ModPlayer : ModPlayer {
	public bool CreamSoda = false;
	public int Timer = 0;
	public override void ResetEffects() {
		CreamSoda = false;
	}
	public override void UpdateEquips() {
		if (CreamSoda) {
			if (++Timer > 300) {
				Timer = 0;
				Item.NewItem(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(500, 500), ModContent.ItemType<Roguelike_CreamSoda_Pickup>());
				//Spawn a soda pickable here
			}
		}
	}
}
public class Roguelike_CreamSoda_Pickup : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.CreamSoda);
	public override void SetDefaults() {
	}
	public override bool OnPickup(Player player) {
		player.Heal(5);
		player.immune = true;
		player.AddImmuneTime(-1, 60);
		return false;
	}
	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
		Main.instance.LoadItem(Type);
		Texture2D texture = TextureAssets.Item[Type].Value;
		Vector2 size2 = texture.Size();
		Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
		Vector2 drawpos = Main.item[whoAmI].position - Main.screenPosition;
		spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3, 0, 2), lightColor, 0, size3 * .5f, scale, SpriteEffects.None, 0);
		return false;
	}
}
