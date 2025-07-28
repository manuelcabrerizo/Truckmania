public class Singleton
{
    private static GameEventManager instance = null;
    public static GameEventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEventManager();
            }
            return instance;
        }
    }
}
