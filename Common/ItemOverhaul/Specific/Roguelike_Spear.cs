using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Contents.Projectiles.Rework;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_Spear_Item : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Spear;
	}
	public override void SetDefaults(Item entity) {
		entity.shoot = ModContent.ProjectileType<EverlastingOverhaul_Spear_Projectile>();
		entity.useTime = entity.useAnimation = 60;
		entity.damage = 80;
	}
	public override bool AltFunctionUse(Item item, Player player) => true;
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.altFunctionUse == 2) {
			if (player.HasBuff<SpearCoolDown>()) {
				return;
			}
			type = ModContent.ProjectileType<SpearThrownProjectile>();
			player.AddBuff<SpearCoolDown>(180);
			velocity = velocity.SafeNormalize(Vector2.Zero) * 8;
		}
	}
}
public class SpearCoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class EverlastingOverhaul_Spear_Projectile : SpearReworkProjectile {
	protected override float HoldoutRangeMax => 65;
	protected override float HoldoutRangeMin => -10;
	protected override int SpearType => ProjectileID.Spear;
}
