using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RogueIslands.Autofac;
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

        private static readonly string MultColor = ColorUtility.ToHtmlStringRGB(new Color(1, 1, 1));

        private static readonly string MultHighlightColor =
            ColorUtility.ToHtmlStringRGBA(new Color(0f, 0.56f, 0.57f, 0));

        private static readonly string AddColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.56f, 0.57f));

        private static readonly string ProbabilityColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.78f, 0.06f));
        private static readonly string ProgressColor = ColorUtility.ToHtmlStringRGB(new Color(0.29f, 0.31f, 0.3f));
        private static readonly string MoneyColor = ColorUtility.ToHtmlStringRGB(new Color(0.76f, 0.55f, 0f));

        private static readonly string CategoryColor = ColorUtility.ToHtmlStringRGB(new Color(0.31f, 0f, 1f));
        private static readonly string SizeColor = CategoryColor;

        private static readonly string TriggerColor = ColorUtility.ToHtmlStringRGB(new Color(0f, 0.22f, 0.39f));
        private static readonly string BonusColor = TriggerColor;

        private const string GameEventOpening = "<e>";
        private const string GameEventClosing = "</e>";
        
        private const string AdditionOpening = "<a>";
        private const string AdditionClosing = "</a>";
        
        private const string MultiplierOpening = "<m>";
        private const string MultiplierClosing = "</m>";
        
        private const string ProbabilityOpening = "<p>";
        private const string ProbabilityClosing = "</p>";
        
        private const string MoneyOpening = "<d>";
        private const string MoneyClosing = "</d>";
        
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
            
            text = text.Replace(GameEventOpening, "<i>");
            text = text.Replace(GameEventClosing, "</i>");
            
            text = text.Replace(AdditionOpening, $"<color=#{AddColor}>");
            text = text.Replace(AdditionClosing, "</color>");
            
            text = text.Replace(MultiplierOpening, $"<mark=#{MultHighlightColor}><color=#{MultColor}>");
            text = text.Replace(MultiplierClosing, "</color></mark>");
            
            text = text.Replace(ProbabilityOpening, $"<color=#{ProbabilityColor}>");
            text = text.Replace(ProbabilityClosing, "</color>");
            
            text = text.Replace(MoneyOpening, $"<color=#{MoneyColor}>");
            text = text.Replace(MoneyClosing, "</color>");

            text += "</i></color>";

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
            var modifyBonus = booster.GetEventAction<BonusAction>();
            var multipliedScoring = booster.GetEventAction<MultipliedScoringAction>();
            var scalingAction = booster.GetEventAction<ScoreScalingAction>();

            if (money != null)
            {
                var moneyText = money.Change > 0
                    ? $"{money.Change:$0.##;-$#.#}".WrapWithColor(MoneyColor)
                    : $"costs {Math.Abs(money.Change):$0.##}".WrapWithColor(MoneyColor);
                text = text.Replace("{money}", moneyText);
            }

            if (scoring?.Multiplier is { } mult)
            {
                var xmult = $"X{mult:0.##}".WrapWithColor(MultColor).WrapWithHighlight(MultHighlightColor);
                text = text.ReplaceIgnoreCase("{mult}", xmult);
                text = text.ReplaceIgnoreCase("{score}", xmult);
            }

            if (scoring?.Addition is { } products)
            {
                var add = $"{products:+0.##;-#.#}".WrapWithColor(AddColor);
                text = text.ReplaceIgnoreCase("{add}", add);
                text = text.ReplaceIgnoreCase("{score}", add);
            }

            if (modifyBonus != null)
            {
                if (modifyBonus.Multiplier is { } bmult)
                {
                    var multText = $"X{bmult:0.##}".WrapWithColor(MultColor).WrapWithHighlight(MultHighlightColor);
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }

                if (modifyBonus.Addition is { } badd)
                {
                    var prodText = $"+{badd:0.##}".WrapWithColor(AddColor);
                    text = text.ReplaceIgnoreCase("{add}", prodText);
                    text = text.ReplaceIgnoreCase("{score}", prodText);
                }

                if (modifyBonus.CategoryMultiplier is { } catMult)
                {
                    var multText = $"X{catMult:0.##}".WrapWithColor(MultColor).WrapWithHighlight(MultHighlightColor);
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }

                if (modifyBonus.ColorMultiplier is { } colMult)
                {
                    var multText = $"X{colMult:0.##}".WrapWithColor(MultColor).WrapWithHighlight(MultHighlightColor);
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }

                if (modifyBonus.SizeMultiplier is { } sizeMult)
                {
                    var multText = $"X{sizeMult:0.##}".WrapWithColor(MultColor).WrapWithHighlight(MultHighlightColor);
                    text = text.ReplaceIgnoreCase("{mult}", multText);
                    text = text.ReplaceIgnoreCase("{score}", multText);
                }
            }

            if (multipliedScoring != null)
            {
                var factor = multipliedScoring.Factor.Get(GameManager.Instance.State, booster).First();


                if (multipliedScoring?.Addition != null)
                {
                    var finalAdd = multipliedScoring.Addition * factor;
                    text +=
                        ("\n(Currently " + $"{finalAdd:0.##}".WrapWithColor(AddColor) + " Score)").WrapWithColor(
                            ProgressColor);
                }
                else if (multipliedScoring?.Multiplier != null)
                {
                    var finalMult = 1 + multipliedScoring.Multiplier * factor;
                    text +=
                        ("\n(Currently " + $"X{finalMult:0.##}".WrapWithColor(MultColor)
                             .WrapWithHighlight(MultHighlightColor) +
                         " Mult)").WrapWithColor(
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
                        ? $"X{multChange:0.##}".WrapWithColor(MultColor).WrapWithHighlight(MultHighlightColor)
                        : "loses " + $"X{Math.Abs(multChange):0.##}".WrapWithColor(MultColor)
                            .WrapWithHighlight(MultHighlightColor);

                    text = text.ReplaceIgnoreCase("{multChange}", changeText);
                }

                if (scalingAction.AdditionChange is { } prodChange)
                {
                    string changeText;
                    if (prodChange > 0)
                        changeText = $"{prodChange:0.##}".WrapWithColor(AddColor);
                    else
                        changeText = "loses " + $"{Math.Abs(prodChange):0.##}".WrapWithColor(AddColor);
                    text = text.ReplaceIgnoreCase("{addChange}", changeText);
                }

                if (scoring?.Addition != null)
                    text += ("\n(Currently " + $"{scoring.Addition:0.##}".WrapWithColor(AddColor) + " Score)")
                        .WrapWithColor(ProgressColor);
                else if (scoring?.Multiplier != null)
                    text += ("\n(Currently " +
                             $"X{scoring.Multiplier:0.##}".WrapWithColor(MultColor)
                                 .WrapWithHighlight(MultHighlightColor) +
                             " Mult)")
                        .WrapWithColor(ProgressColor);
            }

            var copy = booster.GetEventAction<CopyBoosterAction>();
            if (copy != null)
            {
                if (copy.Cloned != null)
                    text += "\n(Compatible)".WrapWithColor(TextColors[ColorTag.Green]);
                else
                    text += "\n(Incompatible)".WrapWithColor(TextColors[ColorTag.Red]);
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
                    $"<color=#{colorHex}>{localization.Get(tag)}</color>");
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
                    $"{localization.Get(cat.Value)}".WrapWithColor(CategoryColor));
            return text;
        }

        private static string ReplaceSize(string text, BuildingSize size)
        {
            var localization = StaticResolver.Resolve<ILocalization>();
            var key = size.ToString();
            return text.ReplaceIgnoreCase($"{{{key}}}", $"{localization.Get(key)}".WrapWithColor(SizeColor));
        }

        private static string GetProbabilityText(ProbabilityCondition probability) =>
            $"{probability.FavorableOutcome * (GameManager.Instance.State.GetRiggedCount() + 1)} in {probability.TotalOutcomes}"
                .WrapWithColor(ProbabilityColor) + " chance";

        private static string WrapInstancesWithColor(string input, string word, string color)
            => Regex.Replace(input, $"\b{word}\b", word.WrapWithColor(color));
    }
}