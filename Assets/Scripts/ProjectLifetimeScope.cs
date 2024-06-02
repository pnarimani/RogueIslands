using RogueIslands.Boosters;
using VContainer;
using VContainer.Unity;

namespace RogueIslands
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            new BoostersInstaller().Install(builder);
        }
    }
}