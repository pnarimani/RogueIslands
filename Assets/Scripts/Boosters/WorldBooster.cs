using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;
using UnityEngine;

namespace RogueIslands.Boosters
{
    public class WorldBooster : IBooster
    {
        public string Name { get; set; }
        public BoosterInstanceId Id { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float Range { get; set; }
        public IDescriptionProvider Description { get; set; }
        public GameAction EventAction { get; set; }
        public IReadOnlyList<ConditionEvaluator> EvaluationOverrides { get; set; }
        public string PrefabAddress { get; set; }
    }
}