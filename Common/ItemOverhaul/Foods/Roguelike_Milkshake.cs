using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Milkshake : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Milkshake;
	public override int ManaAmount() => 375;
	public override int CoolDownBetweenUse() => 420;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Milkshake_Buff>(), ModUtils.ToMinute(37));
	}
}
public class Roguelike_Milkshake_Buff : FoodItemTier3 {
	public override int TypeID => ItemID.Milkshake;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_Milkshake_ModPlayer>().Milkshake = true;
		if (player.buffTime[buffIndex] <= 0) {
			player.GetModPlayer<Roguelike_Milkshake_ModPlayer>().Stack = 0;
		}
	}
}
public class Roguelike_Milkshake_ModPlayer : ModPlayer {
	public bool Milkshake = false;
	public int Stack = 0;
	public override void ResetEffects() {
		Milkshake = false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Milkshake) {
			if (Main.rand.NextBool(20)) {
				Item.NewItem(Player.GetSource_OnHit(target), target.Center, ModContent.ItemType<Roguelike_Milkshake_Pickup>());
			}
			if (Stack >= 10) {
				Stack -= 10;
				Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.Add(0, 500), Vector2.Zero, ModContent.ProjectileType<Roguelike_Milkshake_ModProjectile>(),
					400 + Player.GetWeaponDamage(Player.HeldItem), 1, Player.whoAmI, target.whoAmI);
			}
		}
	}
}
public class Roguelike_Milkshake_Pickup : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Milkshake);
	public override void SetDefaults() {
	}
	public override bool OnPickup(Player player) {
		player.GetModPlayer<Roguelike_Milkshake_ModPlayer>().Stack++;
		return false;
	}
	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
		Main.instance.LoadItem(Type);
		Texture2D texture = TextureAssets.Item[Type].Value;
		Vector2 size2 = texture.Size();
		Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
		Vector2 drawpos = Main.item[whoAmI].position - Main.screenPosition - size3 * .5f;
		spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3, 0, 2), lightColor, 0, size3 * .5f, scale, SpriteEffects.None, 0);
		return false;
	}
}
public class Roguelike_Milkshake_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<NPC>(NPCID.IceTortoise);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 50;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		Projectile.friendly = true;
	}
	int TargetWhoAmI = -1;
	public override void OnSpawn(IEntitySource source) {
		TargetWhoAmI = (int)Projectile.ai[0];
	}
	public override void AI() {
		if (++Projectile.ai[0] >= 90) {
			Projectile.velocity.Y = 20;
		}
		if (TargetWhoAmI < 0 || TargetWhoAmI >= 255) {
			return;
		}
		NPC target = Main.npc[TargetWhoAmI];
		if (!target.active || target.life <= 0) {
			Projectile.Kill();
		}
		if (Projectile.ai[0] < 90) {
			Projectile.Center = target.Center.Add(0, 500);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		SoundEngine.PlaySound(SoundID.Item62 with { Pitch = 1 }, target.Center);
		for (int i = 0; i < 30; i++) {
			Dust dust = Dust.NewDustDirect(target.Center, 0, 0, DustID.Snow);
			dust.velocity = Main.rand.NextVector2Circular(10, 10);
			dust.scale = Main.rand.NextFloat(.8f, 1.4f);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Rectangle framing = texture.Frame(1, 8, 0, 7, 0, 0);
		float rotation = Projectile.rotation;
		Vector2 origin = texture.Size() * .5f;
		origin.Y /= 8;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.spriteBatch.Draw(texture, drawpos, framing, lightColor, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
