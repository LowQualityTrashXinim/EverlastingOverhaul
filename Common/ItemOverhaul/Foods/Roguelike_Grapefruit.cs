using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Grapefruit : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Grapefruit;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int ManaAmount() => 75;
	public override int EnergyAmount() => 25;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 120;
		SetBuff(item, ModContent.BuffType<Roguelike_Grapefruit_ModBuff>(), ModUtils.ToMinute(5));
	}
}
public class Roguelike_Grapefruit_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.Grapefruit;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Rogulike_Grapefruit_ModPlayer>().Grapefruit = true;
	}
}
public class Rogulike_Grapefruit_ModPlayer : ModPlayer {
	public bool Grapefruit = false;
	public int Grapefruit_Stack = 0;
	public int Grapefruit_DecayRate = 0;
	public override void ResetEffects() {
		Grapefruit = false;
		Grapefruit_Stack = Math.Clamp(Grapefruit_Stack, 0, 32);
		if (Grapefruit_Stack > 0) {
			if (++Grapefruit_DecayRate >= ModUtils.ToSecond(10)) {
				Grapefruit_DecayRate = 0;
				Grapefruit_Stack--;
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		Grapefruit_Stack = 0;
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		Grapefruit_Stack = 0;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Grapefruit) {
			return;
		}
		target.AddBuff(ModContent.BuffType<Roguelike_Grapefruit_ModDeBuff>(), ModUtils.ToSecond(Main.rand.Next(3, 10)));
		target.GetGlobalNPC<RoguelikeGlobalNPC>().Grapefruit = Grapefruit_Stack;
		if (Main.rand.NextBool(5)) {
			Item.NewItem(item.GetSource_OnHit(target), target.Hitbox, ModContent.ItemType<Roguelike_Grapefruit_Item>());
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Grapefruit) {
			return;
		}
		target.AddBuff(ModContent.BuffType<Roguelike_Grapefruit_ModDeBuff>(), ModUtils.ToSecond(Main.rand.Next(3, 10)));
		target.GetGlobalNPC<RoguelikeGlobalNPC>().Grapefruit = Grapefruit_Stack;
		if (Main.rand.NextBool(20)) {
			Item.NewItem(proj.GetSource_OnHit(target), target.Hitbox, ModContent.ItemType<Roguelike_Grapefruit_Item>());
		}
	}
}
public class Roguelike_Grapefruit_ModDeBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= npc.GetGlobalNPC<RoguelikeGlobalNPC>().Grapefruit + 1;
	}
}
public class Roguelike_Grapefruit_Item : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Grapefruit);
	public override void SetDefaults() {
	}
	public override bool OnPickup(Player player) {
		player.GetModPlayer<Rogulike_Grapefruit_ModPlayer>().Grapefruit_Stack++;
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
