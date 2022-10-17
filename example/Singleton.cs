public class Singleton<T> where T : class, new()
{
    protected Singleton() { }
    protected static readonly Lazy<T> instance = new Lazy<T>(() => new T(), false);
    public static T Instance { get { return instance.Value; } }
}
