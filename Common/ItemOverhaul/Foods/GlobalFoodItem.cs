using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
public abstract class GlobalFoodItem : GlobalItem {
	public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		ItemID.Sets.CanBeQuickusedOnGamepad[entity.type] = false;
		return (entity.buffType == BuffID.WellFed || entity.buffType == BuffID.WellFed2 || entity.buffType == BuffID.WellFed3) && entity.type == AppliesToFoodType();
	}
	public virtual int AppliesToFoodType() => ItemID.None;
	public virtual int CoolDownBetweenUse() => 0;
	public virtual int LifeAmount() => 0;
	public virtual int ManaAmount() => 0;
	public virtual int EnergyAmount() => 0;
	public virtual byte Tier() => 0;
	public void SetBuff(Item item, int type, int time) {
		item.buffType = type;
		item.buffTime = time;
	}
	public virtual string OverrideBasicTooltip() => null;
	public GlobalFoodItem_ModPlayer Player_FoodPlayer(Player player) => player.GetModPlayer<GlobalFoodItem_ModPlayer>();
	public sealed override void SetDefaults(Item entity) {
		SetFoodDefaults(entity);
	}
	public virtual void SetFoodDefaults(Item item) {

	}
	public override bool CanUseItem(Item item, Player player) => !player.HasBuff<FoodCoolDown>();
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		string tooltip = OverrideBasicTooltip();
		if (string.IsNullOrEmpty(tooltip)) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
		else {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "", tooltip));
		}

		int Life = LifeAmount();
		int Mana = ManaAmount();
		int Energy = EnergyAmount();
		string heal = "";
		if (Life > 0) {
			heal += $"Restore [c/ff0000:{Life} life] ";
		}
		if (Mana > 0) {
			heal += $"Restore [c/0000ff:{Mana} mana] ";
		}
		if (Energy > 0) {
			heal += $"Restore [c/00ffff:{Energy} energy]";
		}
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", heal));
		foreach (var tip in tooltips) {
			if (tip.Name == "ItemName") {
				tip.Text += $" [Tier {Tier() + 1}]";
			}
			if (tip.Name == "Tooltip0") {
				tip.Hide();
			}
			if (tip.Name == "BuffTime") {
				int second = item.buffTime / 60;
				int minute = second / 60;
				int remainSecond = second - minute * 60;
				if (remainSecond > 0) {
					tip.Text = $"{minute} minute {remainSecond}s duration";
				}
				else {
					tip.Text = $"{minute} minute duration";
				}
			}
		}
		tooltips.Add(new(Mod, "", $"{Math.Round(item.useAnimation / 60f, 2)}s eating time"));
		tooltips.Add(new(Mod, "", $"{Math.Round(CoolDownBetweenUse() / 60f, 2)}s cool down"));
	}
	public virtual void OnConsumeFood(Item item, Player player) {

	}
	public sealed override void OnConsumeItem(Item item, Player player) {
		int Life = LifeAmount();
		int Mana = ManaAmount();
		int Energy = EnergyAmount();
		if (Life > 0) {
			player.Heal(Life);
		}
		if (Mana > 0) {
			player.ManaHeal(Mana);
		}
		Player_FoodPlayer(player).SetFoodBuff(item.type, Tier());
		OnConsumeFood(item, player);
		player.AddBuff(ModContent.BuffType<FoodCoolDown>(), CoolDownBetweenUse() + player.itemAnimationMax);
	}
}
public class FoodCoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public sealed override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
	}
	public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
		int TypeID = Main.LocalPlayer.GetModPlayer<GlobalFoodItem_ModPlayer>().FoodJustSet;
		if (TypeID != -1) {
			Main.instance.LoadItem(TypeID);
			Texture2D bgsTexture = drawParams.Texture;
			Vector2 size = bgsTexture.Size();
			Texture2D texture = TextureAssets.Item[TypeID].Value;
			Vector2 size2 = texture.Size();
			Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
			Vector2 drawpos = drawParams.Position + size * .5f;
			spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3), drawParams.DrawColor, 0, size3 * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size, size3, .67f), SpriteEffects.None, 0);
		}
	}
}
public abstract class BaseFoodBuff : ModBuff {
	public virtual int TypeID => -1;
	public override string Texture => ModTexture.EMPTYBUFF;
	/// <summary>
	/// Set this to true if you want to add your own localization
	/// </summary>
	public bool OverrideTooltip => false;
	public sealed override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		if (TypeID != -1) {
			Item item = ContentSamples.ItemsByType[TypeID];
			int tier = 1;
			if (this is FoodItemTier2) {
				tier = 2;
			}
			else if (this is FoodItemTier3) {
				tier = 3;
			}
			buffName = item.Name + $" [Tier {tier}]";
			if (!OverrideTooltip) {
				tip = ModUtils.LocalizationText("RoguelikeRework", item.Name);
			}
		}
	}
	public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
		if (TypeID != -1) {
			if (!DenyDrawCondintion()) {
				return;
			}
			Main.instance.LoadItem(TypeID);
			Texture2D bgsTexture = drawParams.Texture;
			Vector2 size = bgsTexture.Size();
			Texture2D texture = TextureAssets.Item[TypeID].Value;
			Vector2 size2 = texture.Size();
			Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
			Vector2 drawpos = drawParams.Position + size * .5f;
			spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3), drawParams.DrawColor, 0, size3 * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size, size3, .67f), SpriteEffects.None, 0);
		}
	}
	public virtual bool DenyDrawCondintion() {
		return true;
	}
}
public abstract class FoodItemTier1 : BaseFoodBuff {
}
public abstract class FoodItemTier2 : BaseFoodBuff {
}
public abstract class FoodItemTier3 : BaseFoodBuff {
}
public class GlobalFoodItem_ModPlayer : ModPlayer {
	public int T1foodID = -1;
	public int T2foodID = -1;
	public int T3foodID = -1;
	public int FoodJustSet = -1;
	/// <summary>
	/// Upon setting the buff type and tier, will also attempt to delete the existing tier buff
	/// </summary>
	/// <param name="type"></param>
	/// <param name="tier"></param>
	public void SetFoodBuff(int type, int tier) {
		FoodJustSet = type;
		switch (tier) {
			case 0:
				if (T1foodID != type) {
					AttemptToDeleteExistingFoodBuff<FoodItemTier1>(T1foodID);
					T1foodID = type;
				}
				break;
			case 1:
				if (T2foodID != type) {
					AttemptToDeleteExistingFoodBuff<FoodItemTier2>(T2foodID);
					T2foodID = type;
				}
				break;
			case 2:
				if (T3foodID != type) {
					AttemptToDeleteExistingFoodBuff<FoodItemTier3>(T3foodID);
					T3foodID = type;
				}
				break;

		}
	}
	private void AttemptToDeleteExistingFoodBuff<T>(int typeBefore) where T : class {
		if (typeBefore == -1) {
			return;
		}
		for (int i = 0; i < Player.buffType.Length; i++) {
			int bufftype = Player.buffType[i];
			var modBuff = ModContent.GetModBuff(bufftype);
			if (modBuff == null) {
				continue;
			}
			if (modBuff is not T) {
				continue;
			}
			var buff = (BaseFoodBuff)modBuff;
			if (buff.TypeID == typeBefore) {
				Player.DelBuff(i);
				i--;
			}
		}
	}
}
