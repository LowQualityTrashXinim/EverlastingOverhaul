using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Graphics.Primitives;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.NPCsOverhaul.Bosses.DukeFishorn
{
    public class DukeFishorn : NPCReworkerFSM
    {

        public override int VanillaNPCType => NPCID.DukeFishron;
        public override bool UseCustomAnimation() => true;
        public override int frameHeight => 162;
        public override int startingFrame => 0;
        public override int animationSpeed => 4;
        public override int maxFrames => states != null && states.currentState.type == AIState.StateType<Duke_Spawn>() ? 7 : 5;
        public override int[] RegisterStates() => [AIState.StateType<DukeSuperTornado>(), AIState.StateType<Boss_Despawn>(), AIState.StateType<Duke_Dash>(), AIState.StateType<Duke_Circle1>(), AIState.StateType<Duke_SuperDash>(), AIState.StateType<DukeSuperTornado>()];
        public override void ReworkedAI(ref NPC npc)
        {
            base.ReworkedAI(ref npc);
            states.Update();
        }

    }
    public class DukeSuperTornado : AIState 
    {
        float zDepths = 1;
        Vector2 startingPos = Vector2.Zero;
        public override void OnEntered(int oldState)
        {
            base.OnEntered(oldState);
            PunchCameraModifier p = new(npc.Center, Main.rand.NextVector2Circular(3, 3), 15, 10, 60, 100000);
            Main.instance.CameraModifiers.Add(p);
            startingPos = Target.Center - new Vector2(0,250);
            NPCReworkerFSM.NewProjectileWithMPCheck(npc.GetSource_FromAI(), startingPos, Vector2.Zero, ModContent.ProjectileType<Dukenado>(), 100, 0);

        }

        public override void OnStateUpdate(CommonNPCInfo info)
        {
            base.OnStateUpdate(info);
            npc.velocity *= 0.94f;
            npc.spriteDirection = npc.Center.X > startingPos.X ? 1 : -1;
            if (counter == 180)
            {
                npc.velocity = new Vector2(Target.Center.X > npc.Center.X ? -48 : 48, 0);
            }

            if (counter < 180)
                npc.Center = Vector2.Lerp(npc.Center, startingPos + new Vector2(MathF.Cos(counter * MathHelper.Lerp(0.1f, 0.3f, MathHelper.Clamp(counter / 160f, 0, 1))) * 256, MathF.Sin(counter * MathHelper.Lerp(0.1f, 0.3f, MathHelper.Clamp(counter / 160f, 0, 1))) * 64), 0.7f);

            if (counter == 240)
            {
                ChangeState(StateType<Duke_Dash>());

            }
        }
    }
    public class DukeVortexParticle : Particle
    {
        public override void OnSpawn()
        {
            if(parent == null)
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 3)
                    NewParticle(ParticleType<DukeVortexParticle>(), Vector2.Zero, ParticleTemplates._default with { dontDrawSelf = true, stripShaderID = "DukeWaterStream", parent = this, rotation = i, endOpacity = 1, endSize = 1f, startColor = Color.Cyan, endColor = Color.Turquoise, lifetime = 1000 }, null);
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (parentProjectile != null)
            {
                position = parentProjectile.Projectile.Center;
                rotation = parentProjectile.Projectile.rotation + MathHelper.Pi;
                return;
            }
            else
            {
                rotation += 0.2f;
                position = parent.position + (rotation.ToRotationVector2() * 32);
            }

        }
    }
    public class Dukenado : BetterModProjectile
    {
        private static Asset<Texture2D> grad;
        private static Asset<Texture2D> grad2;
        public override void Load()
        {
            base.Load();
            grad = ModContent.Request<Texture2D>("EverlastingOverhaul/Texture/Gradient");
            grad2 = ModContent.Request<Texture2D>("EverlastingOverhaul/Texture/Gradient2");

        }
        public override void BetterSetDefaults()
        {
            base.BetterSetDefaults();
            Projectile.timeLeft = 600;
        }
        private static VertexRectangle rect = new();
        public override void PostDraw(Color lightColor)
        {
            ModdedShaderHandler tornadoShader = EffectsLoader.shaderHandlers["DukeTornado"];
            tornadoShader.setProperties(Color.Turquoise, TextureAssets.Extra[193].Value, grad.Value, grad2.Value, new Vector4(0,MathHelper.Clamp((MathHelper.Lerp(0,1,(Projectile.timeLeft - 450f) / 150f)),0,1),0,0));
            tornadoShader.apply();
            rect.Draw(Projectile.Center - Main.screenPosition, size: new Vector2(256 * 4, 1024), rotationCenter: Projectile.Center - Main.screenPosition);
        }
    }
    public class DukeParticle : Particle
    {
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (parentProjectile != null)
            {
                position = parentProjectile.Projectile.Center;
                rotation = parentProjectile.Projectile.rotation;
            }
        }
    }
    public class DukeWaterStream : BetterModProjectile
    {
        public override int TrailLength => 150;
        public override void BetterSetDefaults()
        {
            base.BetterSetDefaults();
            Projectile.timeLeft = 240;
            Projectile.hostile = true;
            Projectile.friendly = false;
        }
        Vector2 direction = Vector2.Zero;

        public override void AI()
        {
            base.AI();

            if(Projectile.timeLeft == maxTimeLeft)
                direction = Projectile.velocity.RotatedByRandom(.1f);

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.timeLeft % 5 != 0)
                return;

            if (Projectile.velocity.ToRotation() > direction.ToRotation())
                Projectile.velocity = direction.RotatedBy(0.9f * Main.rand.NextFloat() + 0.025f);
            else
                Projectile.velocity = direction.RotatedBy(-.9f * Main.rand.NextFloat() - .025f);


        }

        public override Particle ProjectileParticle()
        {
            return Particle.NewParticle(Particle.ParticleType<DukeVortexParticle>(), Vector2.Zero, ParticleTemplates._default with { dontDrawSelf = true, stripShaderID = "DukeWaterStream", stripWidth = 16, stripEndWidth = 16, rotation = Projectile.velocity.ToRotation(), endOpacity = 1, endSize = 1f, startColor = Color.Cyan, endColor = Color.Turquoise, lifetime = 1000 }, this);
        }
    }

    public class DukeShadows : BetterModProjectile 
    {

        public override bool PreDraw(ref Color lightColor)
        {
            //Main.EntitySpriteDraw(TextureAssets.DukeFishron.Value,Projectile.Center);
            return base.PreDraw(ref lightColor);
        }
    
    }

    public class DukeVortex : BetterModProjectile
    {
        public override int TrailLength => 1;
        public override void BetterSetDefaults()
        {
            base.BetterSetDefaults();
            Projectile.timeLeft = 240;
            Projectile.hostile = true;
            Projectile.friendly = false;
        }
        Vector2 velLength;
        public override void AI()
        {
            base.AI();

            if(maxTimeLeft == Projectile.timeLeft)
                velLength = Projectile.velocity;

            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft >= 190)
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Main.player[(int)Projectile.ai[0]].Center.DirectionFrom(Projectile.Center).ToRotation(),0.015f).ToRotationVector2() * velLength.Length();

            }
        }

        public override Particle ProjectileParticle()
        {
            return Particle.NewParticle(Particle.ParticleType<DukeVortexParticle>(), Vector2.Zero, ParticleTemplates._default with { dontDrawSelf = true, shaderID = "DukeVortex", vertexRectSize = new Vector2(256,129), rotation = Projectile.velocity.ToRotation(), endOpacity = 1, endSize = 1f, startColor = Color.Blue, endColor = Color.Turquoise, lifetime = 1000 }, this);
        }
    }
    public class DukeBiome : ModBiome
    {
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<DukeBG>();

        public override float GetWeight(Player player)
        {
            return 1f;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override bool IsBiomeActive(Player player)
        {
            DisableWorldBackgroundElements();
            return true;
        }

        public static void DisableWorldBackgroundElements()
        {

            for (int i = 0; i < Main.maxClouds; i++)
            {
                Main.cloud[i].active = false;
            }
            Main.cloudBGActive = 0;



        }
        public override void OnInBiome(Player player)
        {
            //Particle.NewParticle(Particle.ParticleType<DukeLightingBGParticle1>(),Main.screenPosition + new Vector2(Main.rand.Next(0,Main.ScreenSize.X), 0), ParticleTemplates._default with { dontDrawSelf = true, stripShaderID = "DukeLightingBG", vertexRectSize = new Vector2(256), rotation = Vector2.UnitY.ToRotation(), stripWidth = 6 , endOpacity = 1, endSize = 1f, startColor = Color.Cyan, endColor = Color.Turquoise, lifetime = 60,velocity = Vector2.UnitY * 35 });
        }
    }
    public class DukeLightingBGParticle1 : Particle
    {
        Vector2 direction = Vector2.Zero;
        Vector2 fixedPos = Vector2.Zero;
        public override void OnSpawn()
        {
            base.OnSpawn();
            direction = velocity.RotatedByRandom(0.2f);

        }
        public override void PostUpdate()
        {
            

            if(timeleft %3 != 0)
                return;

            if (velocity.ToRotation() > direction.ToRotation())
                velocity = direction.RotatedBy(0.4f * Main.rand.NextFloat() + 0.025f);
            else
                velocity = direction.RotatedBy(-.4f * Main.rand.NextFloat() - .025f);

            rotation = velocity.ToRotation();


        }
    }
    public class DukeLightingBGParticle2 : Particle
    {

    }

    public class DukeBG : ModSurfaceBackgroundStyle
    {


        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
        }
        private static VertexRectangle rect = new();

        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            ModdedShaderHandler handler = EffectsLoader.shaderHandlers["DukeBG"];

            handler.setProperties(Color.White, TextureAssets.Extra[193].Value, shaderData: new Vector4(Main.Camera.Center.X,Main.Camera.Center.Y,0,0));
            handler.apply();

            rect.Draw(Main.Camera.Center - Main.screenPosition, Color.White, size: Main.ScreenSize.ToVector2(), rotationCenter: Main.LocalPlayer.Center);
            return false;
        }


    }

    public class Duke_Spawn : AIState
    {

        public override void OnEntered(int oldState)
        {

        }

        public override void OnStateUpdate(CommonNPCInfo info)
        {
            npc.velocity = -Vector2.UnitY * 2;

            if (counter == 14 * 4)
                ChangeState(StateType<Duke_Dash>());
        }

    }

    public class Duke_Circle1 : AIState
    {
        int fireDelay = 0;
        int recoilCounter = 0;
        int currentCircleAngle = 0;
        float currentCircleRot = 0;
        int fireCounter = 0;
        int rotDir = 1;
        public override void OnEntered(int oldState)
        {
            base.OnEntered(oldState);
            npc.velocity = Vector2.Zero;
            currentCircleRot = npc.DirectionFrom(Target.Center).ToRotation();
        }
        public override void OnStateUpdate(CommonNPCInfo info)
        {
            if(recoilCounter > -15) 
            {
                recoilCounter--;
                npc.Center += npc.DirectionFrom(Target.Center) * recoilCounter * 0.5f;
                return;
            }

            currentCircleRot += MathHelper.TwoPi / 30f * MathHelper.Clamp(MathHelper.Lerp(5, 0, ModUtils.InExpo(fireDelay / 120f,11f)),0,1) * rotDir;
            npc.Center = Vector2.Lerp(npc.Center,Target.Center + new Vector2(600,0).RotatedBy(currentCircleRot),0.2f);
            npc.FaceTarget();
            npc.spriteDirection = npc.direction * -1;
            npc.rotation = Target.DirectionFrom(npc.Center).ToRotation() - npc.spriteDirection == 1 ? MathHelper.Pi : 0;

            fireDelay++;

            if (fireDelay > 120)
            {
                npc.rotation = npc.DirectionTo(Target.Center).ToRotation() + (npc.spriteDirection == 1 ? MathHelper.Pi: 0);
                npc.spriteDirection = npc.direction * -1;
                npcHandler.currentFrame = 7;
                if (fireDelay < 140)
                {
                    npc.Center -= npc.DirectionFrom(Target.Center) * 15f;
                    return;
                }

                fireCounter++;
                recoilCounter = 0;
                NPCReworkerFSM.NewProjectileWithMPCheck(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(Target.Center).RotatedBy(0) * 25, ModContent.ProjectileType<DukeVortex>(), 50, 1);
                fireDelay = Main.rand.Next(60,100);
                rotDir = Main.rand.NextBool() == true ? -1 : 1;
            }

            if(fireCounter >= 12) 
            {
                fireCounter = 0;
                ChangeState(StateType<Duke_Dash>());
            }

        }

    }
    public class Duke_SuperDash : AIState
    {

        int dashDelay = 0;
        Vector2 offset = Vector2.Zero;
        public override void OnEntered(int oldState)
        {
            base.OnEntered(oldState);
        }
        public override void OnStateUpdate(CommonNPCInfo info)
        {

            if (counter == 1)
                offset = new Vector2(Main.rand.Next(128, 128), Main.rand.Next(128, 128));

            if (counter <= 30)
            {
                npc.FaceTarget();
                npc.spriteDirection = npc.direction * -1;
                npc.Center = Vector2.Lerp(Vector2.Lerp(npc.Center, npc.Center - Vector2.UnitY * 128, counter / 35f), Target.Center + offset + (npc.Center.X > Target.Center.X ? Vector2.UnitX * 512 : Vector2.UnitX * -512), counter / 60f);
                return;
            }

            dashDelay++;

            if (dashDelay < 65)
            {
                npc.rotation = npc.DirectionTo(Target.Center).ToRotation() - npc.spriteDirection == 1 ? MathHelper.Pi : 0;
                npc.velocity = Vector2.Lerp(npc.DirectionTo(Target.Center) * -32, npc.DirectionTo(Target.Center) * 48, dashDelay / 100f);
                if (dashDelay % 5 == 0)
                    Main.npc[NPCReworkerFSM.NewNPCWithMPCheck(npc.GetSource_FromAI(), npc.Center, 371)].velocity = npc.DirectionTo(Target.Center).RotatedByRandom(0.2) * 75;
                PunchCameraModifier p = new(npc.Center, Main.rand.NextVector2Circular(3, 3), 5, 3, 5, 100000);
                Main.instance.CameraModifiers.Add(p);
                return;
            }

            npc.velocity *= 0.97f;

            if (dashDelay == 160)
            {

                npc.velocity = npc.DirectionTo(Target.Center) * 80;
                npc.rotation = npc.velocity.ToRotation() - npc.spriteDirection == 1 ? MathHelper.Pi : 0;
                npc.spriteDirection = npc.velocity.X < 0 ? 1 : -1;
            }


            if (counter >= 240)
            {
                npc.velocity *= 0.92f;
                npc.rotation = 0;
                dashDelay = 0;

                ChangeState(StateType<Duke_Circle1>());



            }


        }

    }
    public class Duke_Dash : AIState
    {

        int dashCounter = 0;
        int dashDelay = 0;
        Vector2 offset = Vector2.Zero;
        public override void OnStateUpdate(CommonNPCInfo info)
        {

            if (counter == 1)
            {
                offset = new Vector2(Main.rand.Next(256, 256), Main.rand.Next(256, 256));

            }

            if (counter <= 30)
            {
                npc.FaceTarget();
                npc.spriteDirection = npc.direction * -1;
                npc.Center = Vector2.Lerp(Vector2.Lerp(npc.Center, npc.Center - Vector2.UnitY * 128, counter / 35f), Target.Center + offset + (npc.Center.X > Target.Center.X ? Vector2.UnitX * 512 : Vector2.UnitX * -512), counter / 60f);
                return;
            }

            dashDelay++;

            if (dashDelay < 30)
            {
                npc.rotation = npc.DirectionTo(Target.Center).ToRotation() - npc.spriteDirection == 1 ? MathHelper.Pi : 0;
                npc.velocity = Vector2.Lerp(npc.DirectionTo(Target.Center) * -32, npc.DirectionTo(Target.Center) * 16, dashDelay / 30f);
                return;
            }

            npc.velocity.MoveTowards(npc.velocity * 1.2f, 40);

            if (dashDelay == 30)
            {
                npc.velocity = npc.DirectionTo(Target.Center) * 45;
                npc.rotation = npc.velocity.ToRotation() - npc.spriteDirection == 1 ? MathHelper.Pi : 0;
                npc.spriteDirection = npc.velocity.X < 0 ? 1 : -1;
                NPCReworkerFSM.NewProjectileWithMPCheck(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedByRandom(0.2) * 15, ProjectileID.SharknadoBolt, 50, 1);

            }


            if (counter == 120)
            {
                npc.velocity = Vector2.Zero;
                npc.rotation = 0;
                dashDelay = 0;
                dashCounter++;

                if (dashCounter % 7 != 0)
                    ChangeState(StateType<Duke_Dash>());
                else
                    if (dashCounter == 14)
                    {
                        ChangeState(StateType<Duke_SuperDash>());
                        dashCounter = 0;
                    }
                else
                    ChangeState(StateType<Duke_SuperDash>());




            }

        }

    }
}
