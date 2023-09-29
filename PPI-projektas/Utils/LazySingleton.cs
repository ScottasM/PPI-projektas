namespace PPI_projektas.Utils
{
    public class LazySingleton<T> where T : class, new()
    {
        private LazySingleton() { }

        public static bool InstanceExists { get { return _instance != null; } }

        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

        public static T Instance => _instance.Value;

    }
}
