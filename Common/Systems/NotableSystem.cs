using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BuriedHeavens.Core.Progression;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuriedHeavens.Common.Systems {
    public class NotableSystem : ModSystem {
        public const int TIME_LEFT = 1000;

        public const string NOTABLE_LOCATIONS_TAG = "BuriedHeavens:NotableLocations";
        public const string TIME_LEFT_TAG = "BuriedHeavens:TimeLeft";

        public static List<Point16> notableLocations = [];
        public static int timeLeft = 1000;

        public static Point16 Nearby(Point16 position) {
            float distance = float.MaxValue;
            Point16 go = new();
            foreach (Point16 pos in notableLocations) {
                float rot = MathF.Sqrt((pos.X - position.X) * (pos.X - position.X) + (pos.Y - position.Y) * (pos.Y - position.Y));
                if (rot < distance) {
                    go = pos;
                }
            }
            return go;
        }

        public override void SaveWorldData(TagCompound tag) {
            tag.Set(NOTABLE_LOCATIONS_TAG, notableLocations);
            tag.Set(TIME_LEFT_TAG, timeLeft);
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.ContainsKey(NOTABLE_LOCATIONS_TAG)) {
                notableLocations = tag.GetList<Point16>(NOTABLE_LOCATIONS_TAG).ToList();
            } else {
                GenerateNotableLocations();
            }

            if (tag.TryGet(TIME_LEFT_TAG, out int time)) {
                timeLeft = time;
            }
        }

        public void GenerateNotableLocations() {
            int sections = WorldGen.GetWorldSize() switch  {
                0 => 500,
                1 => 1000,
                2 => 1500,
                _ => 2000
            };

            for (int i = 0; i < sections; i++) {
                notableLocations.Add(new Point16(Main.rand.Next(0, Main.maxTilesX), Main.rand.Next(0, Main.maxTilesY)));
            }
        }
    }
}