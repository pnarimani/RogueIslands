using System.Collections.Generic;
using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class ColorCheckCondition : IGameConditionWithSource<Building>
    {
        private readonly HashSet<ColorTag> _existingColors = new();

        public ISource<Building> Source { get; set; }
        public IReadOnlyList<ColorTag> ForcedColors { get; set; }
        public IReadOnlyList<ColorTag> BannedColors { get; set; }

        public ColorCheckCondition()
        {
        }

        public ColorCheckCondition(ColorTag forceColor)
        {
            ForcedColors = new List<ColorTag> { forceColor };
        }
        
        public ColorCheckCondition(ColorTag forceColor, ISource<Building> source)
        {
            ForcedColors = new List<ColorTag> { forceColor };
            Source = source;
        }

        public bool Evaluate(IBooster booster)
        {
            Source ??= new BuildingFromCurrentEvent();
            
            _existingColors.Clear();
            foreach (var tag in Source.Get(booster).Select(b => b.Color))
                _existingColors.Add(tag);

            var state = StaticResolver.Resolve<GameState>();
            if (state.HasBadEyesight())
            {
                if (_existingColors.Contains(ColorTag.Red))
                    _existingColors.Add(ColorTag.Blue);
                if (_existingColors.Contains(ColorTag.Blue))
                    _existingColors.Add(ColorTag.Red);
                if (_existingColors.Contains(ColorTag.Green))
                    _existingColors.Add(ColorTag.Purple);
                if (_existingColors.Contains(ColorTag.Purple))
                    _existingColors.Add(ColorTag.Green);
            }

            if (ForcedColors != null)
            {
                foreach (var force in ForcedColors)
                {
                    if (!_existingColors.Contains(force))
                        return false;
                }
            }

            if (BannedColors != null)
            {
                foreach (var color in _existingColors)
                {
                    if (BannedColors.Contains(color))
                        return false;
                }
            }

            return true;
        }
    }
}