using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace EverlastingOverhaul.Common.Systems;
/// <summary>
/// This not only include main stuff that make everything work but also contain some fixes to vanilla<br/>
/// Also, very unholy class, do not look into it
/// </summary>
internal class UniversalSystem : ModSystem {

	internal UserInterface userInterface;
	internal UserInterface user2ndInterface;

	public DefaultUI defaultUI;

	public static bool EnchantingState = false;
	public static ModKeybind WeaponActionKey { get; private set; }
	public TimeSpan timeBeatenTheGame = TimeSpan.Zero;
	public override void Load() {
		WeaponActionKey = KeybindLoader.RegisterKeybind(Mod, "Weapon action", Keys.X);
		//UI stuff
		if (!Main.dedServ) {

			defaultUI = new();
			user2ndInterface = new();
			userInterface = new();
		}
	}
	public override void Unload() {
		WeaponActionKey = null;

		defaultUI = null;

		userInterface = null;
		user2ndInterface = null;
	}
	public override void UpdateUI(GameTime gameTime) {
		userInterface?.Update(gameTime);
		user2ndInterface?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"EverlastingOverhaul: UI",
				delegate {
					GameTime gametime = new GameTime();
					userInterface.Draw(Main.spriteBatch, gametime);
					user2ndInterface.Draw(Main.spriteBatch, gametime);
					return true;
				},
				InterfaceScaleType.UI)
			);
	}

	public void DeactivateUI() {
		user2ndInterface.SetState(null);
	}
	private static string cachedstringeffect = string.Empty;
	public static string GetRandomGlitchyNameEffect(int delay) {
		if ((int)Main.time % delay == 0) {
			int length = Main.rand.Next(20, 25);
			cachedstringeffect = string.Empty;
			for (int i = 0; i < length; i++) {
				byte b = (byte)(Main.rand.Next() % 256);
				cachedstringeffect += (char)b;
			}
		}
		return cachedstringeffect;
	}
	public bool IsAttemptingToBringItemToNewPlayer = false;
	public string WorldState = "";
	public override void OnWorldUnload() {
		WorldState = "Exited";
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.DeactivateUI();
		timeBeatenTheGame = TimeSpan.Zero;
	}
	public static bool AnyVanillaBossAlive = false;
	public override void PreUpdateEntities() {
		AnyVanillaBossAlive = ModUtils.IsAnyVanillaBossAlive();
	}
}
/// <summary>
/// This is a always active UI, this act as the most basic form of UI where it will always be active<br/>
/// To not to be confused with actual default UI, anything that should be always active regardless of UI should goes here
/// </summary>
public class DefaultUI : UIState {

	UIImage_OutroEffectShower WeaponEff;
    public Roguelike_WeaponUIFrame WeaponBar;
    public override void OnInitialize() {
		WeaponEff = new();
		WeaponEff.HAlign = .44f;
		WeaponEff.VAlign = .02f;
        WeaponEff.Width.Pixels = 52;
        WeaponEff.Height.Pixels = 52;
        Append(WeaponEff);

        WeaponBar = new Roguelike_WeaponUIFrame();
        WeaponBar.VAlign = .535f;
        WeaponBar.HAlign = .5f;
        WeaponBar.Width.Set(100, 0);
        WeaponBar.Height.Set(20, 0);
        Append(WeaponBar);
    }
	public override void OnActivate() {
	}

	public override void Update(GameTime gameTime) {
		TimeSpan time = Main.ActivePlayerFileData.GetPlayTime();
		UniversalSystem system = ModContent.GetInstance<UniversalSystem>();
		base.Update(gameTime);
	}
}
/// <summary>
/// Special kind of UI that most likely only used once through out the mod
/// </summary>
public class Roguelike_WeaponUIFrame : UIElement
{
    protected Asset<Texture2D> texture;
    public float barProgress;
    public bool Hide = false;
    public bool HideBar = false;
    public bool HideText = false;
    private int Delay = 0;
    public Color gradientA, gradientB;
    public void DelayHide(int HideDelay)
    {
        if (Delay <= 0 && !Hide)
        {
            Delay = HideDelay;
        }
        else
        {
            Delay = ModUtils.CountDown(Delay);
            if (Delay <= 1)
            {
                Hide = true;
            }
        }
    }
    public Roguelike_WeaponUIFrame()
    {
        texture = ModContent.Request<Texture2D>(ModTexture.Weapon_FrameUI);
        barFrame = new UIImage(texture); // Frame of our resource bar
        Append(barFrame);
        barFrame.UISetWidthHeight(100, 20);
    }
    private UIImage barFrame;
    WeaponProgress inner_progress = null;
    public void SetWeaponProgress(WeaponProgress progress)
    {
        if (inner_progress != null)
        {
            if (inner_progress.ItemType != progress.ItemType)
            {
                inner_progress = progress;
            }
            Hide = false;
        }
        inner_progress = progress;
    }
    public float BarProgress { get => barProgress; set => barProgress = value; }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (inner_progress != null)
        {
            if (inner_progress.ItemType != Main.LocalPlayer.HeldItem.type)
            {
                Hide = true;
            }
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (inner_progress == null)
        {
            return;
        }
        if (Hide)
        {
            return;
        }
        base.Draw(spriteBatch);
    }
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (inner_progress == null)
        {
            return;
        }
        if (Hide)
        {
            return;
        }
        base.DrawSelf(spriteBatch);
        DrawBarUI(spriteBatch);
    }
    private void DrawBarUI(SpriteBatch spriteBatch)
    {
        float quotient = barProgress;
        quotient = Math.Clamp(quotient, 0f, 1f);

        // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
        Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
        hitbox.Width -= 2;

        // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
        int left = hitbox.Left;

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left, hitbox.Y, hitbox.Width, hitbox.Height), Color.White.ScaleRGB(.1f) with { A = 100 });

        foreach (var item in inner_progress.Setting)
        {
            int starter = left + (int)(hitbox.Width * item.p1);
            int ender = left + (int)(hitbox.Width * item.p2);
            for (int i = starter; i < ender; i++)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(i, hitbox.Y, 1, hitbox.Height), item.color);
            }
        }
        if (inner_progress.Charge)
        {
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i++)
            {
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }
        else
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + (int)(hitbox.Width * quotient), hitbox.Y, 2, hitbox.Height), Color.White);
        }
    }
}
public class WeaponProgress
{
    public int ItemType = 0;
    /// <summary>
    /// This mean that this weapon doesn't contain timing base
    /// </summary>
    public bool Charge = false;
    public List<ProgressInfo> Setting { get; private set; } = new();
    public WeaponProgress()
    {

    }
    public void Set_Progress(float progress)
    {
        Setting.Add(new(progress, progress, Color.White));
    }
    public void Set_Progress(float p1, float p2, Color color)
    {
        Setting.Add(new(p1, p2, color));
    }
}
public struct ProgressInfo
{
    public float p1, p2;
    public Color color;
    public ProgressInfo()
    {

    }
    public ProgressInfo(float progress1, float progress2, Color progressColor)
    {
        p1 = progress1;
        p2 = progress2;
        color = progressColor;
    }
}