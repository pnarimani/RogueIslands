using System;
using System.Linq;
using System.Text;
using IngameDebugConsole;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View;
using RogueIslands.Serialization;

namespace RogueIslands.Debug
{
    public static class BoosterCommands
    {
        [ConsoleMethod("add_booster", "Add a booster to the player's inventory. Usage: add_booster <booster_name>")]
        public static void AddBooster(string name)
        {
            var booster = GameManager.Instance.State.AvailableBoosters.FirstOrDefault(CompareName(name));
            if (booster == null)
            {
                UnityEngine.Debug.LogError($"Failed to find a booster with name {name}");
                return;
            }

            StaticResolver.Resolve<BoosterManagement>().TryAddBooster(booster);
        }

        [ConsoleMethod("remove_booster",
            "Remove a booster from the player's inventory. Usage: remove_booster <booster_name>")]
        public static void RemoveBooster(string name)
        {
            var booster = GameManager.Instance.State.Boosters.FirstOrDefault(CompareName(name));
            if (booster == null)
                return;
            StaticResolver.Resolve<BoosterManagement>().SellBooster(booster.Id);
        }

        [ConsoleMethod("refresh_boosters", "Refreshes owned boosters")]
        public static void Refresh()
        {
            var state = GameManager.Instance.State;
            state.AvailableBoosters = BoosterList.Get();
            foreach (var c in state.Boosters)
            {
                var def = state.AvailableBoosters.FirstOrDefault(CompareName(c.Name));
                if (def == null)
                    continue;
                c.Description = def.Description;
                c.EventAction = StaticResolver.Resolve<ICloner>().Clone(def.EventAction);
                c.BuyPrice = def.BuyPrice;
                c.SellPrice = Math.Max(1, def.BuyPrice / 2);
                c.Rarity = def.Rarity;
            }
        }

        [ConsoleMethod("refresh_descriptions", "Refreshes booster descriptions")]
        public static void RefreshDescriptions()
        {
            var state = GameManager.Instance.State;
            state.AvailableBoosters = BoosterList.Get();
            foreach (var t in state.Boosters)
                t.Description = state.AvailableBoosters.FirstOrDefault(CompareName(t.Name))?.Description;
        }

        [ConsoleMethod("booster_stats", "Prints all booster statistics")]
        public static void PrintStats()
        {
            var state = GameManager.Instance.State;
            state.AvailableBoosters = BoosterList.Get();

            var sb = new StringBuilder();
            sb.AppendLine("Total Boosters: " + state.AvailableBoosters.Count);
            sb.AppendLine("Common: " + state.AvailableBoosters.Count(x => x.Rarity == Rarity.Common));
            sb.AppendLine("Uncommon: " + state.AvailableBoosters.Count(x => x.Rarity == Rarity.Uncommon));
            sb.AppendLine("Rare: " + state.AvailableBoosters.Count(x => x.Rarity == Rarity.Rare));
            sb.AppendLine("Legendary: " + state.AvailableBoosters.Count(x => x.Rarity == Rarity.Legendary));

            UnityEngine.Debug.Log(sb.ToString());
        }

        private static Func<BoosterCard, bool> CompareName(string name)
        {
            name = name.Replace(" ", "");
            return x => x.Name.Replace(" ", "").Equals(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}