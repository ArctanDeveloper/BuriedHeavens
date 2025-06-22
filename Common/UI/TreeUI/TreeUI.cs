using BuriedHeavens.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuriedHeavens.Common.UI.TreeUI {
    public class TreeMenu : UIState {
        private Asset<Texture2D> SephirahNode;
        private UIElement SephirahNodeSingle, SephirahNodeTree;
        public PanelUI DraggablePanel;
        private readonly string KetherString = "Kether", TiphString = "Tiphereth", YesodString = "Jesod", MalkuthString = "Malchuth";
        private readonly UIText KetherText, TiphText, YesodText, MalkuthText;
        private List<BetterItemSlot> itemSlots;
        private readonly float itemSlotSize = 52 * 0.65f;
        private readonly int[] itemSlotsL = [20, 20 + (int)(2.5f * (52 * 0.65f))];

        private static readonly RasterizerState OverflowHiddenRasterizerState = new()
        {
            CullMode = CullMode.None,
            ScissorTestEnable = true
        };

        public override void OnInitialize()
        {
            SephirahNode = ModContent.Request<Texture2D>("BuriedHeavens/Assets/Textures/UI/SephirahNode");

            DraggablePanel = new PanelUI();
            DraggablePanel.SetPadding(0);
            float panelWidth = 500f, panelHeight = 750f;
            Vector2 InitPos = new(Main.screenWidth, Main.screenHeight / 2);
            SetRect(DraggablePanel, panelWidth, panelHeight, InitPos.Y, InitPos.X);
            DraggablePanel.BackgroundColor = new(73, 94, 171, 200);

            SephirahNodeTree = new()
            {
                Left = new StyleDimension(0, 0f),
                Top = new StyleDimension(0, 0f),
                Width = new StyleDimension(-170, 1f),
                Height = new StyleDimension(0, 1f),
            };
            DraggablePanel.Append(SephirahNodeTree);

            HoverImageButton SephirahNodeSingle1 = new(SephirahNode);
            SetRect(SephirahNodeSingle1, 130f, 130f, 20f, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle1);

            HoverImageButton SephirahNodeSingle2 = new(SephirahNode);
            SetRect(SephirahNodeSingle2, 130f, 130f, 20f + 130f * 2, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle2);

            HoverImageButton SephirahNodeSingle3 = new(SephirahNode);
            SetRect(SephirahNodeSingle3, 130f, 130f, 20f + 130f * 3 + 20f, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle3);

            HoverImageButton SephirahNodeSingle4 = new(SephirahNode);
            SetRect(SephirahNodeSingle4, 130f, 130f, 20f + 130f * 4 + 40f, panelWidth / 3f);
            SephirahNodeTree.Append(SephirahNodeSingle4); //Object reference error here apparently

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
            closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
            DraggablePanel.Append(closeButton);

            Append(DraggablePanel);
        }

        private static void SetRect(UIElement uielement, float w = 0, float h = 0, float t = 0, float l = 0, float extraScaleW = 0, float extraScaleH = 0, float extraScaleL = 0, float extraScaleT = 0)
        {
            uielement.Width.Set(w, extraScaleW);
            uielement.Height.Set(h, extraScaleH);
            uielement.Top.Set(t, extraScaleT);
            uielement.Left.Set(l, extraScaleL);
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            CloseUI();
        }

        private static void CloseUI()
        {
            ModContent.GetInstance<TreeDisplaySystem>().HideUI();
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.dead)
            {
                CloseUI();
                return;
            }
            UpdateNode();
            base.Update(gameTime);
        }

        private void UpdateNode()
        {
            var modPlayer = ModContent.GetInstance<TreePlayer>();
            if (modPlayer == null) return;

            if (modPlayer.MalkuthUnlock) MalkuthText.SetText(MalkuthString);
            else MalkuthText.SetText("?????");
            if (modPlayer.YesodUnlock) YesodText.SetText(YesodString);
            else YesodText.SetText("?????");
            if (modPlayer.TiphUnlock) TiphText.SetText(TiphString);
            else TiphText.SetText("?????");
            if (modPlayer.KetherUnlock) KetherText.SetText(KetherString);
            else KetherText.SetText("?????");
        }
    }
}