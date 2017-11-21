using Android.Database.Sqlite;
using Android.OS;
using Java.IO;

namespace MirappDictionaryGame 
{
    public class ManagerRepository
    {
        private static ManagerRepository _instance;
        private static readonly object LockingObject = new object();
        public RepositoryGameSetting<GameSetting> GameSetting;
        public RepositoryGameLevel<GameLevel> GameLevel;
        public RepositoryGameScore<GameScore> GameScore;
        public RepositoryFavoriteWord<FavoriteWord> FavoriteWord;
        public RepositoryMyDictonaryWord<MyDictonaryWord> MyDictonaryWord;

        public static ManagerRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockingObject)
                    {
                        _instance = new ManagerRepository();
                    }
                }
                return _instance;
            }
        }
        private ManagerRepository()
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

            FavoriteWord = new RepositoryFavoriteWord<FavoriteWord>();
            FavoriteWord.Open();
            FavoriteWord.CreateTable();

            MyDictonaryWord = new RepositoryMyDictonaryWord<MyDictonaryWord>();
            MyDictonaryWord.Open();
            MyDictonaryWord.CreateTable();
        }

        public string Path
        {
            get
            {
                var folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var toDatabase = System.IO.Path.Combine(folderPath, "MirappDictonaryGame.db");
                return toDatabase;
            }
        }

        
    }
}