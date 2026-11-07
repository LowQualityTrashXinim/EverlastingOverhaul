using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .NPCsOverhaul.Bosses.Skeletron;
internal class SkeletronHand : GlobalNPC {
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.SkeletronHand;
	public override void SetDefaults(NPC entity) {
		entity.aiStyle = -1;
		entity.lifeMax /= 2;
	}
	public override void AI(NPC npc) {
		npc.spriteDirection = -(int)npc.ai[0];
		if (npc.ai[1] < 0 || npc.ai[1] >= Main.maxNPCs || !Main.npc[(int)npc.ai[1]].active) {
			npc.ai[2] += 10f;
			if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) {
				npc.life = -1;
				npc.HitEffect();
				npc.active = false;
			}
			return;
		}
		NPC boss = Main.npc[(int)npc.ai[1]];

		if (!boss.Center.IsCloseToPosition(npc.Center, 800)) {
			npc.ai[2] = 0f;
			npc.velocity += (boss.Center - npc.Center).SafeNormalize(Vector2.Zero) * (boss.Center - npc.Center).Length() / 32f;
		}

		if (npc.ai[2] == 0f || npc.ai[2] == 3f) {
			if (boss.ai[1] == 3f)
				npc.EncourageDespawn(10);

			if (boss.ai[1] != 0f) {
				if (npc.position.Y > boss.position.Y - 100f) {
					if (npc.velocity.Y > 0f)
						npc.velocity.Y *= 0.96f;

					npc.velocity.Y -= 0.07f;
					if (npc.velocity.Y > 6f)
						npc.velocity.Y = 6f;
				}
				else if (npc.position.Y < boss.position.Y - 100f) {
					if (npc.velocity.Y < 0f)
						npc.velocity.Y *= 0.96f;

					npc.velocity.Y += 0.07f;
					if (npc.velocity.Y < -6f)
						npc.velocity.Y = -6f;
				}

				if (npc.Center.X > boss.Center.X - 120f * npc.ai[0]) {
					if (npc.velocity.X > 0f)
						npc.velocity.X *= 0.96f;

					npc.velocity.X -= 0.1f;
					if (npc.velocity.X > 8f)
						npc.velocity.X = 8f;
				}

				if (npc.Center.X < boss.Center.X - 120f * npc.ai[0]) {
					if (npc.velocity.X < 0f)
						npc.velocity.X *= 0.96f;

					npc.velocity.X += 0.1f;
					if (npc.velocity.X < -8f)
						npc.velocity.X = -8f;
				}
			}
			else {
				npc.ai[3] += 3.5f;

				if (npc.ai[3] % 10 == 0) {
					ShootProjectile(npc, Vector2.Zero, ProjectileID.DemonScythe, 90);
				}

				if (npc.ai[3] >= 300f) {
					npc.ai[2] += 1f;
					npc.ai[3] = 0f;
					npc.netUpdate = true;
				}

				if (npc.position.Y > boss.position.Y + 230f) {
					if (npc.velocity.Y > 0f)
						npc.velocity.Y *= 0.96f;

					npc.velocity.Y -= 0.04f;
					npc.velocity.Y = Math.Clamp(npc.velocity.Y, -4, 4);
				}
				else if (npc.position.Y < boss.position.Y + 230f) {
					if (npc.velocity.Y < 0f)
						npc.velocity.Y *= 0.96f;

					npc.velocity.Y += 0.04f;
					npc.velocity.Y = Math.Clamp(npc.velocity.Y, -4, 4);
				}

				if (npc.Center.X > boss.Center.X - 200f * npc.ai[0]) {
					if (npc.velocity.X > 0f)
						npc.velocity.X *= 0.96f;

					npc.velocity.X -= 0.07f;
					npc.velocity.Y = Math.Clamp(npc.velocity.Y, -8, 8);
				}

				if (npc.Center.X < boss.Center.X - 200f * npc.ai[0]) {
					if (npc.velocity.X < 0f)
						npc.velocity.X *= 0.96f;

					npc.velocity.X += 0.07f;
					npc.velocity.Y = Math.Clamp(npc.velocity.Y, -8, 8);
				}


				if (npc.position.Y > boss.position.Y + 230f) {
					if (npc.velocity.Y > 0f)
						npc.velocity.Y *= 0.96f;

					npc.velocity.Y -= 0.04f;
					if (npc.velocity.Y > 3f)
						npc.velocity.Y = 3f;
				}
				else if (npc.position.Y < boss.position.Y + 230f) {
					if (npc.velocity.Y < 0f)
						npc.velocity.Y *= 0.96f;

					npc.velocity.Y += 0.04f;
					if (npc.velocity.Y < -3f)
						npc.velocity.Y = -3f;
				}

				if (npc.Center.X > boss.Center.X - 200f * npc.ai[0]) {
					if (npc.velocity.X > 0f)
						npc.velocity.X *= 0.96f;

					npc.velocity.X -= 0.07f;
					if (npc.velocity.X > 8f)
						npc.velocity.X = 8f;
				}

				if (npc.Center.X < boss.Center.X - 200f * npc.ai[0]) {
					if (npc.velocity.X < 0f)
						npc.velocity.X *= 0.96f;

					npc.velocity.X += 0.07f;
					if (npc.velocity.X < -8f)
						npc.velocity.X = -8f;
				}
			}

			Vector2 vector22 = npc.Center;
			float num181 = boss.Center.X - 200f * npc.ai[0] - vector22.X;
			float num182 = boss.position.Y + 230f - vector22.Y;
			float num183 = (float)Math.Sqrt(num181 * num181 + num182 * num182);
			npc.rotation = (float)Math.Atan2(num182, num181) + 1.57f;
		}
		else if (npc.ai[2] == 1f) {
			Vector2 vector23 = npc.Center;
			float num184 = boss.Center.X - 200f * npc.ai[0] - vector23.X;
			float num185 = boss.position.Y + 230f - vector23.Y;
			float num186 = (float)Math.Sqrt(num184 * num184 + num185 * num185);
			npc.rotation = (float)Math.Atan2(num185, num184) + 1.57f;
			npc.velocity.X *= 0.95f;
			npc.velocity.Y -= 0.1f;

			npc.velocity.Y -= 0.06f;
			if (npc.velocity.Y < -13f)
				npc.velocity.Y = -13f;


			if (npc.position.Y < boss.position.Y - 200f) {
				npc.TargetClosest();
				npc.ai[2] = 2f;
				vector23 = npc.Center;
				num184 = Main.player[npc.target].Center.X - vector23.X;
				num185 = Main.player[npc.target].Center.Y - vector23.Y;
				num186 = 21f / (float)Math.Sqrt(num184 * num184 + num185 * num185);
				npc.velocity.X = num184 * num186;
				npc.velocity.Y = num185 * num186;
				npc.netUpdate = true;
			}
		}
		else if (npc.ai[2] == 2f) {
			if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
				npc.ai[2] = 3f;
		}
		else if (npc.ai[2] == 4f) {
			Vector2 vector24 = npc.Center;
			float num187 = boss.Center.X - 200f * npc.ai[0] - vector24.X;
			float num188 = boss.position.Y + 230f - vector24.Y;
			float num189 = (float)Math.Sqrt(num187 * num187 + num188 * num188);
			npc.rotation = (float)Math.Atan2(num188, num187) + 1.57f;
			npc.velocity.Y *= 0.95f;
			npc.velocity.X += 0.1f * (0f - npc.ai[0]);

			npc.velocity.X += 0.07f * (0f - npc.ai[0]);

			if (npc.velocity.X < -12f)
				npc.velocity.X = -12f;
			else if (npc.velocity.X > 12f)
				npc.velocity.X = 12f;

			if (npc.Center.X < boss.Center.X - 500f || npc.Center.X > boss.Center.X + (float)(boss.width / 2) + 500f) {
				npc.TargetClosest();
				npc.ai[2] = 5f;
				vector24 = npc.Center;
				num187 = Main.player[npc.target].Center.X - vector24.X;
				num188 = Main.player[npc.target].Center.Y - vector24.Y;
				num189 = 22f / (float)Math.Sqrt(num187 * num187 + num188 * num188);
				npc.velocity.X = num187 * num189;
				npc.velocity.Y = num188 * num189;
				npc.netUpdate = true;
			}
		}
		else if (npc.ai[2] == 5f
			&& npc.velocity.X > 0f && npc.Center.X > Main.player[npc.target].Center.X
			|| npc.velocity.X < 0f && npc.Center.X < Main.player[npc.target].Center.X) {
			npc.ai[2] = 0f;
		}
	}
	private void ShootProjectile(NPC npc, Vector2 vel, int Type, int customTimeLeft = 300) {
		Vector2 center3 = npc.Center;
		int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
		int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3, vel, Type, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
		Main.projectile[num160].timeLeft = customTimeLeft;
		Main.projectile[num160].hostile = true;
		Main.projectile[num160].friendly = false;
	}
}
