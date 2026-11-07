using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.BuffAndDebuff
{
    public class RA_Rotting : ModBuff
    {
        public override string Texture => ModTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            this.BossRushSetDefaultDeBuff();
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 20;
            npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().StatDefense.Base -= 5;
        }
    }
}
