using Terraria.ModLoader;

namespace BuriedHeavens.Common
{
	public class KeybindSystem : ModSystem
	{
		public static ModKeybind OpenTreeStatus {  get; private set; }

        public override void Load()
        {
            OpenTreeStatus = KeybindLoader.RegisterKeybind(Mod, "OpenTreeStatus", Microsoft.Xna.Framework.Input.Keys.None);
        }

        public override void Unload()
        {
            OpenTreeStatus = null;
        }
    }
}