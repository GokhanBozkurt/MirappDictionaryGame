using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using widget = Android.Support.Design.Widget;
namespace MirappDictionaryGame
{
    public class FragmentAdd : Fragment
    {
        private AutoCompleteTextView wordText;
        private AutoCompleteTextView translatedWordText;
        private Repository<MyDictonaryWord> _repository;
        //private Spinner _spinner;
        private widget.FloatingActionButton DicSaveFab;
        private widget.FloatingActionButton AddToListFab;
        
        private MyDictonaryWord dictonaryWords;
        private LinearLayout AddRelativeLayout;
        private Switch AddFavorites;
        private EditText snonymWord;
        private MyDictonaryWord selectedWord;
        private widget.FloatingActionButton DicClearFav;

        public string Language => "Türkçe";

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            LoadViews();

            SetRepository();

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Add, container, false);
            return view;
        }

        public void FindViews()
        {
            translatedWordText = View.FindViewById<AutoCompleteTextView>(Resource.Id.TranslatedWordText);
            //_spinner = View.FindViewById<Spinner>(Resource.Id.spinner);
            wordText = View.FindViewById<AutoCompleteTextView>(Resource.Id.WordTextAutoComplete);
            snonymWord = View.FindViewById<EditText>(Resource.Id.SnonymWord);
            DicSaveFab = View.FindViewById<widget.FloatingActionButton>(Resource.Id.DicSaveFab);
            AddToListFab = View.FindViewById<widget.FloatingActionButton>(Resource.Id.AddToListFab);
            AddRelativeLayout = View.FindViewById<LinearLayout>(Resource.Id.AddRelativeLayout);
            AddFavorites = View.FindViewById<Switch>(Resource.Id.AddFavorites);
            DicClearFav = View.FindViewById<widget.FloatingActionButton>(Resource.Id.DicClearFav);
        }

        public  void HandleEvents()
        {
            wordText.ItemClick += wordTextAutoComplete_ItemClick;
            wordText.AfterTextChanged += wordText_AfterTextChanged;

            translatedWordText.ItemClick += translatedWordText_ItemClick;
            //translatedWordText.AfterTextChanged += translatedWordText_AfterTextChanged;

            DicSaveFab.Click += DicSaveFab_Click;
            AddToListFab.Click += AddToListFab_Click;

            DicClearFav.Click += DicClearFav_Click;
        }

        private void DicClearFav_Click(object sender, EventArgs e)
        {
            wordText.Text = "";
            translatedWordText.Text = "";
            snonymWord.Text = "";
            wordText.Focusable = true;
            selectedWord = null;
        }

        private void translatedWordText_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var ww = (AutoCompleteTextView)sender;
            if (String.IsNullOrEmpty(ww.Text))
            {
                translatedWordText.Text = "";
                snonymWord.Text = "";
                selectedWord = null;
            }
            //else
            //{
            //    translatedWordText_ItemClick(null, null);
            //}
        }

        private void wordText_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var ww = (AutoCompleteTextView)sender;
            if (String.IsNullOrEmpty(ww.Text))
            {
                translatedWordText.Text = "";
                snonymWord.Text = "";
                selectedWord = null;
            }
            //else
            //{
            //    wordTextAutoComplete_ItemClick(null, null);
            //}
        }

        private void AddToListFab_Click(object sender, EventArgs e)
        {
            var ft = FragmentManager.BeginTransaction();
            var myFragmentContainer = new MyFragmentContainer
            {
                MyFragment = new FragmentList2(),
                Name = Resource.String.List
            };
            ft.Replace(Resource.Id.HomeFrameLayout, myFragmentContainer.MyFragment, "List");
            //SupportActionBar.SetTitle(myFragmentContainer.Name);
            ft.Commit();
        }

        public  void LoadViews()
        {
            //var adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //_spinner.Adapter = adapter;

            var aa = ManagerDictionary.WordList;
            aa.AddRange(ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList());

            var adapter = new ArrayAdapter<String>(Activity, Resource.Layout.WordListItem, aa.Select(a => a.Word).Distinct().ToList());
            wordText.Adapter = adapter;

            var adapter2 = new ArrayAdapter<String>(Activity, Resource.Layout.WordListItem, aa.Select(a => a.TranslatedWord).Distinct().ToList());
            translatedWordText.Adapter = adapter2;

        }


        public  void SetRepository()
        {
            _repository = new Repository<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();
        }
        private void DicSaveFab_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void wordTextAutoComplete_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                var count = ManagerDictionary.WordList.Where(a => a.Word == wordText.Text).Select(b => b.TranslatedWord).Count();
                if (count > 0)
                {
                    selectedWord = ManagerDictionary.WordList.Where(a => a.Word == wordText.Text).Select(b => b).First();
                    translatedWordText.Text = selectedWord.TranslatedWord;
                    snonymWord.Text = selectedWord.SnonymWord;
                    return;
                }

                count = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList().Where(a => a.Word == wordText.Text).Select(b => b.TranslatedWord).Count();
                if (count > 0)
                {
                    selectedWord = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList().Where(a => a.Word == wordText.Text).Select(b => b).First();
                    translatedWordText.Text = selectedWord.TranslatedWord;
                    snonymWord.Text = selectedWord.SnonymWord;
                    return;
                }
            }
            catch (Exception)
            {

                
            }
        }

        private void translatedWordText_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                var count = ManagerDictionary.WordList.Where(a => a.TranslatedWord == translatedWordText.Text).Select(b => b.Word).Count();
                if (count > 0)
                {
                    selectedWord = ManagerDictionary.WordList.Where(a => a.TranslatedWord == translatedWordText.Text).Select(b => b).First();
                    wordText.Text = selectedWord.Word;
                    snonymWord.Text = selectedWord.SnonymWord;
                    return;
                }

                count = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList().Where(a => a.TranslatedWord == translatedWordText.Text).Select(b => b.Word).Count();
                if (count > 0)
                {
                    selectedWord = ManagerRepository.Instance.FavoriteWord.ToDictonaryWordList().Where(a => a.TranslatedWord == translatedWordText.Text).Select(b => b).First();
                    wordText.Text = selectedWord.Word;
                    snonymWord.Text = selectedWord.SnonymWord;
                    return;
                }
            }
            catch (Exception)
            {

                 
            }
        }


        private void Save()
        {
            if (CheckForm())
            {
                if (selectedWord!=null)
                {
                    dictonaryWords = new MyDictonaryWord()
                    {
                        Language = selectedWord.Language,
                        Word = wordText.Text,
                        TranslatedWord = translatedWordText.Text,
                        SnonymWord = snonymWord.Text,
                        MyWord = selectedWord.MyWord,
                        Id = selectedWord.Id
                    };
                    _repository.Update(dictonaryWords);
                    if (AddFavorites.Checked)
                    {
                        var count = ManagerRepository.Instance.FavoriteWord.GetRecords().Where(a => a.Word == selectedWord.Word).Count() ;

                        if (count>0)
                        {
                            var FavoriteWord = ManagerRepository.Instance.FavoriteWord.GetRecords().Where(a => a.Word == selectedWord.Word).First();

                            FavoriteWord.Word = wordText.Text;
                            FavoriteWord.TranslatedWord= translatedWordText.Text;
                            FavoriteWord.SnonymWord = snonymWord.Text;
                            ManagerRepository.Instance.FavoriteWord.Update(FavoriteWord);
                        }
                        else
                        {
                            var FavoriteWord = new FavoriteWord
                            {
                                Language = dictonaryWords.Language,
                                TranslatedWord = dictonaryWords.TranslatedWord,
                                SnonymWord = snonymWord.Text,
                                Word = dictonaryWords.Word
                            };
                            ManagerRepository.Instance.FavoriteWord.Add(FavoriteWord);
                        }

                    }
                }
                else
                {
                    dictonaryWords = new MyDictonaryWord()
                    {
                        Language = Language,
                        Word = wordText.Text,
                        TranslatedWord = translatedWordText.Text,
                        SnonymWord = snonymWord.Text,
                        MyWord = true
                    };
                    _repository.Insert(dictonaryWords);
                    if (AddFavorites.Checked)
                    {
                        var FavoriteWord = new FavoriteWord
                        {
                            Language = dictonaryWords.Language,
                            TranslatedWord = dictonaryWords.TranslatedWord,
                            SnonymWord = snonymWord.Text,
                            Word = dictonaryWords.Word
                        };
                        ManagerRepository.Instance.FavoriteWord.Add(FavoriteWord);
                    }
                }
                
                ManagerDictionary.DictonaryUpdated();
                ClearForm();
                LoadViews();
            }
        }
        private bool CheckForm()
        {
            if (wordText.Text == "" || translatedWordText.Text == "")
            {
                var toast = Toast.MakeText(this.Activity, "Please fill the form", ToastLength.Short);
                toast.Show();
                return false;
            }

            return true;
        }

        private void ClearForm(bool showSnackbar=true)
        {
            if (showSnackbar)
            {

                widget.Snackbar
#pragma warning disable CS0618 // 'Html.FromHtml(string)' is obsolete: 'deprecated'
                    .Make(AddRelativeLayout, Android.Text.Html.FromHtml("<font color='white'> <b>Word Added</b>"), widget.Snackbar.LengthLong)
#pragma warning restore CS0618 // 'Html.FromHtml(string)' is obsolete: 'deprecated'
                    .SetAction("Undo", (view) =>
                    {
                        var addeddictonaryWords = (
                                                    from a in ManagerDictionary.WordList
                                                    where a.Word == dictonaryWords.Word && a.TranslatedWord == dictonaryWords.TranslatedWord
                                                    select a)
                                                    .First();
                        _repository.Delete(addeddictonaryWords);
                        ManagerDictionary.DictonaryUpdated();
                        if (AddFavorites.Checked)
                        {
                            var favaddeddictonaryWords = (
                                                   from a in ManagerRepository.Instance.FavoriteWord.GetRecords()
                                                   where a.Word == dictonaryWords.Word && a.TranslatedWord == dictonaryWords.TranslatedWord
                                                   select a)
                                                   .First();
                            ManagerRepository.Instance.FavoriteWord.Delete(favaddeddictonaryWords);
                        }
                    })
                    //.SetActionTextColor(Android.Graphics.Color.White)
                    .Show();
            }
            wordText.Text = "";
            translatedWordText.Text = "";
            snonymWord.Text = "";
            wordText.Focusable = true;
            selectedWord = null;
        }
    }
}