using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Toolbars = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Widget;

namespace MirappDictionaryGame
{
    [Activity(MainLauncher = false,Theme = "@style/AppThemeRedNoActionBar", Label = "Add Word", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DictionaryAddActivity : AppCompatActivityBase
    {
        private AutoCompleteTextView _wordText;
        private EditText _translatedWordText;
        private Repository<MyDictonaryWord> _repository;
        private Spinner _spinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Add);

            base.OnCreate(savedInstanceState);

            //SetRepository();
            //SetToolBar();
        }


        public override void FindViews()
        {
            _translatedWordText = FindViewById<EditText>(Resource.Id.TranslatedWordText);
            _spinner = FindViewById<Spinner>(Resource.Id.spinner);
            _wordText = FindViewById<AutoCompleteTextView>(Resource.Id.WordTextAutoComplete);
        }

        public override void HandleEvents()
        {
            _wordText.ItemClick += WordTextAutoComplete_ItemClick;
        }

        public override void LoadViews()
        {
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Languages, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _spinner.Adapter = adapter;

            adapter = new ArrayAdapter<String>(this, Resource.Layout.WordListItem, ManagerDictionary.WordList.Select(a => a.Word).ToList());
            _wordText.Adapter = adapter;
        }

        public override void SetToolBar()
        {
            //var dictonaryToolbarTop = FindViewById<Toolbars>(Resource.Id.DictonaryToolbarTop);
            //dictonaryToolbarTop.Title = "New Word";
            //SetSupportActionBar(dictonaryToolbarTop);
        }

        public override void SetRepository()
        {
            _repository = new Repository<MyDictonaryWord>();
            _repository.Open();
            _repository.CreateTable();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.DictionaryAddToolBarItem, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToString())
            {
                    case "Save":
                            Save();
                            break;
                    case "List":
                            ToList();
                            break;
                    case "Home":
                            ToHome();
                            break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void Save()
        {
            if (CheckForm())
            {
                MyDictonaryWord dictonaryWords = new MyDictonaryWord()
                {
                    Language = _spinner.SelectedItem.ToString(),
                    Word = _wordText.Text,
                    TranslatedWord = _translatedWordText.Text,
                    MyWord = true
                };
                _repository.Insert(dictonaryWords);
                ManagerDictionary.DictonaryUpdated();
                ClearForm();
            }
        }

        private void WordTextAutoComplete_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var count = ManagerDictionary.WordList.Where(a => a.Word == _wordText.Text).Select(b => b.TranslatedWord).Count();
            if (count > 0)
            {
                _translatedWordText.Text = ManagerDictionary.WordList.Where(a => a.Word == _wordText.Text).Select(b => b.TranslatedWord).First();
            }
        }

        private bool CheckForm()
        {
            if (_wordText.Text == "" || _translatedWordText.Text == "")
            {
                var toast = Toast.MakeText(this, "Please fill the form", ToastLength.Short);
                toast.Show();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {

            _wordText.Text = "";
            _translatedWordText.Text = "";
            _wordText.RequestFocus();
        }

       

    }
}