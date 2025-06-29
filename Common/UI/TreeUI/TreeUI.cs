using BuriedHeavens.Common.Players;
using BuriedHeavens.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuriedHeavens.Common.UI.TreeUI {
    public class TreeMenu : UIState {
        private Asset<Texture2D> SephirahNode;
        //private UIElement SephirahNodeSingle;
        private UIElement SephirahNodeTree;
        public PanelUI DraggablePanel;
        private const string KetherString = "Kether",
            TiphString = "Tiphereth",
            YesodString = "Jesod",
            MalkuthString = "Malchuth";
        private const string KetherStringDeath = "Thaumiel",
            TiphStringDeath = "Tagiriron",
            YesodStringDeath = "Gamaliel",
            MalkuthStringDeath = "Lilith";
        private UIText KetherText, TiphText, YesodText, MalkuthText;
        //private List<BetterItemSlot> itemSlots;
        //private readonly float itemSlotSize = 52 * 0.65f;
        //private readonly int[] itemSlotsL = [20, 20 + (int)(2.5f * (52 * 0.65f))];
        private HoverImageButton SephirahNodeSingle1;
        private HoverImageButton SephirahNodeSingle2;
        private HoverImageButton SephirahNodeSingle3;
        private HoverImageButton SephirahNodeSingle4;

        //private static readonly RasterizerState OverflowHiddenRasterizerState = new()
        //{
        //    CullMode = CullMode.None,
        //    ScissorTestEnable = true
        //};

        public override void OnInitialize()
        {
            SephirahNode = ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/SephirahNode");

            DraggablePanel = new PanelUI() {
                Left = new StyleDimension(0, 0f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(500f, 0f),
                Height = new StyleDimension(750f, 0f),
            };
            DraggablePanel.SetPadding(0);
            DraggablePanel.BackgroundColor = new(73, 94, 171, 200);
            float panelWidth = 500f;

            SephirahNodeTree = new()
            {
                Left = new StyleDimension(0, 0f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(-170, 1f),
                Height = new StyleDimension(0, 1f),
            };
            DraggablePanel.Append(SephirahNodeTree);

            SephirahNodeSingle1 = new(SephirahNode);
            SetRect(SephirahNodeSingle1, 130f, 130f, 20f, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle1);

            SephirahNodeSingle2 = new(SephirahNode);
            SetRect(SephirahNodeSingle2, 130f, 130f, 20f + 130f * 2, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle2);

            SephirahNodeSingle3 = new(SephirahNode);
            SetRect(SephirahNodeSingle3, 130f, 130f, 20f + 130f * 3 + 20f, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle3);

            SephirahNodeSingle4 = new(SephirahNode);
            SetRect(SephirahNodeSingle4, 130f, 130f, 20f + 130f * 4 + 40f, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle4); //Object reference error here apparently

            KetherText = new("");
            TiphText = new("");
            YesodText = new("");
            MalkuthText = new("");

            KetherText.HAlign = KetherText.VAlign = 0.5f;
            SephirahNodeSingle1.Append(KetherText);
            TiphText.HAlign = TiphText.VAlign = 0.5f;
            SephirahNodeSingle2.Append(TiphText);
            YesodText.HAlign = YesodText.VAlign = 0.5f;
            SephirahNodeSingle3.Append(YesodText);
            MalkuthText.HAlign = MalkuthText.VAlign = 0.5f;
            SephirahNodeSingle4.Append(MalkuthText);

            Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonDelete");
            HoverImageButton closeButton = new(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52"));
            SetRect(closeButton, 22f, 22f, 20f, 20f);
            closeButton.OnLeftClick += static (evt, listeningElement) => CloseUI();
            DraggablePanel.Append(closeButton);

            Append(DraggablePanel);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch) {
            Vector2 position0 = SephirahNodeSingle1.GetInnerDimensions().Center();
            Vector2 position1 = SephirahNodeSingle2.GetInnerDimensions().Center();
            Vector2 position2 = SephirahNodeSingle3.GetInnerDimensions().Center();
            Vector2 position3 = SephirahNodeSingle4.GetInnerDimensions().Center();

            Texture2D pixel = TextureAssets.MagicPixel.Value;

            base.DrawChildren(spriteBatch);

            DrawLine(spriteBatch, position0 + new Vector2(0, -4 + SephirahNodeSingle1.GetOuterDimensions().Width * 0.5f), position1 - new Vector2(0, -4 + SephirahNodeSingle1.GetOuterDimensions().Width * 0.5f), pixel, Color.Black);
            DrawLine(spriteBatch, position1 + new Vector2(0, -4 + SephirahNodeSingle1.GetOuterDimensions().Width * 0.5f), position2 - new Vector2(0, -4 + SephirahNodeSingle1.GetOuterDimensions().Width * 0.5f), pixel, Color.Black);
            DrawLine(spriteBatch, position2 + new Vector2(0, -4 + SephirahNodeSingle1.GetOuterDimensions().Width * 0.5f), position3 - new Vector2(0, -4 + SephirahNodeSingle1.GetOuterDimensions().Width * 0.5f), pixel, Color.Black);
        }

        private static void DrawLine(SpriteBatch spriteBatch, Vector2 position0, Vector2 position1, Texture2D pixel, Color color) {
            spriteBatch.Draw(pixel, new Rectangle((int)position0.X, (int)position0.Y, (int)position0.Distance(position1), 10), null, color, position0.AngleTo(position1), new Vector2(0, 5), SpriteEffects.None, 1);
        }

        private static void SetRect(UIElement uielement, float w = 0, float h = 0, float t = 0, float l = 0, float extraScaleW = 0, float extraScaleH = 0, float extraScaleL = 0, float extraScaleT = 0) {
            uielement.Width.Set(w, extraScaleW);
            uielement.Height.Set(h, extraScaleH);
            uielement.Top.Set(t, extraScaleT);
            uielement.Left.Set(l, extraScaleL);
        }

        private static void CloseUI() {
            ModContent.GetInstance<TreeDisplaySystem>().HideUI();
        }

        public override void Update(GameTime gameTime) {
            if (Main.LocalPlayer.dead) {
                CloseUI();
                return;
            }
            UpdateNode();
            base.Update(gameTime);
        }

        private void UpdateNode() {
            TreeSystem tree = ModContent.GetInstance<TreeSystem>();
            switch (tree.worldTree) {
                case (int)WorldTreeID.LIFE:
                    MalkuthText.SetText(tree.pathway.Contains(0) ? MalkuthString : "?????");
                    YesodText.SetText(tree.pathway.Contains(1) ? YesodString : "?????");
                    TiphText.SetText(tree.pathway.Contains(2) ? TiphString : "?????");
                    KetherText.SetText(tree.pathway.Contains(3) ? KetherString : "?????");
                    break;
                case (int)WorldTreeID.DEATH:
                    MalkuthText.SetText(tree.pathway.Contains(3) ? MalkuthString : "?????");
                    YesodText.SetText(tree.pathway.Contains(2) ? YesodString : "?????");
                    TiphText.SetText(tree.pathway.Contains(1) ? TiphString : "?????");
                    KetherText.SetText(tree.pathway.Contains(0) ? KetherString : "?????");
                    break;
                default:
                    MalkuthText.SetText("?????");
                    YesodText.SetText("?????");
                    TiphText.SetText("?????");
                    KetherText.SetText("?????");
                    break;
            }
        }
    }
}