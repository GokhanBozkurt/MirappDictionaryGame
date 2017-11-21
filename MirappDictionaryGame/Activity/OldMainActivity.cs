using Android.App;
using Android.OS;
using Android.Content.PM;

namespace MirappDictionaryGame
{
    [Activity(
    Label = "Mirapp", 
    Theme = "@style/AppTheme", 
    ScreenOrientation = ScreenOrientation.Portrait, 
    MainLauncher = false, 
    Icon = "@drawable/icon")
    ]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            //AddTab("Game", 0, new FragmentGameStart());
            //AddTab("Word", 0, new DictonaryFragment());
            //AddTab("List", 0, new FragmentList());
            //AddTab("About", 0, new FragmentAbout());
            //AddTab("Setting", 0, new SettingFragment());
        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(tabText);
            //tab.SetIcon(iconResourceId);

            // must set event handler before adding tab
            tab.TabSelected += (sender, e) =>
            {
                var fragment = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += (sender, e) => e.FragmentTransaction.Remove(view);

            ActionBar.AddTab(tab);
        }


    }

}

