using System;
using System.Linq;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Localization;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Descriptions
{
    public class DescriptionTextProvider
    {
        public static string Get(IDescribableItem item)
        {
            if (item is IBooster booster) return GetBoosterDescription(booster);
            if (item is Building building) return GetBuildingDescription(building); 
            return item.Description.Text;
        }

        private static string GetBuildingDescription(Building building)
        {
            var text = building.Description.Text;
            text = ReplaceCategories(text);
            text = ReplaceColors(text);
            text = ReplaceSizes(text);
            return text;
        }

        private static string GetBoosterDescription(IBooster booster)
        {
            var text = booster.Description.Text;

            using var conditions = booster.EventAction.GetAllConditions();
            var probability = conditions.OfType<ProbabilityCondition>().FirstOrDefault();

            if (probability != null)
                text = text.ReplaceIgnoreCase("{probability}", GetProbabilityText(probability));

            text = ReplaceCategories(text);
            text = ReplaceColors(text);
            text = ReplaceSizes(text);

            var scoring = booster.GetEventAction<ScoringAction>();
            var modifyBonus = booster.GetEventAction<ModifyBonusAction>();
            var multipliedScoring = booster.GetEventAction<MultipliedScoringAction>();
            var scalingAction = booster.GetEventAction<BoosterScalingAction>();

            if (scoring?.Multiplier is { } mult)
            {
                var xmult = $"x{mult:0.#} score";
                text = text.ReplaceIgnoreCase("{mult}", xmult);
                text = text.ReplaceIgnoreCase("{score}", xmult);
            }

            if (scoring?.Products is { } products)
            {
                var add = $"+{products:0.#} score";
                text = text.ReplaceIgnoreCase("{add}", add);
                text = text.ReplaceIgnoreCase("{score}", add);
            }
            
            if (modifyBonus != null)
            {
                if (modifyBonus.Multiplier is {} bmult)
                {
                    var multText = $"x{bmult:0.#} bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }
                
                if (modifyBonus.Add is {} badd)
                {
                    var prodText = $"+{badd:0.#} bonus";
                    text = text.ReplaceIgnoreCase("{add}", prodText);
                    text = text.ReplaceIgnoreCase("{score}", prodText);
                }
                
                if(modifyBonus.CategoryMultiplier is {} catMult)
                {
                    var multText = $"x{catMult:0.#} bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }
                
                if(modifyBonus.ColorMultiplier is {} colMult)
                {
                    var multText = $"x{colMult:0.#} bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }
                
                if(modifyBonus.SizeMultiplier is {} sizeMult)
                {
                    var multText = $"x{sizeMult:0.#} bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }
            }

            if (multipliedScoring != null)
            {
                var factor = multipliedScoring.Factor.Get(GameManager.Instance.State, booster).First();

                if (multipliedScoring.Multiplier != null)
                    text += $"\nCurrent: {1 + multipliedScoring.Multiplier * factor:0.#}";
                else if (multipliedScoring.Products != null)
                    text += $"\nCurrent: {multipliedScoring.Products * factor:0.#}";
            }
            else if (scalingAction != null)
            {
                if (scalingAction.Delay != null)
                {
                    if (!scalingAction.OneTime || !scalingAction.HasTriggered)
                        text += $"\n({scalingAction.Progress}/{scalingAction.Delay})";
                }

                if (scalingAction.MultiplierChange is { } multChange)
                    text = text.ReplaceIgnoreCase("{multChange}", $"x{multChange:0.#}");

                if (scalingAction.ProductChange is { } prodChange)
                    text = text.ReplaceIgnoreCase("{addChange}", $"{prodChange:+0.#;-#.#}");

                if (scoring?.Products != null)
                    text += $"\nCurrent: {scoring.Products:0.#}";
                else if (scoring?.Multiplier != null)
                    text += $"\nCurrent: {1 + scoring.Multiplier:0.#}";
            }

            return text;
        }

        private static string ReplaceColors(string text)
        {
            var localization = StaticResolver.Resolve<ILocalization>();
            foreach (var (tag, color) in ColorTag.All)
            {
                var colorHex = ColorUtility.ToHtmlStringRGB(color);
                text = text.ReplaceIgnoreCase($"{{{tag}}}", $"<color=#{colorHex}><b>{localization.Get(tag)}</b></color>");
            }
            return text;
        }

        private static string ReplaceSizes(string text)
        {
            text = ReplaceSize(text, BuildingSize.Small);
            text = ReplaceSize(text, BuildingSize.Medium);
            text = ReplaceSize(text, BuildingSize.Large);
            return text;
        }

        private static string ReplaceCategories(string text)
        {
            var localization = StaticResolver.Resolve<ILocalization>();
            foreach (var cat in Category.All)
                text = text.ReplaceIgnoreCase($"{{{cat.Value}}}", $"<b>{localization.Get(cat.Value)}</b>");
            return text;
        }

        private static string ReplaceSize(string text, BuildingSize size)
        {
            var localization = StaticResolver.Resolve<ILocalization>();
            var key = size.ToString();
            return text.ReplaceIgnoreCase($"{{{key}}}", $"<b>{localization.Get(key)}</b>");
        }

        private static string GetProbabilityText(ProbabilityCondition probability) =>
            $"<b>{probability.FavorableOutcome * (GameManager.Instance.State.GetRiggedCount() + 1)} in {probability.TotalOutcomes} chance</b>";
    }
}