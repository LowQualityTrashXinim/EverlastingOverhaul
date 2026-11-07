using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using EverlastingOverhaul.Common.General;
using EverlastingOverhaul.Common.Systems;
using Microsoft.Xna.Framework.Input;

namespace Roguelike.Common.Global;
public class ProcessTriggerSystem : ModSystem
{
    public static ModKeybind SkillActivation { get; private set; }
    public static ModKeybind Open_SkillUI { get; private set; }
    public static ModKeybind Open_DivineUI { get; private set; }
    public static ModKeybind Open_TransmutateUI { get; private set; }
    public static ModKeybind Key_Dash { get; private set; }
    public override void Load()
    {
        Key_Dash = KeybindLoader.RegisterKeybind(Mod, "Dash", Keys.Z);
    }
    public override void Unload()
    {
        SkillActivation = null;
        Open_SkillUI = null;
        Open_DivineUI = null;
        Open_TransmutateUI = null;
    }
}
internal class ProcessTriggerPlayer : ModPlayer
{
    public bool Hold_Shift = false;
    public bool Press_Shift = false;
    public bool Pressed_Shift = false;
    public bool Shift_Option()
    {
        if (ModContent.GetInstance<EverlastingOverhaulConfig>().HoldShift)
        {
            return Hold_Shift;
        }
        else
        {
            return Press_Shift;
        }
    }
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Main.playerInventory)
        {
            Hold_Shift = triggersSet.SmartSelect;
            if (triggersSet.SmartSelect)
            {
                if (!Pressed_Shift)
                {
                    Press_Shift = !Press_Shift;
                }
                Pressed_Shift = true;
            }
            else
            {
                Pressed_Shift = false;
            }
        }
    }
}
