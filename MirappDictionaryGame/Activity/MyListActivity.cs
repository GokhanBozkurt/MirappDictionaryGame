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
    [Activity(Label = "MyListActivity")]
    public class MyListActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.list_item, ManagerRepository.Instance.FavoriteWord.GetRecords().Select(a => a.ToString()).ToList());
            ListAdapter = adapter;

            // Create your application here
        }
    }
}