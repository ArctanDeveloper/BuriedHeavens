using BuriedHeavens.Common.Systems;
using BuriedHeavens.Content.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace BuriedHeavens.Content.NPCs {
    [AutoloadHead]
	public class Death : ModNPC {
        public const string ShopName = "Shop";

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

        int timer = 0;

        public override void Load() {
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 4;

			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 35;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() {
				Velocity = 1f
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Talks a bit about their personality.
			NPC.Happiness
				.SetBiomeAffection<DungeonBiome>(AffectionLevel.Love) // Loves the dead.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Like) // Likes because it's a good place for fossils, but it's a bit too cold for them.
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike) // Dislikes becuase there aren't that many places to find things.
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate) // Hates because the remains are desecrated and unstudyable.
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Love) // Loves because they will sell them back fossils at a cheaper price.
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes because they help them identify fossils sometimes.
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike) // Dislikes because they chastize them for digging up fossils.
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates because they are likely to destroy fossils with their explosives.
			;

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
			);
		}

		public override void SetDefaults() {
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 42;
			NPC.height = 58;
			NPC.aiStyle = 7;
			NPC.damage = 45;
			NPC.defense = 250;
			NPC.lifeMax = 12500;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f; 
		}

        public override void AI() {
            NPC.FaceTarget();
            timer++;
        }

        public override void FindFrame(int frameHeight) {
			NPC.frame.Width = NPC.width;
			NPC.frame.Height = NPC.height;
			NPC.frame.X = 0;
            if (NPC.velocity.Length() == 0) {
                NPC.frame.Y = 0;
            } else {
                NPC.frame.Y = frameHeight * (1 + (timer / 20 % 3));
            }
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Mods.BuriedHeavens.Bestiary.Death")
			]);
		}

		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 2 : 25;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight);
			}

			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				string variant = "";
				if (NPC.IsShimmerVariant)
					variant += "_Shimmer";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) {
			if (ModContent.GetInstance<TreeSystem>().worldTree == (int)WorldTreeID.DEATH) {
				return true;
			}
            
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom) {
			for (int x = left; x <= right; x++) {
				for (int y = top; y <= bottom; y++) {
					int type = Main.tile[x, y].TileType;
					if (type == ModContent.TileType<AberrantOculi>()) {
                        return true;
                    }
                }
			}

			return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return [
				"Death"
			];
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new();
            
			chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Death.StandardDialogue1"));

			string chosenChat = chat;

			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) {
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = ShopName;
                if (ModContent.GetInstance<TreeSystem>().worldTree != (int)WorldTreeID.DEATH)
					ModContent.GetInstance<TreeSystem>().worldTree = (int)WorldTreeID.DEATH;
                if (!ModContent.GetInstance<TreeSystem>().MalkuthCheck())
                {
                    ModContent.GetInstance<TreeSystem>().pathway.Append(0);
                }
            }
		}

		public override void AddShops() {
            NPCShop npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Bone);
            npcShop.Register();
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 50;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileID.DeathSickle;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 1.4f;
		}
	}
}