namespace MirappDictionaryGame 
{
    public class RepositoryManager
    {
        private static RepositoryManager _instance;
        private static readonly object LockingObject = new object();
        public  RepositoryGameSetting<GameSetting> GameSetting;
        public RepositoryGameLevel<GameLevel> GameLevel;
        public RepositoryGameScore<GameScore> GameScore;

        public static RepositoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockingObject)
                    {
                        _instance = new RepositoryManager();
                    }
                }
                return _instance;
            }
        }
        private RepositoryManager()
        {
            GameSetting = new RepositoryGameSetting<GameSetting>();
            GameSetting.Open();
            GameSetting.CreateTable();

            GameLevel = new RepositoryGameLevel<GameLevel>();
            GameLevel.Open();
            GameLevel.CreateTable();

            GameScore = new RepositoryGameScore<GameScore>();
            GameScore.Open();
            GameScore.CreateTable();
        }

    }
}