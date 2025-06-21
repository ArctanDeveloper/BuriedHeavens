using System;
using System.Collections.Generic;
using BuriedHeavens.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens {
	public class BuriedHeavens : Mod {
        public static Dictionary<int, PolishableData> PolishableData = [];

		/// <summary>
		/// Handles inter-mod communication calls with various arguments.
		/// </summary>
		/// <param name="args">
		/// An array of arguments specifying the action and its parameters. 
		/// The first argument should be a <see cref="string"/> corosponding to a implemented action type from the following list:
		/// <br></br>
        /// addPolishable
        /// <list type="number">
		/// <item><description>Item ID of the item to be polishable.</description></item>
		/// <item><description>Results as an array of <see cref="Tuple{int, int, int, int}"/>, paramter one is the item type, parameter two is the min stack, three the max stack, and four the weight of the option.</description></item>
		/// </list>
		/// </param>
		/// <returns>
		/// Returns <see cref="true"/> if the action was successfully handled; otherwise, <see cref="false"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if <paramref name="args"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Thrown if <paramref name="args"/> is empty.
		/// </exception>
        public override object Call(params object[] args) {
            ArgumentNullException.ThrowIfNull(args);

            if (args.Length == 0) {
				throw new ArgumentException("Arguments cannot be empty!");
			}

			if (args[0] is string argument) {
                switch (argument) {
                    case "addPolishable":
                        if (args[1] is int itemID && args[2] is Tuple<int, int, int, int>[] results) {
                            PolishableData[itemID] = new PolishableData(itemID, results);
                			return true;
                        }
                        break;
                }
			}

            return false;
        }

        public override void PostSetupContent() {
            Call("addPolishable",
				ModContent.ItemType<AncientDebris>(),
                new Tuple<int, int, int, int>[] {
                    new(ItemID.Bone, 1, 5, 2),
                    new(ItemID.FossilOre, 1, 4, 4),
                    new(ItemID.DirtBlock, 1, 2, 3),
                    new(ItemID.Amber, 1, 3, 2)
                });
        }
    
	}

	public struct PolishableData {
		public PolishableData(int itemID, PolishableEntry[] entries) {
            this.itemID = itemID;
            this.entries = entries;

			foreach (PolishableEntry entry in this.entries) {
                TotalWeight += entry.weight;
            }
        }

		public PolishableData(int itemID, Tuple<int, int, int, int>[] entries) {
            this.itemID = itemID;
            this.entries = new PolishableEntry[entries.Length];

			for (int i = 0; i < entries.Length; i++) {
                this.entries[i] = new(entries[i].Item1, entries[i].Item2, entries[i].Item3, entries[i].Item4);
                TotalWeight += entries[i].Item4;
            }
        }

		public readonly bool TryGetResult(out int type, out int amount) {
			type = amount = 0;
            if (TotalWeight == 0) {
                return false;
            }
            int index = 0;
            int option = Main.rand.Next(0, TotalWeight);
			while (option > 0) {
                option -= entries[index++].weight;
            }
            index = MathEx.Clamp(index - 1, 0, entries.Length);
            type = entries[index].type;
            amount = Main.rand.Next(entries[index].minAmount, entries[index].maxAmount);
            return true;
        }

        public int itemID;
        public PolishableEntry[] entries;

        public readonly int TotalWeight;
    }

	public struct PolishableEntry(int type, int minAmount, int maxAmount, int weight) {
        public int type = type;
        public int minAmount = minAmount;
        public int maxAmount = maxAmount;
        public int weight = weight;
    }
}
