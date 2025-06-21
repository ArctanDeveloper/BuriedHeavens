using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Terraria;

namespace BuriedHeavens.Core.AberrantOculiCrafting {
    public static class AberrantOculiCraftingManager {
        public static List<AberrantOculiRecipe> Recipes = [];
        public static bool TryGetRecipe(AberrantOculiRecipeInput input, out int aberrantOculiRecipe) {
            for (int i = 0; i < Recipes.Count; i++) {
                if (Recipes[i].validInput?.Invoke(ref input) ?? false) {
                    aberrantOculiRecipe = i;
                    return true;
                }
            }

            aberrantOculiRecipe = -1;
            return false;
        }
    }

    public ref struct AberrantOculiRecipeInput {
        public AberrantOculiRecipeInput(ref Player player, ref Item primary, ref Item secondary, ref Item tertiary, ref Item quaternary, ref Item tome, ref Item relic) {
            this.player = ref player;
            this.primary = ref primary;
            this.secondary = ref secondary;
            this.tertiary = ref tertiary;
            this.quaternary = ref quaternary;
            this.tome = ref tome;
            this.relic = ref relic;
        }

        public ref Player player;
        public ref Item primary;
        public ref Item secondary;
        public ref Item tertiary;
        public ref Item quaternary;
        public ref Item tome;
        public ref Item relic;
    }

    public delegate bool InputDelegate(ref AberrantOculiRecipeInput input);
    public delegate Item OutputDelegate(ref AberrantOculiRecipeInput input);
    public delegate void OperationDelegate(ref AberrantOculiRecipeInput input);

    public struct AberrantOculiRecipe(InputDelegate validInput, OutputDelegate output, OperationDelegate operation) {
        public InputDelegate validInput = validInput;
        public OutputDelegate output = output;
        public OperationDelegate operation = operation;
    }
}