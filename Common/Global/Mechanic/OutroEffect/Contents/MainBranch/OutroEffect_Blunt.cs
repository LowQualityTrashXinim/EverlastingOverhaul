using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents.MainBranch;
internal class OutroEffect_Blunt : OutroEffect
{
    public override void SetStaticDefaults()
    {
        Duration = ModUtils.ToSecond(30);
    }
    public override void WeaponDamage(Player player, Item item, ref StatModifier damage)
    {
        if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Blunt].Contains(item.type))
        {
            damage += .25f;
        }
    }
}
