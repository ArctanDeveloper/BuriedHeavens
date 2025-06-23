using BuriedHeavens.Common.Players;
using BuriedHeavens.Common.Systems;
using BuriedHeavens.Content.Items.Consumables;
using BuriedHeavens.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace BuriedHeavens.Common.UI.GeneSplicerUI {
    internal partial class GeneSplicerUIState : UIState {
        private Asset<Texture2D> dnaStrand;

        private UIPanel background;
        private UIImageButton closeButton;
        private UIButton<string> spliceButton;
        private UIElement geneticsArea;

        private UIScrollbar scrollbar;
        private UIPanel itemArea;
        public List<BetterItemSlot> itemSlots;

        private List<int> deletions = new();
        private bool add = false;

        public override void OnInitialize() {
            dnaStrand = ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/DNAStrand");

            background = new() {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue,
                Left = new StyleDimension(0, 0.35f),
                Top = new StyleDimension(0, 0.1f),
                Width = new StyleDimension(500, 0f),
                Height = new StyleDimension(600, 0f)
            };            

            closeButton = new(ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/CloseButton")) {
                Left = new StyleDimension(0, 0f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(32, 0f),
                Height = new StyleDimension(32, 0f)
            };
            closeButton.OnLeftClick += (mouseEvent, element) => {
                if (mouseEvent.Target == element && Main.LocalPlayer.TryGetModPlayer(out GeneSplicerPlayer geneSplicerPlayer)) {   
                    geneSplicerPlayer.CloseUI(itemSlots);
                    itemSlots.Clear();
                    itemArea.RemoveAllChildren();
                    AddSlot();
                }
            };

            geneticsArea = new() {
                Left = new StyleDimension(0, 0f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(-170, 1f),
                Height = new StyleDimension(0, 1f),
                IgnoresMouseInteraction = true
            };

            spliceButton = new("Splice") {
                BackgroundColor = Color.DarkSlateBlue,
                TextColor = Color.DimGray,
                Left = new StyleDimension(-125, 0.5f),
                Top = new StyleDimension(-64, 1f),
                Width = new StyleDimension(80, 0f),
                Height = new StyleDimension(40, 0f)
            };

            spliceButton.OnLeftClick += new MouseEvent(SpliceButtonClicked);

            itemArea = new() {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue,
                Left = new StyleDimension(-170, 1f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(150, 0f),
                Height = new StyleDimension(0, 1f),
                OverflowHidden = true
            };

            scrollbar = new() {
                Left = new StyleDimension(-20, 1f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(20, 0f),
                Height = new StyleDimension(0, 1f)
            };

            itemSlots = [];
            AddSlot();

            scrollbar.SetView(MathF.Max(background.Height.Pixels, 2 + itemSlots.Count * 52), 2 + itemSlots.Count * 52);

            scrollbar.OnUpdate += (element) => {
                for (int i = 0; i < itemSlots.Count; i++) {
                    itemSlots[i].Top.Set(2 + i * 52 - scrollbar.GetValue(), 0f);
                }
            };

            background.Append(closeButton);
            background.Append(scrollbar);
            background.Append(itemArea);
            background.Append(geneticsArea);
            background.Append(spliceButton);

            // The idea here is you add in an item and a new slot gets added, if you remove an item that slot is removed.
            // Always one empty slot at the end to put an item into.
            // This one should be saved and stored in the tile, but the Aberrant Oculi should not store any items in the tile, when the menu is closed the items should be given back to the player.

            Append(background);
        }

        public void RemoveSlot(int index) {
            if (index >= 0 && index < itemSlots.Count) {
                itemArea.RemoveChild(itemSlots[index]);
                itemSlots.RemoveAt(index);
            }
        }

        public void AddItem(Item item) {
            itemSlots[^1].Item = item;
            AddSlot();
        }

        public void AddSlot() {
            BetterItemSlot temp = new() {
                ValidItemFunc = (item) => {
                    return true;
                },
                ItemChangeFunc = (slot, oldItem, newItem) => {
                    if (newItem.IsAir) {
                        deletions.Add(itemSlots.IndexOf(slot));
                    } else if (oldItem.IsAir) {
                        add = true;
                    }
                    return true;
                },
                extra = [itemSlots.Count],
                Top = new StyleDimension(2 + itemSlots.Count * 52 - scrollbar.GetValue(), 0f)
            };
            itemSlots.Add(temp);
            itemArea.Append(temp);
            scrollbar.SetView(MathF.Min(itemArea.GetInnerDimensions().Height, 2 + itemSlots.Count * 52), 2 + itemSlots.Count * 52);
        
            if (itemSlots.Count > 1) {
                spliceButton.BackgroundColor = Color.MediumBlue;
                spliceButton.TextColor = Color.Black;
            } else {
                spliceButton.BackgroundColor = Color.DarkSlateBlue;
                spliceButton.TextColor = Color.DimGray;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Main.LocalPlayer.TryGetModPlayer(out GeneSplicerPlayer geneSplicerPlayer) || geneSplicerPlayer.hasUIOpen || geneSplicerPlayer.tileEntity == null) {
                return;
            }

            base.Draw(spriteBatch);

            Rectangle inner = geneticsArea.GetInnerDimensions().ToRectangle();
            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)inner.TopLeft().X, (int)inner.TopLeft().Y, 2, 2), null, Color.Blue);
            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)inner.Center().X - 1, (int)inner.Center().Y - 1, 2, 2), null, Color.Blue);
            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)inner.BottomRight().X - 2, (int)inner.BottomRight().Y - 2, 2, 2), null, Color.Blue);

            spriteBatch.Draw(dnaStrand.Value, inner, Color.Red);
        }

        public override void Update(GameTime gameTime) {
            if (!Main.LocalPlayer.TryGetModPlayer(out GeneSplicerPlayer geneSplicerPlayer) || geneSplicerPlayer.hasUIOpen || geneSplicerPlayer.tileEntity == null) {
                return;
            }

            base.Update(gameTime);

            if (geneSplicerPlayer.dirty) {
                for (int i = 0; i < geneSplicerPlayer.tileEntity.inventory.Count; i++) {
                    AddItem(geneSplicerPlayer.tileEntity.inventory[i]);
                }
                geneSplicerPlayer.dirty = false;
            }

            if (deletions.Count > 0) {
                deletions.Sort();
                deletions.Reverse();
                for (int i = 0; i < deletions.Count; i++) {
                    RemoveSlot(deletions[i]);
                }
                deletions.Clear();
                
                for (int i = 0; i < itemSlots.Count; i++) {
                    itemSlots[i].Top.Set(2 + i * 52 - scrollbar.GetValue(), 0f);
                }

                if (itemSlots.Count > 1) {
                    spliceButton.BackgroundColor = Color.MediumBlue;
                    spliceButton.TextColor = Color.White;
                } else {
                    spliceButton.BackgroundColor = Color.DarkSlateBlue;
                    spliceButton.TextColor = Color.DimGray;
                }
            }

            if (add) {
                AddSlot();
                add = false;

                if (itemSlots.Count > 1) {
                    spliceButton.BackgroundColor = Color.MediumBlue;
                    spliceButton.TextColor = Color.White;
                } else {
                    spliceButton.BackgroundColor = Color.DarkSlateBlue;
                    spliceButton.TextColor = Color.DimGray;
                }
            }

            if (RecipeCheck(itemSlots, out int result))
            {
                spliceButton.BackgroundColor = Color.MediumBlue;
                spliceButton.TextColor = Color.White;
            }
            else
            {
                spliceButton.BackgroundColor = Color.DarkSlateBlue;
                spliceButton.TextColor = Color.DimGray;
            }
        }

        private bool CustomCrafting(List<BetterItemSlot> itemSlots)
        {
            var player = Main.LocalPlayer;
            if (RecipeCheck(itemSlots, out int result))
            {
                player.QuickSpawnItem(player.GetSource_FromThis(), result);

                if (!ModContent.GetInstance<TreeSystem>().YesodCheck() && ModContent.GetInstance<TreeSystem>().MalkuthCheck())
                {
                    ModContent.GetInstance<TreeSystem>().pathway.Append(1);
                }

                if (!player.GetModPlayer<TreePlayer>().YesodUnlock) player.GetModPlayer<TreePlayer>().YesodUnlock = true;
                if (result == ModContent.ItemType<DubiousDinosaurEgg>())
                {
                    if (!ModContent.GetInstance<TreeSystem>().TiphCheck() && ModContent.GetInstance<TreeSystem>().YesodCheck())
                    {
                        ModContent.GetInstance<TreeSystem>().pathway.Append(2);
                    }
                }

                if (result == ModContent.ItemType<PaleontologistsCampfireItem>())
                {
                    //ModContent.GetInstance<TreeSystem>().DebugFeature();
                }
                return true;
            }
            return false;
        }

        private static List<int> GetCurrentCombination(List<BetterItemSlot> itemSlots)
        {
            List<int> currentCombination = [];
            for (int i = 0; i < itemSlots.Count; i++)
            {
                currentCombination.Add(itemSlots[i].Item.type);
            }

            return currentCombination;
        }

        private void SpliceButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (RecipeCheck(itemSlots, out int result))
            {
                if (CustomCrafting(itemSlots))
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    SoundEngine.PlaySound(SoundID.ResearchComplete);
                    if (evt.Target == listeningElement && Main.LocalPlayer.TryGetModPlayer(out GeneSplicerPlayer geneSplicerPlayer))
                    {
                        itemSlots.Clear();
                        itemArea.RemoveAllChildren();
                        AddSlot();
                    }
                }
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class GeneSplicerUISystem : ModSystem {
        private UserInterface GeneSplicerUserInterface;

        internal GeneSplicerUIState GeneSplicerUIState;

        public override void Load() {
            GeneSplicerUIState = new();
            GeneSplicerUserInterface = new();
            GeneSplicerUserInterface.SetState(GeneSplicerUIState);
        }

        public override void SetStaticDefaults() {
            GeneSplicerUIState = new();
            GeneSplicerUserInterface = new();
            GeneSplicerUserInterface.SetState(GeneSplicerUIState);
        }

        public override void UpdateUI(GameTime gameTime) {
            GeneSplicerUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1) {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "BuriedHeavens: Gene Splicer",
                    delegate {
                        GeneSplicerUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}