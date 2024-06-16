namespace RogueIslands.View
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        public Singleton()
        {
            Instance = (T)this;
        }
    }
}