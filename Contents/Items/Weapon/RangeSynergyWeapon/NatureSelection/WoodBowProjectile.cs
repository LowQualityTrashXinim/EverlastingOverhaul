 
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection
{
	internal class BorealWoodBowP : BaseBowTemplate {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.BorealWoodBow);
	}
	internal class EbonwoodBowP : BaseBowTemplate {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.EbonwoodBow);
	}
	internal class PalmWoodBowP : BaseBowTemplate {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.PalmWoodBow);
	}
	internal class RichMahoganyBowP : BaseBowTemplate {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.RichMahoganyBow);
	}
	internal class ShadewoodBowP : BaseBowTemplate {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.ShadewoodBow);
	}
	internal class WoodBowP : BaseBowTemplate {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.WoodenBow);
	}
}
