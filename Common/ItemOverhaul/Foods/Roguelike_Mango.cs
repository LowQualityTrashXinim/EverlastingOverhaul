using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Mango : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Mango;
	public override int EnergyAmount() => 92;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(2.75f);
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_Mango_Buff>(), ModUtils.ToMinute(7));
	}
}
public class Roguelike_Mango_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Mango;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_Mango_ModPlayer>().Mango = true;
		if (player.buffTime[buffIndex] <= 0) {
			int index = player.FindBuffIndex(ModContent.BuffType<Roguelike_Mango_ModBuff>());
			if (index != -1 && index < player.buffTime.Length) {
				player.DelBuff(index);
			}
		}
	}
}
public class Roguelike_Mango_ModBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		var modplayer = player.GetModPlayer<Roguelike_Mango_ModPlayer>();
		modplayer.HasBuff = true;
		//player.GetModPlayer<SkillHandlePlayer>().skilldamage += modplayer.Stack * .1f;
		if (player.buffTime[buffIndex] <= 0) {
			modplayer.Stack = 0;
			modplayer.StackCounter.Clear();
		}
	}
	public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
		int TypeID = ItemID.Mango;
		Main.instance.LoadItem(TypeID);
		Texture2D bgsTexture = drawParams.Texture;
		Vector2 size = bgsTexture.Size();
		Texture2D texture = TextureAssets.Item[TypeID].Value;
		Vector2 size2 = texture.Size();
		Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
		Vector2 drawpos = drawParams.Position + size * .5f;
		spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3), drawParams.DrawColor, 0, size3 * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size, size3, .67f), SpriteEffects.None, 0);
		var modplayer = Main.LocalPlayer.GetModPlayer<Roguelike_Mango_ModPlayer>();
		Terraria.Utils.DrawBorderString(spriteBatch, modplayer.Stack.ToString(), drawpos + Vector2.One * 10, drawParams.DrawColor, 1);
	}
}
public class Roguelike_Mango_ModPlayer : ModPlayer {
	public bool Mango = false;
	public int Stack = 0;
	public bool HasBuff = false;
	public List<int> StackCounter = new();
	public override void OnEnterWorld() {
		StackCounter.Clear();
		Stack = 0;
	}
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		Mango = false;
		HasBuff = false;
		if (Stack <= 0) {
			return;
		}
		for (int i = StackCounter.Count - 1; i >= 0; i--) {
			StackCounter[i] = ModUtils.CountDown(StackCounter[i]);
			if (StackCounter[i] <= 0) {
				StackCounter.RemoveAt(i);
				Stack--;
			}
		}
	}
	public int Timer = 0;
	public override void UpdateEquips() {
		if (!Mango) {
			return;
		}
		if (++Timer >= ModUtils.ToSecond(3)) {
			Timer = 0;
			Item.NewItem(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(500, 500), ModContent.ItemType<Roguelike_Mango_Pickup>());
		}
	}
}
public class Roguelike_Mango_Pickup : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Mango);
	public override void SetDefaults() {
	}
	public override bool OnPickup(Player player) {
		var modplayer = player.GetModPlayer<Roguelike_Mango_ModPlayer>();
		if (modplayer.Stack < 5) {
			modplayer.Stack++;
			modplayer.StackCounter.Add(ModUtils.ToSecond(15));
			if (!modplayer.HasBuff) {
				player.AddBuff<Roguelike_Mango_ModBuff>(ModUtils.ToSecond(15));
			}
		}
		return false;
	}
	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
		Main.instance.LoadItem(Type);
		Texture2D texture = TextureAssets.Item[Type].Value;
		Vector2 size2 = texture.Size();
		Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
		Vector2 drawpos = Main.item[whoAmI].position - Main.screenPosition;
		spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3, 0, 2), lightColor, 0, size3 * .5f, scale, SpriteEffects.None, 0);
		return false;
	}
}
