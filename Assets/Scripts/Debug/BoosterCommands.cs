using System;
using System.Linq;
using IngameDebugConsole;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View;

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
            GameManager.Instance.GetUI().RefreshAll();
        }

        [ConsoleMethod("remove_booster",
            "Remove a booster from the player's inventory. Usage: remove_booster <booster_name>")]
        public static void RemoveBooster(string name)
        {
            var booster = GameManager.Instance.State.Boosters.FirstOrDefault(CompareName(name));
            if (booster == null)
                return;
            StaticResolver.Resolve<BoosterManagement>().SellBooster(booster.Id);
            GameManager.Instance.GetUI().RefreshAll();
        }

        private static Func<BoosterCard, bool> CompareName(string name)
        {
            name = name.Replace(" ", "");
            return x => x.Name.Replace(" ", "").Equals(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}