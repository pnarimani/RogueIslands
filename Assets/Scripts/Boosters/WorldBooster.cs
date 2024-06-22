using System;
using System.Collections.Generic;
using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Descriptions;
using UnityEngine;

namespace RogueIslands.Boosters
{
    public class WorldBooster : IBooster, IEquatable<WorldBooster>
    {
        public string Name { get; set; }
        public BoosterInstanceId Id { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float Range { get; set; }
        public IDescriptionProvider Description { get; set; }
        public GameAction EventAction { get; set; }
        public IReadOnlyList<GameConditionEvaluator> EvaluationOverrides { get; set; }
        public string PrefabAddress { get; set; }

        public bool Equals(WorldBooster other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Id.Equals(other.Id) && PrefabAddress == other.PrefabAddress;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorldBooster)obj);
        }

        public override int GetHashCode() => 1;
    }
}