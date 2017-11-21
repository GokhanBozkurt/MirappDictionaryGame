using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using com.refractored.fab;
using Toolbars = Android.Support.V7.Widget.Toolbar;

namespace MirappDictionaryGame
{
    [Activity(
        Label = "List", 
        Icon = "@drawable/icon",
        MainLauncher = false,
        Theme = "@style/AppThemeRedNoActionBar")]
    public class DictonaryListActivity :  AppCompatActivityBase
    {
        private DictionaryRecyclerViewAdapter _dictonaryListAdapter;
        private List<MyDictonaryWord> _listDictonaryWords;
        private FloatingActionButton _gameStartFab;
        private RecyclerView _recyclerView;

       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.DictListRecyclerview);

            base.OnCreate(savedInstanceState);

        }

        public override void FindViews()
        {
            _gameStartFab = FindViewById<FloatingActionButton>(Resource.Id.DicListFab);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.DicListRecyclerView);
        }

        public override void HandleEvents()
        {
            _gameStartFab.Click += _gameStartFab_Click;
        }

        public override void SetToolBar()
        {
            var dictonaryListToolbar = FindViewById<Toolbars>(Resource.Id.DictonaryListToolbar);
            dictonaryListToolbar.Title = "List";
            SetSupportActionBar(dictonaryListToolbar);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.DictionaryListToolBarItem, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToString())
            {
                case "Home":
                    ToHome();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void SetRepository()
        {

        }

        private void _gameStartFab_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(DictionaryAddActivity));
            StartActivityForResult(intent, 100);
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        public override void LoadViews()
        {
            var dictonaryWords = new MyDictonaryWord()
            {
                Language = "Türkçe"
            };

            _listDictonaryWords = ManagerDictionary.PrepareWordList(dictonaryWords);

            if (_listDictonaryWords != null)
            {
                _recyclerView.NestedScrollingEnabled = true;
                _recyclerView.HasFixedSize = true;
                _recyclerView.SetItemAnimator(new DefaultItemAnimator());
                _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
                //recyclerView.AddItemDecoration(new DividerItemDecoration(Activity, DividerItemDecoration.VerticalList));

                _dictonaryListAdapter = new DictionaryRecyclerViewAdapter(this, _listDictonaryWords.OrderBy(a => a.Language).ThenBy(b => b.Word).ThenBy(c => c.TranslatedWord).ToList());
                _dictonaryListAdapter.ItemClick += OnItemClick;
                _recyclerView.SetAdapter(_dictonaryListAdapter);
            }
        }

        void OnItemClick(object sender, string id)
        {
            Android.Widget.Toast.MakeText(this, "This is id " + id, Android.Widget.ToastLength.Short).Show();
        }
    }
}