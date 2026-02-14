using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global;
class RoguelikeGlobalDust : ModSystem
{
    public override void Load()
    {
        On_Main.DrawDust += On_Main_DrawDust;
    }

    private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
    {
        //for (int i = 0; i < dust.Length; i++) {
        //	if (dust[i] == null) {
        //		continue;
        //	}
        //	if (dust[i].Dust == null) {
        //		continue;
        //	}
        //	if (!dust[i].Dust.active) {
        //		continue;
        //	}
        //	Dust dustEntity = dust[i].Dust;
        //	Roguelike_Dust modDust = dust[i];
        //	if (modDust.gfxOffY != 0) {
        //		dustEntity.position.Y += modDust.gfxOffY;
        //	}
        //}
        orig(self);
        //for (int i = 0; i < dust.Length; i++) {
        //	if (dust[i] == null) {
        //		continue;
        //	}
        //	if (dust[i].Dust == null) {
        //		continue;
        //	}
        //	if (!dust[i].Dust.active) {
        //		continue;
        //	}
        //	Dust dustEntity = dust[i].Dust;
        //	Roguelike_Dust modDust = dust[i];
        //	if (modDust.gfxOffY != 0) {
        //		dustEntity.position.Y -= modDust.gfxOffY;
        //	}
        //}
    }
    /// <summary>
    /// Use this to set trail length
    /// </summary>
    public static int[] TrailLength = DustID.Sets.Factory.CreateIntSet(10);
    public static Roguelike_Dust DeadDust => ModContent.GetInstance<RoguelikeGlobalDust>().deaddust;
    public Roguelike_Dust deaddust = new();
    public static Roguelike_Dust[] Dust => ModContent.GetInstance<RoguelikeGlobalDust>().dust;
    public Roguelike_Dust[] dust = new Roguelike_Dust[6001];
    public override void PreUpdateDusts()
    {
        if (dust.Length != Main.dust.Length)
        {
            Array.Resize(ref dust, Main.maxDust);
        }
        for (int i = 0; i < dust.Length; i++)
        {
            if (dust[i] == null)
            {
                dust[i] = new();
            }
            if (Main.dust[i].active)
            {
                dust[i].SetDust(ref Main.dust[i]);
                dust[i].WhoAmI = i;
                if (dust[i].oldPos == null)
                {
                    dust[i].oldPos = new Vector2[TrailLength[Main.dust[i].type]];
                    dust[i].oldRot = new float[TrailLength[Main.dust[i].type]];
                }
                ModUtils.Push(ref dust[i].oldPos, Main.dust[i].position);
                ModUtils.Push(ref dust[i].oldRot, Main.dust[i].rotation);
                dust[i].orgPosition = Main.dust[i].position;
            }
            else
            {
                dust[i].Clear();
            }
        }
    }
    public override void PostUpdateDusts()
    {
        for (int i = 0; i < dust.Length; i++)
        {
            if (dust[i].Dust == null)
            {
                dust[i].Clear();
                continue;
            }
            if (!dust[i].Dust.active)
            {
                dust[i].Clear();
                continue;
            }
            var dustEntity = dust[i].Dust;
            var modDust = dust[i];
            if (modDust.FollowEntity)
            {
                if (modDust.entityToFollow != null)
                {
                    if (modDust.entityToFollow is Player player)
                    {
                        modDust.gfxOffY = player.gfxOffY;
                        if (modDust.OTEdistance == Vector2.Zero)
                        {
                            modDust.OTEdistance = player.Center - modDust.orgPosition;
                        }
                    }
                    if (modDust.entityToFollow is Projectile projectile)
                    {
                        if (modDust.OTEdistance == Vector2.Zero)
                        {
                            modDust.OTEdistance = projectile.Center - modDust.orgPosition;
                        }
                    }
                    modDust.OTEdistance += modDust.Dust.velocity;
                    dustEntity.position = modDust.entityToFollow.Center + modDust.OTEdistance.Add(0, -modDust.gfxOffY) - modDust.entityToFollow.velocity;
                }
            }
        }
    }

}
public class Roguelike_Dust : ICloneable
{
    public Dust Dust = null;
    /// <summary>
    /// By default is set to null
    /// This will make sure some interaction know which dust is belong to or should follow who
    /// </summary>
    public Entity entityToFollow = null;
    public int WhoAmI = -1;
    public bool FollowEntity = false;
    public float gfxOffY = 0;
    public Vector2 orgPosition = Vector2.Zero;
    public Vector2 OTEdistance = Vector2.Zero;
    public Vector2[] oldPos = null;
    public float[] oldRot = null;
    public void Clear()
    {
        WhoAmI = -1;
        entityToFollow = null;
        orgPosition = Vector2.Zero;
        OTEdistance = Vector2.Zero;
        gfxOffY = 0;
        FollowEntity = false;
        Dust = null;
        if (oldPos != null)
        {
            Array.Clear(oldPos);
            Array.Clear(oldRot);
        }
    }
    public object Clone()
    {
        return MemberwiseClone();
    }
    public void SetDust(ref Dust dust)
    {
        Dust = dust;
    }
}

public class Roguelike_Dust_ModDust3x3 : ModDust
{
    public override sealed string Texture => ModTexture.dust_3x3;
}

public class Roguelike_Dust_ModDust5x5T1 : ModDust
{
    public override sealed string Texture => ModTexture.dust_5x5Type1;
}
public class Roguelike_Dust_ModDust5x5T2 : ModDust
{
    public override sealed string Texture => ModTexture.dust_5x5Type2;
}
