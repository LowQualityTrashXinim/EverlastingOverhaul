using EverlastingOverhaul.Contents.Items;
using EverlastingOverhaul.Contents.Items.Weapon;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global;
internal class RoguelikeGlobalRecipe : ModSystem
{
    public override void AddRecipes()
    {
        base.AddRecipes();
    }
    public override void AddRecipeGroups()
    {
        RecipeGroup WoodSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Wood sword", new int[]
        {
                ItemID.WoodenSword,
                ItemID.BorealWoodSword,
                ItemID.RichMahoganySword,
                ItemID.ShadewoodSword,
                ItemID.EbonwoodSword,
                ItemID.PalmWoodSword,
                ItemID.PearlwoodSword,
        });
        RecipeGroup.RegisterGroup("Wood Sword", WoodSword);

        RecipeGroup WoodBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Wood bow", new int[]
        {
                ItemID.WoodenBow,
                ItemID.BorealWoodBow,
                ItemID.RichMahoganyBow,
                ItemID.ShadewoodBow,
                ItemID.EbonwoodBow,
                ItemID.PalmWoodBow,
                ItemID.PearlwoodBow,
        });
        RecipeGroup.RegisterGroup("Wood Bow", WoodBow);

        RecipeGroup OreShortSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore short sword", new int[]
        {
                ItemID.CopperShortsword,
                ItemID.TinShortsword,
                ItemID.IronShortsword,
                ItemID.LeadShortsword,
                ItemID.SilverShortsword,
                ItemID.TungstenShortsword,
                ItemID.GoldShortsword,
                ItemID.PlatinumShortsword,
        });
        RecipeGroup.RegisterGroup("Ore shortsword", OreShortSword);

        RecipeGroup OreBroadSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore broad sword", new int[]
        {
                ItemID.CopperBroadsword,
                ItemID.TinBroadsword,
                ItemID.IronBroadsword,
                ItemID.LeadBroadsword,
                ItemID.SilverBroadsword,
                ItemID.TungstenBroadsword,
                ItemID.GoldBroadsword,
                ItemID.PlatinumBroadsword,
        });
        RecipeGroup.RegisterGroup("Ore broadsword", OreBroadSword);

        RecipeGroup OreBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore Bow", new int[]
        {
                ItemID.CopperBow,
                ItemID.TinBow,
                ItemID.IronBow,
                ItemID.LeadBow,
                ItemID.SilverBow,
                ItemID.TungstenBow,
                ItemID.GoldBow,
                ItemID.PlatinumBow,
        });
        RecipeGroup.RegisterGroup("Ore bow", OreBow);
    }
    public override void PostAddRecipes()
    {
        //foreach (Recipe recipe in Main.recipe)
        //{
        //    SynergyRecipe(recipe);
        //}
    }

    private void SynergyRecipe(Recipe recipe)
    {
        if (recipe.createItem.ModItem is SynergyModItem)
        {
            recipe.AddIngredient(ModContent.ItemType<SynergyEnergy>());
        }
    }
}
