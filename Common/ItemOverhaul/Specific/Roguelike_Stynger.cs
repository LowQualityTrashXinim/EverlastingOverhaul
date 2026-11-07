using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_Stynger : GlobalItem{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Stynger;
	public override void SetDefaults(Item entity) {
		entity.useTime = 5;
		entity.useAnimation = 40;
		entity.reuseDelay = 30;
		entity.damage += 10;
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		SoundEngine.PlaySound(item.UseSound);
		position += (Vector2.UnitY * Main.rand.NextFloat(-6, 6)).RotatedBy(velocity.ToRotation());
	}
}
