using RogueIslands.Boosters;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RogueIslands
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private string _seed;

        public string Seed
        {
            get => _seed;
            set => _seed = value;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            var seedRandom = new System.Random(Seed.GetHashCode());
            builder.RegisterInstance(seedRandom).As<System.Random>();
            new BoostersInstaller().Install(builder);
        }
    }
}