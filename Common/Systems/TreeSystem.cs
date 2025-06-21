using BuriedHeavens.Core.Progression;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuriedHeavens.Common.Systems {
    public class TreeSystem : ModSystem {
        public static Asset<Texture2D> LifeIcon;
        public static Asset<Texture2D> DeathIcon;

        public static Tree LifeTree = new();
        public static Tree DeathTree = new();

        public const string WORLD_TREE_TYPE_TAG = "BuriedHeavens:WorldTreeType";
        public const string WORLD_TREE_PATHWAY_TAG = "BuriedHeavens:WorldTreePathway";

        public int worldTree = -1;
        public int[] pathway = [];

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
                        0 => LifeIcon,
                        1 => DeathIcon,
                        _ => LifeIcon
                    }) { IgnoresMouseInteraction = true, Left = new(4f, 0f) };
                    self.Append(image);
                }
            };
        }

        public override void SaveWorldData(TagCompound tag) {
            tag.Set(WORLD_TREE_PATHWAY_TAG, pathway);
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.ContainsKey(WORLD_TREE_PATHWAY_TAG)) {
                pathway = tag.GetIntArray(WORLD_TREE_PATHWAY_TAG);
            }

            if (Main.ActiveWorldFileData.TryGetHeaderData<TreeSystem>(out TagCompound data) && data.TryGet(WORLD_TREE_TYPE_TAG, out int worldTreeType)) {
                worldTree = worldTreeType;
            }
        }
    }
}