using Android.App;

namespace MirappDictionaryGame
{
    public abstract class MyFragmentBase<T>: Fragment
    {
        public abstract T GetItem(int id);

        public abstract void SetListRecylerView();
    }
}