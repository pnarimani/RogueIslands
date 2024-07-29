using System.Collections.Generic;
using Unity.Mathematics;

namespace RogueIslands.Gameplay.Rand
{
    public class RogueRandom
    {
        private readonly Random[] _randPerAct = new Random[10];
        private Random _randomGenerator;

        public RogueRandom(uint initialSeed) => _randomGenerator = new Random(initialSeed);

        public RogueRandom(uint initialSeed, List<uint> state)
        {
            _randomGenerator = new Random(initialSeed);
            for (var i = 0; i < state.Count; i++)
                _randPerAct[i] = new Random(state[i]);
        }

        public List<uint> GetState()
        {
            var state = new List<uint>();
            foreach (var random in _randPerAct)
            {
                if (random.state != 0)
                    state.Add(random.state);
            }

            return state;
        }

        public RandomForAct ForAct(int act) => new(this, act);

        internal ref Random GetActRandom(int act)
        {
            if (_randPerAct[act].state == default)
            {
                _randPerAct[act] = new Random(_randomGenerator.NextUInt());
            }

            return ref _randPerAct[act];
        }
    }
}