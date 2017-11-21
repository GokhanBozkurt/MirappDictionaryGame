using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.App;
using Android.Content.PM;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MirappDictionaryGame
{
    [Activity(
                ScreenOrientation = ScreenOrientation.Portrait, 
                Label = "x",
                MainLauncher = false, 
                Icon = "@drawable/icon", 
                Theme = "@style/MyTheme")]
    public class AppStartActivity : AppCompatActivityBase
    {
        DrawerLayout _drawerLayout;
        private Toolbar _toolbar;
        private bool goToList;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            SetContentView(Resource.Layout.AppStart);

            base.OnCreate(savedInstanceState);

            ReadIntents();

            SetDrawer();

        }

        private void ReadIntents()
        {
            try
            {
                if (Intent.Extras.GetBoolean("goToList"))
                {
                    goToList = true;
                }
            }
            catch (System.Exception)
            {

            }
        }

        protected override void OnPause()
        {
            //Stop Tracking usage in this activity
            //Tracking.StopUsage(this);

            base.OnPause();
        }


        public override void FindViews()
        {
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _toolbar = FindViewById<Toolbar>(Resource.Id.AppstartAppBar);
        }

        public override void HandleEvents()
        {
           
        }

        public override void LoadViews()
        {
            
        }

        public override void SetToolBar()
        {
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetTitle(Resource.String.ApplicationName);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
        }

        public override void SetRepository()
        {
             
        }

        private void SetDrawer()
        {
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            var drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, _toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
#pragma warning disable CS0618 // 'DrawerLayout.SetDrawerListener(DrawerLayout.IDrawerListener)' is obsolete: 'deprecated'
            _drawerLayout.SetDrawerListener(drawerToggle);
#pragma warning restore CS0618 // 'DrawerLayout.SetDrawerListener(DrawerLayout.IDrawerListener)' is obsolete: 'deprecated'
            drawerToggle.SyncState();

            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            goToList = true;
            if (goToList)
            {
                ft.Add(Resource.Id.HomeFrameLayout, new FragmentList2());
            }
            else
            {
                ft.Add(Resource.Id.HomeFrameLayout, new FragmentGameStart());
            }
            ft.Commit();
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.ApplicationName);
            base.OnResume();
        }

        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            var myFragmentContainer = ManagerFragment.GetNavigationFragment(e.MenuItem.ItemId);
            ft.Replace(Resource.Id.HomeFrameLayout, myFragmentContainer.MyFragment);
            SupportActionBar.SetTitle(myFragmentContainer.Name);
            ft.Commit();
            _drawerLayout.CloseDrawers();
        }

        

        ////add custom icon to tolbar
        //public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.AppStartActionMenu, menu);
        //    if (menu != null)
        //    {
        //        menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
        //        menu.FindItem(Resource.Id.action_attach).SetVisible(false);
        //    }
        //    return base.OnCreateOptionsMenu(menu);
        //}
        //define action for tolbar icon press
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            //this.Activity.Finish();
        //            return true;
        //        case Resource.Id.action_attach:
        //            //FnAttachImage();
        //            return true;
        //        default:
        //            return base.OnOptionsItemSelected(item);
        //    }
        //}
        //to avoid direct app exit on backpreesed and to show fragment from stack
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
            {
                FragmentManager.PopBackStack();// fragmentManager.popBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }

 

}


