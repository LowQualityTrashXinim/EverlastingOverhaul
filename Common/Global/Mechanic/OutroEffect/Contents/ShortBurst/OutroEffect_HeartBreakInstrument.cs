using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents.ShortBurst;
//internal class OutroEffect_HeartBreakInstrument : OutroEffect {
//	public override void SetStaticDefaults() {
//		Duration = 2;
//		Hide = true;
//	}
//	public override void Update(Player player) {
//		var position = player.Center;

//		int type = ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>();
//		for (int i = 0; i < 75; i++) {
//			var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), position + Main.rand.NextVector2Circular(450, 250), Main.rand.NextVector2CircularEdge(1, 1), type, 35 + (int)(player.GetWeaponDamage(player.HeldItem) * .35f), 0, player.whoAmI, 3, 15, i);
//			if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
//				proj.ScaleX = 5f;
//				proj.ScaleY = .5f;
//				projectile.scale = 2;
//			}
//		}
//	}
//}
