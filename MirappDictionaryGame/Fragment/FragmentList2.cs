using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace MirappDictionaryGame
{
    public class FragmentList2 : MyFragmentBase<MyDictonaryWord>
    {
        private const int decresaseValue = 50;
        private FloatingActionButton dicListFab;
        private Button loadMore;
        private Switch ListFavorites;
        private TextView searchWord;
        private Spinner ListOrderSpinner;


        private Repository<MyDictonaryWord> _repository = new Repository<MyDictonaryWord>();
        private RecyclerView recyclerView;
        private DataAdapter adapter;
        private int startCount = 0;
        private List<MyDictonaryWord> adapterList;
        private List<MyDictonaryWord> LoadedList;
        private ArrayAdapter ListOrderSpinneradapter;

        public bool isFavorites
        {
            get
            {
                return ListFavorites.Checked;
            }
        }

        public bool OrderListByDate
        {
            get
            {
                string order;
                try
                {
                    order = ListOrderSpinner.SelectedItem.ToString();
                }
                catch (System.Exception ex)
                {

                    order = ListOrderSpinneradapter.GetItem(0).ToString();
                }

                return order == ListOrderSpinneradapter.GetItem(0).ToString();
            }
        }

        public string GetLanguage
        {
            get
            {
                return "Türkçe";
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            try
            {
                base.OnActivityCreated(savedInstanceState);

                SetRepository();

                FindViews();

                SetListRecylerView();

            }
            catch (Exception ex)
            {

                var toast = Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                toast.Show(); ;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.List2, container, false);
            return view;
        }

        private void SetRepository()
        {
            _repository = new Repository<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();

        }

        protected virtual void FindViews()
        {
            searchWord = View.FindViewById<TextView>(Resource.Id.ivSearchWord2);
            recyclerView = View.FindViewById<RecyclerView>(Resource.Id.DicListRecyclerViewList2);
            dicListFab = View.FindViewById<FloatingActionButton>(Resource.Id.DicListFabList2);
            loadMore = View.FindViewById<Button>(Resource.Id.LoadMoreList2);
            ListFavorites = View.FindViewById<Switch>(Resource.Id.ListFavorites2);
            ListOrderSpinner = View.FindViewById<Spinner>(Resource.Id.ListOrderSpinner);

            dicListFab.Click += dicListFab_Click;
            loadMore.Click += LoadMore_Click;
            ListFavorites.CheckedChange += ListFavorites_CheckedChange;
            searchWord.AfterTextChanged += SearchWord_AfterTextChanged; ;
            ListOrderSpinner.FocusChange += ListOrderSpinner_FocusChange;

            ListOrderSpinneradapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.ListOrderSpinner, Resource.Layout.spinner_item);
            ListOrderSpinneradapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
            ListOrderSpinner.Adapter = ListOrderSpinneradapter;
            ListOrderSpinner.ItemSelected += ListOrderSpinner_ItemSelected;
        }

        private void LoadMore_Click(object sender, EventArgs e)
        {
            UpdateAdapterList();
        }

        private void ListOrderSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            SetListRecylerView(); 
        }

        private void ListOrderSpinner_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            UpdateAdapterList();
        }

        private void SearchWord_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            adapterList = new List<MyDictonaryWord>();
            LoadList(searchWord.Text);
            SetRecyclerView();
        }

        private void ListFavorites_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            SetListRecylerView();
        }

        private bool Adapter_AddEvent(MyDictonaryWord myDictonaryWord, bool favorites)
        {
            bool result;
            if (isFavorites)
            {
                var FavoriteWord = new FavoriteWord
                {
                    Language = myDictonaryWord.Language,
                    TranslatedWord = myDictonaryWord.TranslatedWord,
                    SnonymWord = myDictonaryWord.SnonymWord,
                    Word = myDictonaryWord.Word
                };
                result= ManagerRepository.Instance.FavoriteWord.Insert(FavoriteWord);

            }
            else
            {
                result=_repository.Insert(myDictonaryWord);
                ManagerDictionary.DictonaryUpdated();
            }
            if (result)
            {
                adapterList.Add(myDictonaryWord);
            }
            return result;
        }

        private bool Adapter_DeleteEvent(MyDictonaryWord myDictonaryWord, bool favorites)
        {
            bool result;
            if (isFavorites)
            {
                var myFavoritesDictonaryWord = new FavoriteWord() { Id = myDictonaryWord.Id };
                result=ManagerRepository.Instance.FavoriteWord.Delete(myFavoritesDictonaryWord);

            }
            else
            {
                result = _repository.Delete(myDictonaryWord);
                ManagerDictionary.DictonaryUpdated();
            }
            
            if (result)
            {
                adapterList.Remove(myDictonaryWord);
            }
            return result;
        }

        private void dicListFab_Click(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            var myFragmentContainer = new MyFragmentContainer
            {
                MyFragment = new FragmentAdd(),
                Name = Resource.String.Add
            };
            ft.Replace(Resource.Id.HomeFrameLayout, myFragmentContainer.MyFragment);
            ft.Commit();

            //var intent = new Intent();
            //intent.SetClass(this.Activity, typeof(DictionaryAddActivity));
            //StartActivityForResult(intent, 100);
            //Activity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        private void LoadList(string searchText)
        {

            var dictonaryWords = new MyDictonaryWord()
            {
                Language = GetLanguage
            };
            if (OrderListByDate)
            {
                LoadedList = ManagerDictionary.PrepareWordList(dictonaryWords, searchText, isFavorites).OrderByDescending(c => c.Id).ToList();
            }
            else
            {
                LoadedList = ManagerDictionary.PrepareWordList(dictonaryWords, searchText, isFavorites);
            }

            if (OrderListByDate)
            {
                startCount = LoadedList.Max(C => C.OrderId) - decresaseValue;
            }
            else
            {
                startCount = decresaseValue;

            }

        }

        private void UpdateAdapterList()
        {
            int newstartCount = 0;

            if (OrderListByDate)
            {
                newstartCount = startCount - decresaseValue;
            }
            else
            {
                newstartCount = startCount + decresaseValue;
            }

            var newlist = new List<MyDictonaryWord>();
            if (OrderListByDate)
            {
                var l = LoadedList.OrderByDescending(c => c.Id);
                newlist = l.Where(a => a.OrderId >= newstartCount && a.OrderId < startCount).ToList();
            }
            else
            {
                newlist = LoadedList.OrderBy(b => b.Word).Where(a => a.OrderId > startCount && a.OrderId <= newstartCount).ToList();
            }

            adapterList.AddRange(newlist);
            adapter.NotifyDataSetChanged();
            //recyclerView.SetAdapter(adapter);
            startCount = newstartCount;
        }

        public override void SetListRecylerView()
        {
            LoadList(searchWord.Text);
            SetRecyclerView();
        }

        private void SetRecyclerView()
        {
            recyclerView = null;
            recyclerView = View.FindViewById<RecyclerView>(Resource.Id.DicListRecyclerViewList2);
            recyclerView.HasFixedSize = true;
            recyclerView.NestedScrollingEnabled = false;
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(Activity.ApplicationContext);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetItemAnimator(new DefaultItemAnimator());


            if (OrderListByDate)
            {
                var l = LoadedList.OrderByDescending(c => c.OrderId);
                adapterList = l.Where(a => a.OrderId >= startCount).ToList();
            }
            else
            {
                adapterList = LoadedList.OrderBy(b => b.Word).Where(a => a.OrderId <= startCount).ToList();
            }

            adapter = new DataAdapter(Activity, adapterList, isFavorites);
            adapter.DeleteEvent += Adapter_DeleteEvent;
            adapter.AddEvent += Adapter_AddEvent;
            var yxx = adapter.ItemCount;
            recyclerView.SetAdapter(adapter);

            simpleItemTouchCallback xx = new simpleItemTouchCallback(this);
            xx.EndEvent += ShowSnackBar;
            xx.adapter = adapter;
            Android.Support.V7.Widget.Helper.ItemTouchHelper itemTouchHelper = new Android.Support.V7.Widget.Helper.ItemTouchHelper(xx);
            itemTouchHelper.AttachToRecyclerView(recyclerView);
        }

        public override MyDictonaryWord GetItem(int Id)
        {
            return adapterList.Where(g => g.Id == Id).First();
        }

        private void ShowSnackBar(MyDictonaryWord myDictonaryWord)
        {
            Snackbar
                    .Make(recyclerView, Android.Text.Html.FromHtml("<font color='white'> <b>" + myDictonaryWord.Word + " Deleted</b>"), Snackbar.LengthLong)
                    .SetAction("Undo", (view) =>
                    {
                        adapter.addItem(myDictonaryWord);
                    })
                   .Show();
            adapter.NotifyDataSetChanged();

        }


    }

}