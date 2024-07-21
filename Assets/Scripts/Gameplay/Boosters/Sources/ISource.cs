﻿using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public interface ISource<out T>
    {
        IEnumerable<T> Get(GameState state, IBooster booster);
    }
}