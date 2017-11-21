using Android.Content;
using Android.Views.Animations;

namespace MirappDictionaryGame
{
    internal class ManagerAnimation
    {
        public static Animation Rotate360Animation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.Rotate360);
        }

        public static Animation Scale1To0Animation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.Scale1to0);
        }
    }
}