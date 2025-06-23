using System;
using BuriedHeavens.Content.NPCs;
using BuriedHeavens.Content.Tiles;
using BuriedHeavens.Core.Progression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuriedHeavens.Common.Systems {
    public enum WorldTreeID: int {
        LIFE = 0,
        DEATH = 1,
    }

    public class TreeSystem : ModSystem {
        public static Asset<Texture2D> LifeIcon;
        public static Asset<Texture2D> DeathIcon;

        public static Tree LifeTree = new() {
            nodes = [
                new() {
                    Name = "Malchuth"
                },
                new() {
                    Name = "Jesod"
                },
                new() {
                    Name = "Tiphereth"
                },
                new() {
                    Name = "Kether"
                },
            ],
            connections = [
                [1],
                [2],
                [3],
                []
            ]
        };
        public static Tree DeathTree = new() {
            nodes = [
                new() {
                    Name = "Kether"
                },
                new() {
                    Name = "Tiphereth"
                },
                new() {
                    Name = "Jesod"
                },
                new() {
                    Name = "Malchuth"
                },
            ],
            connections = [
                [1],
                [2],
                [3],
                []
            ]
        };

        public const string UNLOCKED_PALEONTOLOGIST_SPAWN_TAG = "BuriedHeavens:UnlockedPaleontologistSpawn";
        public const string GENERATED_PALEONTOLOGIST_SPAWN_TAG = "BuriedHeavens:GeneratedPaleontologistSpawn";
        public const string WORLD_TREE_TYPE_TAG = "BuriedHeavens:WorldTreeType";
        public const string WORLD_TREE_PATHWAY_TAG = "BuriedHeavens:WorldTreePathway";

        public int worldTree = -1;
        public int[] pathway = [];

        public static bool UnlockedPaleontologistSpawn = false;
        public static bool GeneratedPaleontologistSpawn = false;

        public override void SaveWorldHeader(TagCompound tag) {
            tag.Set(WORLD_TREE_TYPE_TAG, worldTree, true);
        }

        public override void Load() {
            LifeIcon = ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/LifeWorldTree");
            DeathIcon = ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/DeathWorldTree");
            On_UIWorldListItem.DrawSelf += static (orig, self, spriteBatch) => {
                orig(self, spriteBatch);
                if (self.Data.TryGetHeaderData<TreeSystem>(out TagCompound data) && data.TryGet(WORLD_TREE_TYPE_TAG, out int worldTreeType) && worldTreeType != -1) {
                    UIImage image = new(worldTreeType switch {
                        (int)WorldTreeID.LIFE => LifeIcon,
                        (int)WorldTreeID.DEATH => DeathIcon,
                        _ => LifeIcon
                    }) { IgnoresMouseInteraction = true, Left = new(4f, 0f) };
                    self.Append(image);
                }
            };
        }

        public static void GeneratePaleontologistSpawn() {
            GeneratedPaleontologistSpawn = false;
            while (!GeneratedPaleontologistSpawn) {
                Point position = WorldGen.RandomWorldPoint((int)Main.worldSurface - 100, 0, (int)Main.rockLayer, 0);
                for (int i = Math.Max(position.X - 30, 0); i < Math.Min(position.X + 30, Main.maxTilesX); i++) {
                    for (int j = Math.Max(position.Y - 30, 0); j < Math.Min(position.Y + 30, Main.maxTilesY); j++) {
                        if (WorldGen.EmptyTileCheck(i-1, i + 1, j - 1, j) && !(WorldGen.TileEmpty(i - 1, j + 1) || WorldGen.TileEmpty(i, j + 1) || WorldGen.TileEmpty(i + 1, j + 1))) {
                            WorldGen.PlaceTile(i, j, ModContent.TileType<PaleontologistsCampfire>());
                            NPC.NewNPCDirect(Entity.GetSource_NaturalSpawn(), new Point(i, j).ToWorldCoordinates(), ModContent.NPCType<EvilPaleontologist>(), 0, 0, 0, i, j);
                            GeneratedPaleontologistSpawn = true;
                        }
                    }
                }
            }
            Main.NewText(Language.GetTextValue("Mods.BuriedHeavens.ChatNotifications.PaleontologistSetUpCamp"));
        }

        public override void SaveWorldData(TagCompound tag) {
            tag.Set(WORLD_TREE_PATHWAY_TAG, pathway);
            tag.Set(UNLOCKED_PALEONTOLOGIST_SPAWN_TAG, UnlockedPaleontologistSpawn);
            tag.Set(GENERATED_PALEONTOLOGIST_SPAWN_TAG, GeneratedPaleontologistSpawn);
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.ContainsKey(WORLD_TREE_PATHWAY_TAG)) {
                pathway = tag.GetIntArray(WORLD_TREE_PATHWAY_TAG);
            }

            if (!tag.TryGet(UNLOCKED_PALEONTOLOGIST_SPAWN_TAG, out bool unlocked) || !unlocked) {
                if (!tag.TryGet(GENERATED_PALEONTOLOGIST_SPAWN_TAG, out bool generated) || !generated) {
                    GeneratePaleontologistSpawn();
                } else {
                    GeneratedPaleontologistSpawn = true;
                }
                UnlockedPaleontologistSpawn = false;
            } else {
                UnlockedPaleontologistSpawn = true;
            }

            if (Main.ActiveWorldFileData.TryGetHeaderData<TreeSystem>(out TagCompound data) && data.TryGet(WORLD_TREE_TYPE_TAG, out int worldTreeType)) {
                worldTree = worldTreeType;
            }
        }
    }
}