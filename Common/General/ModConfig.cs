using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EverlastingOverhaul.Common.General
{
    public class EverlastingOverhaulConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        public bool HoldShift { get; set; }
    }
}
