using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Graphics.Structs.TrailStructs;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul
{
    public class BossRushUseStyle
    {
        /// <summary>
        /// This is for general sword swing uses, this use style automatically handle itself so no modification needed<br/>
        /// If your sword have a weird hand offset, it is recommend to enable UseSwipeTwo in <see cref="MeleeOverhaulSystem"/>
        /// </summary>
        public const byte Swipe = byte.MaxValue;
        //These below are more for customization in combo attack animation that still want to use the overhaul system
        public const byte Spin = 254;
        public const byte Thrust = 253;
        public const byte SwipeDown = 252;
        public const byte SwipeUp = 251;
        public const byte RapidThurst = 250;
        public const byte Thrust_Style1 = 249;
        /// <summary>
        /// Use this so that you can implement custom useStyle
        /// </summary>
        public const int None = 0;
    }
    internal class MeleeWeaponOverhaul : GlobalItem
    {
        public byte SwingType = 0;
        public int PostSwingAnimationType = 0;
        public float SwingStrength = 7f;
        /// <summary>
        /// This will offset the animation percentage so that it create a still like sword
        /// </summary>
        public float OffSetAnimationPercentage = 1;
        /// <summary>
        /// This is the swing degree of the weapon, default value is 140
        /// </summary>
        public float SwingDegree = 140;
        /// <summary>
        /// use this if your swing type is <see cref="BossRushUseStyle.RapidThurst"/>
        /// </summary>
        public int ThrustAmount = 3;
        /// <summary>
        /// the higher the value, the less i-frame the attack will give
        /// </summary>
        public float IframeDivision = 1;
        /// <summary>
        /// Enable this if your sword have a weird offset, this will attempt to fix it
        /// </summary>
        public bool UseSwipeTwo = false;
        /// <summary>
        /// Use this if the SwingType is Spin<br/>
        /// This will tell the code how much should the player spin the weapon
        /// </summary>
        public float CircleSwingAmount = 1;
        /// <summary>
        /// This will offset the animation time so that the item animation won't start after certain frame
        /// </summary>
        public int ItemAnimationStarTimeOffset = 5;
        /// <summary>
        /// Use this to set animation end duration, the animation end will offset the item animation timer
        /// </summary>
        public int AnimationEndTime = 0;
        public float StartRotation = 0;
        public float RotationAfterMainAnimationEnd = 0;
        public Vector2 PositionAfterMainAnimationEnd = Vector2.Zero;
        public float ShaderOffSetLength = 0;
        public Vector2 scaleWarp = Vector2.One;
        public bool Ignore_AttackSpeed = false;
        /// <summary>
        /// This will temporary disable attack animation during player item animation active
        /// </summary>
        public bool DisableAttackAnimation = false;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            scaleWarp = new(item.scale);
            if (AnimationEndTime != 0)
            {
                item.useAnimation += AnimationEndTime;
                item.useTime += AnimationEndTime;
            }
            if (Ignore_AttackSpeed)
            {
                ItemID.Sets.BonusAttackSpeedMultiplier[item.type] = 0;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ItemID.Sets.BonusAttackSpeedMultiplier[item.type] == 0)
            {
                tooltips.Add(new(Mod, "", "This weapon use speed doesn't get affected by attack speed stats"));
            }
        }
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            //Since we are using entirely new collision detection, we no longer need this
            if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item))
            {
                ModUtils.ModifyProjectileDamageHitbox(ref hitbox, player, item.width, item.height);
            }
        }
        public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target)
        {
            if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item))
            {
                float extra = 0;
                if (target.boss)
                {
                    extra += .25f;
                }
                var texture = TextureAssets.Item[item.type];
                float itemlength = texture.Value.Size().Length();
                float itemsize = itemlength * (player.GetAdjustedItemScale(player.HeldItem) + extra);
                int laserline = (int)itemsize;
                if (laserline <= 0)
                {
                    laserline = 1;
                }
                var modplayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                var utilsplayer = player.GetModPlayer<ModUtilsPlayer>();
                if (SwingType == BossRushUseStyle.Spin)
                {
                    for (int i = 0; i <= laserline; i++)
                    {
                        var point = player.Center + Vector2.One.Vector2DistributeEvenly(laserline, 360, i) * itemsize * .75f;
                        if (ModUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point))
                        {
                            target.immune[player.whoAmI] = (int)Math.Round(player.itemAnimationMax / CircleSwingAmount);
                            return true;
                        }
                    }
                }
                else if (SwingType == BossRushUseStyle.Swipe || SwingType == BossRushUseStyle.SwipeUp || SwingType == BossRushUseStyle.SwipeDown)
                {
                    var offset = player.Center - utilsplayer.PlayerLastPositionBeforeAnimation;
                    var directionTo = (utilsplayer.MouseLastPositionBeforeAnimation + offset - player.Center).SafeNormalize(Vector2.Zero);
                    if (player.itemAnimationMax <= 2)
                    {
                        for (int i = 0; i <= laserline; i++)
                        {
                            var point = player.Center + directionTo.Vector2DistributeEvenly(laserline, SwingDegree * 2, i) * itemsize;
                            if (ModUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    bool checkComboNum = modplayer.ComboNumber == 0 || SwingType == BossRushUseStyle.SwipeDown;
                    int LastCollideCheck, check;
                    float PreviousProgress = ModUtils.InExpo((player.itemAnimation + 1) / (float)player.itemAnimationMax, SwingStrength);
                    float CurrentProgress = ModUtils.InExpo(player.itemAnimation / (float)player.itemAnimationMax, SwingStrength);
                    if (checkComboNum && player.direction == 1 || !checkComboNum && player.direction == -1)
                    {
                        LastCollideCheck = (int)Math.Ceiling(MathHelper.Lerp(0, laserline, PreviousProgress));
                        check = (int)Math.Ceiling(MathHelper.Lerp(0, laserline, CurrentProgress));
                    }
                    else
                    {
                        LastCollideCheck = (int)Math.Ceiling(MathHelper.Lerp(laserline, 0, PreviousProgress));
                        check = (int)Math.Ceiling(MathHelper.Lerp(laserline, 0, CurrentProgress));
                    }
                    int assigned = Math.Min(LastCollideCheck, check);
                    int length = Math.Max(check, LastCollideCheck);
                    for (int i = assigned; i <= length; i++)
                    {
                        var point = player.Center + directionTo.Vector2DistributeEvenly(laserline, SwingDegree * 2, i) * itemsize;
                        if (ModUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (SwingType == BossRushUseStyle.Spin)
            {
                modifiers.HitDirectionOverride = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, target.Center.X);
            }
        }
        public override bool? CanAutoReuseItem(Item item, Player player)
        {
            if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) && player.ItemAnimationActive)
            {
                ModdedUseStyle(item, player);
            }
            return base.CanAutoReuseItem(item, player);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) && player.ItemAnimationActive)
            {
                ModdedUseStyle(item, player);
                StartRotation = player.itemRotation;
            }
            return base.CanUseItem(item, player);
        }
        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if (DisableAttackAnimation)
            {
                return;
            }
            if (item.type == player.HeldItem.type || item.type == Main.mouseItem.type)
            {
                if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item))
                {
                    if (player.itemAnimation <= AnimationEndTime && player.altFunctionUse != 2)
                    {
                        if (Main.mouseLeftRelease)
                        {
                            player.itemAnimation = 0;
                            player.SetItemTime(0);
                        }
                        if (item.axe > 0)
                        {
                            float rotation = MathHelper.ToRadians(50) * player.direction;
                            if (player.itemAnimation <= 5)
                            {
                                player.itemRotation = RotationAfterMainAnimationEnd + rotation;
                                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3));
                                player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3)) + Vector2.UnitX.RotatedBy(player.itemRotation);
                                return;
                            }
                            if (player.itemAnimation <= 25)
                            {
                                player.itemRotation = RotationAfterMainAnimationEnd + rotation * (1 - (player.itemAnimation - 5) / 20f);
                                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3));
                                player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3)) + Vector2.UnitX.RotatedBy(player.itemRotation);
                            }
                        }
                    }
                    else
                    {
                        ModdedUseStyle(item, player);
                    }
                }
            }
        }
        public void ModdedUseStyle(Item item, Player player)
        {
            if (SwingStrength == 0)
            {
                SwingStrength = 7;
            }
            var modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            modPlayer.CountDownToResetCombo = (int)(player.itemAnimationMax * 1.35f);
            switch (SwingType)
            {
                case BossRushUseStyle.Swipe:
                    switch (modPlayer.ComboNumber)
                    {
                        case 0:
                            SwipeAttack(player, 1, SwingDegree, SwingStrength, OffSetAnimationPercentage);
                            break;
                        case 1:
                            SwipeAttack(player, -1, SwingDegree, SwingStrength, OffSetAnimationPercentage);
                            break;
                        case 2:
                            CircleSwingAttack(player);
                            break;
                    }
                    break;
                case BossRushUseStyle.SwipeDown:
                    SwipeAttack(player, 1, SwingDegree, SwingStrength, OffSetAnimationPercentage);
                    break;
                case BossRushUseStyle.SwipeUp:
                    SwipeAttack(player, -1, SwingDegree, SwingStrength, OffSetAnimationPercentage);
                    break;
                case BossRushUseStyle.Spin:
                    CircleSwingAttack(player, CircleSwingAmount);
                    break;
                case BossRushUseStyle.Thrust:
                    Thrust(player, modPlayer, OffsetThrust, DistanceThrust, SwingStrength);
                    break;
                case BossRushUseStyle.Thrust_Style1:
                    Thrust(player, modPlayer, OffsetThrust, DistanceThrust, SwingStrength, 1);
                    break;
                case BossRushUseStyle.RapidThurst:
                    RapidThrust(item, player, modPlayer, ThrustAmount, OffsetThrust, DistanceThrust, SwingStrength);
                    break;
                default:
                    break;
            }
            RotationAfterMainAnimationEnd = player.itemRotation;
            PositionAfterMainAnimationEnd = player.itemLocation;
        }
        public float DistanceThrust = 30;
        public float OffsetThrust = 0;
        public bool HideSwingVisual = false;
        private static void Thrust(Player player, MeleeOverhaulPlayer modPlayer, float offset = 0, float distance = 30, float swingStr = 11, int extra = 0)
        {
            float percentDone = 1 - player.itemAnimation / (float)player.itemAnimationMax;
            if (extra == 0)
            {
                if (player.itemAnimation <= player.itemAnimationMax / 2)
                {
                    percentDone = player.itemAnimation / (float)player.itemAnimationMax;
                }
                percentDone = ModUtils.InOutExpo(percentDone, swingStr);
            }
            else if (extra == 1)
            {
                percentDone = ModUtils.InOutExpo(1 - percentDone, swingStr);
            }
            Poke2(player, modPlayer, percentDone, offset, distance);
        }
        /// <summary>
        /// Massively WIP do not use
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <param name="modPlayer"></param>
        /// <param name="thrustAmount"></param>
        /// <param name="offset"></param>
        /// <param name="distance"></param>
        /// <param name="swingStr"></param>
        private static void RapidThrust(Item item, Player player, MeleeOverhaulPlayer modPlayer, int thrustAmount, float offset = 0, float distance = 30, float swingStr = 11)
        {
            float NormalizePercentage = MathF.Round(1 / (float)thrustAmount, 2);
            int currentThrust = (int)((1 - player.itemAnimation / (float)player.itemAnimationMax) / NormalizePercentage) + 1;
            float percentDone = 1 - player.itemAnimation / (float)player.itemAnimationMax;
            if (player.itemAnimation <= player.itemAnimationMax / 2)
            {
                percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            }
            percentDone = ModUtils.InOutExpo(percentDone, swingStr);
            if (ItemID.Sets.Spears[item.type])
            {
                modPlayer.NormalizeThrustAmount = NormalizePercentage;
                modPlayer.CurrentThrust = currentThrust;
                modPlayer.PercentageHandle = percentDone;
            }
            else
            {
                Poke2(player, modPlayer, percentDone, offset, distance);
            }
        }
        private static void Poke2(Player player, MeleeOverhaulPlayer modPlayer, float percentDone, float offset, float distance)
        {
            var mouse = modPlayer.PlayerToMouseDirection;
            player.itemRotation = mouse.ToRotation();
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, mouse.ToRotation() - MathHelper.PiOver2);
            player.itemLocation = player.Center.PositionOFFSET(mouse, MathHelper.Lerp(-offset, distance, percentDone));
        }
        private static void SwipeAttack(Player player, int direct, float swingDegree = 135, float strength = 7f, float animationSpeedMulti = 1)
        {
            var modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            if (player.itemAnimation == player.itemAnimationMax)
            {
                modPlayer.itemAnimationImproved = player.itemAnimationMax;
            }
            float percentDone = modPlayer.itemAnimationImproved / player.itemAnimationMax;
            percentDone = Math.Clamp(ModUtils.InExpo(percentDone, strength), -.1f, 1.1f);
            float baseAngle = modPlayer.PlayerToMouseDirection.ToRotation();
            float angle = MathHelper.ToRadians(swingDegree) * player.direction;
            float start = baseAngle + angle * direct;
            float end = baseAngle - angle * direct;
            Swipe(start, end, percentDone, player, direct);
            modPlayer.itemAnimationImproved -= 1 * animationSpeedMulti;
        }
        private static void CircleSwingAttack(Player player, float spinAmount = 1)
        {
            float percentDone = 1 - player.itemAnimation / (float)player.itemAnimationMax;
            float end = MathHelper.TwoPi * spinAmount * player.direction;
            float baseAngle = player.GetModPlayer<MeleeOverhaulPlayer>().PlayerToMouseDirection.ToRotation() + MathHelper.Pi;
            Swipe(baseAngle, end + baseAngle, ModUtils.InOutExpo(percentDone), player, 1);
        }
        private static void Swipe(float start, float end, float percentDone, Player player, int direct)
        {
            bool directIsnegative = direct == -1;
            float currentAngle = MathHelper.Lerp(start, end, percentDone);
            player.itemRotation = currentAngle;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
            if (directIsnegative)
            {
                var modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                modPlayer.CustomItemRotation = currentAngle;
                modPlayer.CustomItemRotation += player.direction > 0 ? MathHelper.PiOver4 * 3 : MathHelper.PiOver4;
            }
            player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Quarter, player.itemRotation) + Vector2.UnitX.RotatedBy(currentAngle) * (ModUtilsPlayer.PLAYERARMLENGTH + 3);
        }
    }
    public class MeleeOverhaulSystem : ModSystem
    {
        public override void Load()
        {
            On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += On_PlayerDrawLayers_DrawPlayer_RenderAllLayers;
            On_Player.ApplyAttackCooldown += On_Player_ApplyAttackCooldown;
            On_Player.SetMeleeHitCooldown += On_Player_SetMeleeHitCooldown;
            On_Player.ItemCheck += On_Player_ItemCheck;
        }

        private void On_Player_ItemCheck(On_Player.orig_ItemCheck orig, Player self)
        {
            orig(self);
        }

        private void On_Player_SetMeleeHitCooldown(On_Player.orig_SetMeleeHitCooldown orig, Player self, int npcIndex, int timeInFrames)
        {
            var item = self.HeldItem;
            if (item.TryGetGlobalItem(out MeleeWeaponOverhaul overhaul))
            {
                if (overhaul.SwingType == BossRushUseStyle.Spin)
                {
                    timeInFrames = (int)Math.Round(timeInFrames / Math.Clamp(overhaul.IframeDivision - 1 + overhaul.CircleSwingAmount, 1, int.MaxValue));
                }
                else
                {
                    timeInFrames = (int)Math.Round(timeInFrames / Math.Clamp(overhaul.IframeDivision, 1, int.MaxValue));
                }
            }
            orig(self, npcIndex, timeInFrames);
        }

        private static void DrawSwordTrail(MeleeOverhaulPlayer modplayer)
        {
            var trailShaderSettings = new TrailShaderSettings();
            trailShaderSettings.oldPos = modplayer.swordTipPositions;
            trailShaderSettings.oldRot = modplayer.swordRotations;
            trailShaderSettings.shaderType = SwordSlashTrail.GetShaderType(modplayer.Player);
            trailShaderSettings.image1 = ModContent.Request<Texture2D>(ModTexture.PingpongGradient2);
            trailShaderSettings.image2 = ModContent.Request<Texture2D>(ModTexture.Gradient);
            trailShaderSettings.image3 = ModContent.Request<Texture2D>(ModTexture.PingpongGradient);
            trailShaderSettings.Color = SwordSlashTrail.averageColorByID[modplayer.Player.HeldItem.type];
            default(GenericTrail).Draw(trailShaderSettings,
            (progress) => { return MathHelper.Lerp(modplayer.swordLength, modplayer.swordLength, progress); },
            //Tf this do nothing ??
            (progress) => { return new(0, 0, 0, 0); });

            var trailShaderSettings2 = new TrailShaderSettings();
            trailShaderSettings2.oldPos = modplayer.swordTipPositions;
            trailShaderSettings2.oldRot = modplayer.swordRotations;
            trailShaderSettings2.shaderType = SwordSlashTrail.GetShaderType(modplayer.Player);
            trailShaderSettings2.image1 = ModContent.Request<Texture2D>(ModTexture.PingpongGradient);
            trailShaderSettings2.image2 = ModContent.Request<Texture2D>(ModTexture.Gradient);
            trailShaderSettings2.image3 = ModContent.Request<Texture2D>(ModTexture.PingpongGradient);
            trailShaderSettings2.Color = SwordSlashTrail.averageColorByID[modplayer.Player.HeldItem.type] * 2;
            default(GenericTrail).Draw(trailShaderSettings2,
            (progress) => { return MathHelper.Lerp(modplayer.swordLength, modplayer.swordLength, progress); },
            //Tf this do nothing ??
            (progress) => { return new(0, 0, 0, 0); });
        }

        private void On_Player_ApplyAttackCooldown(On_Player.orig_ApplyAttackCooldown orig, Player self)
        {
            orig(self);
            self.attackCD = 0;
        }

        private void On_PlayerDrawLayers_DrawPlayer_RenderAllLayers(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo)
        {
            var player = Main.LocalPlayer;
            var item = player.HeldItem;
            if (item.noMelee || item.noUseGraphic)
            {
                orig.Invoke(ref drawinfo);
                return;
            }
            if (player.TryGetModPlayer(out MeleeOverhaulPlayer modplayer) && item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
            {
                if (modplayer.ComboNumber == 1
                    || meleeItem.SwingType == BossRushUseStyle.SwipeUp)
                {
                    AdjustDrawingInfo(ref drawinfo, modplayer, meleeItem, player, item);
                }
                if (item.axe <= 0 && SwordSlashTrail.averageColorByID.ContainsKey(item.type) && !meleeItem.HideSwingVisual)
                {
                    DrawSwordTrail(modplayer);
                }
            }
            orig.Invoke(ref drawinfo);
        }

        private static void AdjustDrawingInfo(ref PlayerDrawSet drawinfo, MeleeOverhaulPlayer modplayer, MeleeWeaponOverhaul meleeItem, Player player, Item item)
        {
            DrawData drawdata;
            for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
            {
                var texture = drawinfo.DrawDataCache[i].texture;
                if (texture == TextureAssets.Item[item.type].Value)
                {
                    drawdata = drawinfo.DrawDataCache[i];
                    float scale = drawdata.scale.X;
                    var size = drawdata.texture.Size() * scale;
                    var origin = size * .5f;
                    drawdata.sourceRect = null;
                    drawdata.ignorePlayerRotation = true;
                    drawdata.rotation = modplayer.CustomItemRotation;
                    if (!meleeItem.UseSwipeTwo)
                    {
                        float rotationCs = player.direction == -1 ? MathHelper.PiOver4 : MathHelper.PiOver2 + MathHelper.PiOver4;
                        float rotationAdjustment = (size.ToRotation() - MathHelper.PiOver4) * -player.direction;
                        drawdata.origin = drawdata.texture.Size() * .5f;
                        drawdata.position = drawdata.position.IgnoreTilePositionOFFSET((drawdata.rotation - rotationCs - rotationAdjustment).ToRotationVector2(), origin.X + 13);
                    }
                    else
                    {
                        if (item.ModItem == null || item.scale <= 1.1f)
                        {
                            origin = new Vector2(size.X, size.X) * .5f;
                        }
                        drawdata.position +=
                        Vector2.UnitX.RotatedBy(drawdata.rotation) *
                        (origin.Length() + ModUtilsPlayer.PLAYERARMLENGTH + 3) * -player.direction;
                    }
                    drawinfo.DrawDataCache[i] = drawdata;
                }
            }
        }
    }
    public class MeleeOverhaulPlayer : ModPlayer
    {
        public Vector2 PlayerToMouseDirection;
        public int ComboNumber = 0;
        public int oldHeldItem;
        public int CountDownToResetCombo = 0;
        public float CustomItemRotation = 0;
        public float itemAnimationImproved = 0;
        /// <summary>
        /// This is to force shader to draw regardless of condition<br/>
        /// By default this is set to false and always reset to false
        /// </summary>
        public bool ExtraOverride = false;
        // sword trail fields
        public Vector2[] swordTipPositions = new Vector2[30];
        public float[] swordRotations = new float[30];
        public float swordLength = 0;
        public float lastFrameArmRotation = 0;
        public float startSwordSwingAngle = 0;
        public int swordTrailLength = 30;
        //This is spear value
        public float NormalizeThrustAmount = 0;
        public float CurrentThrust = 0;
        public float PercentageHandle = 0;
        public float Item_LastFrameRotation = 0;
        public Vector2 Item_LastFramePosition = Vector2.Zero;
        public override void PreUpdate()
        {
            ExtraOverride = false;
            var item = Player.HeldItem;
            if (oldHeldItem != item.type)
            {
                oldHeldItem = item.type;
                ComboNumber = 1;
                CountDownToResetCombo = 0;
            }
            CountDownToResetCombo = ModUtils.CountDown(CountDownToResetCombo);
            if (CountDownToResetCombo <= 0)
            {
                ComboNumber = 1;
            }
            if (item.axe > 0)
            {
                ComboNumber = 0;
            }
            if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
            {
                if (meleeItem.SwingType != BossRushUseStyle.Swipe || meleeItem.DisableAttackAnimation)
                {
                    ComboNumber = 0;
                }
            }
        }
        public override bool CanUseItem(Item item)
        {
            if (!Player.ItemAnimationActive && item.type == Player.HeldItem.type)
            {
                PlayerToMouseDirection = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero);
                float baseAngle = PlayerToMouseDirection.ToRotation();
                startSwordSwingAngle = MathHelper.TwoPi * baseAngle / MathHelper.TwoPi;
                if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
                {
                    if (!meleeItem.HideSwingVisual)
                    {
                        Array.Fill(swordTipPositions, Vector2.Zero);
                        Array.Fill(swordRotations, 0);
                    }
                    if (meleeItem.SwingType == BossRushUseStyle.Swipe)
                    {
                        ComboHandleSystem();
                    }
                }
            }
            return base.CanUseItem(item);
        }
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            var item = Player.HeldItem;
            if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) || item.noMelee)
            {
                return;
            }
            if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
            {
                if (meleeItem.SwingType == BossRushUseStyle.SwipeUp || ComboNumber == 1)
                {
                    if (Player.direction == -1)
                    {
                        drawInfo.itemEffect = SpriteEffects.None;
                    }
                    else
                    {
                        drawInfo.itemEffect = SpriteEffects.FlipHorizontally;
                    }
                    return;
                }
            }
        }

        private void ComboHandleSystem()
        {
            ComboNumber = ModUtils.Safe_SwitchValue(ComboNumber, 1);
        }

        public override void PostUpdate()
        {
            var item = Player.HeldItem;
            if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item))
            {
                return;
            }
            if (Player.ItemAnimationActive)
            {
                Player.direction = PlayerToMouseDirection.X > 0 ? 1 : -1;
                var overhaul = item.GetGlobalItem<MeleeWeaponOverhaul>();
                if (overhaul.HideSwingVisual)
                {
                    if (!ExtraOverride)
                    {
                        return;
                    }
                }
                float scale = Player.GetAdjustedItemScale(item);
                swordLength = item.Size.Length() * .55f * scale;
                swordLength += overhaul.ShaderOffSetLength;
                float extraAdd = MathHelper.ToRadians(2) * Player.direction;
                float customAddByXinim = startSwordSwingAngle;
                if (overhaul.SwingType == BossRushUseStyle.Spin)
                {
                    if (Player.direction == -1)
                    {
                        customAddByXinim += MathHelper.TwoPi;
                    }
                }
                float progressOne = MathHelper.Lerp(Player.compositeFrontArm.rotation, customAddByXinim - MathHelper.PiOver2, Player.itemAnimation / (float)Player.itemAnimationMax);
                for (float i = 0; i < 30f; i++)
                {
                    //Slight clean up for your code
                    var dir = (MathHelper.Lerp(progressOne, Player.compositeFrontArm.rotation + extraAdd, i / 30f) + MathHelper.PiOver2).ToRotationVector2();
                    var insertPos = swordLength * dir + Player.Center;
                    ModUtils.Push(ref swordTipPositions, insertPos);
                    ModUtils.Push(ref swordRotations, dir.ToRotation() - MathHelper.PiOver2);
                }
            }
            //can't believe we have to do this
            if (Player.ItemAnimationEndingOrEnded)
            {
                Array.Fill(swordTipPositions, Vector2.Zero);
                Array.Fill(swordRotations, 0);
                startSwordSwingAngle = 0;
            }
        }
        public override void UpdateDead()
        {
            Array.Fill(swordTipPositions, Vector2.Zero);
            Array.Fill(swordRotations, 0);
            startSwordSwingAngle = 0;
        }
    }
}
