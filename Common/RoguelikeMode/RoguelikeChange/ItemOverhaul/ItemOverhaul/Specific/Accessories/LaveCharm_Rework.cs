using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific.Accessories;
internal class Roguelike_LaveCharm : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.LavaCharm;
	}
	public override void UpdateEquip(Item item, Player player) {
		var modplayer = player.GetModPlayer<Roguelike_LaveCharm_ModPlayer>();
		modplayer.LavaCharm = true;
		for (int i = 0; i < 5; i++) {
			var dust = Dust.NewDustDirect(player.Center, 0, 0, DustID.Lava);
			dust.position += Main.rand.NextVector2CircularEdge(150, 150);
			dust.noGravity = true;
			dust.scale += Main.rand.NextFloat(-.1f, .2f);
			dust.Dust_BelongTo(player);
			dust.Dust_GetDust().FollowEntity = true;
		}
		if (modplayer.CD != 60) {
			return;
		}
		player.Center.LookForHostileNPC(out var npclist, 150);
		foreach (var npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(42, 1));
			npc.AddBuff(BuffID.OnFire, 30);
			for (int i = 0; i < 10; i++) {
				var dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.Lava);
				dust.velocity = Main.rand.NextVector2Circular(3, 3);
				dust.noGravity = true;
				dust.scale += Main.rand.NextFloat(-.1f, .2f);
			}
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_LaveCharm_ModPlayer : ModPlayer {
	public bool LavaCharm = false;
	public byte CD = 0;
	public override void ResetEffects() {
		if (++CD > 60) {
			CD = 0;
		}
		LavaCharm = false;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (LavaCharm) {
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
				modifiers.SourceDamage += 1;
			}
		}
	}
}
