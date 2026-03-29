using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.NPCsOverhaul;
public class NPCReworkerFSM : GlobalNPC, IZDepth {
    public int currentState
    {
        get
        {
            if (states != null)
                return states.currentState.type;
            return -1;

        }

    }
    public int currentStateCounter => states.currentState.counter;
    public static int StateType<T>() where T : AIState => ModContent.GetInstance<T>().type;
    public virtual void PostOnSpawn(IEntitySource source)
    {

    }
    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        zDepth = 1;
        states = new(npc, RegisterStates());
        PostOnSpawn(source);
    }
    public virtual int[] RegisterStates()
    {
        return [];
    }
    public virtual void PreStateUpdate() { }
    public virtual void PostStateUpdate()
    {
    }
    public static int NewProjectileWithMPCheck(IEntitySource spawnSource, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
            return Projectile.NewProjectile(spawnSource, position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
        else
            return -1;
    }
    public static int NewNPCWithMPCheck(IEntitySource source, Vector2 Position, int Type, int Start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int Target = 255)
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
            return NPC.NewNPC(source, (int)Position.X, (int)Position.Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        else
            return -1;
    }
    public float zDepth { get; set; }
    public override bool InstancePerEntity => true;
	public virtual int VanillaNPCType => 0;
	public NPCAIStates states = null;

	public override void SetStaticDefaults() {
		NPCID.Sets.TrailingMode[VanillaNPCType] = 3;
		NPCID.Sets.TrailCacheLength[VanillaNPCType] = 30;
	}
	public override void SetDefaults(NPC entity) {
		entity.aiStyle = -1;
	}

	public virtual void PostOnSpawn(NPC npc, IEntitySource source)
	{
		
	}

	public override bool PreAI(NPC npc) {
        UpdateAnimation(npc);
        PreStateUpdate();
		if(states != null)
			states.Update();
        //zDepth = MathF.Sin((float)Main.timeForVisualEffects * 0.01f) * 2 + 2;

        PostStateUpdate();
        return false;
    }


	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		if(states != null)
			states.currentState.StatePreDraw(NPCSpriteDrawData(npc,drawColor,screenPos),spriteBatch,screenPos,drawColor);
		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
    public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        if (states != null)
            states.currentState.StatePostDraw(spriteBatch,screenPos,drawColor);
    }
	public virtual void ReworkedAI(ref NPC npc) 
	{
	}

	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == VanillaNPCType;
	}
	// This uses custom animation system instead of the weird vanilla system, this aims to be faster and easier to setup for npc reworking purpose
	public virtual bool UseCustomAnimation() => false;
	public int currentFrame = 0;
	public virtual int frameHeight => 128;
	public virtual int startingFrame => 0;
	public virtual int animationSpeed => 14;
	public virtual int maxFrames => 0;
	public Rectangle frameRect = new();
	public override void FindFrame(NPC npc, int frameHeight) 
	{
		if(UseCustomAnimation())
			frameRect = new Rectangle(0,currentFrame * this.frameHeight, TextureAssets.Npc[VanillaNPCType].Width(),this.frameHeight);
		else
			base.FindFrame(npc, frameHeight);
	}
	public void UpdateAnimation(NPC npc)
	{
		if(++npc.frameCounter % animationSpeed == 0)
			if(++currentFrame > maxFrames - startingFrame)
				currentFrame = startingFrame;
	}
	public DrawData NPCSpriteDrawData(NPC npc, Color drawColor, Vector2 screenPos)
	{
		if(UseCustomAnimation())
			return new DrawData(TextureAssets.Npc[VanillaNPCType].Value,npc.Center - screenPos,frameRect,drawColor,npc.rotation,new Vector2(TextureAssets.Npc[VanillaNPCType].Width() / 2f,frameHeight / 2f),npc.scale,npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally | SpriteEffects.FlipHorizontally);
		return new DrawData(TextureAssets.Npc[VanillaNPCType].Value,npc.Center - screenPos,npc.frame,drawColor,npc.rotation,new Vector2(TextureAssets.Npc[VanillaNPCType].Width() / 2f,frameHeight / 2f),npc.scale,npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally);
	
	}

}


