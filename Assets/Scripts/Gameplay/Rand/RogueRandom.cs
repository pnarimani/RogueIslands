using System.Collections.Generic;
using Unity.Mathematics;

namespace RogueIslands.Gameplay.Rand
{
    public class RogueRandom
    {
        private readonly Random[] _rand = new Random[200];
        private Random _randomGenerator;

        public uint InitialSeed { get; }

        public RogueRandom(uint initialSeed)
        {
            InitialSeed = initialSeed;
            _randomGenerator = new Random(initialSeed);
        }

        public RogueRandom(uint initialSeed, List<uint> state)
        {
            _randomGenerator = new Random(initialSeed);
            for (var i = 0; i < state.Count; i++) 
                _rand[i] =new Random(state[i]);
        }
        
        public List<uint> GetState()
        {
            var state = new List<uint>();
            foreach (var random in _rand)
            {
                state.Add(random.state);
            }

            return state;
        }

        public RandomForAct ForAct(int act) => new(this, act);

        internal ref Random GetActRandom(int act)
        {
            if (_rand[act].state == default)
            {
                _rand[act] = new Random(_randomGenerator.NextUInt());
            }

            return ref _rand[act];
        }
    }
}