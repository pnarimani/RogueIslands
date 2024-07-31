﻿namespace RogueIslands.Gameplay
{
    public interface IPurchasableItem
    {
        int BuyPrice { get; set; }
        int SellPrice { get; set; }

        int GetIdentityHash();
    }
}