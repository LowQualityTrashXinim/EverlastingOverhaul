using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
public class Avarice_ModPlayer : ModPlayer {
	public int Avarice_Counter = 0;
	public readonly int[] Gems = [ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Amber, ItemID.Ruby, ItemID.Diamond];
	public int Avarice_CounterCached = 0;
	Vector2 ShootPos = Vector2.Zero;
	public override void UpdateEquips() {
		if (Avarice_CounterCached > 0) {
			if (ShootPos == Vector2.Zero) {
				ShootPos = Player.Center;
			}
			Projectile.NewProjectile(Player.GetSource_FromThis(), ShootPos, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.36f, 1.1f), ModContent.ProjectileType<Avarice_Projectile>(), Player.GetWeaponDamage(Player.HeldItem) + 10, 0, Player.whoAmI, Main.rand.Next(Gems), 1);
			Avarice_CounterCached--;
			if (Avarice_CounterCached <= 0) {
				ShootPos = Vector2.Zero;
			}
		}
	}
	public override void PostUpdateBuffs() {
		if (!Player.HasBuff<Avarice_Buff>() && Avarice_Counter > 0) {
			Avarice_CounterCached = Avarice_Counter;
			Avarice_Counter = 0;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Avarice].Contains(item.type)) {
			if (Main.rand.NextBool(10)) {
				Projectile.NewProjectileDirect(Player.GetSource_ItemUse(Player.HeldItem), target.Center + Main.rand.NextVector2CircularEdge(300, 300), Vector2.Zero, ModContent.ProjectileType<Avarice_Projectile>(), hit.Damage, 0, Player.whoAmI, Main.rand.Next(Gems));
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Avarice].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			if (Main.rand.NextBool(10)) {
				Projectile.NewProjectileDirect(Player.GetSource_ItemUse(Player.HeldItem), target.Center + Main.rand.NextVector2CircularEdge(300, 300), Vector2.Zero, ModContent.ProjectileType<Avarice_Projectile>(), hit.Damage, 0, Player.whoAmI, Main.rand.Next(Gems));
			}
		}
	}
}
public class Avarice_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		var modplayer = player.GetModPlayer<Avarice_ModPlayer>();
		player.ModPlayerStats().UpdateDefenseBase.Base += modplayer.Avarice_Counter * .5f;
		player.GetDamage(DamageClass.Generic) += modplayer.Avarice_Counter * .01f;
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		if (player.buffTime[buffIndex] > 0) {
			player.GetModPlayer<Avarice_ModPlayer>().Avarice_Counter++;
			player.buffTime[buffIndex] += (int)player.ModPlayerStats().BuffTime.ApplyTo(60);
			return true;
		}
		return base.ReApply(player, time, buffIndex);
	}
}
public class Avarice_Projectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 180;
		Projectile.penetrate = 1;
	}
	public override void OnSpawn(IEntitySource source) {
		if (GemType == 0) {
			GemType = ItemID.Amethyst;
		}
		AssignDustType();
		for (int i = 0; i < 25; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, GemDustType);
			dust.velocity = Main.rand.NextVector2CircularEdge(15, 15);
			dust.noGravity = true;
		}
		AssignValue();
	}
	public float damagePercentage = 1;
	public int HealPower = 0;
	public int ManaPower = 0;
	public bool PercentageHeal = false;
	public int GemType { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public bool AiSwitch { get => (int)Projectile.ai[1] == 1; }
	public int GemDustType = 0;
	public override void AI() {
		if (AiSwitch) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			damagePercentage = 1;
			Projectile.velocity.X *= .99f;
			if (Projectile.velocity.Y < 16) {
				Projectile.velocity.Y += .5f;
			}
		}
		else {
			var player = Main.player[Projectile.owner];
			if (player.Center.IsCloseToPosition(Projectile.Center, 30)) {
				OnCollecting(player);
				Projectile.Kill();
			}
			else {
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			}
			if (GemType == ItemID.Emerald) {
				for (int i = 0; i < 3; i++) {
					var dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2CircularEdge(150, 150), 0, 0, DustID.GemEmerald);
					dust.noGravity = true;
				}
				Projectile.Center.LookForHostileNPC(out var npclist, 150f);
				foreach (var target in npclist) {
					player.StrikeNPCDirect(target, target.CalculateHitInfo((int)(Projectile.damage * damagePercentage) + 1, 1));
				}
			}
		}
	}
	public void OnCollecting(Player player) {
		player.AddBuff<Avarice_Buff>(ModUtils.ToSecond(10));
		if (HealPower > 0) {
			if (PercentageHeal) {
				player.Heal((int)(player.statLifeMax2 * .01f) + 1);
			}
			else {
				player.Heal(HealPower);
			}
		}
		if (ManaPower > 0) {
			player.ManaHeal(ManaPower);
		}
		if (GemType == ItemID.Emerald) {
			for (int i = 0; i < player.buffTime.Length; i++) {
				if (player.buffTime[i] != 0) {
					player.buffTime[i] += 300;
				}
			}
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage *= damagePercentage;
	}
	private void AssignDustType() {
		switch (GemType) {
			case ItemID.Amethyst:
				GemDustType = DustID.GemAmethyst;
				break;
			case ItemID.Topaz:
				GemDustType = DustID.GemTopaz;
				break;
			case ItemID.Sapphire:
				GemDustType = DustID.GemSapphire;
				break;
			case ItemID.Emerald:
				GemDustType = DustID.GemEmerald;
				break;
			case ItemID.Amber:
				GemDustType = DustID.GemAmber;
				break;
			case ItemID.Ruby:
				GemDustType = DustID.GemRuby;
				break;
			case ItemID.Diamond:
				GemDustType = DustID.GemDiamond;
				break;
		}
	}
	private void AssignValue() {
		switch (GemType) {
			case ItemID.Amethyst:
			case ItemID.Topaz:
				HealPower = ManaPower = 5;
				break;
			case ItemID.Sapphire:
				damagePercentage = .8f;
				HealPower = ManaPower = 10;
				break;
			case ItemID.Emerald:
				damagePercentage = .01f;
				break;
			case ItemID.Amber:
				damagePercentage = 1.25f;
				PercentageHeal = true;
				break;
			case ItemID.Ruby:
			case ItemID.Diamond:
				HealPower = 1;
				damagePercentage = 1.5f;
				PercentageHeal = true;
				break;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(GemType);
		var texture = TextureAssets.Item[GemType].Value;
		var origin = texture.Size() * .5f;
		var drawpos = Projectile.position - Main.screenPosition + origin;
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
