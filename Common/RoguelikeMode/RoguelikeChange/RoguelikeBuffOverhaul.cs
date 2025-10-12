using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange;
internal class RoguelikeBuffOverhaul : GlobalBuff {
	public override void SetStaticDefaults() {
		//I am unsure why this is set to true
		Main.debuff[BuffID.Campfire] = false;
		Main.debuff[BuffID.Honey] = false;
		Main.debuff[BuffID.StarInBottle] = false;
		Main.debuff[BuffID.HeartLamp] = false;
		Main.debuff[BuffID.CatBast] = false;
		Main.debuff[BuffID.Sunflower] = false;
	}
	public override void Update(int type, NPC npc, ref int buffIndex) {
		RoguelikeGlobalNPC globalnpc = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		if (type == BuffID.Electrified) {
			int lifelose = 22;
			if (npc.velocity != Microsoft.Xna.Framework.Vector2.Zero) {
				lifelose += 11;
			}
			if (npc.HasBuff(BuffID.Wet)) {
				lifelose += 22;
			}
			npc.lifeRegen -= lifelose;
			globalnpc.StatDefense.Base -= 10;
			int extraElectric = (int)(npc.Size.Length() / 20) + 2;
			for (int i = 0; i < extraElectric; i++) {
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, Scale: Main.rand.NextFloat(.25f, .5f));
				dust.velocity = Main.rand.NextVector2Circular(3, 3);
				dust.noGravity = true;
			}
		}
		if (ModItemLib.FireDeBuff.Contains(type)) {
			if (npc.HasBuff(BuffID.Slimed)) {
				npc.lifeRegen -= Math.Max(npc.lifeMax / 1000, 1);
			}
		}
	}
	public override void Update(int type, Player player, ref int buffIndex) {
		if (type == BuffID.Frostburn) {
			if (player.GetModPlayer<RoguelikeArmorPlayer>().ActiveArmor.Equals(ArmorLoader.GetModArmor("BorealwoodArmor"))) {
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 35);
			}
		}
		if (type == BuffID.Invisibility) {
			/*					
			 		[+] Your critical damage increases by 35%
					[+] Increases your movement speed by 15%
					[+] You have 1 in 15 chance to dodge
			*/
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1.35f);
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
		}
	}
}
public class RoguelikeOverhaul_Buff_ModPlayer : ModPlayer {
	public override bool FreeDodge(Player.HurtInfo info) {
		if (!Player.immune && Player.HasBuff(BuffID.Invisibility) && Main.rand.NextBool(15)) {
			Player.AddImmuneTime(info.CooldownCounter, 60);
			Player.immune = true;
			return true;
		}
		return base.FreeDodge(info);
	}
}
