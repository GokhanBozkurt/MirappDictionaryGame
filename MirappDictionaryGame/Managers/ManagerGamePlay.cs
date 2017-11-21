using System;

namespace MirappDictionaryGame
{
    public static class ManagerGamePlay
    {
        private static int _easyTryCount = 7;
        private static int _mediumTryCount = 6;
        private static int _hardTryCount = 5;

        private static long _easyTimerMillisInFuture = 20000;
        private static long _mediumTimerMillisInFuture = 15000;
        private static long _hardTimerMillisInFuture = 10000;

        public static int EasyAnimation = 6;
        public static int MediumAnimation = 5;
        public static int HardAnimation = 4;
        public static int LevelFactor = 5;


        public static GameProperty Get(GamePlayLevels gameLevel)
        {
            var gameProperty = new GameProperty();
            switch (gameLevel)
            {
                case GamePlayLevels.Easy:
                    gameProperty.MillisInFuture = _easyTimerMillisInFuture;
                    gameProperty.TryCount = _easyTryCount;
                    break;
                case GamePlayLevels.Medium:
                    gameProperty.MillisInFuture = _mediumTimerMillisInFuture;
                    gameProperty.TryCount = _mediumTryCount;
                    break;
                case GamePlayLevels.Hard:
                    gameProperty.MillisInFuture = _hardTimerMillisInFuture;
                    gameProperty.TryCount = _hardTryCount;
                    break;
            }
            return gameProperty;
        }


        public static GamePlayLevelContainer GetGameLevelContainer(string gameLevel)
        {
            switch (gameLevel)
            {
                case "Easy":
                    return new GamePlayLevelContainerEasy();
                case "Medium":
                    return new GamePlayLevelContainerMedium();
                case "Hard":
                    return new GamePlayLevelContainerHard();
                default:
                    return new GamePlayLevelContainerEasy();
            }

        }

        public static GameLevel GetCurrentLevel()
        {
            var currentGameLevel=ManagerRepository.Instance.GameLevel.GetRecords();
            if (currentGameLevel.Count>1)
            {
                ManagerRepository.Instance.GameLevel.DeleteAll();
            }
            if (currentGameLevel.Count== 0)
            {
                var gameLevel = new GameLevel { Id = 1, LevelNumber = 1 };
                ManagerRepository.Instance.GameLevel.Insert(gameLevel);
                return gameLevel;
            }
            return currentGameLevel[0];
        }

        internal static void UpdateLevel(GameLevel currentGameLevel)
        {
            try
            {
                var newLevel = currentGameLevel;
                newLevel.LevelNumber = currentGameLevel.LevelNumber + 1;
                if (ManagerRepository.Instance.GameLevel.Update(newLevel))
                    return;
                else
                {
                    ManagerRepository.Instance.GameLevel.Update(currentGameLevel);
                }
            }
            catch (Exception)
            {
                ManagerRepository.Instance.GameLevel.Update(currentGameLevel);
            }
        }
    }
}