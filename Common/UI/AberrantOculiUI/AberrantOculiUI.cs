using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using BuriedHeavens.Common.Players;
using BuriedHeavens.Core.AberrantOculiCrafting;
using Terraria.GameContent;
using ReLogic.Graphics;
using BuriedHeavens.Content;
using log4net;

namespace BuriedHeavens.Common.UI.AberrantOculiUI {
    internal class AberrantOculiUIState : UIState {
        private UIPanel background;
        private UIImageButton closeButton;

        private BetterItemSlot output;
        private BetterItemSlot primarySlot;
        private BetterItemSlot secondarySlot;
        private BetterItemSlot tertiarySlot;
        private BetterItemSlot quaternarySlot;
        private BetterItemSlot tomeSlot;
        private BetterItemSlot relicSlot;

        private int cachedRecipe = -1;

        public override void OnInitialize() {
            background = new() {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue,
                Left = new StyleDimension(0, 0.35f),
                Top = new StyleDimension(0, 0.1f),
                Width = new StyleDimension(360, 0f),
                Height = new StyleDimension(300, 0f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
            };            

            closeButton = new(ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/CloseButton")) {
                Left = new StyleDimension(0, 0f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(32, 0f),
                Height = new StyleDimension(32, 0f)
            };
            closeButton.OnLeftClick += (mouseEvent, element) => {
                if (mouseEvent.Target == element && Main.LocalPlayer.TryGetModPlayer(out AberrantOculiPlayer aberrantOculiPlayer)) {   
                    aberrantOculiPlayer.CloseUI();
                    DropItems();
                }
            };

            output = new() {
                Left = new StyleDimension(156, 0f),
                Top = new StyleDimension(96, 0f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ValidItemFunc = (item) => item.IsAir,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    if (!oldItem.IsAir && newItem.IsAir && cachedRecipe != -1) {
                        AberrantOculiRecipeInput inputer = new(ref Main.player[Main.myPlayer], ref primarySlot.Item, ref secondarySlot.Item, ref tertiarySlot.Item, ref quaternarySlot.Item, ref tomeSlot.Item, ref relicSlot.Item);
                        AberrantOculiCraftingManager.Recipes[cachedRecipe].operation.Invoke(ref inputer);
                        UpdateRecipe();
                    }
                    return true;
                }
            };

            primarySlot = new() {
                Left = new StyleDimension(156, 0f),
                Top = new StyleDimension(24, 0f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    UpdateRecipe();
                    return true;
                }
            };

            secondarySlot = new() {
                Left = new StyleDimension(228, 0f),
                Top = new StyleDimension(96, 0f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    UpdateRecipe();
                    return true;
                }
            };

            tertiarySlot = new() {
                Left = new StyleDimension(156, 0f),
                Top = new StyleDimension(168, 0f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    UpdateRecipe();
                    return true;
                }
            };

            quaternarySlot = new() {
                Left = new StyleDimension(84, 0f),
                Top = new StyleDimension(96, 0f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    UpdateRecipe();
                    return true;
                }
            };

            tomeSlot = new() {
                Left = new StyleDimension(24, 0f),
                Top = new StyleDimension(-72, 1f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    UpdateRecipe();
                    return true;
                }
            };

            relicSlot = new() {
                Left = new StyleDimension(-72, 1f),
                Top = new StyleDimension(-72, 1f),
                MarginTop = 0,
                MarginBottom = 0,
                MarginRight = 0,
                MarginLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0,
                PaddingRight = 0,
                PaddingLeft = 0,
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    UpdateRecipe();
                    return true;
                }
            };

            background.Append(closeButton);
            background.Append(output);
            background.Append(primarySlot);
            background.Append(secondarySlot);
            background.Append(tertiarySlot);
            background.Append(quaternarySlot);
            background.Append(tomeSlot);
            background.Append(relicSlot);

            Append(background);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Main.LocalPlayer.TryGetModPlayer(out AberrantOculiPlayer aberrantOculiPlayer) || aberrantOculiPlayer.hasUIOpen || aberrantOculiPlayer.tileEntity == null) {
                return;
            }

            base.Draw(spriteBatch);

            spriteBatch.DrawString(FontAssets.MouseText.Value, $"Recipe: {cachedRecipe}", background.GetOuterDimensions().Center() + new Vector2(0, 120), Color.Black);
        }

        public override void Update(GameTime gameTime) {
            if (!Main.LocalPlayer.TryGetModPlayer(out AberrantOculiPlayer aberrantOculiPlayer) || aberrantOculiPlayer.hasUIOpen || aberrantOculiPlayer.tileEntity == null) {
                return;
            }

            base.Update(gameTime);
        }

        public void UpdateRecipe() {
            //ILog logger = ModContent.GetInstance<BuriedHeavens>().Logger;
            AberrantOculiRecipeInput inputer = new(ref Main.player[Main.myPlayer], ref primarySlot.Item, ref secondarySlot.Item, ref tertiarySlot.Item, ref quaternarySlot.Item, ref tomeSlot.Item, ref relicSlot.Item);
            //logger.Debug($"{inputer.player} {inputer.primary} {inputer.secondary} {inputer.tertiary} {inputer.quaternary} {inputer.tome} {inputer.relic}");
            //logger.Debug("Called");
            if (cachedRecipe != -1 && AberrantOculiCraftingManager.Recipes[cachedRecipe].validInput.Invoke(ref inputer)) {
                //logger.Debug("Duplicate");
                if (output.Item.IsAir) {  
                    output.Item = AberrantOculiCraftingManager.Recipes[cachedRecipe].output.Invoke(ref inputer);    
                }
                return;
            }

            //logger.Debug(AberrantOculiCraftingManager.Recipes);

            cachedRecipe = AberrantOculiCraftingManager.TryGetRecipe(inputer, out int recipeIndex) ? recipeIndex : -1;

            //logger.Debug(cachedRecipe);

            if (cachedRecipe == -1) {
                //logger.Debug("Turn To Air");
                output.Item.TurnToAir();
            } else {
                //logger.Debug("Invoked Output");
                output.Item = AberrantOculiCraftingManager.Recipes[cachedRecipe].output.Invoke(ref inputer);
            }
        }

        public void DropItems() {
            if (!primarySlot.Item.IsAir)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), primarySlot.Item);
            if (!secondarySlot.Item.IsAir)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), secondarySlot.Item);
            if (!tertiarySlot.Item.IsAir)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), tertiarySlot.Item);
            if (!quaternarySlot.Item.IsAir)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), quaternarySlot.Item);
            if (!tomeSlot.Item.IsAir)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), tomeSlot.Item);
            if (!relicSlot.Item.IsAir)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), relicSlot.Item);
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class AberrantOculiUISystem : ModSystem {
        private UserInterface AberrantOculiUserInterface;

        internal AberrantOculiUIState AberrantOculiUIState;

        public override void Load() {
            AberrantOculiUIState = new();
            AberrantOculiUserInterface = new();
            AberrantOculiUserInterface.SetState(AberrantOculiUIState);
        }

        public override void SetStaticDefaults() {
            AberrantOculiUIState = new();
            AberrantOculiUserInterface = new();
            AberrantOculiUserInterface.SetState(AberrantOculiUIState);
        }

        public override void UpdateUI(GameTime gameTime) {
            AberrantOculiUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "BuriedHeavens: Aberrant Oculi",
                    delegate {
                        AberrantOculiUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}