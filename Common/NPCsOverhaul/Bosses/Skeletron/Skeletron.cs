using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .NPCsOverhaul.Bosses.Skeletron;
internal class Skeletron : GlobalNPC {
	enum State {
		DungeonGuardian,
		Hover,
		Spin,
		SpinDash,
		CircleAttack,
		Desperation,
		Stationary,
		Despawn,
	}
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.SkeletronHead;
	public override bool InstancePerEntity => true;
	public override void SetDefaults(NPC entity) {
		state = State.Hover;
		entity.aiStyle = -1;
		if (!Main.dedServ) {
			//entity.soun = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LootboxLord_BossMusic");
		}
	}
	public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position) {
		return false;
	}
	State state = State.Hover;
	bool CanDamagePlayer = true;
	private bool DG_AI(NPC npc) {
		if (state == State.DungeonGuardian) {
			npc.damage = 1000;
			npc.defense = 9999;
			npc.rotation += (float)npc.direction * 0.3f;
			Vector2 vector21 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num176 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector21.X;
			float num177 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector21.Y;
			float num178 = (float)Math.Sqrt(num176 * num176 + num177 * num177);
			num178 = 8f / num178;
			npc.velocity.X = num176 * num178;
			npc.velocity.Y = num177 * num178;
			return true;
		}
		if (Main.IsItDay() && state != State.DungeonGuardian) {
			state = State.DungeonGuardian;
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
			return true;
		}
		return false;
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		state = State.Hover;
		if (Main.netMode != NetmodeID.MultiplayerClient) {
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
			npc.TargetClosest();

			int num148 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, NPCID.SkeletronHand, npc.whoAmI);
			Main.npc[num148].ai[0] = -1f;
			Main.npc[num148].ai[1] = npc.whoAmI;
			Main.npc[num148].target = npc.target;
			Main.npc[num148].netUpdate = true;
			num148 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, NPCID.SkeletronHand, npc.whoAmI);
			Main.npc[num148].ai[0] = 1f;
			Main.npc[num148].ai[1] = npc.whoAmI;
			Main.npc[num148].ai[3] = 150f;
			Main.npc[num148].target = npc.target;
			Main.npc[num148].netUpdate = true;

		}
	}
	public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot) {
		if (state == State.CircleAttack && !CanDamagePlayer) {
			return false;
		}
		return base.CanHitPlayer(npc, target, ref cooldownSlot);
	}
	private void CheckPlayerDead(NPC npc) {
		if (Main.player[npc.target].dead ||
			Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f ||
			Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f) {
			npc.TargetClosest();
			if (Main.player[npc.target].dead) {
				state = State.Despawn;
				return;
			}
			if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f) {
				SoundEngine.PlaySound(SoundID.Roar, npc.position);
				Vector2 pos = Main.player[npc.target].Center + Vector2.UnitX.Vector2RotateByRandom(30) * 750 * Main.player[npc.target].direction;
				npc.TeleportCommon(pos);
				ModUtils.DustStar(npc.Center, DustID.GemDiamond, Color.Azure, 20, 30, 0, 20);
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.GemDiamond);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.9f, 1.34f);
					dust.scale = Main.rand.NextFloat(.9f, 1.4f);
				}
				npc.velocity = Vector2.Zero;
			}
		}
	}
	private void ShootCursedSkull(NPC npc, int handsCount) {
		Vector2 center3 = npc.Center;
		if (Collision.CanHit(center3, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)) {
			float num155 = 3f;
			if (handsCount == 0)
				num155 += 2f;

			Vector2 playerCenter = Main.player[npc.target].Center + Main.rand.NextVector2Circular(50, 50);
			Vector2 vector19 = (playerCenter - center3).SafeNormalize(Vector2.Zero) * (num155 + Vector2.Distance(playerCenter, center3) * .1f);
			vector19 += npc.velocity;
			int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(30f, 30f);
			int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3, vector19, ProjectileID.Skull, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
			Main.projectile[num160].timeLeft = 300;
		}
	}
	private void ShootProjectile(NPC npc, Vector2 vel, int Type, int customTimeLeft = 300) {
		Vector2 center3 = npc.Center;
		int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(30f, 30f);
		int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3, vel, Type, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
		Main.projectile[num160].timeLeft = customTimeLeft;
		Main.projectile[num160].hostile = true;
		Main.projectile[num160].friendly = false;
	}
	public override void AI(NPC npc) {
		if (DG_AI(npc)) {
			return;
		}
		Lighting.AddLight(npc.Center, 50, 50, 50);
		npc.reflectsProjectiles = false;
		npc.defense = npc.defDefense;

		CheckPlayerDead(npc);

		int handsCount = 0;

		for (int num150 = 0; num150 < 200; num150++) {
			if (Main.npc[num150].active && Main.npc[num150].type == NPCID.SkeletronHand)
				handsCount++;
		}
		npc.defense += handsCount * 25;
		if (npc.GetLifePercent() <= .75f) {
			handsCount = 0;
		}
		float value = (1 - npc.life / (float)npc.lifeMax) * .5f;
		npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().Endurance += value;
		npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().DamageIncrease += value;

		if (npc.GetLifePercent() <= .05f && state != State.Desperation && state != State.Despawn) {
			state = State.Desperation;
			npc.ai[3] = 0;
			npc.ai[2] = 0;
			npc.ai[1] = 0;
			npc.velocity = Vector2.Zero;
		}
		switch (state) {
			case State.Hover:
				Hover(npc, handsCount);
				break;
			case State.Spin:
				SpinAttack(npc, handsCount);
				break;
			case State.SpinDash:
				SpinDash(npc);
				break;
			case State.Despawn:
				Despawn(npc);
				break;
			case State.CircleAttack:
				CircleAttack(npc);
				break;
			case State.Stationary:
				Stationary(npc);
				break;
			case State.Desperation:
				Desperation(npc);
				break;
		}

		DustEffect(npc, handsCount);
	}
	/// <summary>
	/// This is attack handler after player successfully destroy the 2 hands or any other condition is fulfilled
	/// </summary>
	/// <param name="npc"></param>
	private void AttackHandler(NPC npc) {
		switch (npc.ai[3]) {
			case 1:
				state = State.Spin;
				break;
			case 2:
				state = State.SpinDash;
				break;
			case 3:
				state = State.CircleAttack;
				break;
			case 4:
				state = State.Stationary;
				break;
		}
		npc.ai[3] = ModUtils.Safe_SwitchValue((int)npc.ai[3], 4, 1);
	}
	private void Hover(NPC npc, int handsCount) {
		if (handsCount < 2 || npc.life < npc.lifeMax * 0.75) {
			float num151 = 80f;
			if (handsCount == 0)
				num151 /= 2f;

			if (Main.getGoodWorld)
				num151 *= 0.8f;

			if (npc.GetLifePercent() < .5f) {
				float percentageAdjust = Math.Clamp(npc.GetLifePercent() + .5f, .1f, 1);
				num151 *= percentageAdjust;
				num151 = (int)num151;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] % num151 == 0f) {
				if (npc.ai[2] % num151 == 0f) {
					ShootCursedSkull(npc, handsCount);
				}
				if (npc.ai[2] % (num151 * 3) == 0f) {
					Player player = Main.player[npc.target];
					if (Collision.CanHit(npc.Center, 1, 1, player.position, player.width, player.height)) {
						Vector2 playerCenter = player.Center + Main.rand.NextVector2Circular(50, 50);
						Vector2 vector19 = (playerCenter - npc.Center).SafeNormalize(Vector2.Zero);
						switch (npc.ai[3]) {
							case 1:
								for (int i = 0; i < 3; i++) {
									ShootProjectile(npc, vector19.Vector2DistributeEvenlyPlus(3, 60, i) * 7, ProjectileID.WaterBolt);
								}
								break;
							case 2:
								for (int i = 0; i < 4; i++) {
									for (int a = 1; a < 3; a++) {
										ShootProjectile(npc, vector19.RotatedBy(MathHelper.PiOver2 * i) * a * 6, ProjectileID.DemonScythe);
									}
								}
								break;
							case 3:
								Vector2 center3 = player.Center + Vector2.UnitX * 1000 * player.direction;
								Vector2 vel = (player.Center - center3).SafeNormalize(Vector2.Zero) * 14;
								int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
								for (int i = -3; i <= 3; i++) {
									int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3.Add(0, i * 30), vel, ProjectileID.ClothiersCurse, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
									Main.projectile[num160].timeLeft = 300;
									Main.projectile[num160].hostile = true;
									Main.projectile[num160].friendly = false;
								}
								break;
							case 4:
								for (int i = 0; i < 3; i++) {
									ShootProjectile(npc, -vector19.Vector2DistributeEvenlyPlus(3, 60, i) * 2, ProjectileID.Skull);
								}
								break;
						}
					}
				}
			}
		}

		npc.damage = npc.defDamage;
		float decreases = 700 * (1 - npc.GetLifePercent());
		if (++npc.ai[2] >= 800f - decreases) {
			npc.ai[2] = 0f;
			if (handsCount > 0) {
				state = State.Spin;
			}
			else {
				AttackHandler(npc);
			}
			npc.TargetClosest();
			Vector2 center3 = npc.Center;
			for (int i = 0; i < 12; i++) {
				Vector2 playerCenter = Main.player[npc.target].Center + Main.rand.NextVector2Circular(50, 50);
				Vector2 vector19 = (playerCenter - center3).SafeNormalize(Vector2.Zero);
				int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
				int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3,
					vector19.Vector2DistributeEvenlyPlus(12, 360, i) * 5
					, ProjectileID.Skull, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
				Main.projectile[num160].timeLeft = 300;
			}
			npc.netUpdate = true;
		}

		npc.rotation = npc.velocity.X / 15f;

		float UpwardAcceleration = 0.03f;
		float VerticalSpeedCap = 4f;
		float HorizontalAcceleration = 0.07f;
		float HorizontalSpeedCap = 9.5f;

		if (Main.getGoodWorld) {
			UpwardAcceleration += 0.01f;
			VerticalSpeedCap += 1f;
			HorizontalAcceleration += 0.05f;
			HorizontalSpeedCap += 2f;
		}
		int VerticalDirection = npc.position.Y > Main.player[npc.target].position.Y - 250f ?
			1 : npc.position.Y < Main.player[npc.target].position.Y - 250f ? -1 : 0;
		if (VerticalDirection != 0) {
			if (npc.velocity.Y * VerticalDirection > 0f)
				npc.velocity.Y *= 0.98f;

			npc.velocity.Y -= UpwardAcceleration * VerticalDirection;
			npc.velocity.Y = Math.Clamp(npc.velocity.Y, -VerticalSpeedCap, VerticalSpeedCap);
		}
		int HorizontalDirection = npc.Center.X > Main.player[npc.target].Center.X ?
			1 : npc.Center.X < Main.player[npc.target].Center.X ? -1 : 0;
		if (VerticalDirection != 0) {
			if (npc.velocity.X * HorizontalDirection > 0f)
				npc.velocity.X *= 0.98f;

			npc.velocity.X -= HorizontalAcceleration * HorizontalDirection;
			npc.velocity.X = Math.Clamp(npc.velocity.X, -HorizontalSpeedCap, HorizontalSpeedCap);
		}
	}
	private void SpinAttack(NPC npc, int handsCount) {
		if (Main.getGoodWorld) {
			if (handsCount > 0) {
				npc.reflectsProjectiles = true;
			}
		}

		npc.defense -= 10;
		npc.ai[2] += 1f;
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
		if (handsCount <= 0) {
			if (npc.ai[2] % 40 == 0) {
				Vector2 vel = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(60);
				ShootProjectile(npc, vel * 5, ProjectileID.Skull);
			}

			if (npc.ai[2] % 10 == 0) {
				Vector2 vel = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
				ShootProjectile(npc, -vel, ProjectileID.DemonScythe);
			}

			if (npc.ai[2] % 120 == 0) {
				Vector2 velOrigin = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
				for (int i = 1; i <= 24; i++) {
					Vector2 vel = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * MathHelper.Lerp(0, 1, (i % 8) / 8f));
					ShootProjectile(npc, vel * (i / 8 + 8),
						ProjectileID.ClothiersCurse);
				}
				for (int i = 0; i < 16; i++) {
					Vector2 vel = velOrigin.Vector2DistributeEvenlyPlus(16, 360, i) + velOrigin * 7;
					ShootProjectile(npc, vel, ProjectileID.WaterBolt);
				}
			}
		}
		else {
			if (npc.ai[2] % 40 == 0) {
				Vector2 vel = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
				ShootProjectile(npc, vel * 15, ProjectileID.DemonScythe);
			}
		}
		if (npc.ai[2] >= 400f) {
			npc.ai[2] = 0f;
			state = State.Hover;
			if (handsCount <= 0) {
				Vector2 velOrigin = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
				for (int i = 0; i < 16; i++) {
					Vector2 vel = velOrigin.Vector2DistributeEvenlyPlus(16, 360, i) * 2 + velOrigin * 15;
					ShootProjectile(npc, vel, ProjectileID.WaterBolt);
				}
			}
		}
		npc.rotation += npc.direction * 0.3f;
		Vector2 npcCenter = npc.Center;
		Vector2 playerCenter = Main.player[npc.target].Center;
		float num174 = Vector2.Distance(npcCenter, playerCenter);
		float num175 = 3.5f;
		npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage, npc.defDamage * 1.3f);

		if (num174 > 150f)
			num175 *= 1.05f;

		if (num174 > 200f)
			num175 *= 1.1f;

		if (num174 > 250f)
			num175 *= 1.1f;

		if (num174 > 300f)
			num175 *= 1.1f;

		if (num174 > 350f)
			num175 *= 1.1f;

		if (num174 > 400f)
			num175 *= 1.1f;

		if (num174 > 450f)
			num175 *= 1.1f;

		if (num174 > 500f)
			num175 *= 1.1f;

		if (num174 > 550f)
			num175 *= 1.1f;

		if (num174 > 600f)
			num175 *= 1.1f;

		num175 *= 1.1f - .05f * handsCount;

		if (Main.getGoodWorld)
			num175 *= 1.3f;

		num174 = num175 / num174;
		npc.velocity = (playerCenter - npcCenter) * num174;
	}
	private void Desperation(NPC npc) {
		npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().Endurance += .2f;
		CanDamagePlayer = true;
		Player player = Main.player[npc.target];
		npc.ai[2]++;
		switch (npc.ai[3]) {
			case 0:
				Inner_DesperationAttack_Dash(player, npc);
				break;
			case 1:
				Inner_DesperationAttack_Station(player, npc);
				break;
			case 2:
				Inner_DesperationAttack_Circle(player, npc);
				break;
			case 3:
				npc.velocity *= .96f;
				npc.velocity = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
				npc.rotation = Terraria.Utils.AngleTowards(npc.rotation, 0, MathHelper.ToRadians(10));
				npc.rotation += MathHelper.ToRadians(Main.rand.NextFloat(-10, 10));
				if (npc.ai[2] >= 240) {
					npc.ai[2] = 0;
					npc.ai[1] = 0;
					npc.ai[3] = ModUtils.Safe_SwitchValue((int)npc.ai[3], 4);
				}
				break;
			case 4:
				Inner_DesperationAttack_SpinDash(player, npc);
				break;
		}

	}
	private void Inner_DesperationAttack_Dash(Player player, NPC npc) {
		npc.rotation += npc.direction * 0.3f;
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
		if (npc.ai[2] % 240 == 0) {
			Vector2 center3 = player.Center + Vector2.UnitX * 1000 * player.direction;
			Vector2 vel = (player.Center - center3).SafeNormalize(Vector2.Zero) * 14;
			int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
			for (int i = -7; i <= 7; i++) {
				int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3.Add(0, i * 30), vel, ProjectileID.ClothiersCurse, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
				Main.projectile[num160].timeLeft = 150;
				Main.projectile[num160].hostile = true;
				Main.projectile[num160].friendly = false;
			}
		}

		if (++npc.ai[1] >= 20) {
			if (npc.ai[1] == 20) {
				SoundEngine.PlaySound(SoundID.Roar, npc.position);
				Vector2 pos = player.Center + Vector2.UnitX.Vector2RotateByRandom(30) * 750 * player.direction;
				npc.TeleportCommon(pos);
				ModUtils.DustStar(npc.Center, DustID.GemDiamond, Color.Azure, 20, 30, 0, 20);
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.GemDiamond);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.9f, 1.34f);
					dust.scale = Main.rand.NextFloat(.9f, 1.4f);
				}
				npc.velocity = Vector2.Zero;
			}
			npc.rotation += npc.direction * 0.1f;
			if (npc.ai[1] >= 60 && npc.ai[1] <= 105) {
				if (npc.ai[1] == 60) {
					Vector2 velOrigin = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
					for (int i = 1; i <= 16; i++) {
						Vector2 vel = velOrigin.InverseVector2DistributeEvenly(16, 360, i) * 10;
						ShootProjectile(npc, vel, ProjectileID.WaterBolt);
					}
					npc.velocity = (player.Center - npc.Center).SafeNormalize(Vector2.Zero) * 35;
				}
				for (int i = 0; i < 8; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.BoneTorch);
					dust.noGravity = true;
					dust.position += Main.rand.NextVector2Circular(npc.width, npc.height);
					dust.velocity = npc.velocity * .35f;
					dust.scale = Main.rand.NextFloat(.9f, 1.4f);
				}
				if (npc.ai[1] % 6 == 0) {
					Vector2 toward = npc.velocity.SafeNormalize(Vector2.Zero);
					for (int i = -1; i < 2; i++) {
						if (i == 0) {
							ShootProjectile(npc, (player.Center - npc.Center).SafeNormalize(Vector2.Zero) * 5, ProjectileID.Skull);
						}
						else {
							ShootProjectile(npc, toward.RotatedBy(MathHelper.PiOver2 * i), ProjectileID.DemonScythe);
						}
					}
				}
				npc.velocity += (player.Center - npc.Center).SafeNormalize(Vector2.Zero) / 8f;
			}
			if (npc.ai[1] >= 105) {
				npc.velocity = Vector2.Zero;
				if (npc.ai[1] % 20 == 0) {
					Vector2 velOrigin = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
					ShootProjectile(npc, velOrigin * 10, ProjectileID.ClothiersCurse);
				}
				if (npc.ai[1] >= 210) {
					npc.ai[1] = 0;
					npc.ai[2] = 0;
					npc.ai[3] = ModUtils.Safe_SwitchValue((int)npc.ai[3], 4);
				}
			}
		}
		else {
			npc.velocity += (player.Center - npc.Center).SafeNormalize(Vector2.Zero) * .1f;
			if (npc.ai[1] >= 30 && npc.ai[1] % 10 == 0) {
				ShootCursedSkull(npc, 0);
			}
		}
	}
	private void Inner_DesperationAttack_Station(Player player, NPC npc) {
		npc.velocity *= .96f;
		npc.velocity = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		npc.rotation = Terraria.Utils.AngleTowards(npc.rotation, 0, MathHelper.ToRadians(10));
		npc.rotation += MathHelper.ToRadians(Main.rand.NextFloat(-10, 10));
		if (npc.ai[2] == 60) {
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
		}
		if (npc.ai[2] >= 70 && npc.ai[2] <= 180) {
			Vector2 towardPlayer = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
			if (npc.ai[2] % 10 == 0) {
				for (int i = 0; i < 6; i++) {
					ShootProjectile(npc, Vector2.One.Vector2DistributeEvenly(6, 360, i) * 4f, ProjectileID.DemonScythe);
				}
			}
			if (npc.ai[2] % 3 == 0) {
				ShootProjectile(npc, Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(12, 18), ProjectileID.ClothiersCurse);
			}
			if (npc.ai[2] % 30 == 0) {
				ShootProjectile(npc, towardPlayer.Vector2RotateByRandom(30) * Main.rand.NextFloat(4, 6), ProjectileID.Skull);
			}
		}
		if (npc.ai[2] >= 220) {
			for (int i = 0; i < 12; i++) {
				ShootProjectile(npc, Vector2.One.Vector2DistributeEvenlyPlus(12, 360, i) * 10f, ProjectileID.WaterBolt);
				ShootProjectile(npc, Vector2.One.Vector2DistributeEvenly(12, 360, i) * 10f, ProjectileID.DemonScythe);
			}
			npc.ai[2] = 0;
			npc.ai[1] = 0;
			npc.ai[3] = ModUtils.Safe_SwitchValue((int)npc.ai[3], 4);
		}
	}
	private void Inner_DesperationAttack_SpinDash(Player player, NPC npc) {
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		if (npc.ai[2] % 20 == 0) {
			Vector2 vel = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
			ShootProjectile(npc, vel * 5, ProjectileID.Skull);
		}

		int count = 7;
		if (Main.getGoodWorld) {
			npc.reflectsProjectiles = true;
			count = 4;
		}
		if (npc.ai[2] % count == 0) {
			ShootProjectile(npc, Vector2.One.RotatedBy(MathHelper.ToRadians(npc.ai[2] * 5)) * 10, ProjectileID.ClothiersCurse);
		}

		npc.rotation += npc.direction * 0.3f;
		Vector2 npcCenter = npc.Center;
		Vector2 playerCenter = Main.player[npc.target].Center;
		float num174 = Vector2.Distance(npcCenter, playerCenter);
		float num175 = 3.5f;
		npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage, npc.defDamage * 1.3f);

		if (num174 > 150f)
			num175 *= 1.05f;

		if (num174 > 200f)
			num175 *= 1.1f;

		if (num174 > 250f)
			num175 *= 1.1f;

		if (num174 > 300f)
			num175 *= 1.1f;

		if (num174 > 350f)
			num175 *= 1.1f;

		if (num174 > 400f)
			num175 *= 1.1f;

		if (num174 > 450f)
			num175 *= 1.1f;

		if (num174 > 500f)
			num175 *= 1.1f;

		if (num174 > 550f)
			num175 *= 1.1f;

		if (num174 > 600f)
			num175 *= 1.1f;

		num175 *= 1.1f;

		if (Main.getGoodWorld)
			num175 *= 1.3f;

		num174 = num175 / num174;
		npc.velocity = (playerCenter - npcCenter) * num174;

		if (npc.ai[2] >= 180f) {
			npc.ai[2] = 0f;
			npc.ai[1] = 0;
			npc.ai[3] = ModUtils.Safe_SwitchValue((int)npc.ai[3], 4);

			for (int i = 1; i <= 24; i++) {
				Vector2 vel = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * MathHelper.Lerp(0, 1, (i % 8) / 8f));
				ShootProjectile(npc, vel * (i / 8 + 8),
					ProjectileID.ClothiersCurse);
			}
		}
	}
	private void Inner_DesperationAttack_Circle(Player player, NPC npc) {
		CanDamagePlayer = false;
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		npc.rotation += npc.direction * 0.3f;

		if (playerPosition == Vector2.Zero) {
			playerPosition = player.Center;
		}
		if (!playerPosition.IsCloseToPosition(player.Center, 200)) {
			playerPosition += (player.Center - playerPosition).SafeNormalize(Vector2.Zero) * ((player.Center - playerPosition).Length() / 8f);
		}
		Vector2 offSet = playerPosition + Vector2.One.RotatedBy(MathHelper.ToRadians(npc.ai[2] * 5)) * 500;
		npc.velocity = (offSet - npc.Center).SafeNormalize(Vector2.Zero) * ((offSet - npc.Center).Length() / 2f);

		//Half a second delay
		if (npc.ai[2] >= 30 && npc.ai[2] <= 150) {
			Vector2 toward = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
			if (npc.ai[2] % 30 == 0) {
				for (int i = 0; i < 5; i++) {
					if (i == 2) {
						continue;
					}
					ShootProjectile(npc, toward.Vector2DistributeEvenlyPlus(5, 60, i) * 6, ProjectileID.WaterBolt, 180);
				}
			}
			if (npc.ai[2] % 15 == 0) {
				ShootProjectile(npc, toward.SafeNormalize(Vector2.Zero), ProjectileID.DemonScythe, 120);
			}
			if (npc.ai[2] % 2 == 0) {
				ShootProjectile(npc, npc.velocity.SafeNormalize(Vector2.Zero), ProjectileID.DemonScythe, 120);
			}
		}
		if (npc.ai[2] >= 180) {
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
			for (int i = 0; i < 12; i++) {
				ShootProjectile(npc, Vector2.One.Vector2DistributeEvenlyPlus(12, 360, i) * 10, ProjectileID.Skull, 300);
			}
			for (int i = 0; i < 18; i++) {
				ShootProjectile(npc, Vector2.One.Vector2DistributeEvenlyPlus(18, 360, i) * 10, ProjectileID.ClothiersCurse, 120);
			}
			npc.velocity *= .1f;
			npc.ai[2] = 0f;
			npc.ai[1] = 0;
			npc.ai[3] = ModUtils.Safe_SwitchValue((int)npc.ai[3], 4);
		}
	}
	private void SpinDash(NPC npc) {
		npc.defense -= 10;
		npc.ai[2] += 1f;

		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		npc.rotation += npc.direction * 0.3f;

		Player player = Main.player[npc.target];

		if (++npc.ai[1] >= 40) {
			if (npc.ai[1] == 40) {
				SoundEngine.PlaySound(SoundID.Roar, npc.position);
				Vector2 pos = player.Center + Vector2.UnitX.Vector2RotateByRandom(30) * 750 * player.direction;
				npc.TeleportCommon(pos);
				ModUtils.DustStar(npc.Center, DustID.GemDiamond, Color.Azure, 20, 30, 0, 20);
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.GemDiamond);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.9f, 1.34f);
					dust.scale = Main.rand.NextFloat(.9f, 1.4f);
				}
				npc.velocity = Vector2.Zero;
			}
			npc.rotation += npc.direction * 0.1f;
			if (npc.ai[1] >= 90) {
				if (npc.ai[1] == 90) {
					Vector2 velOrigin = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
					for (int i = 1; i <= 24; i++) {
						Vector2 vel = velOrigin.RotatedBy(MathHelper.TwoPi * MathHelper.Lerp(0, 1, (i % 8) / 8f));
						ShootProjectile(npc, vel * (i / 8 + 4),
							ProjectileID.ClothiersCurse);
					}
					npc.velocity = (player.Center - npc.Center).SafeNormalize(Vector2.Zero) * 30;
				}
				for (int i = 0; i < 8; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.BoneTorch);
					dust.noGravity = true;
					dust.position += Main.rand.NextVector2Circular(npc.width, npc.height);
					dust.velocity = npc.velocity * .35f;
					dust.scale = Main.rand.NextFloat(.9f, 1.4f);
				}
				if (npc.ai[1] % 6 == 0) {
					Vector2 toward = npc.velocity.SafeNormalize(Vector2.Zero);
					for (int i = -1; i < 2; i++) {
						ShootProjectile(npc, toward.RotatedBy(MathHelper.PiOver2 * i), ProjectileID.DemonScythe);
					}
				}
				npc.velocity += (player.Center - npc.Center).SafeNormalize(Vector2.Zero) / 8f;
			}
			if (npc.ai[1] >= 145) {
				npc.velocity = Vector2.Zero;
				npc.ai[1] = 0;
			}
		}
		else {
			npc.velocity += (player.Center - npc.Center).SafeNormalize(Vector2.Zero) * .1f;
			if (npc.ai[1] >= 30 && npc.ai[1] % 5 == 0) {
				ShootCursedSkull(npc, 0);
			}
		}

		if (npc.ai[2] >= 600f) {
			npc.ai[2] = 0f;
			npc.ai[1] = 0;
			state = State.Hover;
		}

	}
	Vector2 playerPosition = Vector2.Zero;
	private void CircleAttack(NPC npc) {
		npc.defense -= 10;
		npc.ai[2] += 1f;
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		npc.rotation += npc.direction * 0.3f;


		Player player = Main.player[npc.target];
		if (playerPosition == Vector2.Zero) {
			playerPosition = player.Center;
		}
		if (!playerPosition.IsCloseToPosition(player.Center, 200)) {
			playerPosition += (player.Center - playerPosition).SafeNormalize(Vector2.Zero) * ((player.Center - playerPosition).Length() / 8f);
		}
		Vector2 offSet = playerPosition + Vector2.One.RotatedBy(MathHelper.ToRadians(npc.ai[2] * 5)) * 500;
		npc.velocity = (offSet - npc.Center).SafeNormalize(Vector2.Zero) * ((offSet - npc.Center).Length() / 2f);

		//Half a second delay
		if (npc.ai[2] >= 30) {
			Vector2 velOrigin = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
			if (npc.ai[2] % 10 == 0) {
				int type = ProjectileID.Skull;
				switch (++npc.ai[1]) {
					case 0:
						type = ProjectileID.Skull;
						break;
					case 1:
						type = ProjectileID.ClothiersCurse;
						break;
				}
				if (npc.ai[1] >= 1) {
					npc.ai[1] = -1;
				}
				ShootProjectile(npc, velOrigin * 5, type);
			}
			if (npc.ai[2] % 2 == 0) {
				ShootProjectile(npc, npc.velocity.SafeNormalize(Vector2.Zero), ProjectileID.DemonScythe, 120);
			}
			if (npc.ai[2] % 120 == 0) {
				for (int i = 0; i < 6; i++) {
					ShootProjectile(npc, velOrigin.Vector2DistributeEvenlyPlus(6, 60, i) * 4, ProjectileID.WaterBolt);
				}
			}
			if (npc.ai[2] % 240 == 0) {
				Vector2 center3 = player.Center + Vector2.UnitX * 1000 * player.direction;
				Vector2 vel = (player.Center - center3).SafeNormalize(Vector2.Zero) * 14;
				int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
				for (int i = -5; i <= 5; i++) {
					int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3.Add(0, i * 30), vel, ProjectileID.ClothiersCurse, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
					Main.projectile[num160].timeLeft = 300;
					Main.projectile[num160].hostile = true;
					Main.projectile[num160].friendly = false;
				}
			}
		}
		if (npc.ai[2] >= 480) {
			npc.velocity *= .1f;
			npc.ai[2] = 0f;
			npc.ai[1] = 0;
			state = State.Hover;
		}
	}
	private void Stationary(NPC npc) {
		npc.ai[2] += 1f;
		npc.velocity *= .96f;
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		npc.rotation = Terraria.Utils.AngleTowards(npc.rotation, 0, MathHelper.ToRadians(10));
		npc.rotation += MathHelper.ToRadians(Main.rand.NextFloat(-5, 5));
		if (npc.ai[2] >= 120) {
			Player player = Main.player[npc.target];
			Vector2 towardPlayer = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
			if (npc.ai[2] % 4 == 0) {
				ShootProjectile(npc, Vector2.One.RotatedBy(MathHelper.ToRadians(npc.ai[2] * 10)) * .1f, ProjectileID.DemonScythe);
			}
			if (npc.ai[2] % 5 == 0) {
				ShootProjectile(npc, towardPlayer.Vector2RotateByRandom(10) * Main.rand.NextFloat(8, 13), ProjectileID.ClothiersCurse);
			}
			if (npc.ai[2] % 20 == 0) {
				ShootProjectile(npc, towardPlayer.Vector2RotateByRandom(30) * Main.rand.NextFloat(3, 6), ProjectileID.Skull);
			}
		}
		if (npc.ai[2] >= 420) {
			npc.ai[2] = 0f;
			npc.ai[1] = 0;
			state = State.Hover;
		}
	}
	private void Despawn(NPC npc) {
		npc.velocity.Y += 0.1f;
		if (npc.velocity.Y < 0f)
			npc.velocity.Y *= 0.95f;

		npc.velocity.X *= 0.95f;
		npc.timeLeft = npc.timeLeft > 50 ? 50 : npc.timeLeft;
		npc.despawnEncouraged = true;
	}
	private void DustEffect(NPC npc, int handsCount) {
		if (state != State.DungeonGuardian && state != State.Despawn && handsCount != 0) {
			int num179 = Dust.NewDust(new Vector2(npc.position.X + (float)(npc.width / 2) - 15f - npc.velocity.X * 5f, npc.position.Y + (float)npc.height - 2f), 30, 10, DustID.Blood, (0f - npc.velocity.X) * 0.2f, 3f, 0, default, 2f);
			Main.dust[num179].noGravity = true;
			Main.dust[num179].velocity.X *= 1.3f;
			Main.dust[num179].velocity.X += npc.velocity.X * 0.4f;
			Main.dust[num179].velocity.Y += 2f + npc.velocity.Y;
			for (int num180 = 0; num180 < 2; num180++) {
				num179 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 120f), npc.width, 60, DustID.Blood, npc.velocity.X, npc.velocity.Y, 0, default, 2f);
				Main.dust[num179].noGravity = true;
				Dust dust = Main.dust[num179];
				dust.velocity -= npc.velocity;
				Main.dust[num179].velocity.Y += 5f;
			}
		}
	}
}
public class Everlasting_Skeletron_Scene : ModSceneEffect {
	public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Skeletron_BossMusic");
	public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
	public override bool IsSceneEffectActive(Player player) {
		return NPC.AnyNPCs(NPCID.SkeletronHead);
	}
}
