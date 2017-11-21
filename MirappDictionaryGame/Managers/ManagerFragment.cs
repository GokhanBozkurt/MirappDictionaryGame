using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MirappDictionaryGame
{
    public static class ManagerFragment
    {
        public static MyFragmentContainer GetNavigationFragment(int itemId)
        {
            MyFragmentContainer myFragmentContainer=new MyFragmentContainer();
            switch (itemId)
            {
                case (Resource.Id.nav_game):
                    myFragmentContainer.MyFragment = new FragmentGameStart();
                    myFragmentContainer.Name= Resource.String.Game;
                    break;
                //case (Resource.Id.nav_add):
                //    myFragmentContainer.MyFragment = new FragmentAdd();
                //    myFragmentContainer.Name= Resource.String.Add;
                //    break;
                case (Resource.Id.nav_List):
                    myFragmentContainer.MyFragment = new FragmentList();
                    myFragmentContainer.Name= Resource.String.List;
                    break;
                case Resource.Id.nav_Setting:
                    myFragmentContainer.MyFragment = new FragmentSetting();
                    myFragmentContainer.Name= Resource.String.Setting;
                    break;
                case (Resource.Id.nav_About):
                    myFragmentContainer.MyFragment = new FragmentAbout();
                    myFragmentContainer.Name= Resource.String.About;
                    break;
                case (Resource.Id.nav_Scores):
                    myFragmentContainer.MyFragment = new FragmentScores();
                    myFragmentContainer.Name = Resource.String.Scores;
                    break;
                default:
                    myFragmentContainer.MyFragment = new FragmentGameStart();
                    myFragmentContainer.Name= Resource.String.Game;
                    break;
            }
            return myFragmentContainer;
        }
    }

    public class MyFragmentContainer
    {
        public Fragment MyFragment { get; internal set; }
        public int Name { get; internal set; }
    }
}