﻿using RogueIslands.Boosters.Descriptions;

namespace RogueIslands
{
    public interface IDescribableItem
    {
        IDescriptionProvider Description { get; set; }
    }
}