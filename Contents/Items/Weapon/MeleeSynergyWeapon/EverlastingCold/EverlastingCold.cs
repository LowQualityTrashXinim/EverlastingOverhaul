using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.ItemOverhaul;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.EverlastingCold;

internal class EverlastingCold : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		DataStorer.AddContext("Synergy_WinterKnight", new(300, Vector2.Zero, false, Color.Cyan));
	}
	public override void SetDefaults() {
		Item.BossRushSetDefault(92, 92, 120, 5f, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		Item.rare = ItemRarityID.LightPurple;
		Item.value = Item.buyPrice(gold: 50);
		Item.UseSound = SoundID.Item1;
		Item.Set_InfoItem();
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
			meleeItem.SwingType = BossRushUseStyle.Swipe;
	}
	int strikeCounter = 0;
	int strikeCounter_IceMist = 0;
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!player.HasBuff<WinterKnight>()) {
			if (++strikeCounter >= 20) {
				player.AddBuff<WinterKnight>(ModUtils.ToSecond(60));
				strikeCounter = 0;
				strikeCounter_IceMist = 0;
			}
		}
		for (int i = 0; i < 5; i++) {
			Vector2 GetPostDirect2 = new Vector2(player.Center.X + Main.rand.Next(300, 1000) * player.direction, player.Center.Y - Main.rand.Next(300, 700));
			Vector2 GoTo = (new Vector2(target.Center.X + Main.rand.Next(-200, 200), target.Center.Y + Main.rand.Next(-200, 200)) - GetPostDirect2).SafeNormalize(Vector2.UnitX);
			int proj = Projectile.NewProjectile(Item.GetSource_FromThis(), GetPostDirect2, GoTo * 20, ProjectileID.IceBolt, (int)(hit.Damage * 0.65f), hit.Knockback, player.whoAmI);
			Main.projectile[proj].tileCollide = false;
			Main.projectile[proj].timeLeft = 200;
		}
		if (WinterKnight_ModPlayer.Check_WinterKnight(player)) {
			target.AddBuff<FrostInferno>(ModUtils.ToSecond(Main.rand.Next(3, 6)));
			if (++strikeCounter_IceMist >= 30) {
				if (player.ownedProjectileCounts[ModContent.ProjectileType<WinterKnight_IceMist>()] < 1) {
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<WinterKnight_IceMist>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
				}
			}
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.IceBlade)
			.AddIngredient(ItemID.Frostbrand)
			.Register();
	}
}
public class WinterKnight_ModPlayer : ModPlayer {
	public Vector2 WinterKnightAura_Position = Vector2.Zero;
	public int WinterKnightAura_Duration = 0;
	public int WinterKnightAura_CoolDown = 0;
	public override void ResetEffects() {
		if (Player.HasBuff<WinterKnight>()) {
			if (WinterKnightAura_Position == Vector2.Zero) {
				WinterKnightAura_Position = Player.Center;
				SoundEngine.PlaySound(SoundID.Item28 with { Pitch = -1f }, Player.Center);
				for (int i = 0; i < 500; i++) {
					Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Frost);
					dust.velocity = Main.rand.NextVector2Circular(30, 30);
					dust.noGravity = true;
					dust.scale += Main.rand.NextFloat(-.25f, .5f);
					Dust snow = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Snow);
					snow.velocity = Main.rand.NextVector2Circular(30, 30);
					snow.noGravity = true;
					snow.scale += Main.rand.NextFloat(-.25f, .5f);
				}
			}
			WinterKnightAura_Position += (Player.Center - WinterKnightAura_Position).SafeNormalize(Vector2.Zero) * (Player.Center - WinterKnightAura_Position).Length() / 64f;
			DataStorer.ActivateContext(WinterKnightAura_Position, "Synergy_WinterKnight");
		}
		else {
			WinterKnightAura_Position = Vector2.Zero;
		}
	}
	public static bool Check_WinterKnight(Player Player) => Player.HasBuff<WinterKnight>() && Player.GetModPlayer<WinterKnight_ModPlayer>().WinterKnightAura_Position.IsCloseToPosition(Player.Center, 300f);
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Check_WinterKnight(Player)) {
			modifiers.SourceDamage -= Main.rand.NextFloat(.4f, .9f);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Check_WinterKnight(Player)) {
			modifiers.SourceDamage -= Main.rand.NextFloat(.4f, .9f);
		}
	}
}
public class FrostInferno : ModBuff {
	public override string Texture => $"Terraria/Images/Buff_{BuffID.Frostburn}";
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 30;
		for (int i = 0; i < 3; i++) {
			Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.IceTorch);
			dust.noGravity = true;
			dust.velocity = -Vector2.UnitY * Main.rand.NextFloat(1, 5);
			dust.scale += Main.rand.NextFloat(-.25f, .25f);
			Dust frost = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Frost);
			frost.noGravity = true;
			frost.velocity = -Vector2.UnitY * Main.rand.NextFloat(1, 5);
			frost.scale += Main.rand.NextFloat(-.25f, .25f);
		}
	}
}
public class WinterKnight : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.endurance += .3f;
	}
}
public class WinterKnight_IceMist : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.CultistBossIceMist);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 100;
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.timeLeft = 9999;
		Projectile.tileCollide = false;
		Projectile.hide = false;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.rotation += MathHelper.ToRadians(5);
		Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (player.Center - Projectile.Center).Length() / 64f;
		if (++Projectile.ai[0] >= 15) {
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 600)) {
				Vector2 distance = npc.Center - Projectile.Center;
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, distance.SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<WinterKnight_IceShards>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			Projectile.ai[0] = 0;
		}
		if (player.HasBuff<WinterKnight>()) {
			Projectile.timeLeft = 2;
		}
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item88 with { Pitch = 1f }, Projectile.Center);
		for (int i = 0; i < 200; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Frost);
			dust.velocity = Main.rand.NextVector2Circular(20, 20);
			dust.noGravity = true;
			dust.scale += Main.rand.NextFloat(-.25f, .5f);
		}
		for (int i = 0; i < 32; i++) {
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(4, 4) * Main.rand.NextFloat(.25f, 1.25f), ModContent.ProjectileType<WinterKnight_IceShards>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
	}
}
public class WinterKnight_IceShards : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 1200;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.extraUpdates = 5;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
		Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(16, 16), 0, 0, DustID.Frost);
		dust.velocity = Vector2.Zero;
		dust.noGravity = true;
		if (++Projectile.frameCounter >= 6) {
			if (++Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Extra[ExtrasID.CultistIceshard].Value;
		Rectangle rect = texture.Frame(1, 3, 0, Projectile.frame);
		Vector2 origin = rect.Size() * .5f;
		origin.Y /= 3;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawpos, rect, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
