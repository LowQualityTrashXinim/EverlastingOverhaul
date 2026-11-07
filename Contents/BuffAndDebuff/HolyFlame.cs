using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.BuffAndDebuff;
internal class HolyFlame : ModBuff
{
    public override string Texture => ModTexture.EMPTYDEBUFF;
    public override void SetStaticDefaults()
    {
        this.BossRushSetDefaultDeBuff();
    }
    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().Poison_Inner.Flat += 30;
        npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().Poison_Inner += .2f;
        Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.IchorTorch);
        dust.velocity = Main.rand.NextVector2Circular(1, 1) + -Microsoft.Xna.Framework.Vector2.UnitY * Main.rand.NextFloat(3, 4);
        dust.scale = Main.rand.NextFloat(.88f, 1.22f);
        dust.noGravity = Main.rand.NextBool();
    }
}
