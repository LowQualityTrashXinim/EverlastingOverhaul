using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
internal class Roguelike_HallowedWeapon : GlobalItem {
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.Excalibur || item.type == ItemID.TrueExcalibur) {
			target.AddBuff<HallowedGaze>(ModUtils.ToSecond(3));
		}
	}
}
public class Roguelike_HallowedWeapon_Projectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.HallowedGaze].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<HallowedGaze>(ModUtils.ToSecond(5));
		}
	}
}
public class Roguelike_HallowedGaze_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HallowedGaze_Count = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<HallowedGaze>()) {
			modifiers.SourceDamage += .05f * HallowedGaze_Count;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<HallowedGaze>()) {
			modifiers.SourceDamage += .05f * HallowedGaze_Count;
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.owner != Main.myPlayer) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (npc.HasBuff<HallowedGaze>()) {
			if (HallowedGaze_Count >= 12) {
				Vector2 playerPos = player.Center;
				Vector2 pos = new Vector2(npc.Center.X + Main.rand.Next(-100, 100), playerPos.Y - 800);
				Projectile.NewProjectile(projectile.GetSource_FromAI(), pos, npc.Center - pos, ModContent.ProjectileType<HitScanShotv2>(), 1, 0, player.whoAmI);
			}
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (npc.HasBuff<HallowedGaze>()) {
			if (HallowedGaze_Count >= 12) {
				Vector2 playerPos = player.Center;
				Vector2 pos = new Vector2(npc.Center.X + Main.rand.Next(-100, 100), playerPos.Y - 800);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, npc.Center - pos, ModContent.ProjectileType<HitScanShotv2>(), 1, 0, player.whoAmI);
			}
		}
	}
}
internal class HallowedGaze : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		npc.buffTime[buffIndex] = time;
		npc.GetGlobalNPC<Roguelike_HallowedGaze_GlobalNPC>().HallowedGaze_Count++;
		return base.ReApply(npc, time, buffIndex);
	}
}
internal class HitScanShotv2 : ModProjectile
{
    public override string Texture => ModTexture.MissingTexture_Default;
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 1;
        Projectile.penetrate = 2;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 120;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 4;
        Projectile.stopsDealingDamageAfterPenetrateHits = true;
        Projectile.scale = 4;
    }
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (Projectile.timeLeft < 15)
        {
            return false;
        }
        if (!Collision.CanHitLine(Projectile.Center, 1, 1, targetHitbox.Center(), 1, 1))
        {
            return false;
        }
        return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center, Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 3000));
    }
    Vector2 scaleVec = Vector2.One * 5;
    public override void AI()
    {
        if (Projectile.timeLeft == 120)
        {
            SoundStyle ExampleGunSoundStyle = new SoundStyle("EverlastingOverhaul/Assets/SFX/HallowedGaze");
            SoundEngine.PlaySound(ExampleGunSoundStyle, Projectile.Center);
            scaleVec = Vector2.One * 10;
            scaleVec *= Projectile.scale;
            Vector2 toMouse = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Projectile.velocity = Vector2.Zero;
            Projectile.ai[0] = toMouse.X;
            Projectile.ai[1] = toMouse.Y;
            Projectile.rotation = toMouse.ToRotation() - MathHelper.PiOver2;
            Projectile.ai[2] = 2515;
            //When projectile can stop
            scaleVec.Y = Projectile.ai[2] * .001f;
        }

        scaleVec.X *= .9f;
        if (scaleVec.X <= 0)
        {
            Projectile.Kill();
        }
    }
    public override bool? CanDamage()
    {
        return Projectile.penetrate > 1;
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        float damage = Math.Clamp(target.life * .01f, 10, 2000);

        modifiers.SourceDamage.Base += damage;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.GetGlobalNPC<Roguelike_HallowedGaze_GlobalNPC>().HallowedGaze_Count = 0;
    }
    Vector2 ToMouseDirection => new(Projectile.ai[0], Projectile.ai[1]);
    public override bool PreDraw(ref Color lightColor)
    {
        //Ain't the best way
        Vector2 drawpos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) + Vector2.One * .5f;
        Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.White, Projectile.rotation, Vector2.One * .5f, scaleVec, SpriteEffects.None);
        return false;
    }
}
