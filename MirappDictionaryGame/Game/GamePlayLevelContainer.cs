

using Android.App;
using Android.Views.Animations;

namespace MirappDictionaryGame
{
    public abstract class GamePlayLevelContainer
    {
        public int AnimationTime;
        public GamePlayLevels GameLevel { set; get; }
        public Animation GetAnimation(Activity myActivity, long millisUntilFinished)
        {
            if (millisUntilFinished / 1000 <= AnimationTime)
            {
                return GetLoadAnimation(myActivity);

            }
            return null;
        }

        private Animation GetLoadAnimation(Activity myActivity)
        {
            return AnimationUtils.LoadAnimation(myActivity.ApplicationContext, Resource.Animation.game_timer_fade_in);
        }
    }
}