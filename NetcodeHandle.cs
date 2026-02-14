using Terraria;
using System.IO;
using Terraria.ID;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Contents.Items.NoneSynergy;

namespace EverlastingOverhaul {
	partial class EverlastingOverhaul
    {
		internal enum MessageType : byte {
			GambleAddiction,
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			MessageType msgType = (MessageType)reader.ReadByte();
			byte playernumber = reader.ReadByte();
			switch (msgType) {
				case MessageType.GambleAddiction:
					GamblePlayer gamble = Main.player[playernumber].GetModPlayer<GamblePlayer>();
					gamble.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						gamble.SyncPlayer(-1, whoAmI, false);
					}
					break;
			}
		}
	}
}
