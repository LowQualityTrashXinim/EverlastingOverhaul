using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_GingerbreadCookie : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.GingerbreadCookie;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4.25f);
	public override int ManaAmount() => 350;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_GingerbreadCookie_ModBuff>(), ModUtils.ToMinute(48));
	}
	public override void OnConsumeFood(Item item, Player player) {
		player.ModPlayerStats().Set_TemporaryMana(400, 60);
		player.ModPlayerStats().TemporaryMana += 400;
	}
}
public class Roguelike_GingerbreadCookie_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.GingerbreadCookie;
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Magic) *= 1.2f;
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.DebuffTime += .15f;
		handler.Set_TemporaryMana(400, 60);
		player.GetModPlayer<Roguelike_GingerbreadCookie_ModPlayer>().GingerbreadCookie = true;
	}
}
public class Roguelike_GingerbreadCookie_ModPlayer : ModPlayer {
	public bool GingerbreadCookie = false;
	public override void ResetEffects() {
		GingerbreadCookie = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitByEffect();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitByEffect();
	}
	private void OnHitByEffect() {
		if (!GingerbreadCookie || Main.rand.NextFloat() > .15f) {
			return;
		}
		Vector2 randPos = Player.Center + Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(.5f, 1.2f);
		Item.NewItem(Player.GetSource_FromThis(), randPos, ItemID.Star);
	}
}

