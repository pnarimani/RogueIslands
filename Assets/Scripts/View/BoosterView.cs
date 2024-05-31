using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.View
{
    public class BoosterView : MonoBehaviour
    {
        public Booster Data { get; private set; }

        public void Show(Booster booster)
        {
            Data = booster;
        }
    }
}