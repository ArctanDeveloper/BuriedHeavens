using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuriedHeavens.Common.UI.TreeUI {
    [Autoload(Side = ModSide.Client)]
    public class TreeDisplaySystem : ModSystem {
        internal TreeMenu TreeMenu;
        private UserInterface TreeMenuUserInterface;

        public override void Load()
        {
            if (Main.dedServ) return;
            TreeMenu = new TreeMenu();
            TreeMenuUserInterface = new UserInterface();
            TreeMenu.Activate();
        }

        public override void Unload()
        {
            base.Unload();
            TreeMenu.Deactivate();
            TreeMenuUserInterface = null;
            TreeMenu = null;
        }

        public override void PreUpdateTime()
        {
            if (KeybindSystem.OpenTreeStatus.JustPressed)
            {
                if (TreeMenuUserInterface.CurrentState == null) ShowUI();
                else HideUI();
            }
        }

        public override void PostUpdatePlayers()
        {

        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (TreeMenuUserInterface?.CurrentState != null)
            {
                TreeMenuUserInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "BuriedHeavens: Description",
                    delegate
                    {
                        TreeMenuUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void ShowUI()
        {
            TreeMenuUserInterface?.SetState(TreeMenu);
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }

        private void HideUITrue()
        {
            TreeMenuUserInterface?.SetState(null);
        }

        public void HideUI()
        {
            HideUITrue();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}