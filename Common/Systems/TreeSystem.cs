using BuriedHeavens.Core.Progression;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
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

        public static Tree LifeTree = new();
        public static Tree DeathTree = new();

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

        public override void SaveWorldData(TagCompound tag) {
            tag.Set(WORLD_TREE_PATHWAY_TAG, pathway);
            tag.Set(UNLOCKED_PALEONTOLOGIST_SPAWN_TAG, UnlockedPaleontologistSpawn);
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.ContainsKey(WORLD_TREE_PATHWAY_TAG)) {
                pathway = tag.GetIntArray(WORLD_TREE_PATHWAY_TAG);
            }

            if (tag.TryGet(UNLOCKED_PALEONTOLOGIST_SPAWN_TAG, out bool unlocked)) {
                UnlockedPaleontologistSpawn = unlocked;
            }

            if (Main.ActiveWorldFileData.TryGetHeaderData<TreeSystem>(out TagCompound data) && data.TryGet(WORLD_TREE_TYPE_TAG, out int worldTreeType)) {
                worldTree = worldTreeType;
            }
        }
    }
}