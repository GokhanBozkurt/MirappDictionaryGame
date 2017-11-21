using System;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using ActionBar = Android.Support.V7.App.ActionBar;
using actionBarActivity= Android.Support.V7.App.ActionBarActivity;
using com.refractored.fab;


namespace MirappDictionaryGame
{
    [Activity(
        Label = "Mirapp Dictionary Game", 
        ScreenOrientation = ScreenOrientation.Portrait, 
        Theme = "@style/AppTheme", 
        MainLauncher = false, 
        Icon = "@drawable/logo")]
#pragma warning disable CS0618 // 'ActionBarActivity' is obsolete: 'This class is obsoleted in this android platform'
    public class NewMainActivity : actionBarActivity, ActionBar.ITabListener
#pragma warning restore CS0618 // 'ActionBarActivity' is obsolete: 'This class is obsoleted in this android platform'
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            InitActionBar();
        }

        private void InitActionBar()
        {
            if (SupportActionBar == null)
                return;

            var actionBar = SupportActionBar;
            actionBar.NavigationMode = (int) ActionBarNavigationMode.Tabs;

            AddTab(actionBar, "Game");
           // AddTab(actionBar, "List");
            AddTab(actionBar, "About");
            //AddTab(actionBar, "Setting");
        }

        private void AddTab(ActionBar actionBar,String tabText)
        {
            var tab = actionBar.NewTab();
            tab.SetTabListener(this);
            tab.SetText(tabText);
            actionBar.AddTab(tab);
        }

        public void OnTabReselected(ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
        {
        }

        public void OnTabSelected(ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
        {
            //AddTab("Game", 0, new FragmentGameStart());
            //AddTab("Word", 0, new DictonaryFragment());
            //AddTab("List", 0, new FragmentList());
            //AddTab("About", 0, new FragmentAbout());
            //AddTab("Setting", 0, new SettingFragment());

            switch (tab.Text)
#pragma warning disable CS1522 // Empty switch block
            {
#pragma warning restore CS1522 // Empty switch block
                //case "List":
                //    ft.Replace(Android.Resource.Id.Content, new FragmentList());
                //    break;
                //case "Game":
                //    ft.Replace(Android.Resource.Id.Content, new FragmentGameStart());
                //    break;
                //case "About":
                //    ft.Replace(Android.Resource.Id.Content, new FragmentAbout());
                //    break;
                //case "Setting":
                //    ft.Replace(Android.Resource.Id.Content, new SettingFragment());
                //    break;
            }
        }

        public void OnTabUnselected(ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
        {

        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Drawable.Mainnn, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    if (item.ItemId == Resource.Id.about)
        //    {
        //        var text = (TextView) LayoutInflater.Inflate(Resource.Layout.about_view, null);
        //        text.TextFormatted = (Html.FromHtml(GetString(Resource.String.about_body)));
        //        new AlertDialog.Builder(this)
        //            .SetTitle(Resource.String.about)
        //            .SetView(text)
        //            .SetInverseBackgroundForced(true)
        //            .SetPositiveButton(Android.Resource.String.Ok, (sender, args) =>
        //            {
        //                ((IDialogInterface) sender).Dismiss();
        //            }).Create().Show();
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}
    }

    public class ListViewFragment : Android.Support.V4.App.Fragment, IScrollDirectorListener, AbsListView.IOnScrollListener
    {
        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    var root = inflater.Inflate(Resource.Layout.fragment_listview, container, false);

        //    var list = root.FindViewById<ListView>(Android.Resource.Id.List);
        //    var adapter = new ListViewAdapter(Activity, Resources.GetStringArray(Resource.Array.countries));
        //    list.Adapter = adapter;

        //    var fab = root.FindViewById<FloatingActionButton>(Resource.Id.fab);
        //    fab.AttachToListView(list, this, this);
        //    fab.Click += (sender, args) =>
        //    {
        //        Toast.MakeText(Activity, "FAB Clicked!", ToastLength.Short).Show();
        //    };
        //    return root;
        //}

        public void OnScrollDown()
        {
            Console.WriteLine("ListViewFragment: OnScrollDown");
        }

        public void OnScrollUp()
        {
            Console.WriteLine("ListViewFragment: OnScrollUp");
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            Console.WriteLine("ListViewFragment: OnScroll");
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {
            Console.WriteLine("ListViewFragment: OnScrollChanged");
        }
    }
}
