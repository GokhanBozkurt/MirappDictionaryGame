using Android.App;
using Android.Media;

namespace MirappDictionaryGame 
{
    class ManagerSound
    {
        private static ManagerSound _instance;
        private static readonly object LockObject=new object();
        public static ManagerSound Instance
        {
            get
            {
                if (_instance==null)
                {
                    lock (LockObject)
                    {
                        _instance=new ManagerSound();
                    }
                }
                return _instance; 
            }
        }

        public void Sucess(Activity activity)
        {
            Play(activity, Resource.Raw.sucess);
        }

        public void Wrong(Activity activity)
        {
            if (!ManagerGameSetting.Instance.Sound)
            {
                return;
            }
            Play(activity, Resource.Raw.wrong);
        }

        public void GameOver(Activity activity)
        {
            if (!ManagerGameSetting.Instance.Sound)
            {
                return;
            }
            Play(activity, Resource.Raw.gameover);
        }

        public void Start(Activity activity)
        {
            if (!ManagerGameSetting.Instance.Sound)
            {
                return;
            }
            Play(activity, Resource.Raw.newsbringer);
        }

        private void Play(Activity activity, int resid)
        {
            if (!ManagerGameSetting.Instance.Sound)
            {
                return;
            }
            var  player=MediaPlayer.Create(activity, resid);
            player.Start();
        }

        public void NewLevel(Activity activity)
        {
            if (!ManagerGameSetting.Instance.Sound)
            {
                return;
            }
            Play(activity, Resource.Raw.teleporter);
        }

    }
}