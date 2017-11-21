using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Support.V7.Widget;
using com.refractored.fab;
using Android.Support.V4.Widget;
using widget = Android.Support.Design.Widget;

namespace MirappDictionaryGame
{
    //, IOnScrollChangedListener, IScrollDirectorListener
    public class FragmentList : Fragment
    {
        private int startCount = 50;
        private Repository<MyDictonaryWord> _repository = new Repository<MyDictonaryWord>();
        private List<MyDictonaryWord> _listDictonaryWords;
        private DictionaryRecyclerViewAdapter _dictonaryListAdapter;
        private Switch ListFavorites;
        private Switch HideTranslate;
        private widget.CoordinatorLayout ListCoordinatorLayout;
        private Android.Support.Design.Widget.FloatingActionButton dicListFab;
        public ISpinnerAdapter LanguageAdapter { get; set; }
        private RecyclerView recyclerView;
        //private Button searchButton;
        private TextView searchWord;
        private Switch  ShowSnonym;
        private NestedScrollView scrollView;
        private Button loadMore;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            try
            {
                base.OnActivityCreated(savedInstanceState);

                FindViews();

                SetRepository();

                SpecialView();

                LoadList(searchWord.Text, startCount);

                //if (ListView != null)
                //{
                //    ListView.ItemLongClick += listView_ItemLongClick;
                //}
                
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
            var view = inflater.Inflate(Resource.Layout.List, container, false);
            return view;
        }


        protected virtual void FindViews()
        {
            recyclerView = View.FindViewById<RecyclerView>(Resource.Id.DicListRecyclerView2);
            dicListFab = View.FindViewById<Android.Support.Design.Widget.FloatingActionButton>(Resource.Id.DicListFab);
            searchWord = View.FindViewById<TextView>(Resource.Id.ivSearchWord);
            ListCoordinatorLayout = View.FindViewById<widget.CoordinatorLayout>(Resource.Id.ListCoordinatorLayout);
            loadMore = View.FindViewById<Button>(Resource.Id.LoadMore);

            searchWord.AfterTextChanged += SearchWord_AfterTextChanged; ;
            dicListFab.Click += dicListFab_Click;
            loadMore.Click += LoadMore_Click;
            //searchButton.Click += searchButton_Click;

            //recyclerView.SetOnScrollListener(InfiniteScrollListener();


            scrollView = (NestedScrollView)View.FindViewById(Resource.Id.ListNestedScrollView);
            //scrollView.ScrollChange += ScrollView_ScrollChange;

            ListFavorites = View.FindViewById<Switch>(Resource.Id.ListFavorites);
            ListFavorites.CheckedChange += playWithFavorites_CheckedChange;
            HideTranslate = View.FindViewById<Switch>(Resource.Id.HideTranslate);
            HideTranslate.CheckedChange += HideTranslate_CheckedChange;

             ShowSnonym = View.FindViewById<Switch>(Resource.Id.ShowSnonym);
             ShowSnonym.CheckedChange += ShowSnonym_CheckedChange;

            recyclerView.HasFixedSize = true;
            recyclerView.NestedScrollingEnabled = false;
            recyclerView.SetItemAnimator(new DefaultItemAnimator());

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(Activity. ApplicationContext);
            recyclerView.SetLayoutManager(layoutManager);



            //recyclerView.AddItemDecoration(new DividerItemDecoration(Activity, DividerItemDecoration.VerticalList));

        }

        private void LoadMore_Click(object sender, EventArgs e)
        {

            startCount = startCount + 50;
            //LoadList(searchWord.Text, startCount);
            var _dictonaryListAdapter = GetDictonaryListAdapter(startCount);
            recyclerView.SetAdapter(_dictonaryListAdapter);

            
            //_dictonaryListAdapter.NotifyDataSetChanged();
        }


        private void SearchWord_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            LoadList(searchWord.Text, startCount);
        }

        private void ShowSnonym_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            LoadList(searchWord.Text, startCount);
        }

        private void HideTranslate_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            LoadList(searchWord.Text, startCount);
        }

        private void playWithFavorites_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            LoadList(searchWord.Text, startCount);
        }


        //private void searchButton_Click(object sender, EventArgs e)
        //{
        //    LoadList(searchWord.Text);
        //}

        private void ScrollView_ScrollChange(object sender, NestedScrollView.ScrollChangeEventArgs e)
        {

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var layoutManager = new LinearLayoutManager(recyclerView.Context);
            var pastVisiblesItems = layoutManager.FindFirstVisibleItemPosition();
            if ((visibleItemCount
                + pastVisiblesItems
                ) >= totalItemCount)
            {
                var dictonaryWords = new MyDictonaryWord()
                {
                    Language = GetLanguage
                };

                var searching = String.IsNullOrEmpty(searchWord.Text);
                if (searching)
                {
                    _listDictonaryWords = ManagerDictionary.PrepareWordList(dictonaryWords, ListFavorites.Checked, searchWord.Text);
                }
                else
                {
                    _listDictonaryWords = ManagerDictionary.PrepareWordList(dictonaryWords, searchWord.Text, ListFavorites.Checked);
                }

                _dictonaryListAdapter.NotifyDataSetChanged();

                //LoadList(searchWord.Text, startCount);
                //LoadList(this, totalItemCount);
            }
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
            //SupportActionBar.SetTitle(myFragmentContainer.Name);
            ft.Commit();

            //var intent = new Intent();
            //intent.SetClass(this.Activity, typeof(DictionaryAddActivity));
            //StartActivityForResult(intent, 100);
            //Activity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        protected void SpecialView()
        {
            //_listCount = View.FindViewById<TextView>(Resource.Id.ListCount);
            //_listLangugaeSpinner = View.FindViewById<Spinner>(Resource.Id.ListLangugaeSpinner2);

            //_listLangugaeSpinner.ItemSelected += (sender, args) =>
            //                                                        {
            //                                                            LoadList(searchWord.Text);
            //                                                        };
            //LanguageAdapter = ManagerAdapter.GetLanguageAdapter(this.Activity);
            //_listLangugaeSpinner.Adapter = LanguageAdapter;
        }

        private void listView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var dictonaryRowWordId = e.View.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DictionaryEditActivity));
            intent.PutExtra("wordId", Convert.ToInt32(dictonaryRowWordId.Text));
            StartActivityForResult(intent, startCount);
            Activity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        private void SetRepository()
        {
            _repository = new Repository<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();
        }


        private void LoadList(string searchText,int itemCount)
        {
            startCount = 50;

            var dictonaryWords = new MyDictonaryWord()
            {
                Language = GetLanguage
            };

            var searching = String.IsNullOrEmpty(searchText);
            if (searching)
            {
                _listDictonaryWords = ManagerDictionary.PrepareWordList(dictonaryWords, ListFavorites.Checked, searchText);
            }
            else
            {
                _listDictonaryWords = ManagerDictionary.PrepareWordList(dictonaryWords, searchText, ListFavorites.Checked);
            }

            if (_listDictonaryWords != null)
            {
             
                   
                var _dictonaryListAdapter = GetDictonaryListAdapter(itemCount);
                if (_dictonaryListAdapter.ItemCount>= startCount)
                {
                    //var layoutManager = new LinearLayoutManager(recyclerView.Context);
                    //var onScrollListener = new XamarinRecyclerViewOnScrollListener(layoutManager);
                    //onScrollListener.LoadMoreEvent += (object sender, int e) =>
                    //                                    {
                    //                                        LoadList(searchText, itemCount + 15);
                    //                                    };
                    //scrollView.SetOnScrollChangeListener(onScrollListener);
                    //recyclerView.SetLayoutManager(layoutManager);
                }
                
                recyclerView.SetAdapter(_dictonaryListAdapter);
                // initSwap();
                //if (!searching)
                {
                    var toast = Toast.MakeText(Activity, $"First {recyclerView.GetAdapter().ItemCount} Word listed", ToastLength.Short);
                    toast.Show();
                }
            }


        }

        private DictionaryRecyclerViewAdapter GetDictonaryListAdapter(int itemCount)
        {
            //ListFavorites.Checked ? 1000 : 200
            List<MyDictonaryWord> lst = _listDictonaryWords.OrderBy(a => a.Language).ThenBy(b => b.Word).ThenBy(c => c.TranslatedWord).Take(itemCount).ToList();
             _dictonaryListAdapter = new DictionaryRecyclerViewAdapter(Activity, lst, HideTranslate.Checked, ShowSnonym.Checked);

            //_dictonaryListAdapter.ItemClick += OnItemClick;
            //_dictonaryListAdapter.ItemDeleteClick += OnItemDeleteClick;
            //_dictonaryListAdapter.ItemLongClick += OnItemLongClick;
            //_dictonaryListAdapter.ItemEditClick += OnItemEditClick;

            return _dictonaryListAdapter;
        }

        private void OnItemEditClick(object sender, RelativeLayout view)
        {
            var dictonaryRowWordId = view.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
            var intent = new Intent();
            intent.SetClass(Activity, typeof(DictionaryEditActivity));
            intent.PutExtra("wordId", Convert.ToInt32(dictonaryRowWordId.Text));
            intent.PutExtra("favorites", ListFavorites.Checked);
            StartActivityForResult(intent, startCount);
            Activity.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }

        private void OnItemLongClick(object sender, RelativeLayout view)
        {
            var DictonaryRowToWord = view.FindViewById<TextView>(Resource.Id.DictonaryRowToWord);
            var SnonymWord = view.FindViewById<TextView>(Resource.Id.SnonymWord);

            if (DictonaryRowToWord.Visibility==ViewStates.Invisible)
            {
                DictonaryRowToWord.Visibility = ViewStates.Visible;
                //SnonymWord.Visibility = ViewStates.Invisible;
            }
            else
            {
                DictonaryRowToWord.Visibility = ViewStates.Invisible;
                //SnonymWord.Visibility = ViewStates.Visible;
            }

        }

        private void OnItemDeleteClick(object sender, RelativeLayout view)
        {

            AlertDialog.Builder alert = new AlertDialog.Builder(this.Activity);
            alert.SetTitle("Alert");
            alert.SetMessage("Are you sure to delete?");
            alert.SetPositiveButton("OK", (senderAlert, args) =>
            {
                var DictonaryRowWordId = view.FindViewById<TextView>(Resource.Id.DictonaryRowWordID);
                int iddd = int.Parse(DictonaryRowWordId.Text);
                var word = new MyDictonaryWord() { Id = iddd };
                var favoriteWord = new FavoriteWord() { Id = iddd };
                if (ListFavorites.Checked)
                {
                    favoriteWord = ManagerRepository.Instance.FavoriteWord.GetRecord(favoriteWord);
                    ManagerRepository.Instance.FavoriteWord.Delete(favoriteWord);
                }
                else
                {
                    word = ManagerRepository.Instance.MyDictonaryWord.GetRecord(word);
                    ManagerRepository.Instance.MyDictonaryWord.Delete(word);
                    ManagerDictionary.DictonaryUpdated();
                }
                LoadList(searchWord.Text, startCount);
                var delWord = ListFavorites.Checked ? favoriteWord.Word : word.Word;

                widget.Snackbar
#pragma warning disable CS0618 // 'Html.FromHtml(string)' is obsolete: 'deprecated'
                   .Make(ListCoordinatorLayout, Android.Text.Html.FromHtml($"<font color='yellow'>{ delWord } Deleted</b>"), widget.Snackbar.LengthLong)
#pragma warning restore CS0618 // 'Html.FromHtml(string)' is obsolete: 'deprecated'
                   .SetAction("Undo", (vieww) =>
                   {

                       if (ListFavorites.Checked)
                       {
                           ManagerRepository.Instance.FavoriteWord.Insert(favoriteWord);
                       }
                       else
                       {
                           ManagerRepository.Instance.MyDictonaryWord.Insert(word);
                           ManagerDictionary.DictonaryUpdated();
                       }

                       LoadList(searchWord.Text, startCount);

                   })
                   //.SetActionTextColor(Android.Graphics.Color.White)
                   .Show();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {

            });
            Dialog dialog = alert.Create();
            dialog.Show();


        }

        void OnItemClick(object sender, string id)
        {
            //Android.Widget.Toast.MakeText(Activity, "This is id " + id, Android.Widget.ToastLength.Short).Show();
        }
        private void initSwap()
        {
            ItemTouchHelperSimpleCallback simpleItemTouchCallback = new ItemTouchHelperSimpleCallback(0, 0);
            Android.Support.V7.Widget.Helper.ItemTouchHelper itemTouchHelper = new Android.Support.V7.Widget.Helper.ItemTouchHelper(simpleItemTouchCallback);
            itemTouchHelper.AttachToRecyclerView(recyclerView);
        }
        private void onScrollListener_LoadMoreEvent(object sender, EventArgs e)
        {
            var toast = Toast.MakeText(Activity, $"aadsd", ToastLength.Short);
            toast.Show();
        }

        public string GetLanguage
        {
            get
            {
                //string language;
                //try
                //{
                //    language = _listLangugaeSpinner.SelectedItem.ToString();
                //}
                //catch (System.Exception ex)
                //{

                //    language = LanguageAdapter.GetItem(LanguageAdapter.Count).ToString();
                //}
                //return language;
                return "Türkçe";
            }
        }

        public void OnScrollChanged(ScrollView who, int l, int t, int oldl, int oldt)
        {

        }

        public void OnScrollDown()
        {

        }

        public void OnScrollUp()
        {

        }
    }

    public class XamarinRecyclerViewOnScrollListener : NestedScrollView.IOnScrollChangeListener
    {
        public delegate void LoadMoreEventHandler(object sender, int e);
        public event LoadMoreEventHandler LoadMoreEvent;

        private LinearLayoutManager LayoutManager;

        public IntPtr Handle
        {
            get
            {
                return new IntPtr();
            }
        }

        public XamarinRecyclerViewOnScrollListener(LinearLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        //public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        //{
        //    base.OnScrolled(recyclerView, dx, dy);

        //    var visibleItemCount = recyclerView.ChildCount;
        //    var totalItemCount = recyclerView.GetAdapter().ItemCount;
        //    var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();

        //    if ((visibleItemCount 
        //        + pastVisiblesItems
        //        ) >= totalItemCount)
        //    {
        //        LoadMoreEvent(this, totalItemCount);
        //    }
        //}

        // The current offset index of data you have loaded
        private int currentPage = 0;
        // The total number of items in the dataset after the last load
        private int previousTotalItemCount = 0;
        // True if we are still waiting for the last set of data to load.
        private Boolean loading = true;
        // Sets the starting page index
        private int startingPageIndex = 0;
        // The minimum amount of pixels to have below your current scroll position
        // before loading more.
        private int visibleThresholdDistance = 300;


        public void OnScrollChange(NestedScrollView scrollView, int x, int y, int oldx, int oldy)
        {
            View view = scrollView.GetChildAt(scrollView.ChildCount - 1);
            int distanceToEnd = (view.Bottom - (scrollView.Height + scrollView.ScrollY));

            int totalItemCount = LayoutManager.ItemCount;
            // If the total item count is zero and the previous isn't, assume the
            // list is invalidated and should be reset back to initial state
            if (totalItemCount < previousTotalItemCount)
            {
                this.currentPage = this.startingPageIndex;
                this.previousTotalItemCount = totalItemCount;
                if (totalItemCount == 0)
                {
                    this.loading = true;
                }
            }

            // If it’s still loading, we check to see if the dataset count has
            // changed, if so we conclude it has finished loading and update the current page
            // number and total item count.
            if (loading && (totalItemCount > previousTotalItemCount))
            {
                loading = false;
                previousTotalItemCount = totalItemCount;
            }

            // If it isn’t currently loading, we check to see if we have breached
            // the visibleThreshold and need to reload more data.
            // If we do need to reload some more data, we execute onLoadMore to fetch the data.
            // threshold should reflect how many total columns there are too
            if (!loading && distanceToEnd <= visibleThresholdDistance)
            {
                currentPage++;
                //onLoadMore(currentPage, totalItemCount);
                LoadMoreEvent(this, totalItemCount);
                loading = true;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}