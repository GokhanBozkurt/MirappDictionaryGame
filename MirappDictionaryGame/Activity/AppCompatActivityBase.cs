using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace MirappDictionaryGame
{
    public abstract class AppCompatActivityBase : AppCompatActivity
    {
        public abstract void  FindViews();
        public abstract void HandleEvents();
        public abstract void LoadViews();
        public abstract void SetToolBar();
        public abstract void SetRepository();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();

            HandleEvents();

            LoadViews();

            SetToolBar();

            SetRepository();

        }

        public void ToHome()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(AppStartActivity));
            StartActivityForResult(intent, 100);
        }

        public void ToList()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(DictonaryListActivity));
            StartActivityForResult(intent, 100);
        }

        public void ToAdd()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(DictionaryAddActivity));
            StartActivityForResult(intent, 100);
        }

        public void ToSetting()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(OldSettingActivity));
            StartActivityForResult(intent, 100);
        }

        public void ToAbout()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(FragmentAbout));
            StartActivityForResult(intent, 100);
        }
    }
}