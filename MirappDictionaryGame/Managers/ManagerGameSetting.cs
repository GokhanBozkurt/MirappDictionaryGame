using System.Collections.Generic;
using System.Linq;

namespace MirappDictionaryGame
{
    class ManagerGameSetting
    {
        private static ManagerGameSetting _instance;
        private static readonly object LockObject = new object();

        public static ManagerGameSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        _instance = new ManagerGameSetting();
                    }
                }

                return _instance;

            }
        }

        private List<GameSetting> GameSettings
        {
            get
            {
                return ManagerRepository.Instance.GameSetting.GetRecords();
            }
        }

        public bool SettingAvaliable => GameSettings.Count > 0;

        public bool Sound
        {
            get
            {
                try
                {
                    if (SettingAvaliable)
                    {
                        return GameSettings.First().SoundActive;
                    }
                }
                catch (System.Exception)
                {

                    return true;
                }

                return true;

            }
        }
    }
}