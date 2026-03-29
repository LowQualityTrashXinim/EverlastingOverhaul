using EverlastingOverhaul.Common.NPCsOverhaul.Bosses.EyeofCuthulu;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.RGB;
using Terraria.ID;

namespace EverlastingOverhaul.Common.NPCsOverhaul.Bosses.EaterofWorld
{
    public class EaterofWorld : NPCReworkerFSM
    {
        public override int VanillaNPCType => NPCID.EaterofWorldsHead;
        public override void SetDefaults(NPC entity)
        {
            base.SetDefaults(entity);
            entity.lifeMax = 20000;
        }
        public override void ReworkedAI(ref NPC npc)
        {
            states.Update();
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);
            
        }

        public override void PostOnSpawn(NPC npc, IEntitySource source)
        {
            int lastWho = npc.whoAmI;

            for (int i = 0; i < 50; i++) 
            {
                int who = NewNPCWithMPCheck(source,npc.Center + new Vector2(i * 1),NPCID.EaterofWorldsBody);
                Main.npc[who].ai[0] = lastWho;
                Main.npc[who].realLife = npc.whoAmI;
                Main.npc[lastWho].ai[1] = who;
                lastWho = who;
            }

            int tail = NewNPCWithMPCheck(source, npc.Center + new Vector2(51), NPCID.EaterofWorldsTail);
            Main.npc[tail].realLife = npc.whoAmI;
            Main.npc[tail].ai[0] = lastWho;
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Npc[NPCID.EaterofWorldsHead].Value, npc.Center - screenPos, null, drawColor, npc.rotation + MathHelper.Pi / 2f, TextureAssets.Npc[NPCID.EaterofWorldsHead].Value.Size() / 2f, 1f, SpriteEffects.None);
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override int[] RegisterStates() => [
            AIState.StateType<EoW_Idle>(),
            AIState.StateType<EoW_Dash>(),
            AIState.StateType<EoW_Spit>(),
            AIState.StateType<EoW_EarthShatter>(),
            AIState.StateType<Boss_Despawn>()
        ];


    }
    public class EaterBody : NPCReworkerFSM
    {
        public override int VanillaNPCType => NPCID.EaterofWorldsBody;
        public override void ReworkedAI(ref NPC npc)
        {
            states.Update();
        }

        public override void PostOnSpawn(NPC npc, IEntitySource source)
        {

        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Npc[NPCID.EaterofWorldsBody].Value, npc.Center -screenPos, null, drawColor, npc.rotation, TextureAssets.Npc[NPCID.EaterofWorldsBody].Value.Size() / 2f, 1f, SpriteEffects.None);
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override int[] RegisterStates() => [
            AIState.StateType<EoW_Segment>(),
            AIState.StateType<Boss_Despawn>()
        ];

    }

    public class EaterTail : NPCReworkerFSM
    {
        public override int VanillaNPCType => NPCID.EaterofWorldsTail;
        public override void ReworkedAI(ref NPC npc)
        {
            states.Update();
        }

        public override void PostOnSpawn(NPC npc, IEntitySource source)
        {

        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Npc[NPCID.EaterofWorldsTail].Value, npc.Center - screenPos, null, drawColor, npc.rotation + MathHelper.Pi, TextureAssets.Npc[NPCID.EaterofWorldsTail].Value.Size() / 2f, 1f, SpriteEffects.None);
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override int[] RegisterStates() => [
            AIState.StateType<EoW_Segment>(),
            AIState.StateType<Boss_Despawn>()

        ];


    }

    public class EoW_Segment : AIState 
    {

        public override void OnStateUpdate(CommonNPCInfo info)
        {
            if (Main.npc[(int)npc.ai[0]].active) 
            {
                var worm = Main.npc[(int)npc.ai[0]];
                float dirX = worm.Center.X - npc.Center.X;
                float dirY = worm.Center.Y - npc.Center.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + MathHelper.PiOver2;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X += posX;
                npc.position.Y += posY;
            }
        }

    }

    public class EoW_Idle : AIState
    {
        float accel = 0;
        float speed = 0;
        float rotationNeededToReachTarget = 0;
        public override void OnStateUpdate(CommonNPCInfo info)
        {
            base.OnStateUpdate(info);
            npc.velocity = npc.velocity.MoveTowards(npc.DirectionTo(Target.Center) * 15,0.4f);
            npc.rotation = npc.velocity.ToRotation();

            if (counter % 35 == 0)
                 NPCReworkerFSM.NewProjectileWithMPCheck(npc.GetSource_FromAI(),npc.Center,Target.DirectionFrom(npc.Center) *24, ProjectileID.CursedFlameHostile,15,0);

            if (counter >= 600)
                ChangeState(StateType<EoW_Dash>());
        }
    }
    public class EoW_Dash : AIState
    {
        int dashCounter = 0;
        public override void OnStateUpdate(CommonNPCInfo info)
        {
            base.OnStateUpdate(info);
            if (counter < 35)
                npc.velocity *= 0.95f;

            npc.rotation = npc.rotation.AngleTowards(Target.DirectionFrom(npc.Center).ToRotation(),0.07f);
            if (counter >= 35)
            {
                if(counter == 35)
                    npc.velocity = npc.rotation.ToRotationVector2() * 35;

                if(counter >= 75)
                {
                    if(dashCounter >= 5) 
                    {
                        ChangeState(StateType<EoW_EarthShatter>());
                        npc.velocity = Vector2.UnitY * 50;
                        dashCounter = 0;
                        return;
                    }
                    
                    ChangeState(StateType<EoW_Dash>());
                    dashCounter++;

                }



            }

        }
    }
    public class EoW_Spit : AIState
    {
        public override void OnStateUpdate(CommonNPCInfo info)
        {
            base.OnStateUpdate(info);



        }
    }
    public class EoW_EarthShatter : AIState
    {
        public override void OnStateUpdate(CommonNPCInfo info)
        {
            base.OnStateUpdate(info);

            if (WorldGen.SolidTile(npc.Center.ToTileCoordinates()) || counter >= 30) 
            {

                npc.velocity = Vector2.Zero;

                for(int i = 0; i < 15; i++)
                    NPCReworkerFSM.NewProjectileWithMPCheck(npc.GetSource_FromAI(),npc.Center,-Vector2.UnitY.RotatedByRandom(1) * 15,ProjectileID.DeerclopsRangedProjectile,15,0);

                ChangeState(StateType<EoW_Idle>());

            }

        }
    }
}