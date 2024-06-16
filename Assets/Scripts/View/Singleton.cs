namespace RogueIslands.View
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected Singleton()
        {
            Instance = (T)this;
        }
    }
}