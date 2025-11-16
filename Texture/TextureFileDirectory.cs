namespace EverlastingOverhaul.Texture
{
    public static class ModTexture
    {
        public const string CommonTextureStringPattern = "EverlastingOverhaul/Texture/";
        public const string MissingTexture_Folder = "MissingTexture/";
        public const string PinIcon = CommonTextureStringPattern + "UI/PinIcon";

        public const string WHITEDOT = CommonTextureStringPattern + "WhiteDot";
        public const string MISSINGTEXTUREPOTION = CommonTextureStringPattern + "MissingTexturePotion";
        public const string EMPTYBUFF = CommonTextureStringPattern + "EmptyBuff";
        public const string EMPTYDEBUFF = CommonTextureStringPattern + "EmptyDeBuff";
        public const string PLACEHOLDERCHEST = CommonTextureStringPattern + "PlaceHolderTreasureChest";
        public const string WHITEBALL = CommonTextureStringPattern + "WhiteBall";
        public const string DIAMONDSWOTAFFORB = CommonTextureStringPattern + "DiamondSwotaffOrb";
        public const string ACCESSORIESSLOT = "Terraria/Images/Inventory_Back7";
        public const string MENU = CommonTextureStringPattern + "UI/menu";
        public const string SMALLWHITEBALL = CommonTextureStringPattern + "smallwhiteball";
        public const string EMPTYCARD = CommonTextureStringPattern + "EmptyCard";
        public const string EXAMPLEUI = CommonTextureStringPattern + "ExampleFrame";
        public const string SUPPILESDROP = CommonTextureStringPattern + "SuppliesDrop";
        public const string FOURSTAR = CommonTextureStringPattern + "FourStar";
        public const string CrossSprite = CommonTextureStringPattern + "UI/Cross";
        public const string Lock = CommonTextureStringPattern + "UI/lock";
        public const string Perlinnoise = CommonTextureStringPattern + "roguelikePerlinNoise";
        public const string Arrow_Left = CommonTextureStringPattern + "UI/LeftArrow";
        public const string Arrow_Right = CommonTextureStringPattern + "UI/RightArrow";
        public const string PingpongGradient = CommonTextureStringPattern + "PingpongGradient";
        public const string PingpongGradient2 = CommonTextureStringPattern + "PingpongGradient2";
        public const string LinesNoise = CommonTextureStringPattern + "LinesNoise";
        public const string Gradient = CommonTextureStringPattern + "Gradient";
        public const string OuterInnerGlow = CommonTextureStringPattern + "OuterInnerGlow";
        public const string Eye = CommonTextureStringPattern + "EyeOutline";
        public const string EyePupil = CommonTextureStringPattern + "EyePupil";
        /// <summary>
        /// Width : 16 | Height : 16
        /// </summary>
        public const string Boxes = CommonTextureStringPattern + "UI/Boxes";
        public const string QuestionMark_Help = CommonTextureStringPattern + "UI/Help";
        public const string Page_StateSelected = CommonTextureStringPattern + "UI/page_selected";
        public const string Page_StateUnselected = CommonTextureStringPattern + "UI/page_unselected";
        public const string DrawBrush = CommonTextureStringPattern + "UI/Brush";
        public const string BackIcon = CommonTextureStringPattern + "UI/BackIcon";
        public const string FillBucket = CommonTextureStringPattern + "UI/FillBucket";
        public const string Wave = CommonTextureStringPattern + "Wave";
        public static string Get_MissingTexture(string text) => CommonTextureStringPattern + MissingTexture_Folder + $"{text}MissingTexture";
        public const string MissingTexture_Default = CommonTextureStringPattern + MissingTexture_Folder + "MissingTextureDefault";
    }
}
