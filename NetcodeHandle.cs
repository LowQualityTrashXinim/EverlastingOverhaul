using Terraria;
using System.IO;
using Terraria.ID;
using EverlastingOverhaul.Common.Systems.ReviveSystem;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul
{
    partial class EverlastingOverhaul
    {
        internal enum MessageType : byte
        {
            PlayerStatsHandle,
            Revive,
            ReviveSync
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            byte playernumber;
            switch (msgType)
            {
                case MessageType.PlayerStatsHandle:
                //playernumber = reader.ReadByte();
                //PlayerStatsHandle statplayer = Main.player[playernumber].GetModPlayer<PlayerStatsHandle>();
                //statplayer.ReceivePlayerSync(reader);
                //if (Main.netMode == NetmodeID.Server)
                //{
                //    statplayer.SyncPlayer(-1, whoAmI, false);
                //}
                //break;

                case MessageType.Revive:
                case MessageType.ReviveSync:
                    {
                        // Read the player id from the packet
                        byte playerID = reader.ReadByte();

                        // Pass the reader to the RevivePlayer
                        Main.player[playerID].GetModPlayer<RevivePlayer>().ReceiveRevivePacket(reader);

                        // Check if this is a normal revive packet received on the server.
                        // => clients and singleplayer don't have to forward the packet.
                        // => if this is a sync of the player, vanilla will take care of netcode
                        //    by calling ModPlayer.SyncPlayer again.
                        if (Main.netMode != NetmodeID.Server || msgType == MessageType.ReviveSync)
                        {
                            break;
                        }

                        // Forward it to other clients
                        Main.player[playerID].GetModPlayer<RevivePlayer>().SendRevivePacket(-1, whoAmI);
                    }
                    break;
            }
        }
    }
}
