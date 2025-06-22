using System;
using System.Collections.Generic;
using BuriedHeavens.Common.Systems;
using BuriedHeavens.Content.Items;
using BuriedHeavens.Content.Items.Placeable.Fossils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace BuriedHeavens.Content.NPCs {
    [AutoloadHead]
	public class Paleontologist : ModNPC {
        public static List<int> ValidFossilTiles = [];
        public static List<int> ValidFossilItems = [];
        public const string ShopName = "Shop";

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load() {
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 16;

			NPCID.Sets.ExtraFramesCount[Type] = 0;
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 1;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 35;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Talks a bit about their personality.
			NPC.Happiness
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Love) // Loves because of all the fossils there, and it's hot enough to be comfortable.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Like) // Likes because it's a good place for fossils, but it's a bit too cold for them.
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike) // Dislikes becuase there aren't that many places to find things.
				.SetBiomeAffection<DungeonBiome>(AffectionLevel.Hate) // Hates because the remains are desecrated and unstudyable.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Love) // Loves because they will sell them back fossils at a cheaper price.
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
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 45;
			NPC.defense = 25;
			NPC.lifeMax = 1250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Mods.BuriedHeavens.Bestiary.Paleontologist")
			]);
		}

		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 2 : 25;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand);
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
			if (TreeSystem.UnlockedPaleontologistSpawn) {
				return true;
			}
            
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom) {
			for (int x = left; x <= right; x++) {
				for (int y = top; y <= bottom; y++) {
					int type = Main.tile[x, y].TileType;
					if (Paleontologist.ValidFossilTiles.Contains(type)) {
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
			return new List<string>() {
				"Karen",
				"E. Drinker Cope",
				"Bakker",
				"Darwin",
                "Zhiming",
                "Horner",
                "James",
                "Mary",
                "Wolfgang",
                "Carl",
                "Irene Crespin",
                "Margaret C.",
                "Taiping",
                "Georgii",
                "Vera Gromova",
                "Wataru Ishijima",
                "Junchang",
                "Yoshiaki",
                "Ernesto Pérez",
                "Reisz",
                "Guilherme",
                "Rosalvina",
                "Scheibner",
                "Brian J. Ford",
                "Vishnu-Mittre",
                "Xu Xing",
                "Anusuya",
                "Philip J. Senter"
			};
		}

		public override void FindFrame(int frameHeight) {
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

            int merchant = NPC.FindFirstNPC(NPCID.Merchant);
            if (merchant >= 0 && Main.rand.NextBool(3)) {
                chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.MerchantDialogue", Main.npc[merchant].GivenName));
            }

            int guide = NPC.FindFirstNPC(NPCID.Guide);
            if (guide >= 0 && Main.rand.NextBool(4)) {
                chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.GuideDialogue", Main.npc[guide].GivenName));
            }
            
            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad >= 0 && Main.rand.NextBool(4)) {
                chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.DryadDialogue", Main.npc[dryad].GivenName));
            }
            
            int demolitionist = NPC.FindFirstNPC(NPCID.Demolitionist);
            if (demolitionist >= 0 && Main.rand.NextBool(3)) {
                chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.DemolitionistDialogue", Main.npc[demolitionist].GivenName));
            }
            
			chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.StandardDialogue1"));
			chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.StandardDialogue2"));
			chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.StandardDialogue3"));
			chat.Add(Language.GetTextValue("Mods.BuriedHeavens.Dialogue.Paleontologist.StandardDialogue4"));

            

			string chosenChat = chat;

			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) {
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = ShopName;
			}
		}

		public override void AddShops() {
            NPCShop npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Grenade)
                .Add(ItemID.SandBlock)
                .Add(ItemID.DesertFossil);
            if (Main.rand.NextBool(5)) {
                npcShop.Add<SkullFossilItem>(Condition.PlayerCarriesItem(ModContent.ItemType<PaleontologistsCertificate>()), Condition.InDesert);
            } 
            npcShop.Register();
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				if (item == null || item.type == ItemID.None) {
					continue;
				}

				if (NPC.IsShimmerVariant && !ValidFossilItems.Contains(item.type)) {
					int value = item.shopCustomPrice ?? item.value;
					item.shopCustomPrice = (int)Math.Ceiling(value * 0.95f);
				}
			}
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

        public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset) {
            Main.GetItemDrawFrame(ItemID.Revolver, out item, out itemFrame);
            horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1f, ItemID.Revolver).X;
        }

        

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileID.SilverBullet;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 1.4f;
		}
	}
}