using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RogueIslands.DependencyInjection;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Localization;
using UnityEngine;
using UnityEngine.Profiling;

namespace RogueIslands.Gameplay.View.Descriptions
{
    public class DescriptionTextProvider
    {
        private static readonly Dictionary<ColorTag, string> TextColors = new()
        {
            { ColorTag.Blue, ColorUtility.ToHtmlStringRGB(new Color(0f, 0.22f, 0.67f)) },
            { ColorTag.Green, ColorUtility.ToHtmlStringRGB(new Color(0f, 0.45f, 0f)) },
            { ColorTag.Purple, ColorUtility.ToHtmlStringRGB(new Color(0.29f, 0f, 0.49f)) },
            { ColorTag.Red, ColorUtility.ToHtmlStringRGB(new Color(1, 0, 0)) },
        };

        private static readonly string MultColor = ColorUtility.ToHtmlStringRGB(new Color(1, 0, 0));
        private static readonly string AddColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.47f, 0.36f));
        private static readonly string ProbabilityColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.78f, 0.06f));
        private static readonly string ProgressColor = ColorUtility.ToHtmlStringRGB(new Color(0.29f, 0.31f, 0.3f));
        private static readonly string MoneyColor = ColorUtility.ToHtmlStringRGB(new Color(0.76f, 0.55f, 0f));

        private static readonly string CategoryColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.22f, 0.05f));
        private static readonly string SizeColor = CategoryColor;

        private static readonly string TriggerColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.22f, 0.39f));
        private static readonly string BonusColor = TriggerColor;

        public static string Get(IDescribableItem item)
        {
            if (item is IBooster booster) return GetBoosterDescription(booster);
            if (item is Building building) return GetBuildingDescription(building);
            return item.Description.Text;
        }

        private static string GetBuildingDescription(Building building)
        {
            using var profiler = new ProfilerBlock("GetBuildingDescription");

            var text = building.Description.Text;
            text = ReplaceCategories(text);
            text = ReplaceColors(text);
            text = ReplaceSizes(text);
            return text;
        }

        private static string GetBoosterDescription(IBooster booster)
        {
            using var profiler = new ProfilerBlock("GetBoosterDescription");

            var text = booster.Description.Text;

            using var conditions = booster.EventAction.GetAllConditions();
            var probability = conditions.OfType<ProbabilityCondition>().FirstOrDefault();

            if (probability != null)
                text = text.ReplaceIgnoreCase("{probability}", GetProbabilityText(probability));

            text = ReplaceCategories(text);
            text = ReplaceColors(text);
            text = ReplaceSizes(text);

            text = WrapInstancesWithColor(text, "bonus", BonusColor);
            text = WrapInstancesWithColor(text, "trigger", TriggerColor);
            text = WrapInstancesWithColor(text, "triggered", TriggerColor);
            text = WrapInstancesWithColor(text, "triggers", TriggerColor);

            var scoring = booster.GetEventAction<ScoringAction>();
            var money = booster.GetEventAction<ChangeMoneyAction>();
            var modifyBonus = booster.GetEventAction<ModifyBonusAction>();
            var multipliedScoring = booster.GetEventAction<MultipliedScoringAction>();
            var scalingAction = booster.GetEventAction<BoosterScalingAction>();

            if (money != null)
            {
                var moneyText = money.Change > 0 
                    ? $"{money.Change:$0.#;-$#.#}".WrapWithColor(MoneyColor)
                    : $"costs {Math.Abs(money.Change):$0.#}".WrapWithColor(MoneyColor);
                text = text.Replace("{money}", moneyText);
            }

            if (scoring?.Multiplier is { } mult)
            {
                var xmult = $"x{mult:0.#}".WrapWithColor(MultColor) + " score";
                text = text.ReplaceIgnoreCase("{mult}", xmult);
                text = text.ReplaceIgnoreCase("{score}", xmult);
            }

            if (scoring?.Products is { } products)
            {
                var add = $"{products:+0.#;-#.#}".WrapWithColor(AddColor) + " score";
                text = text.ReplaceIgnoreCase("{add}", add);
                text = text.ReplaceIgnoreCase("{score}", add);
            }

            if (modifyBonus != null)
            {
                if (modifyBonus.Multiplier is { } bmult)
                {
                    var multText = $"x{bmult:0.#}".WrapWithColor(MultColor) + " bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }

                if (modifyBonus.Add is { } badd)
                {
                    var prodText = $"+{badd:0.#}".WrapWithColor(AddColor) + " bonus";
                    text = text.ReplaceIgnoreCase("{add}", prodText);
                    text = text.ReplaceIgnoreCase("{score}", prodText);
                }

                if (modifyBonus.CategoryMultiplier is { } catMult)
                {
                    var multText = $"x{catMult:0.#}".WrapWithColor(MultColor) + " bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }

                if (modifyBonus.ColorMultiplier is { } colMult)
                {
                    var multText = $"x{colMult:0.#}".WrapWithColor(MultColor) + " bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }

                if (modifyBonus.SizeMultiplier is { } sizeMult)
                {
                    var multText = $"x{sizeMult:0.#}".WrapWithColor(MultColor) + " bonus";
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }
            }

            if (multipliedScoring != null)
            {
                var factor = multipliedScoring.Factor.Get(GameManager.Instance.State, booster).First();


                if (multipliedScoring?.Products != null)
                {
                    var finalAdd = multipliedScoring.Products * factor;
                    text +=
                        ("\n(Currently " + $"{finalAdd:0.#}".WrapWithColor(AddColor) + " Score)").WrapWithColor(
                            ProgressColor);
                }
                else if (multipliedScoring?.Multiplier != null)
                {
                    var finalMult = 1 + multipliedScoring.Multiplier * factor;
                    text +=
                        ("\n(Currently " + $"{finalMult:0.#}x".WrapWithColor(MultColor) + " Mult)").WrapWithColor(
                            ProgressColor);
                }
            }
            else if (scalingAction != null)
            {
                if (scalingAction.Delay != null)
                    if (!scalingAction.OneTime || !scalingAction.HasTriggered)
                        text += $"\n({scalingAction.Progress}/{scalingAction.Delay})".WrapWithColor(ProgressColor);

                if (scalingAction.MultiplierChange is { } multChange)
                {
                    var changeText = multChange > 0
                        ? $"x{multChange:0.#}".WrapWithColor(MultColor)
                        : "loses " + $"x{Math.Abs(multChange):0.#}".WrapWithColor(MultColor);

                    text = text.ReplaceIgnoreCase("{multChange}", changeText);
                }

                if (scalingAction.ProductChange is { } prodChange)
                {
                    string changeText;
                    if (prodChange > 0)
                        changeText = $"{prodChange:0.#}".WrapWithColor(AddColor);
                    else
                        changeText = "loses " + $"{Math.Abs(prodChange):0.#}".WrapWithColor(AddColor);
                    text = text.ReplaceIgnoreCase("{addChange}", changeText);
                }

                if (scoring?.Products != null)
                    text += ("\n(Currently " + $"{scoring.Products:0.#}".WrapWithColor(AddColor) + " Score)")
                        .WrapWithColor(ProgressColor);
                else if (scoring?.Multiplier != null)
                    text += ("\n(Currently " + $"{scoring.Multiplier:0.#}x".WrapWithColor(MultColor) + " Mult)")
                        .WrapWithColor(ProgressColor);
            }

            return text;
        }

        private static string ReplaceColors(string text)
        {
            var localization = StaticResolver.Resolve<ILocalization>();
            foreach (var colorTag in ColorTag.All)
            {
                var colorHex = TextColors[colorTag];
                var tag = colorTag.Tag;
                text = text.ReplaceIgnoreCase($"{{{tag}}}",
                    $"<color=#{colorHex}><b>{localization.Get(tag)}</b></color>");
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
                text = text.ReplaceIgnoreCase($"{{{cat.Value}}}",
                    $"<b>{localization.Get(cat.Value)}</b>".WrapWithColor(CategoryColor));
            return text;
        }

        private static string ReplaceSize(string text, BuildingSize size)
        {
            var localization = StaticResolver.Resolve<ILocalization>();
            var key = size.ToString();
            return text.ReplaceIgnoreCase($"{{{key}}}", $"<b>{localization.Get(key)}</b>".WrapWithColor(SizeColor));
        }

        private static string GetProbabilityText(ProbabilityCondition probability) =>
            $"<b>{probability.FavorableOutcome * (GameManager.Instance.State.GetRiggedCount() + 1)} in {probability.TotalOutcomes}"
                .WrapWithColor(ProbabilityColor) + " chance</b>";

        private static string WrapInstancesWithColor(string input, string word, string color)
            => Regex.Replace(input, $"\b{word}\b", word.WrapWithColor(color));
    }
}