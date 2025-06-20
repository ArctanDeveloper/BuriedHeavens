using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using BuriedHeavens.Common.Players;
using System;
using ReLogic.Content;

namespace BuriedHeavens.Common.UI.GeneSplicerUI {
    internal class GeneSplicerUIState : UIState {
        private Asset<Texture2D> dnaStrand;

        private UIPanel background;
        private UIImageButton closeButton;
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
            };

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
            itemSlots[itemSlots.Count - 1].Item = item;
            AddSlot();
        }

        public void AddSlot() {
            BetterItemSlot temp = new BetterItemSlot() {
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
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Main.LocalPlayer.TryGetModPlayer(out GeneSplicerPlayer geneSplicerPlayer) || geneSplicerPlayer.hasUIOpen || geneSplicerPlayer.tileEntity == null) {
                return;
            }

            base.Draw(spriteBatch);

            Rectangle inner = geneticsArea.GetInnerDimensions().ToRectangle();

            spriteBatch.Draw(dnaStrand.Value, inner.Center.ToVector2() - new Vector2(200, 225), Color.Red);
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
            }

            if (add) {
                AddSlot();
                add = false;
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