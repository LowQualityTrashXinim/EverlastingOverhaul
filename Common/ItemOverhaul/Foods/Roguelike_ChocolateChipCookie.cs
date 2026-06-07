using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_ChocolateChipCookie : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.ChocolateChipCookie;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6);
	public override int EnergyAmount() => 65;
	public override int ManaAmount() => 105;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_ChocolateChipCookie_ModBuff>(), ModUtils.ToMinute(13));
	}
}
public class Roguelike_ChocolateChipCookie_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.ChocolateChipCookie;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_ChocolateChipCookie_ModPlayer>().ChocolateChipCookie = true;
	}
}
public class Roguelike_ChocolateChipCookie_ModPlayer : ModPlayer {
	public bool ChocolateChipCookie = false;
	public int Timer = 0;
	public override void ResetEffects() {
		ChocolateChipCookie = false;
	}
	public override void UpdateEquips() {
		if (!ChocolateChipCookie) {
			return;
		}
		if (++Timer >= 600) {
			Timer = 0;
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2Circular(400, 400),
				Vector2.Zero, ModContent.ProjectileType<Roguelike_ChocolateChipCookie_ModProjectile>(), 0, 0, Player.whoAmI);
		}
	}
}
public class Roguelike_ChocolateChipCookie_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.ChocolateChipCookie);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.timeLeft = 600;
		Projectile.tileCollide = false;
	}
	public override void AI() {
		if (Projectile.Center.IsCloseToPosition(Main.MouseWorld, 30) && Main.mouseLeft) {
			Projectile.Kill();
			Main.player[Projectile.owner].AddBuff<Roguelike_ChocolateChipCookie_Buff>(ModUtils.ToSecond(12));
			Main.player[Projectile.owner].Heal(10);
		}
		Projectile.rotation += MathHelper.ToRadians(10);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		Vector2 size3 = new Vector2(origin.X, origin.Y / 3);
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawpos, texture.Frame(1, 3), lightColor, Projectile.rotation, size3, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
public class Roguelike_ChocolateChipCookie_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Generic) += .1f;
		player.GetCritChance(DamageClass.Generic) += 10;
		player.ModPlayerStats().UpdateCritDamage += .1f;
	}
	public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
		Main.instance.LoadItem(ItemID.ChocolateChipCookie);
		Texture2D bgsTexture = drawParams.Texture;
		Vector2 size = bgsTexture.Size();
		Texture2D texture = TextureAssets.Item[ItemID.ChocolateChipCookie].Value;
		Vector2 size2 = texture.Size();
		Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
		Vector2 drawpos = drawParams.Position + size * .5f;
		spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3), drawParams.DrawColor, 0, size3 * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size, size3, .67f), SpriteEffects.None, 0);
	}
}

