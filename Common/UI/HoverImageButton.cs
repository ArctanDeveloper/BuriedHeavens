using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;

namespace BuriedHeavens.Common.UI
{
    internal class HoverImageButton : UIImageButton
    {
        internal string hoverText;

        public HoverImageButton(Asset<Texture2D> texture, string hoverText = " ") : base(texture)
        {
            this.hoverText = hoverText;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (IsMouseHovering)
                Main.hoverItemName = hoverText;
        }
    }
}
